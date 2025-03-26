using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartaPersonaje : Carta
{

    public int vida;
    public int vidaAux;
    public int mana;
    public ParticleSystem efectoAtaque;
    public int manaAux;
    public bool mazoYaGenerado=false;
    public Dictionary<GameObject, bool> manoActual=new();
    public GameObject ataque1;
    public GameObject ataque2;
    public GameObject ataque3;
    public TextMeshPro textoVida;


    public void RestarVida(int daño)
    {
        vida -= daño;
        textoVida.text = vida.ToString();
    }
    public void CurarVida(int cura)
    {
        vida += cura;
        if (vida > vidaAux) vida = vidaAux;
        textoVida.text = vida.ToString();
    }



    void Start()
    {
        vida = vidaAux;
        textoVida.text = vida.ToString();

        manaAux = mana;

        // Obtener el material del objeto 3D
        Renderer renderer = GetComponent<Renderer>();

        // Cambiar la textura en el material
        renderer.material.mainTexture = imagenCarta;
    }

}



