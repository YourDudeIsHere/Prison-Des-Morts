
using System.Collections;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    // This is the player object that the AI will follow.
    public GameObject player;
    // This is the speed at which the AI will move.
    public float speed = 8.5f;
    // This is the distance between the AI and the player.
    private float distance;
    //To Decide if the player is grabbed or not
    public bool IsGrabbed;
    // Cooldown duration in seconds
    public float grabCooldown = 5.0f; 
    //Prevents grabbing for a certain amount of time after grabbing once
    private float grabCooldownTimer;
    
    public Player playerScript;

    public Image healthVignette;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the coroutine reference
        _healthReductionCoroutine = null;
    }


    // Update is called once per frame
    void Update()
    {
        // Update the cooldown timer
        if (grabCooldownTimer > 0)
        {
            grabCooldownTimer -= Time.deltaTime;
        }
        // Calculates the distance between the AI and the player, then moves the AI towards the player.
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position; 
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Turns Placeholder square to players position, may not need for game
        
        // If the distance between the AI and the player are less than 10, the AI will move towards the player. 
        if (distance < 10)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        print ("Grab State =" + IsGrabbed);
        if(IsGrabbed)
        {
            speed = 0;
        }
        else
        {
            speed = 5f;
        }
     
    }
    
    // This function is called when the AI is grabbed by the player.
    public void Grab()
    {
        if (grabCooldownTimer <= 0)
        {
            IsGrabbed = true;
            grabCooldownTimer = grabCooldown; // Reset the cooldown timer
            //Checks if the coroutine is running and starts it if it is not, Reducing the players health by 1 every second.
            _healthReductionCoroutine ??= StartCoroutine(ReduceHealthOverTime());
        }
    }
    
    // This function is called when the AI is released by the player.
    public void Release()
    {
        IsGrabbed = false;
        //Checks for the coroutine and stops it if it is running
        if (_healthReductionCoroutine != null)
        {
            StopCoroutine(_healthReductionCoroutine);
            _healthReductionCoroutine = null;
        }
    }
    private Coroutine _healthReductionCoroutine;
    //Coroutine to reduce the players health by 1 every second.
    private IEnumerator ReduceHealthOverTime()
    {
        while (IsGrabbed)
        {
            if (playerScript != null)
            {
                playerScript.playerHealth -= 1;
                UpdateImageAlpha();
            }
            yield return new WaitForSeconds(0.25f);
        }

       
    }
    private void UpdateImageAlpha()
    {
        if (healthVignette != null)
        {
            Color color = healthVignette.color;
            color.a = Mathf.Clamp01(color.a + 0.01f); // Increase the alpha value by 0.01
            healthVignette.color = color;
            print ("Health Vignette Alpha = " + color.a);
        }
    }
    
  
}
