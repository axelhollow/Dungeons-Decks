using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public  class SaveSystem : MonoBehaviour
{
    private  string path => Application.persistentDataPath + "/cartas_guardadas.json";
    private string pathCartasRecursos => Application.persistentDataPath + "/cartasRecursosYconstrucciones_guardadas.json";
    private string pociones => Application.persistentDataPath + "/cartasPociones_guardadas.json";

    public static SaveSystem Instancia;
  void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Si ya existe uno, destruye este duplicado
        }
    }
    public  void GuardarTodas()
    {
        GuardarCartas(path, "CartaPersonaje");
        GuardarCartas(pathCartasRecursos, "MaterialesYconstrucciones");
        GuardarCartas(pociones, "Pocion");

        

        //PlayerPrefs.SetInt("Dia", GameManager.instance.currentDay);
        //PlayerPrefs.SetFloat("ProgresoDia", GameManager.instance.currentPercentage);
        //PlayerPrefs.Save();
    }
    public void GuardarCartas(string ruta,string tag) 
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag(tag);
        
        if (tag == "CartaPersonaje") 
        { 
            CartasGuardadas dataGlobal = new();

            foreach (GameObject obj in objetos)
            {
                var carta = obj.GetComponent<CartaPersonaje>();
                if (carta != null)
                {
                    dataGlobal.cartas.Add(carta.ToData());
                }
            }

            string json = JsonUtility.ToJson(dataGlobal, true);
            File.WriteAllText(ruta, json);
        }
        if (tag == "MaterialesYconstrucciones")
        {
            CartasGuardadasRecursos dataGlobal = new();
            foreach (GameObject obj in objetos)
            {
                var carta = obj.GetComponent<Carta>();
                if (carta != null)
                {
                    dataGlobal.cartas.Add(carta.ToData());
                    string prueba = JsonUtility.ToJson(dataGlobal, true);

                }
            }

            string json = JsonUtility.ToJson(dataGlobal, true);
            Debug.Log("Guardando JSON: " + json);
            File.WriteAllText(ruta, json);
        }
        if (tag == "Pocion")
        {
            CartasGuardadasItems dataGlobal = new();
            foreach (GameObject obj in objetos)
            {
                var carta = obj.GetComponent<CartaItems>();
                if (carta != null)
                {
                    dataGlobal.cartas.Add(carta.ToData());
                    string prueba = JsonUtility.ToJson(dataGlobal, true);

                }
            }

            string json = JsonUtility.ToJson(dataGlobal, true);
            Debug.Log("Guardando JSON: " + json);
            File.WriteAllText(ruta, json);
        }




        Debug.Log("Todas las cartas guardadas en: " + ruta);
    }

    public  void CargarTodas()
    {
        string path = Application.persistentDataPath + "/cartas_guardadas.json";
        cargarCartas(path);
        path = Application.persistentDataPath + "/cartasRecursosYconstrucciones_guardadas.json";
        cargarCartas(path);
        path = Application.persistentDataPath + "/cartasPociones_guardadas.json";
        cargarCartas(path);


        //GameManager gameManager = new GameManager();

        //if (MazoActual.Instancia == null)
        //{
        //    gameManager.AddComponent<GameManager>();
        //}
        ////Cargar datso dia
        //if (GameManager.instance != null)
        //{
        //    gameManager.currentDay = PlayerPrefs.GetInt("Dia", 0);
        //    gameManager.currentPercentage = PlayerPrefs.GetFloat("ProgresoDia", 0);
        //}
        //else
        //{
        //    Debug.LogError("GameManager.instance es null");
        //}

    }
    public void cargarCartas(string path) 
    {

        if (path == Application.persistentDataPath + "/cartas_guardadas.json")
        {
            // Verificar si el archivo existe
            if (!File.Exists(path))
            {
                Debug.LogWarning("No se encontraron datos guardados.");
                return;
            }

            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(path);

            // Deserializar los datos en un objeto de tipo CartasGuardadas
            CartasGuardadas dataGlobal = JsonUtility.FromJson<CartasGuardadas>(json);

            // Comprobar si la lista de cartas tiene elementos
            if (dataGlobal.cartas == null || dataGlobal.cartas.Count == 0)
            {
                Debug.LogWarning("No hay personajes para cargar.");
                return;
            }

            // Iterar sobre cada carta en la lista de cartas
            foreach (var data in dataGlobal.cartas)
            {
                // Cargar el prefab principal de la carta
                GameObject prefab = Resources.Load<GameObject>("Prefabs/" + data.prefabName);

                if (prefab != null)
                {
                    // Instanciar la carta
                    GameObject nuevaCarta = GameObject.Instantiate(prefab);

                    // Obtener el script CartaPersonaje de la carta instanciada
                    var cartaScript = nuevaCarta.GetComponent<CartaPersonaje>();

                    if (cartaScript != null)
                    {
                        // Cargar los datos en la carta
                        cartaScript.LoadFromData(data);
                    }
                    else
                    {
                        Debug.LogWarning("La carta instanciada no tiene el componente CartaPersonaje.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró el prefab de la carta: " + data.prefabName);
                }
            }
        }
        if(path == Application.persistentDataPath + "/cartasRecursosYconstrucciones_guardadas.json") 
        {

            // Verificar si el archivo existe
            if (!File.Exists(path))
            {
                Debug.LogWarning("No se encontraron datos guardados.");
                return;
            }

            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(path);

            // Deserializar los datos en un objeto de tipo CartasGuardadas
            CartasGuardadasRecursos dataGlobal = JsonUtility.FromJson<CartasGuardadasRecursos>(json);

            // Comprobar si la lista de cartas tiene elementos
            if (dataGlobal.cartas == null || dataGlobal.cartas.Count == 0)
            {
                Debug.LogWarning("No hay materiales para cargar.");
                return;
            }

            // Iterar sobre cada carta en la lista de cartas
            foreach (var data in dataGlobal.cartas)
            {
                // Cargar el prefab principal de la carta
                string input = data.nombre;
                string resultado = input.Split('(')[0].Trim();
                GameObject prefab = Resources.Load<GameObject>("Prefabs/" + resultado);

                if (prefab != null)
                {
                    // Instanciar la carta
                    GameObject nuevaCarta = GameObject.Instantiate(prefab);

                    // Obtener el script CartaPersonaje de la carta instanciada
                    var cartaScript = nuevaCarta.GetComponent<Carta>();

                    if (cartaScript != null)
                    {
                        // Cargar los datos en la carta
                        cartaScript.LoadFromData(data);
                    }
                    else
                    {
                        Debug.LogWarning("La carta instanciada no tiene el componente Carta.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró el prefab de la carta: " + resultado);
                }
            }


        }
        if (path == Application.persistentDataPath + "/cartasPociones_guardadas.json") 
        {
            // Verificar si el archivo existe
            if (!File.Exists(path))
            {
                Debug.LogWarning("No se encontraron datos guardados.");
                return;
            }

            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(path);

            // Deserializar los datos en un objeto de tipo CartasGuardadas
            CartasGuardadasItems dataGlobal = JsonUtility.FromJson<CartasGuardadasItems>(json);

            // Comprobar si la lista de cartas tiene elementos
            if (dataGlobal.cartas == null || dataGlobal.cartas.Count == 0)
            {
                Debug.LogWarning("No hay pociones para cargar.");
                return;
            }

            // Iterar sobre cada carta en la lista de cartas
            foreach (var data in dataGlobal.cartas)
            {
                // Cargar el prefab principal de la carta
                string input = data.prefabName;
                string resultado = input.Split('(')[0].Trim();
                GameObject prefab = Resources.Load<GameObject>("Prefabs/" + resultado);

                if (prefab != null)
                {
                    // Instanciar la carta
                    GameObject nuevaCarta = GameObject.Instantiate(prefab);

                    // Obtener el script CartaPersonaje de la carta instanciada
                    var cartaScript = nuevaCarta.GetComponent<CartaItems>();

                    if (cartaScript != null)
                    {
                        // Cargar los datos en la carta
                        cartaScript.FromData(data);
                    }
                    else
                    {
                        Debug.LogWarning("La carta instanciada no tiene el componente CartaItems.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró el prefab de la carta: " + resultado);
                }
            }
        }
            
    }
}