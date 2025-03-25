using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackCardsSelection : MonoBehaviour
{
    public Canvas canvas;
    public List<GameObject> cartasGuerrero;
    public List<Button> botones;
    public List<TMP_Text> textos;
    public Button clearButton;

    CartaPersonaje carta;

    public bool ataque1select = false;
    public bool ataque2select = false;
    public bool ataque3select = false;


    void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas no asignado en el inspector.");
        }

        clearButton.onClick.AddListener(ResetSeleccion);
    }
    

    void OnTransformChildrenChanged()
    {
        bool foundPersonaje = false;
        carta = null;
        ataque1select = false;
        ataque2select = false;
        ataque3select = false;

        foreach (TMP_Text text in textos)
        {
            text.text = "0";
        }

        foreach (Transform child in transform)
        {
            carta = child.GetComponent<CartaPersonaje>();
            if (carta != null && carta.tipo == TipoCarta.Personaje)
            {
                foundPersonaje = true;
                break;
            }
        }

        foreach (var boton in botones)
        {
            if (boton != null)
            {
                // Reinicia el ColorBlock al estado por defecto
                ColorBlock colors = boton.colors;
                colors.normalColor = Color.white; // O el color original que desees
                boton.colors = colors;

                // Además, actualiza el color de la imagen para reflejar el cambio inmediatamente
                if (boton.image != null)
                {
                    boton.image.color = Color.white;
                }
            }
        }

        if (foundPersonaje && canvas != null)
        {
            canvas.gameObject.SetActive(true);
            CargarCartas(carta);
        }
        else if (!foundPersonaje && canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }


    }

    public void CargarCartas(CartaPersonaje cartaPersonaje)
    {
        for (int i = 0; i < botones.Count && i < cartasGuerrero.Count; i++)
        {
            GameObject cartaObj = cartasGuerrero[i];
            Minicarta minicarta = cartaObj.GetComponent<Minicarta>();
            if (minicarta != null)
            {
                Image imageComponent = botones[i].GetComponentInChildren<Image>();
                Texture2D cartaTexture = minicarta.imagenCarta;

                if (minicarta.name == cartaPersonaje.ataque1.name)
                {
                    ColorBlock colors = botones[i].colors;
                    colors.normalColor *= 0.8f; // Reduce el brillo en un 20% para oscurecerlo
                    botones[i].colors = colors;
                    ataque1select = true;
                    textos[i].text = (int.Parse(textos[i].text) + 1).ToString();
                }
                if (minicarta.name == cartaPersonaje.ataque2.name)
                {
                    ColorBlock colors = botones[i].colors;
                    colors.normalColor *= 0.8f; // Reduce el brillo en un 20% para oscurecerlo
                    botones[i].colors = colors;
                    ataque2select = true;
                    textos[i].text = (int.Parse(textos[i].text) + 1).ToString();
                }
                if (minicarta.name == cartaPersonaje.ataque3.name)
                {
                    ColorBlock colors = botones[i].colors;
                    colors.normalColor *= 0.8f; // Reduce el brillo en un 20% para oscurecerlo
                    botones[i].colors = colors;
                    ataque3select = true;
                    textos[i].text = (int.Parse(textos[i].text) + 1).ToString();
                }


                if (cartaTexture != null && imageComponent != null)
                {
                    imageComponent.sprite = Sprite.Create(cartaTexture, new Rect(0, 0, cartaTexture.width, cartaTexture.height), new Vector2(0.5f, 0.5f));
                }



            }
        }
    }

    public void ResetSeleccion()
    {
        ataque1select = false;
        ataque2select = false;
        ataque3select = false;

        foreach(TMP_Text text in textos)
        {
            text.text = "0";
        }

        foreach (var boton in botones)
        {
            if (boton != null)
            {
                // Reinicia el ColorBlock al estado por defecto
                ColorBlock colors = boton.colors;
                colors.normalColor = Color.white; // O el color original que desees
                boton.colors = colors;

                // Además, actualiza el color de la imagen para reflejar el cambio inmediatamente
                if (boton.image != null)
                {
                    boton.image.color = Color.white;
                }
            }
        }

        // Forzar la actualización de la UI para que los cambios se reflejen de inmediato
        Canvas.ForceUpdateCanvases();
    }

    public void SeleccionarAtaque(Button botonPulsado)
    {
        int index = botones.IndexOf(botonPulsado); // Obtener el índice del botón pulsado
        if (index == -1) return; // Si el botón no está en la lista, salir

        // Asigna el ataque en el primer espacio disponible
        if (!ataque1select)
        {
            ataque1select = true;
            carta.ataque1 = cartasGuerrero[index];
            textos[index].text = (int.Parse(textos[index].text) + 1).ToString();
        }
        else if (!ataque2select)
        {
            ataque2select = true;
            carta.ataque2 = cartasGuerrero[index];
            textos[index].text = (int.Parse(textos[index].text) + 1).ToString();
        }
        else if (!ataque3select)
        {
            ataque3select = true;
            carta.ataque3 = cartasGuerrero[index];
            textos[index].text = (int.Parse(textos[index].text) + 1).ToString();
        }
        else
        {
            return; // Si todos los ataques están seleccionados, salir
        }

        // Modifica el ColorBlock para las transiciones futuras
        ColorBlock colors = botonPulsado.colors;
        Color nuevoColor = new Color(
            colors.normalColor.r * 0.8f,
            colors.normalColor.g * 0.8f,
            colors.normalColor.b * 0.8f,
            colors.normalColor.a
        );
        colors.normalColor = nuevoColor;
        botonPulsado.colors = colors;

        // Actualiza directamente el color de la imagen para reflejar el cambio inmediato
        if (botonPulsado.image != null)
        {
            botonPulsado.image.color = nuevoColor;
        }

        // Opcional: Forzar actualización de la UI
        Canvas.ForceUpdateCanvases();
    }
}
