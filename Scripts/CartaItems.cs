using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaItems : Carta
{
    public bool aldea;
    public TipoPocion tipoPocion;
    public int cantidadEfecto;
    private bool usada;
    //tama�o
    public Vector3 originalScale; // Guarda el tama�o original
    private float scaleFactor = 2.5f; // Factor de escala (75% m�s grande)
    void Start()
    {
        originalScale = transform.localScale;
    }
    void OnMouseEnter()
    {
        if (aldea != true)
        {
            transform.localScale = originalScale * scaleFactor; // Aumenta el tama�o
        }
    }

    void OnMouseExit()
    {
        if (aldea != true)
        {
            transform.localScale = originalScale; // Restaura el tama�o original
        }
    }
}
public enum TipoPocion
{
    vida,
    mana,
    ataque
}

