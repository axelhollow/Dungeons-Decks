using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaScript : MonoBehaviour
{
    public Slider diaSlider;         // Asigna el Slider en el Inspector
    public TMP_Text diaTexto;        // Asigna el TextMeshPro para mostrar el día
    public float duration = 10f;     // Tiempo en segundos para vaciar el slider (100% -> 0%)

    private int currentDay = 1;
    private float currentPercentage = 100f;

    void Start()
    {
        diaTexto.text = "Día " + currentDay;
        diaSlider.maxValue = 100f;
        diaSlider.value = currentPercentage;
    }

    void Update()
    {

        if (currentPercentage > 0)
        {
            if (GameManager.gameSpeed > 0f) // Si gameSpeed es mayor a 0, el tiempo avanza
            {
                currentPercentage -= (100f / duration) * Time.deltaTime * GameManager.gameSpeed;
                currentPercentage = Mathf.Max(currentPercentage, 0);
                diaSlider.value = currentPercentage;
            }               
        }
        else
        {
            // Al llegar a 0, incrementa el día y reinicia el slider
            currentDay++;
            diaTexto.text = "Día " + currentDay;
            currentPercentage = 100f;
            diaSlider.value = currentPercentage;
        }
        
    }
}
