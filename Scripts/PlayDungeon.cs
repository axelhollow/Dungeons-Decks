using System.Collections;
using System.Collections.Generic;
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
