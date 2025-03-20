using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CartasCombinaciones : MonoBehaviour
{
    public List<int> cartasIDs = new List<int>(); // Lista para guardar los IDs

    public GameObject Carta;

    public void ComprobacionCartas()
    {

        
        // Verificar si este objeto no tiene padre (es un "ra�z")
        if (transform.parent == null)
        {
            cartasIDs.Clear(); // Limpiar la lista antes de llenarla nuevamente

            Carta carta = GetComponent<Carta>();
            if (carta != null) 
            {
                cartasIDs.Add(carta.id);
            }

            // Llamar a la funci�n recursiva para recorrer los hijos y sus hijos
            RecorrerHijos(transform);

            // Imprimir los IDs en la consola
            Debug.Log("IDs de todas las cartas hijas y subhijas: " + string.Join(", ", cartasIDs));
        }else if (transform.parent != null)
        {
            cartasIDs.Clear();
        }

        ComprobarCombinaciones();
    }

    private void RecorrerHijos(Transform objeto)
    {
        foreach (Transform hijo in objeto)
        {
            // Obtener el componente "Carta"
            Carta carta = hijo.GetComponent<Carta>();

            if (carta != null) // Si el objeto tiene el script "Carta"
            {
                cartasIDs.Add(carta.id); // Guardar el ID en la lista
            }

            // Llamada recursiva para explorar los hijos del hijo
            RecorrerHijos(hijo);
        }
    }

    private void ComprobarCombinaciones()
    {
        // Lista de la combinaci�n exacta que queremos verificar
        List<int> combinacionCorrecta = new List<int> { 1, 1, 1, 2, 10 };

        // Ordenar ambas listas para compararlas correctamente
        cartasIDs.Sort();
        combinacionCorrecta.Sort();

        // Comparar si las dos listas son id�nticas
        if (cartasIDs.SequenceEqual(combinacionCorrecta))
        {
            //Destroy(gameObject);

            // Instanciar la nueva carta en la misma posici�n y rotaci�n, pero sin ser hija de nadie
        }
    }
}
