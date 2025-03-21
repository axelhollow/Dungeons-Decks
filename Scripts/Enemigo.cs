using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Texture2D imagenCarta;
    public int vida;
    public int vidaAux;
    public string nombre;

    void Start()
    {
        vida = vidaAux;
        // Obtener el material del objeto 3D
        Renderer renderer = GetComponent<Renderer>();

        // Cambiar la textura en el material
        renderer.material.mainTexture = imagenCarta;
    }

    public void RestarVida(int daño) 
    {
        vida = vida - daño;
    }

}
