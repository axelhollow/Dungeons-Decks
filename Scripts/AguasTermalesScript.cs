using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AguasTermalesScript : MonoBehaviour
{
    public GameObject fondoBarraPrefab;
    public GameObject barraProgresoPrefab;

    private GameObject fondoBarra;
    private GameObject barraProgreso;

    public float tiempoCuracion = 5f;

    private Coroutine curacionCoroutine;
    private CartaPersonaje cartaCandidata;

    void Update()
    {
        List<CartaPersonaje> cartasPersonaje = ObtenerCartasPersonaje();
        List<int> ids = ObtenerIDsCartas();

        // Solo permitir si hay una sola carta con id 99 y vida incompleta
        if (ids.Count == 1 && ids[0] == 99 && cartasPersonaje.Count == 1 && cartasPersonaje[0].vidaMax==false)
        {
            if (curacionCoroutine == null)
            {
                cartaCandidata = cartasPersonaje[0];
                IniciarCuracion();
                
            }
        }
        else
        {
            if (curacionCoroutine != null)
            {
                StopCoroutine(curacionCoroutine);
                curacionCoroutine = null;
                ResetearBarra();
            }
        }
    }

    private void IniciarCuracion()
    {
        if (curacionCoroutine != null)
        {
            StopCoroutine(curacionCoroutine);
            ResetearBarra();
        }

        CrearBarraProgreso();
        curacionCoroutine = StartCoroutine(CurarDespuesDeTiempo(tiempoCuracion));
    }

    private IEnumerator CurarDespuesDeTiempo(float tiempo)
    {
        float tiempoRestante = tiempo;

        while (tiempoRestante > 0)
        {
            if (cartaCandidata == null || cartaCandidata.vida > cartaCandidata.vidaAux)
            {
                ResetearBarra();
                curacionCoroutine = null;
                yield break;
            }

            tiempoRestante -= Time.deltaTime;
            ActualizarBarraProgreso(tiempoRestante / tiempo);
            yield return null;
        }

        if (cartaCandidata != null && cartaCandidata.vida <= cartaCandidata.vidaAux)
        {
            cartaCandidata.AumentarVida(cartaCandidata.vidaAux + 3);
            cartaCandidata.vidaMax = true;
        }

        ResetearBarra();
        curacionCoroutine = null;
    }

    private List<int> ObtenerIDsCartas()
    {
        List<int> ids = new List<int>();
        RecorrerHijosParaIDs(transform, ids);
        return ids;
    }

    private void RecorrerHijosParaIDs(Transform objeto, List<int> ids)
    {
        foreach (Transform hijo in objeto)
        {
            Carta carta = hijo.GetComponent<Carta>();
            if (carta != null)
            {
                ids.Add(carta.id);
            }
            RecorrerHijosParaIDs(hijo, ids);
        }
    }

    private List<CartaPersonaje> ObtenerCartasPersonaje()
    {
        List<CartaPersonaje> cartas = new List<CartaPersonaje>();
        RecorrerHijosParaCartas(transform, cartas);
        return cartas;
    }

    private void RecorrerHijosParaCartas(Transform objeto, List<CartaPersonaje> cartas)
    {
        foreach (Transform hijo in objeto)
        {
            CartaPersonaje carta = hijo.GetComponent<CartaPersonaje>();
            if (carta != null)
            {
                cartas.Add(carta);
            }
            RecorrerHijosParaCartas(hijo, cartas);
        }
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
