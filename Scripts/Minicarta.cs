using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Minicarta : MonoBehaviour
{
    public string nombre;
    public int da�o;
    public int coste;
    public Texture2D imagenCarta;



    //INFO UI
    public TextMeshPro textoVida;
    public TextMeshPro textoMana;


    //tama�o
    public Vector3 originalScale; // Guarda el tama�o original
    public float scaleFactor = 1.25f; // Factor de escala (25% m�s grande)


    void Start()
    {
        textoVida.text = da�o.ToString();
        textoMana.text = coste.ToString();

        originalScale = transform.localScale;

        // Obtener el material del objeto 3D
        Renderer renderer = GetComponent<Renderer>();

        // Cambiar la textura en el material
        renderer.material.mainTexture = imagenCarta;
    }
    void OnMouseEnter()
    {
        transform.localScale = originalScale * scaleFactor; // Aumenta el tama�o
    }

    void OnMouseExit()
    {
        transform.localScale = originalScale; // Restaura el tama�o original
    }

}
