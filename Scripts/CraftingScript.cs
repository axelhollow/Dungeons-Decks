using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingScript : MonoBehaviour
{
    public List<int> cartasIDs = new List<int>();
    

    private List<int> lastCartasIDs = new List<int>();
    private Coroutine craftingCoroutine;

    public GameObject fondoBarraPrefab;
    public GameObject barraProgresoPrefab;
    private GameObject fondoBarra;
    private GameObject barraProgreso;

    private List<string> prefabsNames = new List<string> { "Palo", "Tablon", "Ladrillo", "Daga", "Espada", "Baston","CraftingTable", "Horno", "Pozo", "Caldero","Invernadero", "Barracon", "Vertedero","AguasTermales" };
    private List<float> tiemposDeCrafting = new List<float> { 5f, 6f, 10f, 10f, 10f, 15f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f,10f,10f,10f,10f,10f };
    private List<List<int>> combinacionesCorrectas = new List<List<int>>
    {
        new List<int> { 1 },//1 Tronco -> 1 palo
        new List<int> { 1, 1 },//2 Troncos -> 1 Tabla
        new List<int> { 3, 3 },//2 piedras -> 1 ladrillo
        new List<int> { 2,2,4,4 },//2Palos+2Lingotes -> 1 Daga
        new List<int> { 2,4,4 },//1Palos+2Lingotes -> 1 Espada
        new List<int> { 2,2,2,11,11 },//3 Palos+2 PolvosMagicos -> 1 Baston
        new List<int> { 1,1,2,2 },//2troncos + 2 palos -> 1 Mesa crafteo
        new List<int> { 1,1,3,3,3,3 },//2troncos + 4 piedras -> 1 Horno
        new List<int> { 8,8,8 },//3ladrillos -> 1 Pozo
        new List<int> { 4,4,4 },//3 hierro + 1 polvomagico -> 1 Caldero
        new List<int> { 5,10,11 },//1 agua+ 1 manzana+ 1 polvomagico -> 1 Invernadero
        new List<int> { 4,4,8,8,8 },//2 hierro + 3 ladrillo -> 1 Barracon
        new List<int> { 7,7,7,11 },//3 tablas + 1 polvo -> 1 Vertedero
        new List<int> { 3,3,3,3,10,10,11 },//4 pierdas + 2 aguas + 2 polvos -> 1 AguaTermal
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
                Instantiate(prefab, posicionOriginal + new Vector3(0f, 2f, -4f), rotacionOriginal);
                foreach (Transform hijo in transform)
                {
                    Destroy(hijo.gameObject);
                }

                switch (prefabIndex)
                {
                    case 0: RecetasScript.instance.palo = true; break;
                    case 1: RecetasScript.instance.tablones = true; break;
                    case 2: RecetasScript.instance.ladrillo = true; break;
                    case 3: RecetasScript.instance.daga = true; break;
                    case 4: RecetasScript.instance.espada = true; break;
                    case 5: RecetasScript.instance.baston = true; break;
                    case 6: RecetasScript.instance.mesacrafteo = true; break;
                    case 7: RecetasScript.instance.horno = true; break;
                    case 8: RecetasScript.instance.pozo = true; break;
                    case 9: RecetasScript.instance.caldero = true; break;
                    case 10: RecetasScript.instance.invernadero = true; break;
                    case 11: RecetasScript.instance.barracon = true; break;
                    case 12: RecetasScript.instance.vertedero = true; break;
                    case 13: RecetasScript.instance.aguasTermales = true; break;

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
