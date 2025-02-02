using UnityEngine;

public class MusicSound : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource musicSource;

    public AudioClip angel;
    public AudioClip stage1;
    public AudioClip slash;
    public AudioClip sword;

    public void Start()
    {
        musicSource.clip = stage1;
        musicSource.Play();
    }
    public void SFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
