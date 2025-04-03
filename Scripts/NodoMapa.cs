using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoMapa : MonoBehaviour
{
    public bool boss;
    public bool combate;
    public bool tp;
    public bool puntoPartida;
    public bool elejido = false;
    public GameObject caminoIzqu;
    public GameObject caminoDerech;
    public TipoEvento tipoEvento;
    public Material DisenoCombate;
    public Material DisenoTienda;
    public Material DisenoAldeano;
    public Material DisenoRecursos;
    public Material DisenoTP;
    public Material DisenoBoss;
    public Material DisenoX;
    public Material DisenoPuntoPartida;
    public Dictionary<string, Material> DiccionarioCartas;
    private Renderer rendererito;
    public GameObject selector;




    // Start is called before the first frame update
    void Start()
    {
        GameObject mazoObject = new GameObject();

        if (MazoActual.Instancia == null)
        {
            mazoObject.AddComponent<MazoActual>();
        }

        rendererito = GetComponent<Renderer>();

        selector.GetComponent<MeshRenderer>().enabled = false;

        int nR = Random.Range(0, 12);

        if (nR >=0 && nR<=5) 
        {
            tipoEvento = TipoEvento.Combate;
        }
        if (nR ==6 || nR==7)
        {
            tipoEvento = TipoEvento.Aldeanos;
        }
        if (nR>7)
        {
            tipoEvento = TipoEvento.Recursos;
        }

        DiccionarioCartas = new() { { "Combate", DisenoCombate }, { "Aldeanos", DisenoAldeano }, { "Recursos", DisenoRecursos }, { "Tp", DisenoTP }, { "Boss", DisenoBoss }, { "X", DisenoX },{"PuntoPartida",DisenoPuntoPartida } };


        // Cambiar la textura en el material
        GetComponent<Renderer>().material = DiccionarioCartas[tipoEvento.ToString()];
        if (puntoPartida)
        {
            GetComponent<Renderer>().material = DiccionarioCartas["PuntoPartida"];

        }
        if (combate) 
        {
            GetComponent<Renderer>().material = DiccionarioCartas["Combate"];
            tipoEvento = TipoEvento.Combate;
        }
        if (tp)
        {
            GetComponent<Renderer>().material = DiccionarioCartas["Tp"];
            tipoEvento = TipoEvento.Tp;

        }
        if (boss)
        {
            GetComponent<Renderer>().material = DiccionarioCartas["Boss"];
            tipoEvento = TipoEvento.Boss;
            MazoActual.Instancia.bossEvent = true;
        }
    }

}


public enum TipoEvento
{
    Combate,
    Recursos,
    Aldeanos,
    Tp,
    Boss
}


