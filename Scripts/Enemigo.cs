using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Texture2D imagenCarta;
    public int da�o;
    public int vida;
    public int vidaAux;
    public string nombre;


    //INFO UI
   public TextMeshPro textoVida;
    //Efectos
    public GameObject efectoAtaque;

    //tama�o
    public Vector3 originalScale; // Guarda el tama�o original
    public float scaleFactor = 1.25f; // Factor de escala (25% m�s grande)

    void Start()
    {
        originalScale= transform.localScale;
        vida = vidaAux;
        textoVida.text = vida.ToString();
        // Obtener el material del objeto 3D
        Renderer renderer = GetComponent<Renderer>();

        // Cambiar la textura en el material
        renderer.material.mainTexture = imagenCarta;
    }

    public void RestarVida(int da�o) 
    {
        vida = vida - da�o;
        textoVida.text = vida.ToString();
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
