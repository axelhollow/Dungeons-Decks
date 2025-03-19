using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TableroManager : MonoBehaviour
{
    public static TableroManager Instance { get; private set; }

    //Mazos
    public List<GameObject> mazo;
    private List<GameObject> mazoPersonajes =new();
    private List<GameObject> mazotilizables = new();

    //Grid Personajes
    public Transform[] gridPersonajes;


    //Grid Items
    public Transform[] gridItems;

    //Grid Ataques
    public Transform[] gridAtaques;

    //Gestion de seleccion
    GameObject objetoSeleccionado = null; // Guarda el objeto previamente seleccionado
    Color colorOriginal = Color.white; // Define el color original de las cartas

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
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.collider.gameObject;

                if (obj.CompareTag("CartaPersonaje"))
                {
                    // Restaurar el color del objeto anterior
                    if (objetoSeleccionado != null && objetoSeleccionado != obj)
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

                    // Cambiar su color
                    Renderer rend = obj.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        colorOriginal = rend.material.color; // Guarda el color original la primera vez
                        rend.material.color = Color.red;
                    }

                    Debug.Log("¡Carta seleccionada!");
                    MostrarMinimazo(obj);
                    return;
                }
                if (obj.CompareTag("CartaAtaque")) 
                {
                    if (objetoSeleccionado != null && objetoSeleccionado != obj)
                    {
                        int dañoCarta = obj.GetComponentInChildren<Minicarta>().daño;
                        print("Aplicaste "+ dañoCarta+" de daño");

                    }
                    return;

                }
            }
        }
    }

    public void MostrarMinimazo(GameObject cartaPersonaje)
    {

        List<GameObject> minimazo = cartaPersonaje.GetComponent<CartaPersonaje>().minimazo;

        int n = 0;
        if (minimazo!=null) 
        {         
            //Metemos las cartas de atauqe en su grid
            foreach (GameObject carta in minimazo)
            {
                if (carta != null)
                {
                    Vector3 posicionCarta = gridAtaques[n].transform.position;
                    carta.transform.position = posicionCarta;
                    Instantiate(carta);
                    n++;
                }
            }
        }

    }


    public void OcultarMinimazo()
    {
        GameObject[] objetosAEliminar = GameObject.FindGameObjectsWithTag("CartaAtaque");

        foreach (GameObject obj in objetosAEliminar)
        {
            Destroy(obj);
        }

    }


    public void SetMazo(List<GameObject> mazo_Cartas)
    {
        mazo = mazo_Cartas;
    }
}
