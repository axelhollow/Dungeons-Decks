using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartaPersonaje : Carta
{

    public int vida;
    public int vidaAux;
    public int mana;
    public bool vidaMax=false;
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
        vida = cura;
        vidaAux = cura;
        textoVida.text = vida.ToString();
    }

    public void DisminuirVidaMax(int disminucion)
    {
        vida -= disminucion;
        vidaAux -= disminucion;
        textoVida.text = vida.ToString();
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


    void Start()
    {
        vida = vidaAux;
        textoVida.text = vida.ToString();
        manaAux = mana;
        TextoMana.text = mana.ToString();
    }

    public CartaPersonajeData ToData()
    {
        return new CartaPersonajeData
        {
            prefabName = gameObject.name.Replace("(Clone)", "").Trim(),
            vida = this.vida,
            vidaAux = this.vidaAux,
            vidaMax = this.vidaMax,
            mana = this.mana,
            manaAux = this.manaAux,
            mazoYaGenerado = this.mazoYaGenerado,
            efectoAtaque = efectoAtaque?.name,
            ataque1 = ataque1?.name,
            ataque2 = ataque2?.name,
            ataque3 = ataque3?.name,

        };
    }
    public void LoadFromData(CartaPersonajeData data)
    {
        vida = data.vida;
        vidaAux = data.vidaAux;
        vidaMax = data.vidaMax;
        mana = data.mana;
        manaAux = data.manaAux;
        mazoYaGenerado = data.mazoYaGenerado;

        efectoAtaque = Resources.Load<GameObject>("Prefabs/EfectosAtaque/" + data.efectoAtaque);
        ataque1 = Resources.Load<GameObject>("Assets/Prefabs/Ataques/" + data.ataque1);
        print(data.ataque1);
        ataque2 = Resources.Load<GameObject>("Assets/Prefabs/Ataques/" + data.ataque2);
        ataque3 = Resources.Load<GameObject>("Assets/Prefabs/Ataques/" + data.ataque3);

      
    }

}



