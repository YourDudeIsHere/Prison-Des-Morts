
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
        // Placeholder for final level
        if (BasementLevel == 0)
        {
            SceneManager.LoadScene("PrisonTimedEnd");
        }
    }
}
