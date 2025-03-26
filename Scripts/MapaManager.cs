using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapaManager : MonoBehaviour
{
    public static MapaManager Instancia { get; private set; }  // Singleton
    public GameObject nodoActual;
    public GameObject nodoAnterior;
    private NodoMapa nodoMapaActual;
    public List<GameObject> listaNodos;


    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this; // Asigna esta instancia
            DontDestroyOnLoad(gameObject); // Mantener en todas las escenas
        }
        else
        {
            Destroy(gameObject); // Elimina duplicados si existen
        }
    }

    private void Start()
    {

        nodoActual = listaNodos[0];
        nodoMapaActual = nodoActual.GetComponent<NodoMapa>();
        print(nodoActual.name);
    }
    private void Update()
    {
        foreach( Transform hijo in nodoActual.transform) 
        {
            hijo.GetComponent<MeshRenderer>().enabled = true;
            if (nodoAnterior != null) 
            {
                foreach (Transform hijito in nodoAnterior.transform) 
                {
                    if (nodoAnterior.GetComponent<NodoMapa>().elejido==true) 
                    {
                        hijito.GetComponent<MeshRenderer>().enabled = false;
                    }
                }


            }
        }

        if (Input.GetMouseButtonDown(0))  // Click izquierdo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))  // Si el rayo impacta algo
            {
                GameObject gameObject = hit.collider.gameObject;
                if (gameObject.tag == "Nodo")
                {

                    if (gameObject == nodoMapaActual.caminoIzqu)
                    {
                        print("Has elegido la izquierda");


                        nodoMapaActual.GetComponent<Renderer>().material = nodoMapaActual.DiccionarioCartas["X"];

                        nodoMapaActual.caminoIzqu.GetComponent<NodoMapa>().elejido = true;
                        CambiarNodo(nodoMapaActual.caminoIzqu);



                    }
                    if (gameObject == nodoMapaActual.caminoDerech)
                    {
                        print("Has elegido la derecha");

                        nodoMapaActual.GetComponent<Renderer>().material = nodoMapaActual.DiccionarioCartas["X"];

                        nodoMapaActual.caminoDerech.GetComponent<NodoMapa>().elejido = true;
                        CambiarNodo(nodoMapaActual.caminoDerech);
                    }

                }
            }
        }
    }

    public void CambiarNodo(GameObject nodoNuevo) 
    {
        nodoAnterior = nodoActual;
        nodoActual = nodoNuevo;
        nodoMapaActual = nodoActual.GetComponent<NodoMapa>();
        SceneManager.LoadScene("TableroJuego");

        print("Nodo Actual: " + nodoActual);
        print("Nodo Anterior: "+nodoAnterior);
    
    }
}
