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


