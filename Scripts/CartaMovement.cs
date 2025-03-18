using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaMovement : MonoBehaviour
{
    private Vector3 posicionOriginal;
    private bool estaSiendoArrastrada = false;

    public float alturaLevante = 2f; // Cuánto se levanta la carta cuando se hace clic
    public float velocidadRotacion = 5f; // La velocidad de inclinación
    public float anguloMaximo = 10f; // Ángulo máximo de inclinación

    private Vector3 ultimaPosicionRatón;

    void Start()
    {
        // Guardamos la posición original de la carta
        posicionOriginal = transform.position;
        ultimaPosicionRatón = transform.position; // Inicializamos la posición del ratón
    }

    void Update()
    {
        // Si se está presionando el botón del ratón
        if (estaSiendoArrastrada)
        {
            // Movemos la carta a la posición del ratón, manteniendo la altura levantada
            Vector3 posicionRatón = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            transform.position = new Vector3(posicionRatón.x, alturaLevante, posicionRatón.z);

            // Calculamos el vector de movimiento para inclinar la carta
            Vector3 direccionMovimiento = posicionRatón - ultimaPosicionRatón;

            // Si hay movimiento, inclinamos la carta en los ejes X y Z
            if (direccionMovimiento.magnitude > 0.1f)
            {
                // Inclinamos la carta en los ejes X y Z dependiendo de la dirección del movimiento
                float inclinacionX = Mathf.Clamp(direccionMovimiento.y, -1f, 1f) * anguloMaximo;
                float inclinacionZ = Mathf.Clamp(direccionMovimiento.x, -1f, 1f) * -anguloMaximo;

                // Aplicamos la inclinación suavemente con una interpolación
                Quaternion inclinacionObjetivo = Quaternion.Euler(inclinacionX, 0f, inclinacionZ);
                transform.rotation = Quaternion.Slerp(transform.rotation, inclinacionObjetivo, Time.deltaTime * velocidadRotacion);
            }
            else
            {
                // Si el ratón no se mueve, restablecemos la rotación a la normal
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * velocidadRotacion);
            }

            // Actualizamos la última posición del ratón
            ultimaPosicionRatón = posicionRatón;
        }
    }

    // Detectamos el clic en la carta
    private void OnMouseDown()
    {
        estaSiendoArrastrada = true;
        // Levantamos la carta un poco cuando hacemos clic
        transform.position = new Vector3(transform.position.x, alturaLevante, transform.position.z);
    }

    // Cuando soltamos el clic
    private void OnMouseUp()
    {
        estaSiendoArrastrada = false;
        // Guardamos la posición final del ratón al soltar el clic pero ajustando la altura a 0
        Vector3 posicionFinal = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        transform.position = new Vector3(posicionFinal.x, 0f, posicionFinal.z);

        // Restablecemos la rotación a 0, 0, 0 directamente al soltar el clic
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
