using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TableroManager : MonoBehaviour
{
    public static TableroManager Instance { get; private set; }

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

    GameObject ataqueSelecionado = null; // Guarda el objeto previamente seleccionado

    //Mana
    public TextMeshProUGUI textoMana;

    //listaEnemigosCarta
    public List<GameObject> listaEnemigos=new();


    //AtaqueSeleccionado
    private GameObject ataqueSeleccioando;
    private GameObject personajeSeleccionado;


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
                        print(carta.name);
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
            Instantiate(carta);
            n++;
        }
         n = 0;
        //Metemos las cartas de iteam en su grid
        foreach (GameObject carta in mazotilizables)
        {
            Vector3 posicionCarta = gridItems[n].transform.position;
            carta.transform.position = posicionCarta;
            Instantiate(carta);
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


    }

    private void Update()
    {
        //gestionarEnemigos y victoria
        try
        {
            if (listaEnemigos.Count > 0)
            {
                foreach (var item in listaEnemigos)
                {


                    if (item.GetComponent<Enemigo>().vida <= 0)
                    {
                        item.SetActive(false);
                        listaEnemigos.Remove(item);


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
                    print("turno terminado");
                    // ReactivarCartas();
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

                    if (obj.GetComponent<CartaPersonaje>().mazoYaGenerado == false)
                    {
                        GenerarMinimazo(obj);
                    }
                    else
                    {
  
                        //MostrarMinimazo(obj);
                        GenerarMinimazo(obj);
                    }
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
                        textoMana.text = ataqueSeleccioando.GetComponent<CartaPersonaje>().mana.ToString();
                    }
                    return;
                }

                if (ataqueSeleccioando != null && obj.tag == "Enemigo")
                {
                    print("Enemigo seleccionado");
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
                        int dañoCarta = ataqueSeleccioando.GetComponentInChildren<Minicarta>().daño;
                        print("Aplicaste " + dañoCarta + " de daño, al " + obj.GetComponent<Enemigo>().nombre);
                        obj.GetComponent<Enemigo>().RestarVida(dañoCarta);
                    }

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
                print("minimazo tiene cosas");
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
            print(cartaP.manoActual);

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

      
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;

        yield return new WaitForSeconds(2f);
        ReactivarCartas();
        Cursor.lockState = CursorLockMode.Confined;
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
                //hijo.gameObject.SetActive(true); // Activa cada hijo
                Debug.Log($"Activado: {hijo.gameObject.name} (hijo de {obj.name})");
            }
        }

        foreach (Transform hijo in objetoSeleccionado.transform) // Recorre todos los hijos
        {
            hijo.gameObject.SetActive(true);  
        }
        //ponemos el valor al mana
        textoMana.text = objetoSeleccionado.GetComponent<CartaPersonaje>().mana.ToString();
    }
}
