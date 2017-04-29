using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This controls music and ambience for the level.
/// </summary>
public class MusicAmbienceController : Singleton<MusicAmbienceController>
{

    public AudioSource MusicSource { get; set; }
    public AudioSource AmbienceSource { get; set; }
    
    public float MusicVolume { get; set; }
    public float AmbienceVolume { get; set; }

    private MusicAmbienceController() { }

    /// <summary>
    /// This is used to save a track if another one wants to temporarly play over it.
    /// </summary>
    public AudioSource TempSource { get; set; }

	// Use this for initialization
	void Awake ()
    {
        DontDestroyOnLoad(gameObject);
        MusicSource = gameObject.AddComponent<AudioSource>();
        AmbienceSource = gameObject.AddComponent<AudioSource>();
        TempSource = gameObject.AddComponent<AudioSource>();

        MusicSource.loop = true;
        AmbienceSource.loop = true;

        MusicSource.playOnAwake = false;
        AmbienceSource.playOnAwake = false;    
	}

    public void PlayMusic(AudioClip music, float atVolume)
    {

        if (MusicSource.isPlaying)
            MusicSource.Stop();

        //Enforce volume standards.
        if (!(0 >= atVolume && 1 <= atVolume))
            Debug.LogWarning("Recieved volume is not within the acceptable volume ranges. Clamping it..", gameObject);

        atVolume = Mathf.Clamp(atVolume, 0, 1);

        MusicSource.volume = atVolume;
        MusicSource.clip = music;
        MusicSource.Play();
    }

    public void PlayMusicTemporarly(AudioClip music, float atVolume)
    {
        //Don't interupt the music.
        MusicSource.Pause();
        TempSource.clip = music;

        //Enforce volume standards.
        if (!(0 >= atVolume && 1 <= atVolume))
            Debug.LogWarning("Recieved volume is not within the acceptable volume ranges. Clamping it..", gameObject);

        atVolume = Mathf.Clamp(atVolume, 0, 1);

        TempSource.volume = atVolume;
        TempSource.Play();
    }

    public void DonePlayingMusicTemporarly()
    {
        TempSource.clip = null;
        TempSource.Stop();
        //Theres no "Check if paused.", so I can't enforce that.
        //Todo: Write a extension method that keeps track of stuff like this.
        MusicSource.UnPause();
    }

    public void PlayAmbience(AudioClip ambience, float atVolume)
    {
        if (AmbienceSource.isPlaying) AmbienceSource.Stop();

        //Enforce volume standards.
        if (!(0 >= atVolume && 1 <= atVolume))
            Debug.LogWarning("Recieved volume is not within the acceptable volume ranges. Clamping it..", gameObject);

        atVolume = Mathf.Clamp(atVolume, 0, 1);
        AmbienceSource.volume = atVolume;
        AmbienceSource.clip = ambience;
        AmbienceSource.Play();
    }

    public void StopMusic()
    {
        this.MusicSource.Stop();
    }


    public void StopAmbience()
    {
        this.AmbienceSource.Stop();
    }

}
