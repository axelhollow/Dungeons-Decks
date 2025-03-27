using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovementAldea : MonoBehaviour
{
    public Transform followTarget;        // El objeto vacío que la cámara va a seguir
    public float moveSpeed = 5f;          // Velocidad de movimiento
    public float zoomSpeed = 2f;          // Velocidad del zoom
    public float minZoom = 40f;           // Zoom mínimo (Orthographic Size)
    public float maxZoom = 80f;           // Zoom máximo (Orthographic Size)

    private CinemachineVirtualCamera virtualCamera;  // La cámara virtual de Cinemachine
    private float currentZoom = 60f;      // Valor de zoom actual

    public float minX = -200f, maxX = 200f;
    public float minZ = -100f, maxZ = 100f;

    public bool combatiendo=false;

    public static CameraMovementAldea instance;
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (virtualCamera != null && virtualCamera.m_Lens.Orthographic)
        {
            currentZoom = virtualCamera.m_Lens.OrthographicSize;
        }
    }

    void Update()
    {
        if (combatiendo == false)
        {
            // Movimiento con WASD
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
            followTarget.Translate(move, Space.World);

            // Limitar el movimiento dentro del área permitida
            Vector3 clampedPosition = followTarget.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
            followTarget.position = clampedPosition;

            // **Evitar Zoom si el cursor está sobre UI**
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // Zoom con el scroll del ratón
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                currentZoom -= scroll * zoomSpeed;
                currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

                if (virtualCamera != null && virtualCamera.m_Lens.Orthographic)
                {
                    virtualCamera.m_Lens.OrthographicSize = currentZoom;
                }
            }
        }
        
    }

    public void BloquearCamaraCombate()
    {
        combatiendo = true;
        virtualCamera.m_Lens.OrthographicSize = 54;
        followTarget.position = new Vector3(0f, 100f, 10f);
    }
}
