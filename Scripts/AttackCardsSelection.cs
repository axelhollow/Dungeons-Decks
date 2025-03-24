using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardsSelection : MonoBehaviour
{
    public Canvas canvas;

    void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas no asignado en el inspector.");
        }
    }

    void OnTransformChildrenChanged()
    {
        bool foundPersonaje = false;

        foreach (Transform child in transform)
        {
            Carta carta = child.GetComponent<Carta>();
            if (carta != null && carta.tipo == TipoCarta.Personaje)
            {
                foundPersonaje = true;
                break;
            }
        }

        if (foundPersonaje && canvas != null)
        {
            canvas.gameObject.SetActive(true);
        }
        else if (!foundPersonaje && canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
