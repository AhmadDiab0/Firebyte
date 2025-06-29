using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep SoundManager alive across scenes
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public void PlaySound(AudioClip _sound)
    {
        if (_sound != null)
        {
            source.PlayOneShot(_sound);

        }
    }
}
