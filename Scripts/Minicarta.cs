using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Minicarta : MonoBehaviour
{
    public string nombre;
    public int damage;
    public int damageAux;
    public int coste;
    public Texture2D imagenCarta;



    //INFO UI
    public TextMeshPro textoDamage;
    public TextMeshPro textoMana;


    //tamaño
    public Vector3 originalScale; // Guarda el tamaño original
    public float scaleFactor = 1.25f; // Factor de escala (25% más grande)


    void Start()
    {
        damage = damageAux;
        textoDamage.text = damage.ToString();
        textoMana.text = coste.ToString();

        originalScale = transform.localScale;

        //// Obtener el material del objeto 3D
        //Renderer renderer = GetComponent<Renderer>();

        //// Cambiar la textura en el material
        //renderer.material.mainTexture = imagenCarta;
    }

    public void AumentarDamage(int damaguito) 
    {
        damage += damaguito;
        textoDamage.text = damage.ToString();

    }
    public void RestaurarDamage() 
    {
        damage = damageAux;
        textoDamage.text = damage.ToString();
    }
    void OnMouseEnter()
    {
        transform.localScale = originalScale * scaleFactor; // Aumenta el tamaño
    }

    void OnMouseExit()
    {
        transform.localScale = originalScale; // Restaura el tamaño original
    }

}
