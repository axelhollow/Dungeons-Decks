using System;
using System.Collections.Generic;
using UnityEngine;

public class TableroManager : MonoBehaviour
{
    public static TableroManager Instance { get; private set; }

    //Mazos
    public List<Carta> mazo;
    private List<Carta> mazoPersonajes;
    private List<Carta> mazotilizables;

    //Grid Personajes
    public Transform[] gridPersonajes;


    //Grid Items
    public Transform[] gridItems;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//opcional ya veremos si lo necesitamos
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //Clasificamos las cartas que nos pasan
        if (mazo != null)
        {
            foreach (Carta carta in mazo)
            {
                if (carta.tipo == TipoCarta.Personaje)
                {
                    print(carta.name);
                    mazoPersonajes.Add(carta);
                }
                if (carta.tipo == TipoCarta.Item)
                {
                    mazotilizables.Add(carta);
                }
            }
        }
        int n = 0;
        //Metemos las cartas de persoanje en su grid
        foreach (CartaPersonaje carta in mazoPersonajes)
        {
            Vector3 posicionCarta = gridPersonajes[n].transform.position;
            carta.transform.position = posicionCarta;


            n++;
        }


        //Metemos las cartas de iteam en su grid
        foreach (CartaPersonaje carta in mazotilizables)
        {
            Vector3 posicionCarta = gridItems[n].transform.position;
            carta.transform.position = posicionCarta;


            n++;
        }
    }

    public void SetMazo(List<Carta> mazo_Cartas)
    {
        mazo = mazo_Cartas;
    }
}
