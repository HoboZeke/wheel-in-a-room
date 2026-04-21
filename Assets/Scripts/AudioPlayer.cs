using UnityEngine;
using UnityEngine.Rendering;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] float baseVolume;
    [SerializeField] AudioSource[] additionalSources;

    private void Awake()
    {
        baseVolume = source.volume;
    }

    private void OnValidate()
    {
        baseVolume = source.volume;
    }

    public void SetVolume(float volume)
    {
        source.volume = baseVolume * volume;
        for(int i = 0; i < additionalSources.Length; i++) { additionalSources[i].volume = baseVolume * volume; }
    }

    public void SetMute(bool mute)
    {
        source.mute = mute;
        for (int i = 0; i < additionalSources.Length; i++) { additionalSources[i].mute = mute; }
    }

    public void SetPitch(float p)
    {
        source.pitch = p;
        for (int i = 0; i < additionalSources.Length; i++) { additionalSources[i].pitch = p; }    }

    public void Play()
    {
        if (source.isPlaying && additionalSources.Length > 0)
        {
            for (int i = 0; i < additionalSources.Length; i++)
            {
                if (!additionalSources[i].isPlaying) { additionalSources[i].Play(); return; }
            }
        }

        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
