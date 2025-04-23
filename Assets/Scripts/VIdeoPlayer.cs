using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VIdeoPlayer : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        //Gets the enu Video Player to play
        VideoPlayer vp = GetComponent<VideoPlayer>();
        vp.Play(); 
    }

    public void Play()
    {
        //If the play button is clicked, load the first level.
        SceneManager.LoadScene("PrisonMain");
    }

    public void Quit()
    {
        //When Quit is clicked, quit the application.
        Application.Quit();
        Debug.Log("Quit");
    }
}
