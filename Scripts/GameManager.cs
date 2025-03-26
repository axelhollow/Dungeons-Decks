using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float gameSpeed = 1f; // Velocidad del juego
    public float velocidad;
    public TMP_Text textoVelocidad;

    private void Start()
    {
        velocidad = gameSpeed;
        textoVelocidad.text = "x1";
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Cambiar entre pausa, normal y velocidad x2
            if (gameSpeed == 1f)
            {
                gameSpeed = 2f;
                textoVelocidad.text = "x2"; // Aumenta velocidad
            }
            else if (gameSpeed == 2f)
            {
                gameSpeed = 0f;
                textoVelocidad.text = "x0";
            }
            else if (gameSpeed == 0f)
            {
                gameSpeed = 1f;
                textoVelocidad.text = "x1";
            }
            velocidad = gameSpeed;
            
        }
    }
}
