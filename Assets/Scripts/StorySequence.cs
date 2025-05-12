using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StoryVideoSequence : MonoBehaviour
{
    // UI elements and video player references
    public GameObject MenuVideo; // The main menu video object
    public Button quitButton; // Button to quit the game
    public Button optionsButton; // Button to open options menu
    public RawImage videoDisplay; // UI element to display the video
    public VideoPlayer videoPlayer; // Video player component
    public Button playButton; // Button to start the video sequence
    public Button continueButton; // Button to continue to the next video

    public string[] videoFileNames; // Array of video file names to play in sequence

    private int currentVideoIndex = 0; // Tracks the current video being played

    void Start()
    {
        // Initialize UI elements
        videoDisplay.gameObject.SetActive(false); // Hide video display initially
        continueButton.gameObject.SetActive(false); // Hide continue button initially

        // Add listeners to buttons
        playButton.onClick.AddListener(StartSequence); // Start video sequence on play button click
        continueButton.onClick.AddListener(NextVideo); // Play next video on continue button click

        // Subscribe to the video player's event for when a video finishes
        videoPlayer.loopPointReached += OnVideoFinished;

        // Set up the video player's render texture
        videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0); // Adjust resolution as needed
        videoDisplay.texture = videoPlayer.targetTexture; // Assign the render texture to the RawImage
    }

    void StartSequence()
    {
        // Hide menu UI and show video display
        MenuVideo.SetActive(false);
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        videoDisplay.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);

        // Start playing the first video
        currentVideoIndex = 0;
        PlayVideo(currentVideoIndex);
    }

    void PlayVideo(int index)
    {
        // Set the video player's URL to the selected video file
        string path = $"{Application.streamingAssetsPath}/{videoFileNames[index]}.mp4";
        videoPlayer.url = path;
        videoPlayer.isLooping = true; // Enable looping for the video
        videoPlayer.Play(); // Start playing the video
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Enable the continue button once the video finishes
        continueButton.gameObject.SetActive(true);
    }

    void NextVideo()
    {
        // Hide the continue button and move to the next video
        continueButton.gameObject.SetActive(false);
        currentVideoIndex++;

        // Check if there are more videos to play
        if (currentVideoIndex < videoFileNames.Length)
        {
            PlayVideo(currentVideoIndex); // Play the next video
        }
        else
        {
            EndSequence(); // End the sequence if all videos are played
        }
    }

    void EndSequence()
    {
        // Stop the video player and hide the video display
        videoPlayer.Stop();
        videoDisplay.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        // Load the next scene (e.g., the main game scene)
        SceneManager.LoadScene(1);
    }
}