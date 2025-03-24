using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CartasCombinaciones : MonoBehaviour
{
    public List<int> cartasIDs = new List<int>(); // Lista para guardar los IDs

    public GameObject Carta;

    public string prefabNamePrueba = "CubePrueba";



    public void ComprobacionCartas()
    {

        
        // Verificar si este objeto no tiene padre (es un "raíz")
        if (transform.parent == null)
        {
            cartasIDs.Clear(); // Limpiar la lista antes de llenarla nuevamente

            Carta carta = GetComponent<Carta>();
            if (carta != null) 
            {
                cartasIDs.Add(carta.id);
            }

            // Llamar a la función recursiva para recorrer los hijos y sus hijos
            RecorrerHijos(transform);

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

            if (carta != null) 
            {
                cartasIDs.Add(carta.id); // Guardar el ID en la lista
            }

            // Llamada recursiva para explorar los hijos del hijo
            RecorrerHijos(hijo);
        }
    }

    private void ComprobarCombinaciones()
    {
        // Lista de la combinación exacta que queremos verificar
        List<int> combinacionCorrecta = new List<int> { 2, 10 };

        // Ordenar ambas listas para compararlas correctamente
        cartasIDs.Sort();
        combinacionCorrecta.Sort();

        // Comparar si las dos listas son idénticas
        if (cartasIDs.SequenceEqual(combinacionCorrecta))
        {

            Vector3 posicionOriginal = transform.position;
            Quaternion rotacionOriginal = transform.rotation;

            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabNamePrueba);

            if (prefab != null)
            {
                Instantiate(prefab, posicionOriginal, rotacionOriginal);
                Destroy(gameObject);
            }

            
        }
    }
}
