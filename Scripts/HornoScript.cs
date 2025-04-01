using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HornoScript : MonoBehaviour
{
    public List<int> cartasIDs = new List<int>();
    

    private List<int> lastCartasIDs = new List<int>();
    private Coroutine craftingCoroutine;

    public GameObject fondoBarraPrefab;
    public GameObject barraProgresoPrefab;
    private GameObject fondoBarra;
    private GameObject barraProgreso;

    private List<string> prefabsNames = new List<string> { "Hierro", "ManzanaAsada", "Botella" };
    private List<float> tiemposDeCrafting = new List<float> { 15f, 8f, 12f };
    private List<List<int>> combinacionesCorrectas = new List<List<int>>
    {
        new List<int> { 6 },//1 MineralHierro -> 1 Hierro
        new List<int> { 5 },//1 Manzana -> 1 ManzanaAsada
        new List<int> { 12, 12 },//2 Cristal -> 1 Botella
    };

    void Update()
    {

        // Seguir detectando combinaciones y cambios en las cartas
        List<int> currentCartasIDs = ObtenerIDsCartas();
        currentCartasIDs.Sort();

        if (!currentCartasIDs.SequenceEqual(lastCartasIDs))
        {
            if (craftingCoroutine != null)
            {
                StopCoroutine(craftingCoroutine);
                craftingCoroutine = null;
                ResetearBarra();
            }
        }
        else if (craftingCoroutine == null && ObtenerPrefabIndex(currentCartasIDs) != -1)
        {
            int prefabIndex = ObtenerPrefabIndex(currentCartasIDs);
            float tiempoCrafting = tiemposDeCrafting[prefabIndex];
            CrearBarraProgreso();
            craftingCoroutine = StartCoroutine(CraftDespuesDeTiempo(tiempoCrafting, prefabIndex));
        }

        lastCartasIDs = new List<int>(currentCartasIDs);
    }

    private IEnumerator CraftDespuesDeTiempo(float tiempo, int prefabIndex)
    {
        float tiempoRestante = tiempo;

        while (tiempoRestante > 0)
        {
            while (GameManager.gameSpeed == 0f) // Si está en pausa, espera
            {
                yield return null;
            }

            tiempoRestante -= Time.deltaTime * GameManager.gameSpeed;
            ActualizarBarraProgreso(tiempoRestante / tiempo);
            yield return null;
        }

        // Creación del objeto
        if (prefabIndex != -1 && prefabIndex < prefabsNames.Count)
        {
            Vector3 posicionOriginal = transform.position;
            Quaternion rotacionOriginal = transform.rotation;
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabsNames[prefabIndex]);

            if (prefab != null)
            {
                Instantiate(prefab, posicionOriginal, rotacionOriginal);
                foreach (Transform hijo in transform)
                {
                    Destroy(hijo.gameObject);
                }
            }
        }

        ResetearBarra();
        craftingCoroutine = null;
    }

    private List<int> ObtenerIDsCartas()
    {
        List<int> ids = new List<int>();
        RecorrerHijos(transform, ids);
        ids.Sort();
        return ids;
    }

    private void RecorrerHijos(Transform objeto, List<int> ids)
    {
        foreach (Transform hijo in objeto)
        {
            Carta carta = hijo.GetComponent<Carta>();
            if (carta != null)
            {
                ids.Add(carta.id);
            }
            RecorrerHijos(hijo, ids);
        }
    }

    private int ObtenerPrefabIndex(List<int> ids)
    {
        for (int i = 0; i < combinacionesCorrectas.Count; i++)
        {
            if (ids.SequenceEqual(combinacionesCorrectas[i]))
            {
                return i;
            }
        }
        return -1;
    }

    private void CrearBarraProgreso()
    {
        if (fondoBarra == null)
        {
            fondoBarra = Instantiate(fondoBarraPrefab, transform.position + new Vector3(0, 1, 17), Quaternion.identity, transform);
        }

        if (barraProgreso == null)
        {
            barraProgreso = Instantiate(barraProgresoPrefab, transform.position + new Vector3(0, 20, 17), Quaternion.identity, fondoBarra.transform);
            barraProgreso.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void ActualizarBarraProgreso(float porcentaje)
    {
        if (barraProgreso != null)
        {
            barraProgreso.transform.localScale = new Vector3(porcentaje, 1, 1);
        }
    }

    private void ResetearBarra()
    {
        if (barraProgreso != null)
        {
            Destroy(barraProgreso);
            barraProgreso = null;
        }
        if (fondoBarra != null)
        {
            Destroy(fondoBarra);
            fondoBarra = null;
        }
    }
}
