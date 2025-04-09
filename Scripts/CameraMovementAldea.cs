using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovementAldea : MonoBehaviour
{
    public Transform followTarget;        // El objeto vacío que la cámara va a seguir
    public float moveSpeed = 5f;            // Velocidad de movimiento
    public float zoomSpeed = 2f;            // Velocidad del zoom
    public float minZoom = 40f;             // Zoom mínimo (Orthographic Size)
    public float maxZoom = 80f;             // Zoom máximo (Orthographic Size)

    public CinemachineVirtualCamera virtualCamera;  // La cámara virtual de Cinemachine
    private float currentZoom = 60f;        // Valor de zoom actual

    public float minX = -200f, maxX = 200f;
    public float minZ = -100f, maxZ = 100f;

    public bool combatiendo = false;

    // Variables para el arrastre con el ratón
    private bool isDragging = false;
    private Vector3 lastMousePosition;

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
        if (!combatiendo)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                // Lanzamos un raycast desde la posición del ratón
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // Comprobamos si el objeto impactado tiene el layer "Tablero"
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Tablero"))
                    {
                        isDragging = true;
                        lastMousePosition = Input.mousePosition;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                // Convertir la posición actual y la anterior del ratón al mundo
                Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
                Vector3 lastMouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(lastMousePosition.x, lastMousePosition.y, Camera.main.transform.position.y));

                // Calculamos el delta y lo invertimos para mover la cámara en sentido contrario
                Vector3 delta = currentMouseWorldPos - lastMouseWorldPos;
                Vector3 move = new Vector3(-delta.x, 0, -delta.z);

                followTarget.position += move;

                // Actualizamos la posición del ratón para el siguiente frame
                lastMousePosition = Input.mousePosition;

                // Limitar el movimiento dentro del área permitida
                Vector3 clampedPosition = followTarget.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
                clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
                followTarget.position = clampedPosition;
            }
            else
            {
                // Movimiento con WASD (opcional: puedes decidir desactivarlo durante el arrastre)
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                Vector3 move = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
                followTarget.Translate(move, Space.World);


                // Limitar el movimiento dentro del área permitida
                Vector3 clampedPosition = followTarget.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
                clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
                followTarget.position = clampedPosition;
            }

            // Evitar zoom si el cursor está sobre UI
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
    public void DesbloquearBloquearCamaraCombate()
    {
        combatiendo = false;
        virtualCamera.m_Lens.OrthographicSize = 150;
        followTarget.position = new Vector3(0f, 100f, 10f);
    }

}
