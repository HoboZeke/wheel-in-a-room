using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] float baseVolume;

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
    }

    public void SetMute(bool mute)
    {
        source.mute = mute;
    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
