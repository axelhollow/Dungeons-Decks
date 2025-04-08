using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CartaPersonajeData
{
    public string prefabName;  // Nombre del prefab principal
    public int vida;
    public int vidaAux;
    public bool vidaMax;
    public int mana;
    public int manaAux;
    public bool mazoYaGenerado;
    public string efectoAtaque;  // Ruta del prefab para el efecto de ataque
    public string ataque1;       // Ruta del prefab para ataque 1
    public string ataque2;       // Ruta del prefab para ataque 2
    public string ataque3;       // Ruta del prefab para ataque 3
    public float x;
    public float y;
    public float z;
}
