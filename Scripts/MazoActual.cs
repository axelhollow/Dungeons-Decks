using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazoActual : MonoBehaviour
{
    public static MazoActual Instancia { get; private set; }

    public bool mazoIniciado=false;
    public bool mazoObjetosIniciado = false;
    public bool bossEvent = false;

    public Dictionary<GameObject, bool> mazoActual = new();
    public Dictionary<GameObject, bool> mazoObjetosActual = new();

    public List<GameObject> listaRecursos=new();

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject); // Destruye duplicados
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject); // Mantener entre escenas
    }


}

