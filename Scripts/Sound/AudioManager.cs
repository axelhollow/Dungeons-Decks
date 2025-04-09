using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer mainAudioMixer;
    public AudioMixerGroup musicAudioMixerGroup;
    public AudioMixerGroup fxAudioMixerGroup;

    public Sound[] music;
    [Range(0.0001f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0.0001f, 1f)]
    public float fxVolume = 0.5f;

    public Sound[] fx;

    // Sliders para ajustar volumen
    public Slider musicVolumeSlider;
    public Slider fxVolumeSlider;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = musicAudioMixerGroup;
        }
        foreach (Sound s in fx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = fxAudioMixerGroup;
        }
    }

    private void Start()
    {
        // Cargar configuracion guardada de PlayerPrefs
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        fxVolume = PlayerPrefs.GetFloat("FXVolume", 0.5f);

        // Configura sliders con los valores cargados
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicVolume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (fxVolumeSlider != null)
        {
            fxVolumeSlider.value = fxVolume;
            fxVolumeSlider.onValueChanged.AddListener(SetFXVolume);
        }

        // Actualiza los mezcladores con los valores iniciales
        mainAudioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        mainAudioMixer.SetFloat("FXVolume", Mathf.Log10(fxVolume) * 20);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Inicio")
        {
            GameObject musicaSliderObj = GameObject.Find("Musica");
            GameObject efectosSliderObj = GameObject.Find("EfectosFX");

            if (musicaSliderObj != null)
            {
                musicVolumeSlider = musicaSliderObj.GetComponent<Slider>();
                musicVolumeSlider.value = musicVolume;
                musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            }

            if (efectosSliderObj != null)
            {
                fxVolumeSlider = efectosSliderObj.GetComponent<Slider>();
                fxVolumeSlider.value = fxVolume;
                fxVolumeSlider.onValueChanged.AddListener(SetFXVolume);
            }

        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        mainAudioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
    }

    public void SetFXVolume(float volume)
    {
        fxVolume = volume;
        mainAudioMixer.SetFloat("FXVolume", Mathf.Log10(fxVolume) * 20);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("FXVolume", fxVolume);
        PlayerPrefs.Save();
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, s => s.name == name);

        if (s != null)
        {
            StartCoroutine(FadeIn(s.source, s.volume, 2f));
        }
        else
        {
            Debug.LogWarning("Music " + name + " not found");
        }
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(music, s => s.name == name);

        if (s != null)
        {
            StartCoroutine(FadeOut(s.source, 2f));
        }
        else
        {
            Debug.LogWarning("Music " + name + " not found");
        }
    }

    public void PlayFX(string name)
    {
        Sound s = Array.Find(fx, s => s.name == name);

        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning("FX " + name + " not found");
        }
    }

    public void StopFX(string name)
    {
        Sound s = Array.Find(fx, s => s.name == name);

        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning("FX " + name + " not found");
        }
    }

    private IEnumerator FadeIn(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = 0f;
        source.volume = startVolume;
        source.Play();

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
            yield return null;
        }
        source.volume = 0f;
        source.Stop();
    }
}
