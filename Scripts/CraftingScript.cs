using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingScript : MonoBehaviour
{
    public List<int> cartasIDs = new List<int>();
    public GameObject Carta;
    public string prefabNamePrueba = "CubePrueba";
    private List<int> lastCartasIDs = new List<int>();
    private Coroutine craftingCoroutine;

    public GameObject fondoBarraPrefab;
    public GameObject barraProgresoPrefab;
    private GameObject fondoBarra;
    private GameObject barraProgreso;
    private float tiempoTotal = 5f;

    void Update()
    {
        List<int> currentCartasIDs = ObtenerIDsCartas();
        List<int> combinacionCorrecta = new List<int> { 2, 10 };
        combinacionCorrecta.Sort();

        if (!currentCartasIDs.SequenceEqual(lastCartasIDs))
        {
            if (craftingCoroutine != null)
            {
                StopCoroutine(craftingCoroutine);
                craftingCoroutine = null;
                ResetearBarra();
            }
        }
        else if (craftingCoroutine == null && currentCartasIDs.SequenceEqual(combinacionCorrecta))
        {
            CrearBarraProgreso();
            craftingCoroutine = StartCoroutine(CraftDespuesDeTiempo(tiempoTotal));
        }

        lastCartasIDs = new List<int>(currentCartasIDs);
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

    private IEnumerator CraftDespuesDeTiempo(float tiempo)
    {
        float tiempoRestante = tiempo;

        while (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarBarraProgreso(tiempoRestante / tiempoTotal);
            yield return null;
        }

        List<int> currentCartasIDs = ObtenerIDsCartas();
        List<int> combinacionCorrecta = new List<int> { 2, 10 };
        combinacionCorrecta.Sort();

        if (currentCartasIDs.SequenceEqual(combinacionCorrecta))
        {
            Vector3 posicionOriginal = transform.position;
            Quaternion rotacionOriginal = transform.rotation;
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabNamePrueba);

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
