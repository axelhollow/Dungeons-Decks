using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEditor.SearchService;
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



    public void CartasRecuperdasAventura(List<GameObject> listaCartas,List<GameObject> listaObjetos) 
    {
        GameObject mazoObject = new GameObject();

        if (MazoActual.Instancia == null)
        {
            mazoObject.AddComponent<MazoActual>();
        }

        print("Recuperamos " + listaCartas.Count + " aliados");
        float z = 4f;
        float y = 0.3f;
        if (listaCartas != null)
        {
            foreach (GameObject carta in listaCartas)
            {
                GameObject cartita = Instantiate(carta);
              

                cartita.GetComponent<CartaMovement>().holderDungeon = false;
                cartita.GetComponent<Renderer>().material.color = Color.white;
                cartita.transform.position = padreCartasRecuperadas.transform.position + new Vector3(0, y, -z);
                cartita.transform.localScale = padreCartasRecuperadas.transform.localScale;
                cartita.transform.SetParent(padreCartasRecuperadas.transform);
                cartita.SetActive(true);
                z += 4;
                y += 0.3f;
            }
        }
        if (listaObjetos != null)
        {
            foreach (GameObject carta in listaObjetos)
            {
                GameObject cartita = Instantiate(carta);
               
                if (cartita.tag == "Pocion")
                {
                    cartita.GetComponent<CartaItems>().aldea = true;

                }
                cartita.GetComponent<CartaMovement>().holderDungeon = false;
                cartita.GetComponent<Renderer>().material.color = Color.white;
                cartita.transform.position = padreCartasRecuperadas.transform.position + new Vector3(0, 0, -z);
                cartita.transform.localScale = padreCartasRecuperadas.transform.localScale;
                cartita.transform.SetParent(padreCartasRecuperadas.transform);
                cartita.SetActive(true);
                z += 4;
                y += 0.3f;
            }
        }
        if (MazoActual.Instancia.listaRecursos!=null) 
        {
            foreach (GameObject carta in MazoActual.Instancia.listaRecursos)
            {
                carta.GetComponent<CartaMovement>().holderDungeon = false;
                carta.GetComponent<Renderer>().material.color = Color.white;
                carta.transform.position = padreCartasRecuperadas.transform.position + new Vector3(0, 0, -z);
                carta.transform.localScale = padreCartasRecuperadas.transform.localScale;
                carta.transform.SetParent(padreCartasRecuperadas.transform);
                z += 4;
                y += 0.3f;
            }
        }
        
        

        GameManager.instance.ContinueDayShowMap();
        GameManager.instance.SeguirDay();
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

        GameManager.instance.StopDayShowMap();
        CameraMovementAldea.instance.BloquearCamaraCombate();
        SceneManager.LoadScene("Mapa",LoadSceneMode.Additive);
        
        
    }
}
