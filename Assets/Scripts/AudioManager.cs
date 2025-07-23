using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    
    public GameManagerScript gameManager;
    public PlayerMovement2D player;
    
    public EventInstance music;
    public static AudioManager instance { get; private set; }

    // music state accessible by whole class
    public float musicState = -1.0f;

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

    private void Update()
    {
        // MusicControl();
        StartCoroutine(ShowMusic());
    }
    
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

    public void UpdateMusic(float updateMusicState)
    {
        musicState = updateMusicState;
        music.setParameterByName("MusicState", musicState);
    }

    public IEnumerator ShowMusic()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log(musicState.ToString());
    }

    public void StopMusic()
    {
        music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        music.release();
    }

    // public void MusicControl()
    // {
    //     if (gameManager.friendTimeTimer >= gameManager.midTimerStanding)
    //     {
    //         if (!player.masc)
    //         {
    //             UpdateMusic(1.0f);
    //         }
    //         else
    //         {
    //             UpdateMusic(3.0f);
    //         }
    //     }
    //
    //     if (gameManager.familyTimeTimer >= gameManager.midTimerStanding)
    //     {
    //         if (!player.masc)
    //         {
    //             UpdateMusic(1.0f);
    //         }
    //         else
    //         {
    //             UpdateMusic(3.0f);
    //         }
    //     }
    // }

}
