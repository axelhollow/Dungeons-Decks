using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AguasTermalesScript : MonoBehaviour
{
    // Prefabs de la barra de progreso
    public GameObject fondoBarraPrefab;
    public GameObject barraProgresoPrefab;

    private GameObject fondoBarra;
    private GameObject barraProgreso;

    // Tiempo de curación (en segundos)
    public float tiempoCuracion = 5f;

    // Referencia a la carta de tipo Personaje detectada
    private CartaPersonaje cartaPersonajeReferencia;
    // Controla la coroutine de curación
    private Coroutine curacionCoroutine;

    void Update()
    {
        // Buscar la carta Personaje entre los hijos en cada frame
        CartaPersonaje cartaActual = GetComponentInChildren<CartaPersonaje>();

        // Si la carta actual es distinta a la anterior, hay un cambio (añadida o removida)
        if (cartaActual != cartaPersonajeReferencia)
        {
            // Si se quitó la carta y había una coroutine en curso, detenerla y reiniciar la barra
            if (cartaActual == null && curacionCoroutine != null)
            {
                StopCoroutine(curacionCoroutine);
                curacionCoroutine = null;
                ResetearBarra();
            }

            // Actualizar la referencia a la carta
            cartaPersonajeReferencia = cartaActual;

            // Si se ha colocado una carta Personaje y no está al 100% de vida, iniciar la curación
            if (cartaPersonajeReferencia != null && cartaPersonajeReferencia.vida <= cartaPersonajeReferencia.vidaAux)
            {
                IniciarCuracion();
            }
        }
    }

    private void IniciarCuracion()
    {
        // Si ya hay una curación en curso, detenerla
        if (curacionCoroutine != null)
        {
            StopCoroutine(curacionCoroutine);
            ResetearBarra();
        }

        // Si la vida ya está completa, no hacer nada
        if (cartaPersonajeReferencia.vida > cartaPersonajeReferencia.vidaAux)
            return;

        CrearBarraProgreso();
        curacionCoroutine = StartCoroutine(CurarDespuesDeTiempo(tiempoCuracion));
    }

    private IEnumerator CurarDespuesDeTiempo(float tiempo)
    {
        float tiempoRestante = tiempo;

        // Actualizar la barra de progreso cada frame
        while (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarBarraProgreso(tiempoRestante / tiempo);
            yield return null;
        }

        // Al completar el tiempo, aumenta la vida en 3
        if (cartaPersonajeReferencia != null)
        {
            cartaPersonajeReferencia.AumentarVida(cartaPersonajeReferencia.vidaAux+3);
        }

        ResetearBarra();
        curacionCoroutine = null;
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
            // La escala en X representa el porcentaje de la barra (1 = completa, 0 = vacía)
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
