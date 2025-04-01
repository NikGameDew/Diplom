using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip rollSound;
    [SerializeField] private AudioClip swipeLeftAndRightSound;
    [SerializeField] private AudioClip pickUpCoinSound;
    [SerializeField] private AudioClip endSound;
    [SerializeField] private AudioClip explosionSound;
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // Проверка на существование других экземпляров
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void PlayJumpSound()
    {
        audioSource.clip = jumpSound;
        audioSource.Play();
    }

    public void PlayRollSound()
    {
        audioSource.clip = rollSound;
        audioSource.Play();
    }
    public void PlaySwipeSound()
    {
        audioSource.clip = swipeLeftAndRightSound;
        audioSource.Play();
    }
    public void PlayPickUpCoinSound()
    {
        audioSource.clip = pickUpCoinSound;
        audioSource.Play();
    }
    public void PlayEndSound()
    {
        audioSource.clip = endSound;
        audioSource.Play();
    }
    public void PlayExplosionSound()
    {
        audioSource.clip = explosionSound;
        audioSource.Play();
    }
}
