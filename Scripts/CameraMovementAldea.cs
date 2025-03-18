using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraMovementAldea : MonoBehaviour
{
    public Transform followTarget;        // El objeto vacío que la cámara va a seguir
    public float moveSpeed = 5f;          // Velocidad de movimiento
    public float zoomSpeed = 2f;          // Velocidad del zoom
    public float minZoom = 40f;           // Zoom mínimo (Orthographic Size)
    public float maxZoom = 80f;           // Zoom máximo (Orthographic Size)

    private CinemachineVirtualCamera virtualCamera;  // La cámara virtual de Cinemachine

    private float currentZoom = 60f;      // Valor de zoom actual (el tamaño ortográfico)

    // Límites para el movimiento en el eje X y Z (el cuadrado predeterminado)
    public float minX = -200f, maxX = 200f;
    public float minZ = -100f, maxZ = 100f;

    // Start is called before the first frame update
    void Start()
    {
        // Obtiene la cámara virtual de Cinemachine
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Configura el zoom inicial de la cámara (por si acaso)
        if (virtualCamera != null && virtualCamera.m_Lens.Orthographic)
        {
            currentZoom = virtualCamera.m_Lens.OrthographicSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento de la cámara con WASD
        float horizontal = Input.GetAxis("Horizontal");  // A/D o flechas izquierda/derecha
        float vertical = Input.GetAxis("Vertical");      // W/S o flechas arriba/abajo

        // Movimiento del objeto vacío en el eje Z y X
        Vector3 move = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;

        // Aplicamos el movimiento al objeto vacío que la cámara sigue
        followTarget.Translate(move, Space.World);

        // Limitar el movimiento dentro de los límites definidos (cuadrado)
        Vector3 clampedPosition = followTarget.position;

        // Limitar en el eje X
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);

        // Limitar en el eje Z
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);

        // Aplicar la posición limitada al objeto vacío
        followTarget.position = clampedPosition;

        // Zoom con la rueda del ratón (ajustando Orthographic Size)
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            // Si la cámara es ortográfica, actualiza el tamaño ortográfico
            if (virtualCamera != null && virtualCamera.m_Lens.Orthographic)
            {
                virtualCamera.m_Lens.OrthographicSize = currentZoom;
            }
        }
    }
}
