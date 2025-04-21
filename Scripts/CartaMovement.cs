using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaMovement : MonoBehaviour
{
    private Vector3 posicionOriginal;
    private bool estaSiendoArrastrada = false;
    private Vector3 offset; // Diferencia entre el punto de clic y el centro de la carta

    public float alturaLevante = 6f; // Cu�nto se levanta la carta cuando se hace clic
    public float distanciaRaycast = 10f; // Distancia del Raycast para detectar cartas debajo

    private Vector3 ultimaPosicionRat�n;

    private float limiteXMin = -235f;
    private float limiteXMax = 235f;
    private float limiteZMin = -120f;
    private float limiteZMax = 120f;

    public bool seleccionadaDungeon = false;
    public bool holderDungeon = false;

    void Start()
    {
        // Guardamos la posici�n original de la carta
        posicionOriginal = transform.position;
        ultimaPosicionRat�n = transform.position; // Inicializamos la posici�n del rat�n
    }

    void Update()
    {
        if (estaSiendoArrastrada)
        {
            // Convertir la posici�n del rat�n a coordenadas del mundo
            Vector3 posicionRat�n = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

            // Limitar la posici�n en X y Z
            float posX = Mathf.Clamp(posicionRat�n.x, limiteXMin, limiteXMax);
            float posZ = Mathf.Clamp(posicionRat�n.z, limiteZMin + ObtenerProfundidadApilada() * 15, limiteZMax);
            transform.position = new Vector3(posX, alturaLevante, posZ);


            ultimaPosicionRat�n = posicionRat�n;
        }
    }

    private void OnMouseDown()
        {
            if (!holderDungeon)
            {
                estaSiendoArrastrada = true;
                seleccionadaDungeon = false;
                transform.SetParent(null);

                // Obtener la posici�n del rat�n en el mundo usando nearClipPlane para el offset
                Vector3 posicionRat�n = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                offset = posicionRat�n - transform.position;

                // Levantar la carta cuando se hace clic
                transform.position = new Vector3(transform.position.x, alturaLevante, transform.position.z);

                AudioManager.instance.PlayFX("CardGrab");
            }
        }

    private void OnMouseUp()
    {
        if (!holderDungeon)
        {
            estaSiendoArrastrada = false;

            // Restablecer la rotaci�n al soltar la carta
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Lanzar Raycast hacia abajo para detectar una carta debajo
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, distanciaRaycast))
            {
                Carta cartaActual = GetComponent<Carta>();

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Carta") &&
                    hit.collider.GetComponent<CartaMovement>() != null &&
                    !hit.collider.GetComponent<CartaMovement>().seleccionadaDungeon)
                {
                    Transform cartaPadre = hit.collider.transform;
                    Transform ultimaCartaHija = cartaPadre;

                    // Recorrer la jerarqu�a para encontrar la �ltima carta hija
                    while (ultimaCartaHija.childCount > 0)
                    {
                        Transform posibleUltima = ultimaCartaHija.GetChild(ultimaCartaHija.childCount - 1);
                        if (posibleUltima.GetComponent<Carta>() != null)
                        {
                            ultimaCartaHija = posibleUltima;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // Establecer la nueva carta como la �ltima hija encontrada
                    transform.SetParent(ultimaCartaHija);
                    transform.localPosition = new Vector3(0f, 0.1f, -0.15f);
                    transform.localRotation = Quaternion.identity;
                }
                else if (cartaActual.tipo == TipoCarta.Personaje &&
                         hit.collider.gameObject.layer == LayerMask.NameToLayer("HolderPersonaje") &&
                         ObtenerProfundidadApilada() == 0)
                {
                    Transform cartaPadre = hit.collider.transform;
                    transform.SetParent(cartaPadre);
                    transform.localPosition = new Vector3(0f, 0.1f, 0f);
                    transform.localRotation = Quaternion.identity;
                    seleccionadaDungeon = true;
                }
                else if (cartaActual.tipo == TipoCarta.Item &&
                         hit.collider.gameObject.layer == LayerMask.NameToLayer("HolderObjeto") &&
                         ObtenerProfundidadApilada() == 0)
                {
                    Transform cartaPadre = hit.collider.transform;
                    transform.SetParent(cartaPadre);
                    transform.localPosition = new Vector3(0f, 0.1f, 0f);
                    transform.localRotation = Quaternion.identity;
                    seleccionadaDungeon = true;
                }
                else
                {
                    // Si no se cumple ninguna condici�n, la carta vuelve a su posici�n ajustada
                    Vector3 posicionFinal = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
                    float posX = Mathf.Clamp(posicionFinal.x, limiteXMin, limiteXMax);
                    float posZ = Mathf.Clamp(posicionFinal.z, limiteZMin + ObtenerProfundidadApilada() * 15, 75);
                    transform.position = new Vector3(posX, 0f, posZ);
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
            AudioManager.instance.PlayFX("CardDrop");   
        }
        
    }


    private float ObtenerProfundidadApilada()
        {
            float profundidadBase = 0f;

            foreach (Transform hijo in transform)
            {
                if(hijo.gameObject.layer == LayerMask.NameToLayer("Carta"))
                {
                    profundidadBase += 0.25f; // Ajusta esto seg�n el tama�o real de cada carta
                    profundidadBase += ObtenerProfundidadHijos(hijo); // Recursi�n para contar todas las cartas en la pila
                }
                
            }

            return profundidadBase;
        }

    private float ObtenerProfundidadHijos(Transform carta)
        {
            float profundidad = 0f;

            foreach (Transform hijo in carta)
            {
                if (hijo.gameObject.layer == LayerMask.NameToLayer("Carta"))
                {
                    profundidad += 0.25f;
                    profundidad += ObtenerProfundidadHijos(hijo);
                }
                
            }

            return profundidad;
        }
    }
