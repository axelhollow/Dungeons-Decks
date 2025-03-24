using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Minicarta : MonoBehaviour
{
    public string nombre;
    public int daño;
    public int coste;
    public Texture2D imagenCarta;



    //INFO UI
    public TextMeshPro textoVida;
    public TextMeshPro textoMana;


    //tamaño
    public Vector3 originalScale; // Guarda el tamaño original
    public float scaleFactor = 1.25f; // Factor de escala (25% más grande)


    void Start()
    {
        textoVida.text = daño.ToString();
        textoMana.text = coste.ToString();

        originalScale = transform.localScale;

        // Obtener el material del objeto 3D
        Renderer renderer = GetComponent<Renderer>();

        // Cambiar la textura en el material
        renderer.material.mainTexture = imagenCarta;
    }
    void OnMouseEnter()
    {
        transform.localScale = originalScale * scaleFactor; // Aumenta el tamaño
    }

    void OnMouseExit()
    {
        transform.localScale = originalScale; // Restaura el tamaño original
    }

}
