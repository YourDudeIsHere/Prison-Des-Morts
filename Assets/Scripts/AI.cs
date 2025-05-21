
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    public float visionRange = 15f; // The range within which the AI can see

    private void OnDrawGizmosSelected()
    {
        // Set the color of the gizmo
        Gizmos.color = new Color(0, 0, 1, 0.5f); // Red with some transparency

        // Draw a wireframe sphere to represent the vision range
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
    public float AIhealth = 10f;
    // This is the player object that the AI will follow.
    public GameObject player;
    public UIManager uiManager;
    // This is the speed at which the AI will move.
    public float speed = 6f;
    // This is the distance between the AI and the player.
    private float distance;
    //To Decide if the player is grabbed or not
    [FormerlySerializedAs("IsGrabbed")] public bool IsGrabbing;
    // Cooldown duration in seconds
    public float grabCooldown = 5.0f; 
    //Prevents grabbing for a certain amount of time after grabbing once
    private float grabCooldownTimer;
    private Rigidbody2D _rigidbody;
    
    public bool canGrab = true;
    
    public Player playerScript;
    
    private bool isInForcedRelease;

    public Image healthVignette;
    
    public static AI currentGrabbingAI;
    
    public bool canMove = true;
    
    public void Initialize(GameObject playerObj, Player script, UIManager ui)
    {
        player = playerObj;
        playerScript = script;
        uiManager = ui;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (playerScript == null)
            playerScript = player.GetComponent<Player>();
        
        _rigidbody = GetComponent<Rigidbody2D>();
        
        // Initialize the coroutine reference
        _healthReductionCoroutine = null;
        
    }


    // Update is called once per frame
    void Update()
    {
        if (playerScript.enableInput && !playerScript.isGrabbed && IsGrabbing)
        {
            ForceResetGrabState();
        }
        
        if (!IsGrabbing && !playerScript.aiIsShoved)
        {
            canMove = true;
        }
        if (AIhealth <= 0)
        {
            Destroy(gameObject);
        }
        // Update the cooldown timer
        if (grabCooldownTimer > 0f)
        {
            grabCooldownTimer -= Time.deltaTime;
        }
        
        if (!canMove)
        {
            Debug.Log("Is unable to move CS-72");
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        if(canMove)
        {
            // Calculates the distance between the AI and the player, then moves the AI towards the player.
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position; 
            direction.Normalize();
            
           // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //Turns Placeholder square to players position, may not need for game
        
            // If the distance between the AI and the player are less than 15, the AI will move towards the player. 
            if (distance < 15)
            {
                //transform.rotation = Quaternion.Euler(Vector3.forward * angle);
                _rigidbody.velocity = new Vector2( speed * direction.x, speed * direction.y);
                if (canMove)
                {
                    Debug.Log("Is able to move CS-87");
                    // Calculate the angle and snap to the nearest 90 degrees
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    angle = Mathf.Round(angle / 90) * 90;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                } 
                
            }
        } 
        print ("Grab State =" + IsGrabbing);
     
    }
    
    // This function is called when the AI is grabbed by the player.
    public void Grab()
    {
        if (IsGrabbing || grabCooldownTimer > 0f || isInForcedRelease)
        {
            Debug.Log($"{name} cannot grab right now (grabbing: {IsGrabbing}, cooldown: {grabCooldownTimer}, forcedRelease: {isInForcedRelease})");
            return;
        }
        
        // --- passed all checks ---
        Debug.Log($"[{name}] Successfully started grabbing.");

        currentGrabbingAI = this;
        IsGrabbing = true;
        canMove = false;
        grabCooldownTimer = grabCooldown;

        UIManager.Instance.ShowGrabbedUI();

        if (playerScript != null)
        {
            playerScript.SetGrabbedState(true);
            playerScript.enableInput = false;
        }

        _healthReductionCoroutine ??= StartCoroutine(ReduceHealthOverTime());
    }
    
    // This function is called when the AI is released by the player.
    public void Release()
    {
        Debug.Log("RELEASE IS BEING CALLED");
        ForceResetGrabState();
        UIManager.Instance.HideGrabUI();
        if (!IsGrabbing) return;

        Debug.Log($"{name} has been forced to release the player.");

        IsGrabbing = false;
        canMove = false;
        isInForcedRelease = true;

        if (currentGrabbingAI == this)
            currentGrabbingAI = null;

        if (_healthReductionCoroutine != null)
        {
            StopCoroutine(_healthReductionCoroutine);
            _healthReductionCoroutine = null;
        }

        playerScript.enableInput = true;
        playerScript.SetGrabbedState(false);

        

        StartCoroutine(ForceReleaseCooldown());
    }
    
    public void ForceResetGrabState()
    {
        Debug.LogWarning($"{name}: FORCE RESET triggered.");

        // Stop health drain
        if (_healthReductionCoroutine != null)
        {
            StopCoroutine(_healthReductionCoroutine);
            _healthReductionCoroutine = null;
        }

        // Reset key flags
        IsGrabbing = false;
        canMove = true;
        isInForcedRelease = false;
        grabCooldownTimer = grabCooldown;

        // Clear global reference if needed
        if (currentGrabbingAI == this)
            currentGrabbingAI = null;

        // Reset player grab state
        if (playerScript != null)
        {
            playerScript.enableInput = true;
            playerScript.SetGrabbedState(false);
        }

        // Reset UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideGrabUI();
        }
    }
    
    private IEnumerator ForceReleaseCooldown()
    {
        // Disable any grabbing logic for 2 seconds
        yield return new WaitForSeconds(2f);
        canMove = true;
        isInForcedRelease = false;
    }
    
    private void ResetCanGrab()
    {
        canGrab = true;
    }
    private Coroutine _healthReductionCoroutine;
    //Coroutine to reduce the players health by 1 every second.
    private IEnumerator ReduceHealthOverTime()
    {
        Debug.Log("[AI] Health coroutine started");
    
        while (IsGrabbing)
        {
            Debug.Log("[AI] Still grabbing, reducing health");

            if (playerScript != null)
            {
                playerScript.playerHealth -= 1;
                playerScript.UpdateImageAlpha();
            }
            yield return new WaitForSeconds(0.25f);
        }

        Debug.Log("[AI] Coroutine exit: IsGrabbing is now false");

       
    }
    
    public void ResetMovement()
    {
        canMove = true;
    }
    
  
}
