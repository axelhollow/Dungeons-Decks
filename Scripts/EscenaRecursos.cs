using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EscenaRecursos : MonoBehaviour
{
    public List<GameObject> listaRecursos;

    //public Button eleccion1;
    //public Button eleccion2;
    //public Button eleccion3;

    //void Start()
    //{
    //    int n=Random.Range(0, 7);

    //    // Obtiene el material del objeto
    //    Material material = listaRecursos[n].GetComponent<Renderer>().material;

    //    if (material != null && material.mainTexture != null)
    //    {
    //        // Convierte la textura en un Sprite y la asigna al Image del botón
    //        Texture2D textura = (Texture2D)material.mainTexture;
    //        boton.image.sprite = Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), new Vector2(0.5f, 0.5f));
    //    }

    //    eleccion1.GetComponent<Image>().sprite = listaRecursos[n].GetComponent<Image>().sprite;
    //    n = Random.Range(0, 7);
    //    eleccion2.GetComponent<Image>().sprite = listaRecursos[n].GetComponent<Image>().sprite;
    //    n = Random.Range(0, 7);
    //    eleccion3.GetComponent<Image>().sprite = listaRecursos[n].GetComponent<Image>().sprite;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
