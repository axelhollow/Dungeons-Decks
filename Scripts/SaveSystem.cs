using UnityEngine;
using System.IO;

public  class SaveSystem : MonoBehaviour
{
    private  string path => Application.persistentDataPath + "/cartas_guardadas.json";

    public  void GuardarTodas()
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag("CartaPersonaje");
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
        File.WriteAllText(path, json);
        Debug.Log("Todas las cartas guardadas en: " + path);
    }

    public  void CargarTodas()
    {
        // Ruta al archivo de guardado
        string path = Application.persistentDataPath + "/cartas_guardadas.json";

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
            Debug.LogWarning("No hay cartas para cargar.");
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
}