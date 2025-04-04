using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertederoScript : MonoBehaviour
{
    // Se ejecuta cada vez que se modifica la jerarqu�a de hijos
    void OnTransformChildrenChanged()
    {
        // Obtener todas las cartas entre los hijos (incluyendo al propio objeto si tuviera)
        Carta[] cartas = GetComponentsInChildren<Carta>();

        // Iterar sobre todas las cartas encontradas
        foreach (Carta carta in cartas)
        {
            // Evitar eliminar el objeto en el que est� este script (por si tuviera tambi�n componente Carta)
            if (carta.gameObject != this.gameObject)
            {
                Destroy(carta.gameObject);
            }
        }
    }
}
