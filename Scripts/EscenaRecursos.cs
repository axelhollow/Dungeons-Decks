using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;


public class EscenaRecursos : MonoBehaviour
{
    public List<GameObject> listaRecursos;

    public Button eleccion1;
    public Button eleccion2;
    public Button eleccion3;
    int cantidad;
    int n=0;

    public Color colorSeleccionado = new Color(1f, 0.84f, 0f); // Dorado
    private Color colorOriginal;
    private Button botonActual = null; // Último botón seleccionado

    public GameObject recursoelegido;

    private Dictionary<Button,GameObject> diccionarioMateriales=new();

    private Dictionary<GameObject, int> materialSeleccionado=new();




    void Start()
    {
        GameObject obj = GameObject.Find("MapaScene");
        if (obj != null)
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        GameObject mazoObject = new GameObject();

        if (MazoActual.Instancia == null)
        {
            mazoObject.AddComponent<MazoActual>();
        }


         n = Random.Range(0, 6);
        cantidad = Random.Range(1, 5);

        // Obtiene el material del objeto

        Material material;
        material=AnadirMaterial(eleccion1);
        if (material != null && material.mainTexture != null)
        {
            // Convierte la textura en un Sprite y la asigna al Image del botón
            Texture2D textura = (Texture2D)material.mainTexture;
            eleccion1.image.sprite = Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), new Vector2(0.5f, 0.5f));

            foreach (Transform hijo in eleccion1.gameObject.transform)
            {
                hijo.gameObject.GetComponent<TextMeshProUGUI>().text = cantidad.ToString();

            }
        }

        material = AnadirMaterial(eleccion2);
        if (material != null && material.mainTexture != null)
        {
            // Convierte la textura en un Sprite y la asigna al Image del botón
            Texture2D textura = (Texture2D)material.mainTexture;
            eleccion2.image.sprite = Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), new Vector2(0.5f, 0.5f));
            foreach (Transform hijo in eleccion2.gameObject.transform)
            {
                cantidad = Random.Range(1, 5);
                hijo.gameObject.GetComponent<TextMeshProUGUI>().text = cantidad.ToString();
            }
        }

        material = AnadirMaterial(eleccion3);

        if (material != null && material.mainTexture != null)
        {
            // Convierte la textura en un Sprite y la asigna al Image del botón
            Texture2D textura = (Texture2D)material.mainTexture;
            eleccion3.image.sprite = Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), new Vector2(0.5f, 0.5f));
            foreach (Transform hijo in eleccion3.gameObject.transform)
            {
                cantidad = Random.Range(1, 5);
                hijo.gameObject.GetComponent<TextMeshProUGUI>().text = cantidad.ToString();
            }

        }


        
    }

    public void seleccionarRecurso(Button botton) 
    {
        materialSeleccionado.Clear();
        switch (botton.name) 
        {
            case "Elecion 1":
                print("has elegido el boton 1");
                materialSeleccionado.Add(diccionarioMateriales[eleccion1], int.Parse(eleccion1.GetComponentInChildren<TextMeshProUGUI>().text));
                recursoelegido = diccionarioMateriales[eleccion1];
                SeleccionarBoton(eleccion1);
                print(diccionarioMateriales[eleccion1].name);
                print(int.Parse(eleccion1.GetComponentInChildren<TextMeshProUGUI>().text));
                break;
            case "Elecion 2": 
                print("has elegido el boton 2");
                materialSeleccionado.Add(diccionarioMateriales[eleccion2], int.Parse(eleccion2.GetComponentInChildren<TextMeshProUGUI>().text));
                recursoelegido = diccionarioMateriales[eleccion2];
                SeleccionarBoton(eleccion2);
                print(diccionarioMateriales[eleccion2].name);
                print(int.Parse(eleccion2.GetComponentInChildren<TextMeshProUGUI>().text));
                break;
            case "Elecion 3": 
                print("has elegido el boton 3");
                materialSeleccionado.Add(diccionarioMateriales[eleccion3], int.Parse(eleccion3.GetComponentInChildren<TextMeshProUGUI>().text));
                recursoelegido = diccionarioMateriales[eleccion3];
                SeleccionarBoton(eleccion3);
                print(diccionarioMateriales[eleccion3].name);
                print(int.Parse(eleccion3.GetComponentInChildren<TextMeshProUGUI>().text));
                break;
        }
        
    }

    public void SeleccionarBoton(Button nuevoBoton)
    {
        // Restaurar el color del botón anterior si existe
        if (botonActual != null)
        {
            ColorBlock coloresAntiguos = botonActual.colors;
            coloresAntiguos.selectedColor = colorOriginal;
            botonActual.colors = coloresAntiguos;
        }

        // Guardar el nuevo botón y cambiar su color
        botonActual = nuevoBoton;
        colorOriginal = botonActual.colors.selectedColor; // Guardar el color original

        ColorBlock nuevosColores = botonActual.colors;
        nuevosColores.selectedColor = colorSeleccionado;
        botonActual.colors = nuevosColores; // Aplicar el cambio
    }

    public void botonAceptar() 
    {
        int vueltas = materialSeleccionado[recursoelegido];

        for (int i = 1; i <= vueltas; i++)
        {
            GameObject copiasrecurso = Instantiate(recursoelegido);
            MazoActual.Instancia.listaRecursos.Add(copiasrecurso);
        }
        SceneManager.UnloadSceneAsync("RecursosEscene");
        GameObject obj = GameObject.Find("MapaScene");
        if (obj != null)
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(true);
            }
        }


    }

    public Material AnadirMaterial(Button buton)
    {
        // Si el recurso no está en el diccionario, lo agregamos directamente.
        if (!diccionarioMateriales.ContainsValue(listaRecursos[n]))
        {
            diccionarioMateriales.Add(buton, listaRecursos[n]);
        }
        else
        {
            // Buscar un nuevo recurso hasta encontrar uno que no esté en el diccionario.
            int intentos = 100; 
            while (diccionarioMateriales.ContainsValue(listaRecursos[n]) && intentos > 0)
            {
                n = Random.Range(0, listaRecursos.Count); // Elegir un nuevo índice aleatorio
                intentos--;
            }

            // Si encontramos un nuevo recurso válido, lo añadimos.
            if (!diccionarioMateriales.ContainsValue(listaRecursos[n]))
            {
                diccionarioMateriales.Add(buton, listaRecursos[n]);
            }
            else
            {
                Debug.LogWarning("No se pudo encontrar un recurso único después de varios intentos.");
            }
        }


        // Retornar el material del recurso asignado.
        return listaRecursos[n].GetComponent<MeshRenderer>().sharedMaterial;

    }

}
