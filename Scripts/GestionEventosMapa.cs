using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionEventosMapa : MonoBehaviour
{
    public bool boss;
    public bool tp;
    public GameObject caminoIzqu;
    public GameObject caminoDerech;
    public TipoEvento tipoEvento;
    public Texture2D DiseñoCombate;
    public Texture2D DiseñoTienda;
    public Texture2D DiseñoAldeano;
    public Texture2D DiseñoRecursos;
    public Dictionary<string, Texture2D> DiccionarioCartas;
    


 

    // Start is called before the first frame update
    void Start()
    {
      
        int nR = Random.Range(0, 4);

        if (nR == 0) 
        {
            tipoEvento = TipoEvento.Combate;
        }
        if (nR == 1)
        {
            tipoEvento = TipoEvento.Tienda;
        }
        if (nR == 2)
        {
            tipoEvento = TipoEvento.Aldeanos;
        }
        if (nR == 3)
        {
            tipoEvento = TipoEvento.Recursos;
        }

        DiccionarioCartas = new() { { "Combate", DiseñoCombate }, { "Tienda", DiseñoCombate }, { "Aldeanos", DiseñoCombate }, { "Recursos", DiseñoCombate } };


        print(tipoEvento);
        // Obtener el material del objeto 3D
        Renderer renderer = GetComponent<Renderer>();

        // Cambiar la textura en el material
        renderer.material.mainTexture = DiccionarioCartas[tipoEvento.ToString()];
    }

}
public enum TipoEvento
{
    Combate,
    Recursos,
    Aldeanos,
    Tienda
}


