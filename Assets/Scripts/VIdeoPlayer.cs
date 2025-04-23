using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VIdeoPlayer : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        VideoPlayer vp = GetComponent<VideoPlayer>();
        vp.Play(); 
    }

    public void Play()
    {
        SceneManager.LoadScene("PrisonMain");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
