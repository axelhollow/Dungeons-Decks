using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CartaPersonajeData
{
    public int vida;
    public int vidaAux;
    public bool vidaMax;
    public int mana;
    public int manaAux;
    public bool mazoYaGenerado;
    public Dictionary<string, bool> manoActual;
    public string efectoAtaque;
    public string ataque1;
    public string ataque2;
    public string ataque3;
}
