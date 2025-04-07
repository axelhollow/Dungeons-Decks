using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    public void Start()
    {
        //AudioManager.instance.PlayMusic("");
    }
    public void LoadGame()
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
