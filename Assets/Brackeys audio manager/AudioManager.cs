using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound currSound = Array.Find(sounds, sound => sound.name == name);
        if(currSound == null)
        {
            return;
        }
        currSound.source.Play();
    }
}
