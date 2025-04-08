using System;
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
    public CartaPocionData ToData()
    {
        return new CartaPocionData
        {
            prefabName=this.name,
            aldea = this.aldea,
            tipoPocion = this.tipoPocion, 
            cantidadEfecto = this.cantidadEfecto,
            usada = this.usada,
            originalScale = this.originalScale,
            x=transform.position.x,
            y=transform.position.y,
            z=transform.position.z
        };
    }
    public void FromData(CartaPocionData data)
    {
        this.aldea = data.aldea;
        this.tipoPocion = data.tipoPocion;
        this.cantidadEfecto = data.cantidadEfecto;
        this.usada = data.usada;
        this.originalScale = data.originalScale;
        gameObject.transform.position = new Vector3(data.x, data.y, data.z);
    }
}
public enum TipoPocion
{
    vida,
    mana,
    ataque
}


