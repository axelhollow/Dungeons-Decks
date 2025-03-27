using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapaManager : MonoBehaviour
{
    public GameObject nodoActual;
    public GameObject nodoAnterior;
    private NodoMapa nodoMapaActual;
    public List<GameObject> listaNodos;




    private void Start()
    {
        nodoActual = listaNodos[0];
        nodoMapaActual = nodoActual.GetComponent<NodoMapa>();


        print(nodoActual.name);
    }
    private void Update()
    {

        foreach ( Transform hijo in nodoActual.transform) 
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
                        nodoMapaActual.caminoIzqu.GetComponent<NodoMapa>().elejido = true;
                        StartCoroutine( CambiarNodo(nodoMapaActual.caminoIzqu));
                    }
                    if (gameObject == nodoMapaActual.caminoDerech)
                    {
                        nodoMapaActual.caminoDerech.GetComponent<NodoMapa>().elejido = true;
                        StartCoroutine(CambiarNodo(nodoMapaActual.caminoDerech));
                    }

                }
            }
        }
    }

    IEnumerator CambiarNodo(GameObject nodoNuevo) 
    {
       
        nodoAnterior = nodoActual;
        nodoActual = nodoNuevo;
        nodoMapaActual = nodoActual.GetComponent<NodoMapa>();
        nodoMapaActual.GetComponent<Renderer>().material = nodoMapaActual.DiccionarioCartas["X"];

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("TableroJuego", LoadSceneMode.Additive);

        print("Nodo Actual: " + nodoActual);
        print("Nodo Anterior: "+nodoAnterior);
    
    }
}
