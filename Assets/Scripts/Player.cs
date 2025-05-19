using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XInput;

public class Player : MonoBehaviour
{
    public UnityEngine.UI.Image healthVignette; // Reference to the health vignette image
    public Weapon Weapon; // Reference to the player's weapon
    public float speed = 8f; // Movement speed of the player
    private float _directionHorizontal; // Horizontal movement direction
    private float _directionVertical; // Vertical movement direction
    private Rigidbody2D _rigidbody; // Rigidbody2D component for physics-based movement
    public AI ai; // Reference to the AI component
    public bool enableInput = true; // Flag to enable/disable player input
    public float playerHealth; // Player's health
    public GameObject ShoveZone; // Reference to the shove zone GameObject
    public float ShoveCooldown; // Cooldown duration for shove
    public bool DisableControllerConnectedAnimation = false; // Flag to disable controller connection animation
    public bool aiIsShoved = false; // Flag to track if AI is shoved

    public bool ControllerConnected = false; // Tracks if a controller is connected
    public Animator controllerUIAnimator; // Animator for controller UI animations

    public PlayerInput playerInput; // Reference to the PlayerInput component

    private PlayerControls _controls; // Input controls for the player
    
    public bool isGrabbed;
    public void SetGrabbedState(bool grabbed)
    {
        isGrabbed = grabbed;
        Debug.Log("Player grab state: " + grabbed);
        
        if (isGrabbed)
        {
            enableInput = false;
            speed = 0;
            _directionHorizontal = 0;
            _directionVertical = 0;
            _rigidbody.velocity = Vector2.zero;
        }
        
    }

    private void Awake()
    {
        // Initialize PlayerInput and PlayerControls
        playerInput = GetComponent<PlayerInput>();
        Debug.Log("Input System initialized: " + InputSystem.settings.updateMode);
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Subscribe to control scheme changes and device changes
        playerInput.onControlsChanged += OnControlsChanged;
        if (Application.isPlaying)
        {
            Debug.Log("Subscribing to InputSystem.onDeviceChange during Play mode.");
            InputSystem.onDeviceChange += (device, change) =>
            {
                Debug.Log($"Device changed: {device.displayName} - {change}");
            };
        }
        // Enable gameplay controls and bind shove input
        _controls.Gameplay.Enable();
        _controls.Gameplay.Shove.performed += OnShove;
    }

    private void OnDisable()
    {
        // Unsubscribe from control scheme changes and device changes
        playerInput.onControlsChanged -= OnControlsChanged;

        if (Application.isPlaying)
        {
            Debug.Log("Unsubscribing from InputSystem.onDeviceChange during Play mode.");
        }
        // Disable gameplay controls and unbind shove input
        _controls.Gameplay.Disable();
        _controls.Gameplay.Shove.performed -= OnShove;
    }

    private void OnControlsChanged(PlayerInput input)
    {
        // Handle control scheme changes (e.g., switching between gamepad and keyboard)
        if (input.currentControlScheme == "Gamepad")
        {
            Debug.Log("Gamepad is now active!");
            // Trigger controller connection animation
            controllerUIAnimator.SetTrigger("ShowController");
        }
        else
        {
            Debug.Log("Switched to non-gamepad input.");
        }
    }
    
    public void UpdateImageAlpha()
    {
        if (healthVignette != null)
        {
            Color color = healthVignette.color;
            color.a = Mathf.Clamp01(color.a + 0.01f); // Increase the alpha value by 0.01
            healthVignette.color = color;
            print ("Health Vignette Alpha = " + color.a);
        }
    }

    void Start()
    {
        // Log connected gamepads
        foreach (var gamepad in Gamepad.all)
        {
            Debug.Log("Connected gamepad: " + gamepad.displayName);
        }

        // Initialize player state
        ShoveZone.SetActive(false);
        playerHealth = 100f;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnShove(InputAction.CallbackContext context)
    {
        // Activate shove zone and start coroutine to disable it after a delay
        ShoveZone.SetActive(true);
        StartCoroutine(ShoveZoneEnabledTime());
    }

    IEnumerator ShoveZoneEnabledTime()
    {
        // Disable shove zone after 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        ShoveZone.SetActive(false);
    }

    void Update()
    {
        // Update the input system
        InputSystem.Update();

        // Disable input and movement if the player is grabbed by AI
        if (isGrabbed)
        {
            enableInput = false;
            speed = 0;
            _directionHorizontal = 0;
            _directionVertical = 0;
            _rigidbody.velocity = Vector2.zero;
        }

        // Enable input and reset speed if the player is not grabbed
        if (!isGrabbed)
        {
            enableInput = true;
            speed = 8.5f;

            // Handle player movement input
            if (enableInput)
            {
                Vector2 move = _controls.Gameplay.Move.ReadValue<Vector2>().normalized;

                // Apply movement and rotation based on input
                _rigidbody.velocity = move * speed;
                Debug.Log(move);
                if (move.x > 0f)
                    transform.rotation = Quaternion.Euler(0, 0, -90); // Right
                if (move.x < 0f)
                    transform.rotation = Quaternion.Euler(0, 0, 90);  // Left
                if (move.y > 0f)
                    transform.rotation = Quaternion.Euler(0, 0, 0);   // Up
                if (move.y < 0f)
                    transform.rotation = Quaternion.Euler(0, 0, 180); // Down
            }
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle AI entering the shove zone
        if (other.CompareTag("AI"))
        {
            Debug.Log("AI entered the collider");
            AI ai = other.GetComponent<AI>();
            if (ai != null && Weapon.AbleToShove)
            {
                StartCoroutine(AIShoveEffect());
            }
            else
            {
                Debug.Log("AI component not found on the object");
            }
        }
        else
        {
            Debug.Log("Non-AI object entered the collider");
        }

        // Handle item pickup
        if (other.gameObject.CompareTag("Item"))
        {
            Debug.Log("Player has entered the item collider");
            if (other.gameObject != null)
            {
                other.gameObject.GetComponent<ItemObject>().OnHandlePickupItem();
            }
            else
            {
                Debug.LogError("Item is null.");
            }
        }
    }

    public IEnumerator AIShoveEffect()
    {
        // Temporarily disable AI movement
        ai.canMove = false;
        aiIsShoved = true;
        yield return new WaitForSeconds(2f);
        ai.canMove = true;
        aiIsShoved = false;
    }

    // Disable controller connection animation (Currently Unused)
    public void DisableControlllerConnectedAnimation()
    {
        DisableControllerConnectedAnimation = true;
    }
}