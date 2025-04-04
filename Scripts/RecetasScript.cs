using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecetasScript : MonoBehaviour
{
    public bool agua;
    public List<GameObject> imagenesAgua;

    public bool aldeano;
    public List<GameObject> imagenesAldeano;

    public bool asesino;
    public List<GameObject> imagenesAsesino;

    public bool barracon;
    public List<GameObject> imagenesBarracon;

    public bool baston;
    public List<GameObject> imagenesBaston;

    public bool botella;
    public List<GameObject> imagenesBotella;

    public bool caldero;
    public List<GameObject> imagenesCaldero;

    public bool cristal;
    public List<GameObject> imagenesCristal;

    public bool daga;
    public List<GameObject> imagenesDaga;

    public bool espada;
    public List<GameObject> imagenesEspada;

    public bool guerrero;
    public List<GameObject> imagenesGuerrero;

    public bool hierro;
    public List<GameObject> imagenesHierro;

    public bool horno;
    public List<GameObject> imagenesHorno;

    public bool invernadero;
    public List<GameObject> imagenesInvernadero;

    public bool ladrillo;
    public List<GameObject> imagenesLadrillo;

    public bool mago;
    public List<GameObject> imagenesMago;

    public bool manzana;
    public List<GameObject> imagenesManzana;

    public bool manzanaAsada;
    public List<GameObject> imagenesManzanaAsada;

    public bool mesacrafteo;
    public List<GameObject> imagenesMesaCrafteo;

    public bool mineralhierro;
    public List<GameObject> imagenesMineralHierro;

    public bool palo;
    public List<GameObject> imagenesPalo;

    public bool piedra;
    public List<GameObject> imagenesPiedra;

    public bool pociondano;
    public List<GameObject> imagenesPocionDano;

    public bool pocionmana;
    public List<GameObject> imagenesPocionMana;

    public bool pocionvida;
    public List<GameObject> imagenesPocionVida;

    public bool polvomagico;
    public List<GameObject> imagenesPolvoMagico;

    public bool pozo;
    public List<GameObject> imagenesPozo;

    public bool tablones;
    public List<GameObject> imagenesTablones;

    public bool tronco;
    public List<GameObject> imagenesTronco;

    public bool vertedero;
    public List<GameObject> imagenesVertedero;

    public static RecetasScript instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(DesactivarImagenesCada3Segundos());
    }

    IEnumerator DesactivarImagenesCada3Segundos()
    {
        while (true)
        {
            // Ejecutamos el desactivado de las imágenes cada 3 segundos
            if (agua) SetImagenesInactive(imagenesAgua);
            if (aldeano) SetImagenesInactive(imagenesAldeano);
            if (asesino) SetImagenesInactive(imagenesAsesino);
            if (barracon) SetImagenesInactive(imagenesBarracon);
            if (baston) SetImagenesInactive(imagenesBaston);
            if (botella) SetImagenesInactive(imagenesBotella);
            if (caldero) SetImagenesInactive(imagenesCaldero);
            if (cristal) SetImagenesInactive(imagenesCristal);
            if (daga) SetImagenesInactive(imagenesDaga);
            if (espada) SetImagenesInactive(imagenesEspada);
            if (guerrero) SetImagenesInactive(imagenesGuerrero);
            if (hierro) SetImagenesInactive(imagenesHierro);
            if (horno) SetImagenesInactive(imagenesHorno);
            if (invernadero) SetImagenesInactive(imagenesInvernadero);
            if (ladrillo) SetImagenesInactive(imagenesLadrillo);
            if (mago) SetImagenesInactive(imagenesMago);
            if (manzana) SetImagenesInactive(imagenesManzana);
            if (manzanaAsada) SetImagenesInactive(imagenesManzanaAsada);
            if (mesacrafteo) SetImagenesInactive(imagenesMesaCrafteo);
            if (mineralhierro) SetImagenesInactive(imagenesMineralHierro);
            if (palo) SetImagenesInactive(imagenesPalo);
            if (piedra) SetImagenesInactive(imagenesPiedra);
            if (pociondano) SetImagenesInactive(imagenesPocionDano);
            if (pocionmana) SetImagenesInactive(imagenesPocionMana);
            if (pocionvida) SetImagenesInactive(imagenesPocionVida);
            if (polvomagico) SetImagenesInactive(imagenesPolvoMagico);
            if (pozo) SetImagenesInactive(imagenesPozo);
            if (tablones) SetImagenesInactive(imagenesTablones);
            if (tronco) SetImagenesInactive(imagenesTronco);

            yield return new WaitForSeconds(3f);
        }
    }


    void SetImagenesInactive(List<GameObject> imagenes)
    {
        foreach (GameObject imagen in imagenes)
        {
            imagen.SetActive(false);
        }
    }
}
