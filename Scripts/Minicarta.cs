using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minicarta : MonoBehaviour
{
    public string nombre;
    public int daño;
    public int coste;
    public Texture2D imagenCarta;

    void Start()
    {
        // Obtener el material del objeto 3D
        Renderer renderer = GetComponent<Renderer>();

        // Cambiar la textura en el material
        renderer.material.mainTexture = imagenCarta;
    }
}
