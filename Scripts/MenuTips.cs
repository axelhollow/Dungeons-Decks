using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTips : MonoBehaviour
{
    public GameObject menuTips; // Asigna el objeto UI en el inspector
    public float moveDistance = 200f; // Distancia a moverse
    public float moveSpeed = 0.5f; // Velocidad de animación
    private bool menuBool = false;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = menuTips.transform.localPosition; // Guarda la posición original
    }

    public void MostrarMenu()
    {
        menuBool = !menuBool;
        StopAllCoroutines(); // Detiene cualquier animación en progreso

        if (menuBool)
        {
            StartCoroutine(MoveMenu(initialPosition + new Vector3(moveDistance, 0, 0)));
        }
        else
        {
            StartCoroutine(MoveMenu(initialPosition));
        }
    }

    IEnumerator MoveMenu(Vector3 targetPosition)
    {
        float elapsedTime = 0;
        Vector3 startPosition = menuTips.transform.localPosition;

        while (elapsedTime < moveSpeed)
        {
            menuTips.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        menuTips.transform.localPosition = targetPosition; // Asegura la posición final
    }
}
