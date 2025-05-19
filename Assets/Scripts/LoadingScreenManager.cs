using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LoadingScreenManager : MonoBehaviour
{
    public GameObject placeholderImage;
    public AudioSource MenuMusic;
    public GameObject loadingScreen;
    public Animator loadingScreenAnimator;
    public Player player;

    private void Start()
    {
        loadingScreen.SetActive(true);
        loadingScreenAnimator.ResetTrigger("LoadingScreen"); // Ensures no previous trigger interferes (Added By AI Snippet)
        loadingScreenAnimator.SetTrigger("LoadingScreen");
        StartCoroutine(DisablePlaceholderWithDelay());


    }

    IEnumerator DisablePlaceholderWithDelay()
    {
        yield return new WaitForSeconds(3f); // Wait for 10 seconds
        placeholderImage.SetActive(false);
    }

    public void ShutLoadingScreen()
    {
        MenuMusic.Play();
       
        loadingScreen.SetActive(false);
    }

    
}
