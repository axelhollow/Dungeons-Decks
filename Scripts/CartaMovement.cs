using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaMovement : MonoBehaviour
{
    private Vector3 posicionOriginal;
    private bool estaSiendoArrastrada = false;

    public float alturaLevante = 2f; // Cu�nto se levanta la carta cuando se hace clic
    public float velocidadRotacion = 5f; // La velocidad de inclinaci�n
    public float anguloMaximo = 10f; // �ngulo m�ximo de inclinaci�n

    private Vector3 ultimaPosicionRat�n;

    void Start()
    {
        // Guardamos la posici�n original de la carta
        posicionOriginal = transform.position;
        ultimaPosicionRat�n = transform.position; // Inicializamos la posici�n del rat�n
    }

    void Update()
    {
        // Si se est� presionando el bot�n del rat�n
        if (estaSiendoArrastrada)
        {
            // Movemos la carta a la posici�n del rat�n, manteniendo la altura levantada
            Vector3 posicionRat�n = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            transform.position = new Vector3(posicionRat�n.x, alturaLevante, posicionRat�n.z);

            // Calculamos el vector de movimiento para inclinar la carta
            Vector3 direccionMovimiento = posicionRat�n - ultimaPosicionRat�n;

            // Si hay movimiento, inclinamos la carta en los ejes X y Z
            if (direccionMovimiento.magnitude > 0.1f)
            {
                // Inclinamos la carta en los ejes X y Z dependiendo de la direcci�n del movimiento
                float inclinacionX = Mathf.Clamp(direccionMovimiento.y, -1f, 1f) * anguloMaximo;
                float inclinacionZ = Mathf.Clamp(direccionMovimiento.x, -1f, 1f) * -anguloMaximo;

                // Aplicamos la inclinaci�n suavemente con una interpolaci�n
                Quaternion inclinacionObjetivo = Quaternion.Euler(inclinacionX, 0f, inclinacionZ);
                transform.rotation = Quaternion.Slerp(transform.rotation, inclinacionObjetivo, Time.deltaTime * velocidadRotacion);
            }
            else
            {
                // Si el rat�n no se mueve, restablecemos la rotaci�n a la normal
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * velocidadRotacion);
            }

            // Actualizamos la �ltima posici�n del rat�n
            ultimaPosicionRat�n = posicionRat�n;
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
        // Guardamos la posici�n final del rat�n al soltar el clic pero ajustando la altura a 0
        Vector3 posicionFinal = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        transform.position = new Vector3(posicionFinal.x, 0f, posicionFinal.z);

        // Restablecemos la rotaci�n a 0, 0, 0 directamente al soltar el clic
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
