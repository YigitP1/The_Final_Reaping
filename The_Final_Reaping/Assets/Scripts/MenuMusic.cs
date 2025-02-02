using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip angel;

    public void Start()
    {
        audioSource.clip = angel;
        audioSource.Play();
    }
}
