using UnityEngine;

public class AudioObject : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Vector2 pitchRange;
    [SerializeField] bool playOnImpact;
    float baseVolume;

    private void Start()
    {
        baseVolume = audioSource.volume;
        UpdateVolume();
        AudioManager.main.OnAudioSettingsUpdate += UpdateVolume;
    }

    void UpdateVolume(object sender = null, AudioEventArgs args = null)
    {
        audioSource.volume = baseVolume * AudioManager.main.SFXVolume;
        audioSource.mute = AudioManager.main.Mute;
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.Play();
    }

    private void OnDestroy()
    {
        AudioManager.main.OnAudioSettingsUpdate -= UpdateVolume;
    }
}
