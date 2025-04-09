using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    public static event Action<bool> OnEstadoCambiado; // Evento que pasa un bool
  


    public void Start()
    {
        AudioManager.instance.PlayMusic("Musica1");

    }
    public void NuevaPartida()
    {
        SceneManager.LoadScene("AldeaNueva", LoadSceneMode.Single);
    }
    public void CargarPartida()
    {
        SceneManager.LoadScene("Aldea", LoadSceneMode.Single);
    }
   


    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Esto solo tiene efecto fuera del editor de Unity.
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ToggleFullscreen()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
            Debug.Log("Saliendo de pantalla completa.");
        }
        else
        {
            Screen.fullScreen = true;
            Debug.Log("Entrando en pantalla completa.");
        }
    }

}
