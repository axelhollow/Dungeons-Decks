using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Carta : MonoBehaviour
{

    public TipoCarta tipo;
    public string nombre;
    public int id;
    public Texture2D imagenCarta;

    public CartaRecursoYConstruccionData ToData()
    {
        return new CartaRecursoYConstruccionData
        {
            nombre = gameObject.name.Replace("(Clone)", "").Trim(),
            tipo = this.tipo,
            id = this.id,
            x = gameObject.transform.position.x,
            y = gameObject.transform.position.y,
            z = gameObject.transform.position.z,

        };
    }
    public void LoadFromData(CartaRecursoYConstruccionData data)
    {
        this.nombre = data.nombre.Replace("(Clone)", "").Trim(); // Para limpiar el nombre si es necesario
        this.tipo = data.tipo;
        this.id = data.id;

        // Asignamos la posición de la carta
        gameObject.transform.position = new Vector3(data.x, data.y, data.z);

    }

}
public enum TipoCarta
{
    Item,
    Personaje,
    Material,
    Comida,
    Edificio
}

