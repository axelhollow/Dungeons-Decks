using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class TableroManager : MonoBehaviour
{
    //Singleton
    public static TableroManager Instance { get; private set; }

    //Particulas
    public GameObject efectoActual;

    //Mazos
    public List<GameObject> mazo;
    private List<GameObject> mazoPersonajes;
    private List<GameObject> mazotilizables=new();

    public List<GameObject> mazoEnemigos;
    public List<GameObject> mazoEnemigosAux;

    public bool recomepnsaBoss=false;
    public bool miTurno = true;

    //Grid Personajes
    public Transform[] gridPersonajes;
    //Grid Items
    public Transform[] gridItems;
    //Grid Ataques
    public Transform[] gridAtaques;
    //Grid Ataques
    public Transform[] gridEnemigos;

    public Dictionary<GameObject, GameObject> diccVivos = new();
    public Dictionary<GameObject, GameObject> diccObjetosVivos = new();


    private Dictionary<GameObject, Color> coloresOriginales = new Dictionary<GameObject, Color>();
    Color colorOriginal = Color.white; // Define el color original de las cartas

    //listas
    public List<GameObject> listaEnemigos = new();
    public List<GameObject> listaAliados = new();
    public List<GameObject> listaItems = new();
    

    //BOSS
    public GameObject boss;
    public GameObject esbirroBossDer;
    public GameObject esbirroBossIzqu;

    int numPersonajes;

    //elementos seleccionados
    private GameObject ataqueSeleccioando;
    private GameObject personajeSeleccionado;
    private GameObject pocionSeleccionada;

    //listas padres
    public GameObject listaPersonajesPadre;

    //tamano item
    Vector3 tamanoItem = new Vector3(12.4047499f, 0.206034645f, 18.6071262f);


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//opcional ya veremos si lo necesitamos
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameObject mazoObject = new GameObject();

        if (MazoActual.Instancia == null)
        {
            mazoObject.AddComponent<MazoActual>();
        }
        #region AliadosYEnemigos
       


        if (MazoActual.Instancia.mazoActual.Count == 0)
        {
            print("Mazo Vacio");
            mazoPersonajes = new();
            mazoPersonajes = PlayDungeon.instance.RecuperarPersonajes();

        }
        else
        {
            print("recuperamos mazo");
            mazoPersonajes = new();

            foreach (KeyValuePair<GameObject, bool> par in MazoActual.Instancia.mazoActual)
            {
                GameObject cartaGuardada = par.Key;   // Accede a la clave (GameObject)
                bool viva = par.Value;// Accede al valor (bool)
                if (viva != false)
                {
                    mazoPersonajes.Add(cartaGuardada);
                }

            }

        }

        GameObject obj = GameObject.Find("MapaScene");
        if (obj != null)
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(false);
            }
        }


        int n = 0;
        //Metemos las cartas de persoanje en su grid
        foreach (GameObject carta in mazoPersonajes)
        {
            print("ENTRA");
            Vector3 posicionCarta = gridPersonajes[n].transform.position;
            Vector3 tamanoCarta = gridPersonajes[n].transform.localScale;
            gridPersonajes[n].gameObject.SetActive(false);
            if (carta != null)
            {
                GameObject cartita = Instantiate(carta);

                cartita.GetComponent<CartaMovement>().holderDungeon = true;
                cartita.transform.position = posicionCarta;
                cartita.transform.SetParent(GameObject.FindWithTag("ListaDeAliados").transform);
                cartita.transform.localScale = tamanoCarta;


              
                cartita.SetActive(true);
                GenerarMinimazo(cartita);
                listaAliados.Add(cartita);
                if (MazoActual.Instancia.mazoIniciado==false)
                {
                    MazoActual.Instancia.mazoActual.Add(carta, true);
                    diccVivos.Add(cartita, carta);
                }
                
                carta.SetActive(false);
            }
            n++;
        }
        MazoActual.Instancia.mazoIniciado = true;
        numPersonajes = listaAliados.Count();
        n = 0;
        foreach (GameObject personaje in listaAliados)
        {
            foreach (Transform hijo in personaje.transform)
            {

                if (hijo.tag == "CartaAtaque")
                {
                    hijo.gameObject.SetActive(false);

                }
            }


        }
        if (MazoActual.Instancia.mazoObjetosIniciado==false)
        {
            print("Mazo Objeto Vacio");
            mazotilizables = new();
            mazotilizables = PlayDungeon.instance.RecuperarObjetos();

        }
        else
        {
            print("recuperamos mazo Objetos");
            mazotilizables = new();

            foreach (KeyValuePair<GameObject, bool> par in MazoActual.Instancia.mazoObjetosActual)
            {
                GameObject cartaGuardada = par.Key;   // Accede a la clave (GameObject)
                bool viva = par.Value;// Accede al valor (bool)
                if (viva != false)
                {
                    print(cartaGuardada.name);
                    mazotilizables.Add(cartaGuardada);
                }

            }

        }
        n = 0;
        //Metemos las cartas de Objeto en su grid
        foreach (GameObject carta in mazotilizables)
        {
            print("ENTRA objetos");
            Vector3 posicionCarta = gridItems[n].transform.position;
            gridItems[n].gameObject.SetActive(false);
            if (carta != null)
            {

                GameObject cartita = Instantiate(carta);
                cartita.SetActive(true);
                cartita.GetComponent<CartaMovement>().holderDungeon = true;
                cartita.GetComponent<CartaItems>().aldea = false;
                cartita.transform.position = posicionCarta;
                cartita.transform.SetParent(GameObject.FindWithTag("ListaDePotis").transform);
                cartita.transform.localScale = tamanoItem;


                diccObjetosVivos.Add(cartita, carta);
                listaItems.Add(cartita);
                if (MazoActual.Instancia.mazoObjetosIniciado == false)
                {
                    MazoActual.Instancia.mazoObjetosActual.Add(carta, true);

                }
                carta.SetActive(false);
            }
            n++;
        }
        MazoActual.Instancia.mazoObjetosIniciado = true;

        if (MazoActual.Instancia.bossEvent == false)
        {
           int numeroEnemigos= Random.Range(1, mazoEnemigos.Count+1);
           
           int vuelta = 1;
           mazoEnemigos = mazoEnemigosAux;

            foreach (GameObject carta in mazoEnemigos)
            {
                if (vuelta <= numeroEnemigos)
                {
                    int enemigoIndice = Random.Range(0, mazoEnemigos.Count);

                    Vector3 posicionCarta = gridEnemigos[vuelta-1].transform.position;
                    gridEnemigos[vuelta - 1].gameObject.SetActive(false);
                    var cartita = Instantiate(mazoEnemigos[enemigoIndice]);
                    cartita.transform.position = posicionCarta;
                    cartita.transform.SetParent(GameObject.FindWithTag("ListaDeEnemigos").transform);
                    listaEnemigos.Add(cartita);
                    cartita.SetActive(true);
                    vuelta++;
                }
            }

            
        }
        else 
        {
            //Boss Event
            recomepnsaBoss=true; 
            //Boss
            var enemyBoss =Instantiate(boss);
            enemyBoss.transform.position= gridEnemigos[1].transform.position;
            enemyBoss.transform.localScale = gridEnemigos[1].transform.localScale;
            gridEnemigos[1].gameObject.SetActive(false);
            enemyBoss.transform.SetParent(GameObject.FindWithTag("ListaDeEnemigos").transform);
            listaEnemigos.Add(enemyBoss);

            //Esbirros dere
            var esbirroD = Instantiate(esbirroBossDer);
            esbirroD.transform.position = gridEnemigos[2].transform.position;
            esbirroD.transform.localScale = gridEnemigos[2].transform.localScale;
            gridEnemigos[2].gameObject.SetActive(false);
            esbirroD.transform.SetParent(GameObject.FindWithTag("ListaDeEnemigos").transform);
            listaEnemigos.Add(esbirroD);


            //Esbirro izqui
            var esbirroI = Instantiate(esbirroBossIzqu);
            esbirroI.transform.position = gridEnemigos[0].transform.position;
            esbirroI.transform.localScale = gridEnemigos[0].transform.localScale;
            gridEnemigos[0].gameObject.SetActive(false);
            esbirroI.transform.SetParent(GameObject.FindWithTag("ListaDeEnemigos").transform);
            listaEnemigos.Add(esbirroI);
            MazoActual.Instancia.bossEvent = false;

        }
        #endregion

       
    }

    private void Update()
    {
            GestionarMuerteYderrota();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.CompareTag("TerminarTurno"))
                {
                    if (miTurno)
                    {
                        miTurno = false;
                        StartCoroutine("AcabarTurno");
                    }
                }

                if (obj.CompareTag("CartaPersonaje"))
                {

                    OcultarMinimazo();
                    // Si el mismo personaje ya est� seleccionado, deseleccionarlo
                    if (personajeSeleccionado == obj)
                    {
                        RestaurarColor(personajeSeleccionado);
                        personajeSeleccionado = null;
                        return;
                    }

                    // Restaurar color del personaje anterior
                    if (personajeSeleccionado != null)
                    {
                        RestaurarColor(personajeSeleccionado);
                    }

                    // Cambiar su color a rojo
                    CambiarColor(obj, Color.gray);

                    // Guardar el nuevo personaje seleccionado
                    personajeSeleccionado = obj;
                    GenerarMinimazo(personajeSeleccionado);
                    return;
                }
                if (obj.CompareTag("CartaAtaque"))
                {
                    // Si el mismo ataque ya est� seleccionado, deseleccionarlo
                    if (ataqueSeleccioando == obj)
                    {
                        RestaurarColor(ataqueSeleccioando);
                        ataqueSeleccioando = null;
                        return;
                    }

                    // Restaurar color del ataque anterior
                    if (ataqueSeleccioando != null)
                    {
                        RestaurarColor(ataqueSeleccioando);
                    }

                    // Guardar el nuevo ataque seleccionado
                    ataqueSeleccioando = obj;

                    // Cambiar su color a rojo
                    CambiarColor(obj, Color.red);
                    return;
                }
                if (obj.CompareTag("Pocion"))
                {
                    if (personajeSeleccionado != null)
                    {
                        if (pocionSeleccionada != null && pocionSeleccionada != obj)
                        {
                            //volver a ponerle su color original
                            Renderer rendPrev = pocionSeleccionada.GetComponent<Renderer>();
                            if (rendPrev != null)
                            {
                                rendPrev.material.color = colorOriginal;
                            }

                        }
                        pocionSeleccionada = obj;
                        // Cambiar su color
                        Renderer rend = pocionSeleccionada.GetComponent<Renderer>();
                        if (rend != null)
                        {
                            colorOriginal = rend.material.color; // Guarda el color original la primera vez
                            rend.material.color = Color.gray;

                        }
                    }
                    return;
                }

                if (ataqueSeleccioando != null && obj.tag == "Enemigo")
                {
                    if (ataqueSeleccioando.transform.parent == personajeSeleccionado.transform)
                    {
                        StartCoroutine(EfectoAtaqueCartasPersonajes(personajeSeleccionado.GetComponent<CartaPersonaje>().efectoAtaque, obj));
                      
                    }

                }
            }
        }
        if (personajeSeleccionado != null && pocionSeleccionada != null)
        {
            print("poti");
            //pillamos el personaje
            CartaPersonaje personaje = personajeSeleccionado.GetComponent<CartaPersonaje>();

            for (int i = listaItems.Count - 1; i >= 0; i--)
            {

                if (listaItems[i] == pocionSeleccionada)
                {
                    CartaItems pocion = pocionSeleccionada.GetComponent<CartaItems>();

                    if (pocion.tipoPocion == TipoPocion.vida)
                    {
                        personaje.CurarVida(pocion.cantidadEfecto);

                    }
                    if (pocion.tipoPocion == TipoPocion.mana)
                    {
                        personaje.mana += pocion.cantidadEfecto;
                        personaje.TextoMana.text = personaje.mana.ToString();
                    }
                    if (pocion.tipoPocion == TipoPocion.ataque)
                    {
                        foreach (Transform hijo in personaje.transform)
                        {

                            if (hijo.tag == "CartaAtaque")
                            {

                                hijo.transform.gameObject.GetComponent<Minicarta>().AumentarDamage(pocion.cantidadEfecto);

                            }
                        }
                    }
                    GameObject pocionActual = diccObjetosVivos[pocionSeleccionada];
                        print("diccVivos contiene ese personaje");
                        if (MazoActual.Instancia.mazoObjetosActual.ContainsKey(pocionActual))
                        {
                            print("se elimina la carta del mazo");
                            MazoActual.Instancia.mazoObjetosActual[pocionActual] = false;
                            MazoActual.Instancia.mazoObjetosActual.Remove(pocionActual);
                        }

                    pocionSeleccionada.SetActive(false);
                    listaItems.Remove(pocionSeleccionada);
                    pocionSeleccionada = null;

                }
            }
        }
    }
    void CambiarColor(GameObject obj, Color nuevoColor)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            if (!coloresOriginales.ContainsKey(obj))
            {
                coloresOriginales[obj] = rend.material.color; // Guardar color original
            }
            rend.material.color = nuevoColor;
        }
    }

    // Funci�n para restaurar el color original
    void RestaurarColor(GameObject obj)
    {
        if (obj != null && coloresOriginales.ContainsKey(obj))
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = coloresOriginales[obj]; // Restaurar color original
            }
        }
    }
    public void GenerarMinimazo(GameObject cartaPersonaje)
    {
        CartaPersonaje cartaP = cartaPersonaje.GetComponent<CartaPersonaje>();
        cartaP.mazoYaGenerado = true;

        if (cartaP.manoActual == null || cartaP.manoActual.Count == 0)
        {

            cartaP.manoActual = new Dictionary<GameObject, bool>();
            Vector3 posicionCarta = gridAtaques[0].transform.position;
            cartaP.ataque1.transform.position = posicionCarta;
            var cartita1 = Instantiate(cartaP.ataque1);
            cartaP.manoActual.Add(cartita1, true);
            cartita1.transform.SetParent(cartaP.transform);

            Vector3 posicionCarta2 = gridAtaques[1].transform.position;
            cartaP.ataque2.transform.position = posicionCarta2;
            var cartita2 = Instantiate(cartaP.ataque2);
            cartaP.manoActual.Add(cartita2, true);
            cartita2.transform.SetParent(cartaP.transform);

            Vector3 posicionCarta3 = gridAtaques[2].transform.position;
            cartaP.ataque3.transform.position = posicionCarta3;
            var cartita3 = Instantiate(cartaP.ataque3);
            cartita3.transform.SetParent(cartaP.transform);
            cartaP.manoActual.Add(cartita3, true);
        }
        else
        {
            if (cartaP.manoActual != null)
            {
                foreach (var carta in cartaP.manoActual)
                {
                    if (carta.Value) carta.Key.SetActive(true);
                }
            }
        }
    }
    public void OcultarMinimazo()
    {
        GameObject[] objetosAEliminar = GameObject.FindGameObjectsWithTag("CartaAtaque");
        foreach (GameObject obj in objetosAEliminar)
        {
            obj.SetActive(false);
        }
    }

    public void GestionarMuerteYderrota() 
    {
        //gestionarMuertePersonajes Y Derrota
        try
        {
            if (listaAliados.Count > 0)
            {
                for (int i = listaAliados.Count - 1; i >= 0; i--)
                {
                    if (listaAliados[i] != null)
                    {
                        GameObject personaje = listaAliados[i];
                        CartaPersonaje mazo = personaje.GetComponent<CartaPersonaje>();
                        if (mazo != null && mazo.vida <= 0)
                        {
                            listaAliados.RemoveAt(i);
                            numPersonajes--;
                            if (numPersonajes < 0) numPersonajes = 0;
                            if (i < 0) i = 0;
                            if (diccVivos.ContainsKey(personaje))
                            {
                                print("diccVivos contiene ese personaje");
                                GameObject carta = diccVivos[personaje];
                                if (MazoActual.Instancia.mazoActual.ContainsKey(carta))
                                {
                                    print("se elimina la carta del mazo");
                                    MazoActual.Instancia.mazoActual[carta] = false;
                                    MazoActual.Instancia.mazoActual.Remove(carta);
                                    listaAliados.Remove(carta);
                                }

                            }
                            personaje.SetActive(false);


                        }


                    }
                }

            }

        }
        catch (System.IndexOutOfRangeException e)
        {
            print("Exception de derrota" + e);
        }
        if (listaAliados == null || listaAliados.Count == 0)
        {
            print("perdiste");
           StartCoroutine(volverALaAldeaPerdiste());
            

        }

        //gestionarEnemigos y victoria
        try
        {
            if (listaEnemigos.Count > 0)
            {


                for (int i = listaEnemigos.Count - 1; i >= 0; i--)
                {
                    if (listaEnemigos[i].GetComponent<Enemigo>().vida <= 0)
                    {
                        listaEnemigos[i].SetActive(false);
                        listaEnemigos.Remove(listaEnemigos[i]);


                    }
                }
            }
        }
        catch (System.IndexOutOfRangeException e)
        {
            //Ganaste mi pana
            print("Exception de victoria" + e);
        }
        if (listaEnemigos.Count == 0)
        {

            print("ganaste");
            if (recomepnsaBoss == false)
            {
                SceneManager.LoadScene("RecompensaCombate", LoadSceneMode.Additive);
            }
            else 
            {
                SceneManager.LoadScene("RecompensaCombateBOSS", LoadSceneMode.Additive);
            }
            SceneManager.UnloadSceneAsync("TableroJuego");
            foreach (GameObject aliado in listaAliados)
            {
                foreach (Transform hijoTrans in aliado.transform)
                {
                    if (hijoTrans.gameObject.tag == "CartaAtaque")
                    {
                        Destroy(hijoTrans.gameObject);
                    }

                }

            }
            recomepnsaBoss = false;
        }
    }
    IEnumerator volverALaAldeaPerdiste()
    {
        yield return new WaitForSeconds(1f);
        Cursor.visible = true;
        PlayDungeon.instance.CartasRecuperdasAventura(null, null,true);
        SceneManager.UnloadSceneAsync("Mapa");
        SceneManager.UnloadSceneAsync("TableroJuego");
    }

    IEnumerator AcabarTurno()
    {
        Cursor.visible = false;

        //Turno Enemigo
        foreach (GameObject enemigo in listaEnemigos)
        {
            
            Enemigo enemiguito = enemigo.GetComponent<Enemigo>();
            //seleccionamos objetivo al que atacar
            yield return new WaitForSeconds(1f);
            int numeroAleatorio = Random.Range(0, listaAliados.Count());
            if (listaAliados.Count() > 0)
            {
                CartaPersonaje personaje = listaAliados[numeroAleatorio].GetComponent<CartaPersonaje>();
                efectoActual = enemiguito.efectoAtaque;
                efectoActual.transform.position = personaje.transform.position;
                enemiguito.transform.localScale = enemiguito.originalScale * enemiguito.scaleFactor;

                AudioManager.instance.PlayFX("Guerrero");
                Instantiate(efectoActual);

                yield return new WaitForSeconds(0.8f);
                personaje.RestarVida(enemiguito.att);
                print(personaje.name);
                enemiguito.transform.localScale = enemiguito.originalScale;
            } 
        }
        ReactivarCartas();
        Cursor.visible = true;
    }

    IEnumerator EfectoAtaqueCartasPersonajes(GameObject efecto,GameObject enemigo) 
    {
      
        int manaActual = personajeSeleccionado.GetComponent<CartaPersonaje>().mana;
        int costeCarta = ataqueSeleccioando.GetComponentInChildren<Minicarta>().coste;
        int manaRestante = manaActual - costeCarta;

        if (manaRestante >= 0)
        {
            efecto.transform.position = enemigo.transform.position;
            efecto.transform.position = new Vector3(efecto.transform.position.x, efecto.transform.position.y + 10, efecto.transform.position.z);
            Instantiate(efecto);
            AudioManager.instance.PlayFX(personajeSeleccionado.GetComponent<CartaPersonaje>().nombre);
            yield return new WaitForSeconds(0.8f);
            //Gestionar uso de mana
            manaActual = manaRestante;
            personajeSeleccionado.GetComponent<CartaPersonaje>().TextoMana.text = manaActual.ToString();
            ataqueSeleccioando.transform.parent.GetComponent<CartaPersonaje>().mana = manaActual;

            //Marcar Carta Como Usada
            if (personajeSeleccionado != null)
            {
                CartaPersonaje cartaPersonaje = personajeSeleccionado.GetComponent<CartaPersonaje>();

                if (cartaPersonaje.manoActual.ContainsKey(ataqueSeleccioando))
                {
                    cartaPersonaje.manoActual[ataqueSeleccioando] = !cartaPersonaje.manoActual[ataqueSeleccioando];

                    ataqueSeleccioando.SetActive(false);

                    //Restar vida al enemigo
                    int da�oCarta = ataqueSeleccioando.GetComponentInChildren<Minicarta>().damage;
                    enemigo.GetComponent<Enemigo>().RestarVida(da�oCarta);
                }
            }
            foreach (Transform hijo in ataqueSeleccioando.transform.parent.transform)
            {
                if (hijo.tag == "CartaAtaque")
                {
                    hijo.gameObject.GetComponent<Minicarta>().RestaurarDamage();
                }

            }
        }
    }

    public void ReactivarCartas()
    {

        foreach (GameObject obj in listaAliados)
        {
            //Reactivamos el mana
            obj.GetComponent<CartaPersonaje>().mana = obj.GetComponent<CartaPersonaje>().manaAux;
            obj.GetComponent<CartaPersonaje>().TextoMana.text = obj.GetComponent<CartaPersonaje>().mana.ToString();
            foreach (Transform hijo in obj.transform) // Recorre todos los hijos
            {
                obj.GetComponent<CartaPersonaje>().manoActual[hijo.gameObject] = true;

                if (hijo.gameObject.tag == "CartaAtaque")
                {
                    hijo.gameObject.GetComponent<Minicarta>().RestaurarDamage();
                }
            }
        }
        if (listaAliados != null)
        {
            if (listaAliados != null && listaAliados[0].transform.childCount > 0)
            {
                foreach (GameObject aliado in listaAliados)
                {

                    foreach (Transform hijo in aliado.transform) // Recorre todos los hijos
                    {
                        if (hijo.tag == "CartaAtaque")
                        {
                            hijo.gameObject.SetActive(false);
                        }
                    }
                    RestaurarColor(aliado);
                }
            }
        }
        miTurno = true;

    }
}
