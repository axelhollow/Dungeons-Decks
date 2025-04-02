using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaItems : Carta
{
    public bool aldea;
    public TipoPocion tipoPocion;
    public int cantidadEfecto;
    private bool usada;
    //tamaño
    public Vector3 originalScale; // Guarda el tamaño original
    private float scaleFactor = 2.5f; // Factor de escala (75% más grande)
    void Start()
    {
        originalScale = transform.localScale;
    }
    void OnMouseEnter()
    {
        if (aldea != true)
        {
            transform.localScale = originalScale * scaleFactor; // Aumenta el tamaño
        }
    }

    void OnMouseExit()
    {
        if (aldea != true)
        {
            transform.localScale = originalScale; // Restaura el tamaño original
        }
    }
}
public enum TipoPocion
{
    vida,
    mana,
    ataque
}

