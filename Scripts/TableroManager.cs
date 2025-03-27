using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class TableroManager : MonoBehaviour
{
    //Singleton
    public static TableroManager Instance { get; private set; }

    //Particulas
    public GameObject efectoActual;

    //Mazos
    public List<GameObject> mazo;
    private List<GameObject> mazoPersonajes =new();
    private List<GameObject> mazotilizables = new();
    public List<GameObject> mazoEnemigos;

    //Grid Personajes
    public Transform[] gridPersonajes;
    //Grid Items
    public Transform[] gridItems;
    //Grid Ataques
    public Transform[] gridAtaques;
    //Grid Ataques
    public Transform[] gridEnemigos;


    //Gestion de seleccion
    GameObject objetoSeleccionado = null; // Guarda el objeto previamente seleccionado
    Color colorOriginal = Color.white; // Define el color original de las cartas

    //Mana
    public TextMeshProUGUI textoMana;

    //listas
    public List<GameObject> listaEnemigos=new();
    public List<GameObject> listaPersonajes = new();
    public List<GameObject> listaItems= new();

    int numPersonajes;

    //elementos seleccionados
    private GameObject ataqueSeleccioando;
    private GameObject personajeSeleccionado;
    private GameObject pocionSeleccionada;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//opcional ya veremos si lo necesitamos
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        GameObject obj = GameObject.Find("MapaScene");
        if (obj != null)
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(false); // Desactiva cada hijo individualmente
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto 'MapaScene'.");
        }

        //Clasificamos las cartas que nos pasan
        if (mazo != null)
        {
            foreach (GameObject carta in mazo)
            {
                if (carta != null)
                {
                    Carta cartita = carta.GetComponentInChildren<Carta>();
                    if (cartita.tipo == TipoCarta.Personaje)
                    {
                        mazoPersonajes.Add(carta);
                    }
                    if (cartita.tipo == TipoCarta.Item)
                    {
                        mazotilizables.Add(carta);
                    }
                }
            }
        }
        int n = 0;
        //Metemos las cartas de persoanje en su grid
        foreach (GameObject carta in mazoPersonajes)
        {
            Vector3 posicionCarta = gridPersonajes[n].transform.position;
            carta.transform.position = posicionCarta;
            var cartita=Instantiate(carta);
            listaPersonajes.Add(cartita);
            GenerarMinimazo(cartita);
            n++;
        }

        numPersonajes = listaPersonajes.Count();
        n = 0;
        //Metemos las cartas de iteam en su grid
        foreach (GameObject carta in mazotilizables)
        {
            Vector3 posicionCarta = gridItems[n].transform.position;
            carta.transform.position = posicionCarta;
            var cartita=Instantiate(carta);
            listaItems.Add(cartita);
            n++;
        }
        n = 0;

        //Metemos las cartas de enemigas en su grid
        foreach (GameObject carta in mazoEnemigos)
        {
            Vector3 posicionCarta = gridEnemigos[n].transform.position;
            carta.transform.position = posicionCarta;
            var cartita=Instantiate(carta);
            cartita.transform.SetParent(GameObject.FindWithTag("ListaDeEnemigos").transform);
            listaEnemigos.Add(cartita);
            n++;
        }

        foreach (GameObject personaje in listaPersonajes)
        {
            foreach (Transform hijo in personaje.transform)
            {

                if (hijo.tag == "CartaAtaque")
                {
                    hijo.gameObject.SetActive(false);

                }
            }


        }


    }

    private void Update()
    {
        //gestionarMuertePersonajes Y Derrota
        try
        {
            if (listaPersonajes.Count > 0)
            {
                for (int i = listaPersonajes.Count - 1; i >= 0; i--)
                {
                    if (listaPersonajes[i].GetComponent<CartaPersonaje>().vida <= 0)
                    {
                        listaPersonajes[i].SetActive(false);
                        listaPersonajes.RemoveAt(i);
                        numPersonajes--;
                        if (numPersonajes < 0) numPersonajes = 0;
                    }
                }
               
            }

        }
        catch (System.IndexOutOfRangeException e)
        {
            //Ganaste mi pana
            print("Exception de derrota");
        }
        if (listaPersonajes == null || listaPersonajes.Count == 0)
        {
            print("perdiste");

        }

        //gestionarEnemigos y victoria
        try
        {
            if (listaEnemigos.Count > 0)
            {


                for (int i = listaEnemigos.Count - 1; i >= 0; i--)
                {
                    if (listaEnemigos[i].GetComponent<Enemigo>().vida <= 0)
                    {
                        listaEnemigos[i].SetActive(false);
                        listaEnemigos.Remove(listaEnemigos[i]);


                    }
                }
            }
            
        }
        catch (System.IndexOutOfRangeException e)
        {
            //Ganaste mi pana
            print("Exception de victoria");
        }
        if(listaEnemigos==null || listaEnemigos.Count == 0) 
        {

            print("ganaste");

            GameObject obj = GameObject.Find("MapaScene");
            if (obj != null)
            {
                foreach (Transform child in obj.transform)
                {
                    child.gameObject.SetActive(true); // Activan cada hijo individualmente
                }
                SceneManager.UnloadSceneAsync("TableroJuego");

                GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(); // Obtiene todos los objetos de la escena

                foreach (GameObject obji in allObjects)
                {
                    if (obji.name != "MapaScene" && obji.transform.parent!= obj.transform) // Si no es el objeto que queremos conservar
                    {
                        Destroy(obji); // Lo destruimos
                    }
                }

            }
            else
            {
                Debug.LogWarning("No se encontró el objeto 'MapaScene'.");
            }


        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.CompareTag("TerminarTurno"))
                {
                    StartCoroutine("AcabarTurno");
                }

                if (obj.CompareTag("CartaPersonaje"))
                {
                    //cambiar de color el ataque
                    if (ataqueSeleccioando != null && ataqueSeleccioando != obj)
                    {
                        Renderer renderAtaque = ataqueSeleccioando.GetComponent<Renderer>();
                        if (renderAtaque != null)
                        {
                            renderAtaque.material.color = colorOriginal;
                        }
                    }
                    ataqueSeleccioando = null;

                    // Restaurar el color del objeto anterior
                    if (objetoSeleccionado != null )
                    {
                        Renderer rendPrev = objetoSeleccionado.GetComponent<Renderer>();
                        if (rendPrev != null)
                        {
                            rendPrev.material.color = colorOriginal;
                            OcultarMinimazo();
                        }
                    }

                    // Guardar el nuevo objeto seleccionado
                    objetoSeleccionado = obj;
                    personajeSeleccionado=objetoSeleccionado;

                    // Cambiar su color
                    Renderer rend = obj.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        colorOriginal = rend.material.color; // Guarda el color original la primera vez
                        rend.material.color = Color.red;
                        textoMana.text = obj.GetComponent<CartaPersonaje>().mana.ToString();
                    }

                    GenerarMinimazo(obj);
                    
                    return;
                }
                if (obj.CompareTag("CartaAtaque"))
                {
                    if (ataqueSeleccioando != null && ataqueSeleccioando != obj)
                    {
                        //volver a ponerle su color original
                        Renderer rendPrev = ataqueSeleccioando.GetComponent<Renderer>();
                        if (rendPrev != null)
                        {
                            rendPrev.material.color = colorOriginal;
                        }

                    }
                    ataqueSeleccioando = obj;
                    // Cambiar su color
                    Renderer rend = ataqueSeleccioando.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        colorOriginal = rend.material.color; // Guarda el color original la primera vez
                        rend.material.color = Color.red;
                    }
                    return;
                }
                if (obj.CompareTag("Pocion")) 
                {
                    if (personajeSeleccionado != null)
                    {
                        if (pocionSeleccionada != null && pocionSeleccionada != obj)
                        {
                            //volver a ponerle su color original
                            Renderer rendPrev = pocionSeleccionada.GetComponent<Renderer>();
                            if (rendPrev != null)
                            {
                                rendPrev.material.color = colorOriginal;
                            }

                        }
                        pocionSeleccionada = obj;
                        // Cambiar su color
                        Renderer rend = pocionSeleccionada.GetComponent<Renderer>();
                        if (rend != null)
                        {
                            colorOriginal = rend.material.color; // Guarda el color original la primera vez
                            rend.material.color = Color.red;

                        }
                    }
                    return;
                }

                if (ataqueSeleccioando != null && obj.tag == "Enemigo")
                {
                    int manaActual = int.Parse(textoMana.text);
                    int costeCarta = ataqueSeleccioando.GetComponentInChildren<Minicarta>().coste;
                    int manaRestante = manaActual - costeCarta;

                    if (manaRestante >= 0)
                    {

                        //Gestionar uso de mana
                        manaActual = manaRestante;
                        textoMana.text = manaActual.ToString();
                        ataqueSeleccioando.transform.parent.GetComponent<CartaPersonaje>().mana = manaActual;

                        //Marcar Carta Como Usada
                        CartaPersonaje cartaPersonaje = personajeSeleccionado.GetComponent<CartaPersonaje>();
                        cartaPersonaje.manoActual[ataqueSeleccioando] = !cartaPersonaje.manoActual[ataqueSeleccioando];
                        ataqueSeleccioando.SetActive(false);

                        //Restar vida al enemigo
                        int dañoCarta = ataqueSeleccioando.GetComponentInChildren<Minicarta>().damage;
                        obj.GetComponent<Enemigo>().RestarVida(dañoCarta);

                        foreach(Transform hijo in ataqueSeleccioando.transform.parent.transform) 
                        {
                            if (hijo.tag == "CartaAtaque")
                            {
                                hijo.gameObject.GetComponent<Minicarta>().RestaurarDamage();
                            }

                        }
                    }

                }
            }
        }
        if (personajeSeleccionado != null && pocionSeleccionada != null)
        {
            //pillamos el personaje
            CartaPersonaje personaje = personajeSeleccionado.GetComponent<CartaPersonaje>();
            
            for (int i = listaItems.Count - 1; i >= 0; i--)
            {
               
                if (listaItems[i] == pocionSeleccionada)
                {
                    CartaItems pocion = pocionSeleccionada.GetComponent<CartaItems>();

                    if (pocion.tipoPocion == TipoPocion.vida)
                    {
                        personaje.CurarVida(pocion.cantidadEfecto);
                    }
                    if (pocion.tipoPocion == TipoPocion.mana)
                    {
                        personaje.mana += pocion.cantidadEfecto;
                        textoMana.text = personaje.mana.ToString();
                    }
                    if (pocion.tipoPocion == TipoPocion.ataque)
                    {
                        foreach(Transform hijo in personaje.transform) 
                        {

                            if (hijo.tag=="CartaAtaque")
                            {
                                
                               hijo.transform.gameObject.GetComponent<Minicarta>().AumentarDamage(pocion.cantidadEfecto);

                            }
                        }
                    }

                    listaItems[i].SetActive(false);
                    listaItems.Remove(listaItems[i]);
                    pocionSeleccionada = null;

                }
            }


        }
    }
    public void GenerarMinimazo(GameObject cartaPersonaje)
    {
        CartaPersonaje cartaP = cartaPersonaje.GetComponent<CartaPersonaje>();
        cartaP.mazoYaGenerado = true;
        
        if (cartaP.manoActual == null || cartaP.manoActual.Count==0)
        {
     
            cartaP.manoActual = new Dictionary<GameObject, bool>();
                        Vector3 posicionCarta = gridAtaques[0].transform.position;
                        cartaP.ataque1.transform.position = posicionCarta;
                        var cartita1 = Instantiate(cartaP.ataque1);
                        cartaP.manoActual.Add(cartita1, true);
                        cartita1.transform.SetParent(cartaP.transform);

                        Vector3 posicionCarta2 = gridAtaques[1].transform.position;
                        cartaP.ataque2.transform.position = posicionCarta2;
                        var cartita2 = Instantiate(cartaP.ataque2);
                        cartaP.manoActual.Add(cartita2, true);
                        cartita2.transform.SetParent(cartaP.transform);

                        Vector3 posicionCarta3 = gridAtaques[2].transform.position;
                        cartaP.ataque3.transform.position = posicionCarta3;
                        var cartita3 = Instantiate(cartaP.ataque3);
                        cartita3.transform.SetParent(cartaP.transform);
                        cartaP.manoActual.Add(cartita3, true);     
        }
        else
        {

            foreach (var carta in cartaP.manoActual)
            {
                if (carta.Value) carta.Key.SetActive(true);
            }
        }
    }
    public void OcultarMinimazo()
    {
        GameObject[] objetosAEliminar = GameObject.FindGameObjectsWithTag("CartaAtaque");
        foreach (GameObject obj in objetosAEliminar)
        {
            obj.SetActive(false);
        }
    }

    IEnumerator AcabarTurno()
    { 
        Cursor.visible = false;

        //Turno Enemigo
        foreach (GameObject enemigo in listaEnemigos) 
        {
            yield return new WaitForSeconds(0.4f);
            Enemigo enemiguito = enemigo.GetComponent<Enemigo>();
            //seleccionamos objetivo al que atacar
            int numeroAleatorio = Random.Range(0, numPersonajes);

            print(numPersonajes);
            CartaPersonaje personaje = listaPersonajes[numeroAleatorio].GetComponent<CartaPersonaje>();

            efectoActual= enemiguito.efectoAtaque;
            efectoActual.transform.position = personaje.transform.position;
            enemiguito.transform.localScale = enemiguito.originalScale * enemiguito.scaleFactor;
            Instantiate(efectoActual);
            print(efectoActual);
            yield return new WaitForSeconds(1f);
            personaje.RestarVida(enemiguito.daño);
            enemiguito.transform.localScale = enemiguito.originalScale;
        }
        ReactivarCartas();
        Cursor.visible = true;
        
    }



    public void ReactivarCartas() 
    {
        //Reactivamos los ataques
        GameObject[] objetosConTag = GameObject.FindGameObjectsWithTag("CartaPersonaje");

        foreach (GameObject obj in objetosConTag)
        {
            //Reactivamos el mana
            obj.GetComponent<CartaPersonaje>().mana = obj.GetComponent<CartaPersonaje>().manaAux;
            foreach (Transform hijo in obj.transform) // Recorre todos los hijos
            {
                obj.GetComponent<CartaPersonaje>().manoActual[hijo.gameObject] = true;

                if (hijo.gameObject.tag == "CartaAtaque") 
                {
                    hijo.gameObject.GetComponent<Minicarta>().RestaurarDamage();
                }
            }
        }
        try
        {
            if (objetoSeleccionado.transform != null && objetoSeleccionado.transform.childCount > 0)
            {
                foreach (Transform hijo in objetoSeleccionado.transform) // Recorre todos los hijos
                {
                    hijo.gameObject.SetActive(true);
                }
            }
        }
        catch(Exception ex) 
        {
        
        }

        if (objetoSeleccionado != null) 
        {
            textoMana.text = objetoSeleccionado.GetComponent<CartaPersonaje>().mana.ToString();
        }
    }
}
