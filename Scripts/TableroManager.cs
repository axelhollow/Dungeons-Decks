using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class TableroManager : MonoBehaviour
{
    //Singleton
    public static TableroManager Instance { get; private set; }

    //Particulas
    public GameObject efectoActual;

    //Mazos
    public List<GameObject> mazo;
    private List<GameObject> mazoPersonajes;
    private List<GameObject> mazotilizables = new();

    public List<GameObject> mazoEnemigos;
    public List<GameObject> mazoEnemigosAux;

    //Grid Personajes
    public Transform[] gridPersonajes;
    //Grid Items
    public Transform[] gridItems;
    //Grid Ataques
    public Transform[] gridAtaques;
    //Grid Ataques
    public Transform[] gridEnemigos;

    public Dictionary<GameObject, GameObject> diccVivos = new();


    private Dictionary<GameObject, Color> coloresOriginales = new Dictionary<GameObject, Color>();
    Color colorOriginal = Color.white; // Define el color original de las cartas

    //Mana
    public TextMeshProUGUI textoMana;

    //listas
    public List<GameObject> listaEnemigos = new();
    public List<GameObject> listaAliados = new();
    public List<GameObject> listaItems = new();


    int numPersonajes;

    //elementos seleccionados
    private GameObject ataqueSeleccioando;
    private GameObject personajeSeleccionado;
    private GameObject pocionSeleccionada;

    //listas padres
    public GameObject listaPersonajesPadre;



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
        GameObject mazoObject = new GameObject();

        if (MazoActual.Instancia == null)
        {
            print("GENERANDO SINGLE");
            mazoObject.AddComponent<MazoActual>();
        }
        print("EMPEZAMOS");
        if (MazoActual.Instancia.mazoActual.Count == 0)
        {
            print("Mazo Vacio");
            mazoPersonajes = new();
            mazoPersonajes = PlayDungeon.instance.RecuperarPersonajes();

        }
        else
        {
            print("recuperamos mazo");
            mazoPersonajes = new();

            foreach (KeyValuePair<GameObject, bool> par in MazoActual.Instancia.mazoActual)
            {
                GameObject cartaGuardada = par.Key;   // Accede a la clave (GameObject)
                bool viva = par.Value;// Accede al valor (bool)
                if (viva != false)
                {
                    mazoPersonajes.Add(cartaGuardada);
                }

            }

        }

        GameObject obj = GameObject.Find("MapaScene");
        if (obj != null)
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(false);
            }
        }


        int n = 0;
        //Metemos las cartas de persoanje en su grid
        foreach (GameObject carta in mazoPersonajes)
        {
            print("ENTRA");
            Vector3 posicionCarta = gridPersonajes[n].transform.position;
            Vector3 tamanoCarta = gridPersonajes[n].transform.localScale;
            gridPersonajes[n].gameObject.SetActive(false);
            if (carta != null)
            {
                GameObject cartita = Instantiate(carta);

                cartita.GetComponent<CartaMovement>().holderDungeon = true;
                cartita.transform.position = posicionCarta;
                cartita.transform.SetParent(GameObject.FindWithTag("ListaDeAliados").transform);
                cartita.transform.localScale = tamanoCarta;


              
                cartita.SetActive(true);
                GenerarMinimazo(cartita);
                listaAliados.Add(cartita);
                if (MazoActual.Instancia.mazoIniciado==false)
                {
                    MazoActual.Instancia.mazoActual.Add(carta, true);
                   
                }
                diccVivos.Add(cartita, carta);
                carta.SetActive(false);
            }
            print("dic vivos numero: " + diccVivos.Count());
            n++;
        }
        MazoActual.Instancia.mazoIniciado = true;


        numPersonajes = listaAliados.Count();
        n = 0;
        //Metemos las cartas de iteam en su grid
        foreach (GameObject carta in mazotilizables)
        {
            Vector3 posicionCarta = gridItems[n].transform.position;
            carta.transform.position = posicionCarta;
            var cartita = Instantiate(carta);
            listaItems.Add(cartita);
            cartita.transform.SetParent(GameObject.FindWithTag("ListaDePotis").transform);
            n++;
        }
        n = 0;

        mazoEnemigos = mazoEnemigosAux;
        //Metemos las cartas de enemigas en su grid
        foreach (GameObject carta in mazoEnemigos)
        {
            Vector3 posicionCarta = gridEnemigos[n].transform.position;
            gridEnemigos[n].gameObject.SetActive(false);
            var cartita = Instantiate(carta);
            cartita.transform.position = posicionCarta;
            cartita.transform.SetParent(GameObject.FindWithTag("ListaDeEnemigos").transform);
            listaEnemigos.Add(cartita);
            n++;
            cartita.SetActive(true);
        }

        foreach (GameObject personaje in listaAliados)
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
            GestionarMuerteYderrota();

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

                    OcultarMinimazo();
                    // Si el mismo personaje ya está seleccionado, deseleccionarlo
                    if (personajeSeleccionado == obj)
                    {
                        RestaurarColor(personajeSeleccionado);
                        personajeSeleccionado = null;
                        textoMana.text = "";
                        return;
                    }

                    // Restaurar color del personaje anterior
                    if (personajeSeleccionado != null)
                    {
                        RestaurarColor(personajeSeleccionado);
                    }

                    // Cambiar su color a rojo
                    CambiarColor(obj, Color.red);

                    // Guardar el nuevo personaje seleccionado
                    personajeSeleccionado = obj;

                    GenerarMinimazo(personajeSeleccionado);


                    print("OBJETO SELECCIONADO: " + personajeSeleccionado);


                    // Mostrar el mana
                    CartaPersonaje carta = obj.GetComponent<CartaPersonaje>();
                    if (carta != null)
                    {
                        textoMana.text = carta.mana.ToString();
                    }

                 
                    return;
                }
                if (obj.CompareTag("CartaAtaque"))
                {
                    // Si el mismo ataque ya está seleccionado, deseleccionarlo
                    if (ataqueSeleccioando == obj)
                    {
                        RestaurarColor(ataqueSeleccioando);
                        ataqueSeleccioando = null;
                        return;
                    }

                    // Restaurar color del ataque anterior
                    if (ataqueSeleccioando != null)
                    {
                        RestaurarColor(ataqueSeleccioando);
                    }

                    // Guardar el nuevo ataque seleccionado
                    ataqueSeleccioando = obj;

                    // Cambiar su color a rojo
                    CambiarColor(obj, Color.red);
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
                        if (personajeSeleccionado != null)
                        {
                            CartaPersonaje cartaPersonaje = personajeSeleccionado.GetComponent<CartaPersonaje>();

                            if (cartaPersonaje.manoActual.ContainsKey(ataqueSeleccioando))
                            {
                                cartaPersonaje.manoActual[ataqueSeleccioando] = !cartaPersonaje.manoActual[ataqueSeleccioando];

                                ataqueSeleccioando.SetActive(false);

                                //Restar vida al enemigo
                                int dañoCarta = ataqueSeleccioando.GetComponentInChildren<Minicarta>().damage;
                                obj.GetComponent<Enemigo>().RestarVida(dañoCarta);
                            }
                        }
                        foreach (Transform hijo in ataqueSeleccioando.transform.parent.transform)
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
                        foreach (Transform hijo in personaje.transform)
                        {

                            if (hijo.tag == "CartaAtaque")
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
    void CambiarColor(GameObject obj, Color nuevoColor)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            if (!coloresOriginales.ContainsKey(obj))
            {
                coloresOriginales[obj] = rend.material.color; // Guardar color original
            }
            rend.material.color = nuevoColor;
        }
    }

    // Función para restaurar el color original
    void RestaurarColor(GameObject obj)
    {
        if (obj != null && coloresOriginales.ContainsKey(obj))
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = coloresOriginales[obj]; // Restaurar color original
            }
        }
    }
    public void GenerarMinimazo(GameObject cartaPersonaje)
    {
        CartaPersonaje cartaP = cartaPersonaje.GetComponent<CartaPersonaje>();
        cartaP.mazoYaGenerado = true;

        if (cartaP.manoActual == null || cartaP.manoActual.Count == 0)
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
            if (cartaP.manoActual != null)
            {
                foreach (var carta in cartaP.manoActual)
                {
                    if (carta.Value) carta.Key.SetActive(true);
                }
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

    public void GestionarMuerteYderrota() 
    {
        //gestionarMuertePersonajes Y Derrota
        try
        {
            if (listaAliados.Count > 0)
            {
                for (int i = listaAliados.Count - 1; i >= 0; i--)
                {
                    print(listaAliados.Count - 1);

                    if (listaAliados[i] != null)
                    {
                        GameObject personaje = listaAliados[i];
                        CartaPersonaje mazo = personaje.GetComponent<CartaPersonaje>();
                        if (mazo != null && mazo.vida <= 0)
                        {
                            listaAliados.RemoveAt(i);
                            numPersonajes--;
                            if (numPersonajes < 0) numPersonajes = 0;
                            if (i < 0) i = 0;
                            print(i);
                            print(diccVivos);
                            if (diccVivos.ContainsKey(personaje))
                            {
                                print("diccVivos contiene ese personaje");
                                GameObject carta = diccVivos[personaje];
                                if (MazoActual.Instancia.mazoActual.ContainsKey(carta))
                                {
                                    print("se elimina la carta del mazo");
                                    MazoActual.Instancia.mazoActual[carta] = false;
                                }

                            }
                            personaje.SetActive(false);


                        }


                    }
                }

            }

        }
        catch (System.IndexOutOfRangeException e)
        {
            //Ganaste mi pana
            print("Exception de derrota" + e);

        }
        if (listaAliados == null || listaAliados.Count == 0)
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
            print("Exception de victoria" + e);
        }
        if (listaEnemigos.Count == 0)
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
            }
            else
            {
                Debug.LogWarning("No se encontró el objeto 'MapaScene'.");
            }
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
            if (listaAliados.Count() > 0)
            {
                CartaPersonaje personaje = listaAliados[numeroAleatorio].GetComponent<CartaPersonaje>();

                efectoActual = enemiguito.efectoAtaque;
                efectoActual.transform.position = personaje.transform.position;

                enemiguito.transform.localScale = enemiguito.originalScale * enemiguito.scaleFactor;
                Instantiate(efectoActual);
                yield return new WaitForSeconds(1f);
                personaje.RestarVida(enemiguito.daño);
                enemiguito.transform.localScale = enemiguito.originalScale;
            }
        }
        ReactivarCartas();
        Cursor.visible = true;

    }



    public void ReactivarCartas()
    {

        foreach (GameObject obj in listaAliados)
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
        if (listaAliados[0].transform != null && listaAliados[0].transform.childCount > 0)
        {
            foreach (Transform hijo in listaAliados[0].transform) // Recorre todos los hijos
            {
                hijo.gameObject.SetActive(true);
            }
        }
        textoMana.text = listaAliados[0].GetComponent<CartaPersonaje>().mana.ToString();

    }
}
