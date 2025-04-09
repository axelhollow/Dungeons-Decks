using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuESC : MonoBehaviour
{
    private void Start()
    {
        GameObject saveSystem = new GameObject();

        if (SaveSystem.Instancia == null)
        {
            saveSystem.AddComponent<SaveSystem>();
        }
    }
    public void CargarYCerrarEscena()
    {

        SceneManager.LoadScene("Inicio");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

    }


   
    public void GuardarPartida()
    {

       SaveSystem.Instancia.GuardarTodas();
    }
    public void CargarPartida()
    {

        SaveSystem.Instancia.CargarTodas();
    }
}
