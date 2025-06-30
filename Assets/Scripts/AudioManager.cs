using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance introMusic;
    
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one AudioManager found in scene.");
        }
        instance = this;
        
        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.music);
    }

    private void InitializeMusic(EventReference introMusicEventReference)
    {
        introMusic = CreateEventInstance(introMusicEventReference);
        introMusic.setParameterByName("MusicState", -1.0f);
        introMusic.start();
    }

    public void PlayOneShot(EventReference sound, Vector3 scenePosition)
    {
        RuntimeManager.PlayOneShot(sound, scenePosition);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
