using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaMovement : MonoBehaviour
{
    private Vector3 posicionOriginal;
    private bool estaSiendoArrastrada = false;
    private Vector3 offset; // Diferencia entre el punto de clic y el centro de la carta

    public float alturaLevante = 2f; // Cuánto se levanta la carta cuando se hace clic
    public float velocidadRotacion = 5f; // La velocidad de inclinación
    public float anguloMaximo = 10f; // Ángulo máximo de inclinación
    public float distanciaRaycast = 1.5f; // Distancia del Raycast para detectar cartas debajo

    private Vector3 ultimaPosicionRatón;

    private float limiteXMin = -235f;
    private float limiteXMax = 235f;
    private float limiteZMin = -120f;
    private float limiteZMax = 120f;

    public bool seleccionadaDungeon = false;

    void Start()
    {
        // Guardamos la posición original de la carta
        posicionOriginal = transform.position;
        ultimaPosicionRatón = transform.position; // Inicializamos la posición del ratón
    }

    void Update()
    {
        if (estaSiendoArrastrada)
        {
            Vector3 posicionRatón = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

            float posX = Mathf.Clamp(posicionRatón.x, limiteXMin, limiteXMax);
            float posZ = Mathf.Clamp(posicionRatón.z, limiteZMin + ObtenerProfundidadApilada()*30, limiteZMax);
            transform.position = new Vector3(posX, alturaLevante, posZ);

            Vector3 direccionMovimiento = posicionRatón - ultimaPosicionRatón;

            if (direccionMovimiento.magnitude > 0.1f)
            {
                float inclinacionX = Mathf.Clamp(direccionMovimiento.y, -1f, 1f) * anguloMaximo;
                float inclinacionZ = Mathf.Clamp(direccionMovimiento.x, -1f, 1f) * -anguloMaximo;

                Quaternion inclinacionObjetivo = Quaternion.Euler(inclinacionX, 0f, inclinacionZ);
                transform.rotation = Quaternion.Slerp(transform.rotation, inclinacionObjetivo, Time.deltaTime * velocidadRotacion);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * velocidadRotacion);
            }

            ultimaPosicionRatón = posicionRatón;
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log(ObtenerProfundidadApilada());
        estaSiendoArrastrada = true;
        seleccionadaDungeon = false;
        transform.SetParent(null);

        // Obtener la posición del ratón en el mundo
        Vector3 posicionRatón = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        // Guardar la diferencia entre la posición del ratón y el objeto (offset)
        offset = posicionRatón - transform.position;

        // Levantar la carta cuando se hace clic
        transform.position = new Vector3(transform.position.x, alturaLevante, transform.position.z);
    }

    private void OnMouseUp()
    {
        estaSiendoArrastrada = false;

        

        // Restablecer la rotación al soltar la carta
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // **Lanzar Raycast hacia abajo para detectar una carta debajo**
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanciaRaycast))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Carta") && hit.collider.GetComponent<CartaMovement>() != null && !hit.collider.GetComponent<CartaMovement>().seleccionadaDungeon)
            {
                Transform cartaPadre = hit.collider.transform;

                // Si la carta padre tiene hijas, encontrar la última hija
                while (cartaPadre.childCount > 0)
                {
                    cartaPadre = cartaPadre.GetChild(cartaPadre.childCount - 1);
                }

                // Hacer que la carta se convierta en hija de la última hija encontrada (o de la original si no tenía hijas)
                transform.SetParent(cartaPadre);
                transform.localPosition = new Vector3(0f, 1f, -0.25f);
                transform.localRotation = Quaternion.identity;

            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("HolderPersonaje") && ObtenerProfundidadApilada()==0 && hit.collider.GetComponent<CartaMovement>() != null &&!hit.collider.GetComponent<CartaMovement>().seleccionadaDungeon)
            {
                Transform cartaPadre = hit.collider.transform;

                transform.SetParent(cartaPadre);
                transform.localPosition = new Vector3(0f, 1f, 0f);
                transform.localRotation = Quaternion.identity;  
                seleccionadaDungeon = true;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("HolderObjeto") && ObtenerProfundidadApilada() == 0 && hit.collider.GetComponent<CartaMovement>() != null && !hit.collider.GetComponent<CartaMovement>().seleccionadaDungeon)
            {
                Transform cartaPadre = hit.collider.transform;

                
                transform.SetParent(cartaPadre);
                transform.localPosition = new Vector3(0f, 1f, 0f);
                transform.localRotation = Quaternion.identity;  
                seleccionadaDungeon = true;
            }
            else
            {
                Vector3 posicionFinal = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
                // Ajustamos la posición dentro de los límites
                float posX = Mathf.Clamp(posicionFinal.x, limiteXMin, limiteXMax);
                float posZ = Mathf.Clamp(posicionFinal.z, limiteZMin + ObtenerProfundidadApilada() * 30, 75);
                transform.position = new Vector3(posX, 0f, posZ);

                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

        }
        
    }

    private float ObtenerProfundidadApilada()
    {
        float profundidadBase = 0f;

        foreach (Transform hijo in transform)
        {
            profundidadBase += 0.25f; // Ajusta esto según el tamaño real de cada carta
            profundidadBase += ObtenerProfundidadHijos(hijo); // Recursión para contar todas las cartas en la pila
        }

        return profundidadBase;
    }

    private float ObtenerProfundidadHijos(Transform carta)
    {
        float profundidad = 0f;

        foreach (Transform hijo in carta)
        {
            profundidad += 0.25f;
            profundidad += ObtenerProfundidadHijos(hijo);
        }

        return profundidad;
    }

}
