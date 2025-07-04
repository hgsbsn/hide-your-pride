using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    
    public EventInstance music;
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one AudioManager found in scene.");
        }
        instance = this;
        
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.music);
    }

    /*
    private void Update()
    {
        if (npc.playerSpotted && player.masc)
        {
            UpdateMusic(3.0f);
        }
        if (npc.playerSpotted && !player.masc)
        {
            UpdateMusic(1.0f);
        }
    }
    */
    
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    private void InitializeMusic(EventReference introMusicEventReference)
    {
        music = CreateEventInstance(introMusicEventReference);
        music.setParameterByName("MusicState", -1.0f);
        music.start();
    }

    public void UpdateMusic(float musicState)
    {
        music.setParameterByName("MusicState", musicState);
    }

    public void StopMusic()
    {
        music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        music.release();
    }
}
