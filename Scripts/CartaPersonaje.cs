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
            manoActual = manoActual.ToDictionary(kvp => kvp.Key.name, kvp => kvp.Value)
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

        efectoAtaque = Resources.Load<GameObject>("Prefabs/" + data.efectoAtaque);
        ataque1 = Resources.Load<GameObject>("Prefabs/" + data.ataque1);
        ataque2 = Resources.Load<GameObject>("Prefabs/" + data.ataque2);
        ataque3 = Resources.Load<GameObject>("Prefabs/" + data.ataque3);

        manoActual = new Dictionary<GameObject, bool>();
        foreach (var kvp in data.manoActual)
        {
            var go = Resources.Load<GameObject>("Prefabs/" + kvp.Key);
            if (go != null) manoActual.Add(go, kvp.Value);
        }
    }

}



