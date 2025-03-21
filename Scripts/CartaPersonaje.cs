using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartaPersonaje : Carta
{

    public int vida;
    public int mana;

    public int manaAux;

    public bool mazoYaGenerado=false;
    public Dictionary<GameObject, bool> manoActual;


    public GameObject ataque1;
    public GameObject ataque2;
    public GameObject ataque3;

    public Texture2D imagenCarta;

    void Start()
    {
        manaAux = mana;

        // Obtener el material del objeto 3D
        Renderer renderer = GetComponent<Renderer>();

        // Cambiar la textura en el material
        renderer.material.mainTexture = imagenCarta;
    }

    private void Update()
    {

    }

}



