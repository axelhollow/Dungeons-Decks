using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapaManager : MonoBehaviour
{
    public GameObject nodoActual;
    public GameObject nodoAnterior;
    private NodoMapa nodoMapaActual;
    public List<GameObject> listaNodos;
    public Canvas canvasTP;




    private void Start()
    {
        nodoActual = listaNodos[0];
        nodoMapaActual = nodoActual.GetComponent<NodoMapa>();
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


        yield return new WaitForSeconds(1f);
        if (nodoMapaActual.tipoEvento == TipoEvento.Combate) 
        {
            nodoMapaActual.GetComponent<Renderer>().material = nodoMapaActual.DiccionarioCartas["X"];
            SceneManager.LoadScene("TableroJuego", LoadSceneMode.Additive);
        }

        if (nodoMapaActual.tipoEvento == TipoEvento.Recursos)
        {
            nodoMapaActual.GetComponent<Renderer>().material = nodoMapaActual.DiccionarioCartas["X"];
            SceneManager.LoadScene("RecursosEscene", LoadSceneMode.Additive);
        }
        if (nodoMapaActual.tipoEvento == TipoEvento.Aldeanos)
        {
            nodoMapaActual.GetComponent<Renderer>().material = nodoMapaActual.DiccionarioCartas["X"];
            SceneManager.LoadScene("AldeanosEscene", LoadSceneMode.Additive);
        }
        if (nodoMapaActual.tipoEvento == TipoEvento.Tp)
        {
            print("tp pulsado");
            nodoMapaActual.GetComponent<Renderer>().material = nodoMapaActual.DiccionarioCartas["X"];

            canvasTP.gameObject.SetActive(true);

        }

        if (nodoMapaActual.tipoEvento == TipoEvento.Boss) 
        {
            SceneManager.LoadScene("TableroJuego", LoadSceneMode.Additive);
            nodoMapaActual.GetComponent<Renderer>().material = nodoMapaActual.DiccionarioCartas["X"];
            MazoActual.Instancia.bossEvent = true;
        }
    }

    public void QuitarCanvas() 
    {
        canvasTP.gameObject.SetActive(false);
    }

    public void volverALaAldea() 
    {
        GameObject mazoObject = new GameObject();
        if (MazoActual.Instancia == null)
        {
            mazoObject.AddComponent<MazoActual>();
        }
        List<GameObject> listaAliados = new List<GameObject>(MazoActual.Instancia.mazoActual.Keys);
        List<GameObject> listaItems = new List<GameObject>(MazoActual.Instancia.mazoObjetosActual.Keys);

       
        foreach (GameObject aliado in listaAliados)
        {
            foreach (Transform hijoTrans in aliado.transform)
            {
                if (hijoTrans.gameObject.tag == "CartaAtaque")
                {
                    Destroy(hijoTrans.gameObject);
                }
            }

        }
        PlayDungeon.instance.CartasRecuperdasAventura(listaAliados, listaItems,false);
        SceneManager.UnloadSceneAsync("Mapa");

    }
}
