using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscenaAldeanos : MonoBehaviour
{
    public GameObject aldeano;
    public Button botonEleccion;
    public Color dorado = new Color(1f, 0.84f, 0f); // Dorado
    // Start is called before the first frame update

    private void Start()
    {
        GameObject obj = GameObject.Find("MapaScene");
        if (obj != null)
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        GameObject mazoObject = new GameObject();

        if (MazoActual.Instancia == null)
        {
            mazoObject.AddComponent<MazoActual>();
        }

    }

    public void CambiarColroBoton()
    {
        ColorBlock coloresAntiguos = botonEleccion.colors;

        coloresAntiguos.selectedColor = dorado;
        botonEleccion.colors = coloresAntiguos;
    }

    public void botonAceptar()
    {
        GameObject copiasrecurso = Instantiate(aldeano);
        MazoActual.Instancia.listaRecursos.Add(copiasrecurso);
        
        SceneManager.UnloadSceneAsync("AldeanosEscene");
        GameObject obj = GameObject.Find("MapaScene");
        if (obj != null)
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(true);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
