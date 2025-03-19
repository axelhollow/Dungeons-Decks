using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Carta : MonoBehaviour
{
    public TipoCarta tipo;
    public string nombre;

}
public enum TipoCarta
{
    Item,
    Personaje
}