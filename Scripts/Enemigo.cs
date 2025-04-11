using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Texture2D imagenCarta;
    public int att;
    public int vida;
    public int vidaAux;
    public string nombre;


    //INFO UI
   public TextMeshPro textoVida;
    public TextMeshPro textoAtt;
    //Efectos
    public GameObject efectoAtaque;

    //tamaño
    public Vector3 originalScale; // Guarda el tamaño original
    public float scaleFactor = 1.25f; // Factor de escala (25% más grande)

    void Start()
    {
        originalScale= transform.localScale;
        vida = vidaAux;
        textoVida.text = vida.ToString();
        textoAtt.text = att.ToString();
        //// Obtener el material del objeto 3D
        //Renderer renderer = GetComponent<Renderer>();

        //// Cambiar la textura en el material
        //renderer.material.mainTexture = imagenCarta;


    }

    public void RestarVida(int daño) 
    {
        vida = vida - daño;
        if(vida<0)vida=0;
        textoVida.text = vida.ToString();
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
