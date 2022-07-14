using System;
using UnityEngine;

public class ObjectAudioController : MonoBehaviour
{
    [SerializeField] bool overallVolume = default;
    [SerializeField] [Range(0, 1)] float volume = default;
    [SerializeField] [Range(0, 100)] float distance = 30f;
    [SerializeField] Sound[] sounds = default;

    public static ObjectAudioController Instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            if (overallVolume)
            {
                s.source.volume = volume;
            }
            else
            {
                s.source.volume = s.volume;
            }
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = 1f;
            s.source.maxDistance = distance;
            s.source.rolloffMode = AudioRolloffMode.Linear;
        }
    }
    public void PlaySound(string trackName)
    {
        if (trackName.Equals("Random") && sounds.Length > 0)
        {
            int ran = UnityEngine.Random.Range(0, sounds.Length);
            sounds[ran].source.Stop();
            sounds[ran].source.Play();
        }
        else
        {
            Sound s = Array.Find(sounds, sound => sound.name == trackName);
            if (s == null)
            {
                Debug.Log("Sound " + name + " not found!");
            }
            else
            {
                s.source.Stop();
                s.source.Play();
            }
        }
    }
    public void StopSound(string trackName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == trackName);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found!");
        }
        s.source.Stop();
    }
}