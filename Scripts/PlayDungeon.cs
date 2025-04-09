using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayDungeon : MonoBehaviour
{
    private bool isMouseDown = false;

    public List<GameObject> personajes;
    public List<GameObject> objetos;

    //-----------BORRAR DESPUES-----------
    public List<GameObject> personajesPaLaDungeon;
    public List<GameObject> objetosPaLaDungeon;

    public static PlayDungeon instance;
    public List<GameObject> listaCartasRecuperadas;

    public GameObject padreCartasRecuperadas;

    public GameObject gameManagerObj;
    public GameManager gameManaguer;

    public void CartasRecuperdasAventura(List<GameObject> listaCartas,List<GameObject> listaObjetos) 
    {
        float z = 4f;
        float y = 0.3f;
        if (listaCartas != null)
        {
            foreach (GameObject carta in listaCartas)
            {
                GameObject cartita = Instantiate(carta);
                //cartita.transform.SetParent(padreCartasRecuperadas.transform);
                cartita.SetActive(true);
                cartita.GetComponent<CartaMovement>().holderDungeon = false;
                cartita.GetComponent<Renderer>().material.color = Color.white;

                cartita.transform.localScale = padreCartasRecuperadas.transform.localScale;

                cartita.transform.position = padreCartasRecuperadas.transform.position + new Vector3(0, y, -z);

                if (cartita.GetComponent<CartaPersonaje>().vidaMax == true) 
                {
                    cartita.GetComponent<CartaPersonaje>().vidaMax = false;
                    cartita.GetComponent<CartaPersonaje>().DisminuirVidaMax(3);
                }

                z += 4;
                y += 0.3f;
            }
        }
        if (listaObjetos != null)
        {
            foreach (GameObject carta in listaObjetos)
            {
                GameObject cartita = Instantiate(carta);
                //cartita.transform.SetParent(padreCartasRecuperadas.transform);
                if (cartita.tag == "Pocion")
                {
                    cartita.GetComponent<CartaItems>().aldea = true;

                }
                cartita.SetActive(true);
                cartita.GetComponent<CartaMovement>().holderDungeon = false;
                cartita.GetComponent<Renderer>().material.color = Color.white;
                cartita.transform.position = padreCartasRecuperadas.transform.position + new Vector3(0, y, -z);
                cartita.transform.localScale = padreCartasRecuperadas.transform.localScale;


                z += 4;
                y += 0.3f;
            }
        }
        if (MazoActual.Instancia.listaRecursos!=null) 
        {
            foreach (GameObject carta in MazoActual.Instancia.listaRecursos)
            {
                GameObject cartita = Instantiate(carta);
                //cartita.transform.SetParent(padreCartasRecuperadas.transform);
                cartita.GetComponent<CartaMovement>().holderDungeon = false;
                cartita.GetComponent<Renderer>().material.color = Color.white;
                cartita.transform.position = padreCartasRecuperadas.transform.position + new Vector3(0, y, -z);
                cartita.transform.localScale = padreCartasRecuperadas.transform.localScale;
  
                z += 4;
                y += 0.3f;
            }
        }
        
        //Borramos las cartas del banner
        foreach(GameObject personaje in personajesPaLaDungeon) 
        {
            Destroy(personaje);
        
        }
        foreach (GameObject objetos in objetosPaLaDungeon)
        {
            Destroy(objetos);
        }
        MazoActual.Instancia.mazoActual = new();
        MazoActual.Instancia.mazoIniciado = false;
        MazoActual.Instancia.mazoObjetosIniciado = false;
        personajesPaLaDungeon.Clear();
        gameManaguer.ContinueDayShowMap();
        gameManaguer.SeguirDay();
        CameraMovementAldea.instance.DesbloquearBloquearCamaraCombate();
    }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        if (isMouseDown && IsMouseOverObject())
        {
            RecogerListas();
        }
        isMouseDown = false;
    }

    private bool IsMouseOverObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }

    public void RecogerListas()
    {
        Debug.Log("RecogerLista");
        personajesPaLaDungeon.Clear();
        objetosPaLaDungeon.Clear();

        for (int i = 0; i < personajes.Count; i++)
        {
            if (personajes[i].transform.childCount > 0)
            {
                GameObject hijo = personajes[i].transform.GetChild(0).gameObject;
                personajesPaLaDungeon.Add(hijo);
            }
        }

        for (int i = 0; i < objetos.Count; i++)
        {
            if (objetos[i].transform.childCount > 0)
            {
                GameObject hijo = objetos[i].transform.GetChild(0).gameObject;
                objetosPaLaDungeon.Add(hijo);
            }
        }

        if (personajesPaLaDungeon.Count == 0)
        {
            Debug.Log("No puedes ir a la dungeon sin personajes.");
            return;
        }

        GenerarDungeon();
    }

    private void Start()
    {
        if (gameManagerObj==null) 
        {
        gameManagerObj= GameObject.Find("GameManager");
        }
        gameManaguer=gameManagerObj.GetComponent<GameManager>();
    }
    public List<GameObject> RecuperarPersonajes()
    {
        return personajesPaLaDungeon;
    }

    public List<GameObject> RecuperarObjetos()
    {
        return objetosPaLaDungeon;
    }

    public void GenerarDungeon()
    {
        Debug.Log("Número de personajes en la dungeon: " + personajesPaLaDungeon.Count);
        Debug.Log("Número de objetos en la dungeon: " + objetosPaLaDungeon.Count);

        gameManaguer.StopDayShowMap();
        CameraMovementAldea.instance.BloquearCamaraCombate();
        SceneManager.LoadScene("Mapa",LoadSceneMode.Additive);
        
        
    }
}
