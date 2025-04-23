
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public int BasementLevel = 3;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hatch"))
        {
            SceneManager.LoadScene("PrisonMain");
            BasementLevel -= 1;
        }
        // Will create a final level, contains a timer and forces the player to rush to the exit 
        if (BasementLevel == 0)
        {
            SceneManager.LoadScene("PrisonTimedEnd");
        }
    }
}
