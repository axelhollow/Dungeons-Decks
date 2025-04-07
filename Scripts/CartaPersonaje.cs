using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartaPersonaje : Carta
{

    public int vida;
    public int vidaAux;
    public bool vidaMax=false;
    public int mana;
    public GameObject efectoAtaque;
    public int manaAux;
    public bool mazoYaGenerado=false;
    public Dictionary<GameObject, bool> manoActual=new();
    public GameObject ataque1;
    public GameObject ataque2;
    public GameObject ataque3;
    public TextMeshPro textoVida;
    public TextMeshPro TextoMana;


    public void RestarVida(int attak)
    {
        print("rESTANDO VIDA A :" + gameObject.name);
        vida = vida- attak;
        if (vida < 0) vida = 0;
        textoVida.text = vida.ToString();
        print("daño: " + attak);
        print("vida: "+vida);
    }
    public void CurarVida(int cura)
    {
        vida += cura;
        if (vida > vidaAux) vida = vidaAux;
        textoVida.text = vida.ToString();
    }

    public void AumentarVida(int cura)
    {
        vidaAux = cura;
        textoVida.text = vidaAux.ToString();
    }
    public void DismunyuirVidaMax(int cura)
    {
        vidaAux = cura;
        textoVida.text = vidaAux.ToString();
    }


    public void RestarMana(int manaUsado)
    {
        mana -= manaUsado;
        TextoMana.text = vida.ToString();
    }
    public void SumarMana(int anadir)
    {
        mana += anadir;
        TextoMana.text = vida.ToString();
    }

   // aassas


    void Start()
    {
        
        vida = vidaAux;
        textoVida.text = vida.ToString();
        manaAux = mana;
        TextoMana.text = mana.ToString();
    }

}



