using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static float gameSpeed = 1f; // Velocidad del juego
    public TMP_Text textoVelocidad;

    public Slider diaSlider;         // Asigna el Slider en el Inspector
    public TMP_Text diaTexto;        // Asigna el TextMeshPro para mostrar el día
    public float duration = 10f;     // Tiempo en segundos para vaciar el slider (100% -> 0%)

    private int currentDay = 1;
    private float currentPercentage = 100f;

    private bool endDay = false;
    public List<CartaPersonaje> cartasPersonaje = new List<CartaPersonaje>(); // Lista de cartas encontradas
    public List<Carta> cartasComida = new List<Carta>(); // Lista para cartas de comida

    public Canvas canvas;

    public static GameManager instance;
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

    private void Start()
    {
        textoVelocidad.text = "x1";
        diaTexto.text = "Día " + currentDay;
        diaSlider.maxValue = 100f;
        diaSlider.value = currentPercentage;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && endDay == false)
        {
            // Cambiar entre pausa, normal y velocidad x2
            if (gameSpeed == 1f)
            {
                gameSpeed = 2f;
                textoVelocidad.text = "x2";
            }
            else if (gameSpeed == 2f)
            {
                gameSpeed = 0f;
                textoVelocidad.text = "x0";
            }
            else if (gameSpeed == 0f)
            {
                gameSpeed = 1f;
                textoVelocidad.text = "x1";
            }
        }

        if (currentPercentage > 0)
        {
            if (GameManager.gameSpeed > 0f) // Si gameSpeed es mayor a 0, el tiempo avanza
            {
                currentPercentage -= (100f / duration) * Time.deltaTime * GameManager.gameSpeed;
                currentPercentage = Mathf.Max(currentPercentage, 0);
                diaSlider.value = currentPercentage;
            }
        }
        else
        {
            // Al llegar a 0, incrementa el día y reinicia el slider
            currentDay++;
            diaTexto.text = "Día " + currentDay;
            currentPercentage = 100f;
            diaSlider.value = currentPercentage;
            StopDay();
            if (currentDay > 1)
            {
                StartCoroutine("BuscarCartasPersonaje");
            }

        }

    }

    public void StopDay()
    {
        endDay = true;
        gameSpeed = 0f;
        textoVelocidad.text = "x0";
    }

    public void StopDayShowMap()
    {
        
        endDay = true;
        gameSpeed = 0f;
        textoVelocidad.text = "x0";
        canvas.gameObject.SetActive(false);
    }
    public void SeguirDay()
    {
        endDay = false;
        gameSpeed = 1f;
        textoVelocidad.text = "x1";
    }

    public void ContinueDayShowMap()
    {
        textoVelocidad.text = "x1";
        canvas.gameObject.SetActive(true);
    }



    IEnumerator BuscarCartasPersonaje()
    {
        yield return new WaitForSeconds(0.3f); // Espera un momento para que los objetos se inicialicen

        CartaPersonaje[] cartasEncontradas = FindObjectsOfType<CartaPersonaje>(); // Busca todas las cartas en la escena

        cartasPersonaje.Clear(); // Limpiar la lista antes de llenarla

        foreach (CartaPersonaje carta in cartasEncontradas)
        {
            cartasPersonaje.Add(carta);
        }
        Debug.Log("Se encontraron " + cartasPersonaje.Count + " cartas de personaje.");

        BuscarCartasComida();

        for (int i = 0; i < cartasPersonaje.Count; i++)
        {
            if (cartasComida.Count > 0) // Si hay comida disponible
            {
                // Desvincular la carta de comida antes de eliminarla
                Transform cartaComida = cartasComida[0].transform;
                cartaComida.SetParent(null);

                List<Transform> hijosCartaComida = new List<Transform>();
                foreach (Transform hijo in cartaComida)
                {
                    hijosCartaComida.Add(hijo);
                }

                foreach (Transform hijo in hijosCartaComida)
                {
                    hijo.SetParent(null);
                }

                Destroy(cartasComida[0].gameObject); // Destruye la carta de comida
                cartasComida.RemoveAt(0); // Elimina la comida de la lista
            }
            else // Si no hay comida, se destruye el personaje
            {
                // Desvincular todos los hijos que sean de la capa "Carta"
                List<Transform> hijosCarta = new List<Transform>();

                foreach (Transform hijo in cartasPersonaje[i].transform)
                {
                    if (hijo.gameObject.layer == LayerMask.NameToLayer("Carta"))
                    {
                        hijosCarta.Add(hijo);
                    }
                }

                // Desvincular los hijos encontrados
                foreach (Transform hijo in hijosCarta)
                {
                    hijo.SetParent(null);
                }

                // Ahora sí, destruir la carta del personaje
                Destroy(cartasPersonaje[i].gameObject);
                cartasPersonaje.RemoveAt(i);
                i--; // Ajustar el índice tras eliminar un elemento
            }
            yield return new WaitForSeconds(0.3f);
        }

        if (cartasPersonaje.Count == 0 && currentDay > 1)
        {
            StopDay();
            Debug.Log("HAS PERDIDO BOBO");
        }
        else
        {
            Debug.Log("Fin Del dia");
            SeguirDay();
        }
    }

    public void BuscarCartasComida()
    {
        Carta[] todasLasCartas = FindObjectsOfType<Carta>(); // Busca todas las cartas en la escena
        cartasComida.Clear(); // Limpia la lista antes de llenarla

        foreach (Carta carta in todasLasCartas)
        {
            if (carta.tipo == TipoCarta.Comida) // Filtra las cartas de tipo Comida
            {
                cartasComida.Add(carta);
            }
        }

        Debug.Log("Se encontraron " + cartasComida.Count + " cartas de comida.");
    }

}
