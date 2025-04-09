using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public CameraMovementAldea cameraMovementScript;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Si quieres que este también persista
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (cameraMovementScript == null)
        {
            cameraMovementScript = FindObjectOfType<CameraMovementAldea>();
        }

        if (cameraMovementScript != null)
        {
            if (scene.name == "Inicio")
            {
                cameraMovementScript.enabled = false;
            }
            else
            {
                cameraMovementScript.enabled = true;
            }
        }
    }
}
