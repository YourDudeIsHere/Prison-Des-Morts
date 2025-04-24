
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;



public class Player : MonoBehaviour
{
    public ItemObject item;
    public float speed = 8f;
    private float _directionHorizontal;
    private float _directionVertical;
    private Rigidbody2D _rigidbody;
    public AI ai;
    public bool enableInput = true;
    public float playerHealth;
    private float AI_Speed_Recharge = 15f;
    public GameObject ShoveZone;
    public float ShoveCooldown;

    // Start is called before the first frame update
    void Start()
    {
        ShoveZone.SetActive(false);
        playerHealth = 100f;
        //rb = RigidBody
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //Disables
        if (ai.IsGrabbed)
        {
            enableInput = false;
            speed = 0;
            _directionHorizontal = 0;
            _directionVertical = 0;
            _rigidbody.velocity = new Vector2(0, 0);
        }

        if (ai.IsGrabbed == false)
        {
            enableInput = true;
            speed = 8.5f;

            if (enableInput)
            {
                // Horizontal Movement
                this._directionHorizontal = Input.GetAxis("Horizontal");
                if (this._directionHorizontal > 0f)
                {
                    _rigidbody.velocity = new Vector2(_directionHorizontal * speed, _rigidbody.velocity.x);
                    transform.rotation = Quaternion.Euler(0, 0, -90); // Face right
                }

                else if (_directionHorizontal < 0f)
                {
                    _rigidbody.velocity = new Vector2(_directionHorizontal * speed, _rigidbody.velocity.x);
                    transform.rotation = Quaternion.Euler(0, 0, 90); // Face left
                }
                else
                {
                    _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
                }

                //Vertical Movement
                _directionVertical = Input.GetAxis("Vertical");
                if (_directionVertical > 0f)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _directionVertical * speed);
                    transform.rotation = Quaternion.Euler(0, 0, 0); // Face up
                }
                else if (_directionVertical < 0f)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _directionVertical * speed);
                    transform.rotation = Quaternion.Euler(0, 0, 180); // Face down
                }
                else
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
                }

                print($"THE COOLDOWN FOR SHOVING IS = {ShoveCooldown}");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    //If the AI is not grabbed and the cooldown is 0, the shove zone will be activated.
                    if (ai.IsGrabbed == false && ShoveCooldown == 0)
                    {
                        ShoveZone.SetActive(true);
                        //Forces AI to not grab after being shoved
                        ai.grabCooldown = ShoveCooldown;
                        ai.IsGrabbed = false;
                        StartCoroutine(DisableAfterDelay());
                    }
                }
            }
            
         





        }
        
      
       
    }
    //Used to shortly activate the shove before disabling it again
    private IEnumerator DisableAfterDelay()
    {
        //Activates the shove zone for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        //Disables the shove zone
        ShoveZone.SetActive(false);
        //Shove goes on a 3-second cooldown
        ShoveCooldown = 3f; //Adjust this for the cooldown time of shoving!
        //Starts the cooldown timer for shoving
        _shoveCooldown ??= StartCoroutine(ShoveCooldownTime());
    }

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Item"))
        {
            //AI Generated Snippet 
            if (item != null)
            {
                //item.HandlePickupItem();
            }
            else
            {
                Debug.LogError("Item is null.");
            }
            //End of AI Generated Snippet
        }
    }

    private Coroutine _stunTime;
    private Coroutine _shoveCooldown;
    //Used to detect when the AI is in the shove zone and to stop the AI from moving
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Checks if the object that entered the collider is an AI
        if (other.CompareTag("AI"))
        {
            Debug.Log("AI entered the collider");
            AI ai = other.GetComponent<AI>();
            if (ai != null)
            {
                ai.canMove = false;
                Debug.Log("AI speed set to 0");
                if (_stunTime != null)
                {
                    StopCoroutine(_stunTime);
                    _stunTime = null;
                }
                _stunTime = StartCoroutine(ShoveTime(ai));
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
    }
    
    //Used for timing until the AI can move again after being shoved
    private IEnumerator ShoveTime(AI ai)
    {
        AI_Speed_Recharge = 15f;
        while (AI_Speed_Recharge > 0f)
        {
            //Forces the AI to stand still
            ai.canMove = false;
            //Reduces 1 from the AI speed recharge every 0.1 seconds
            AI_Speed_Recharge -= 1f;
            print($"AI SPEED RECHARGE IS = {ShoveCooldown}");
            yield return new WaitForSeconds(0.1f);
        }
        
        print("AI speed is back to normal");
        ai.ResetMovement();
        _stunTime = null;
       
    }
    // Used for the cooldown timer of shoving. Counts down from 3 seconds to 0.
    private IEnumerator ShoveCooldownTime()
    {
        
        while (ShoveCooldown > 0)
        {
            ShoveCooldown -= 1;
            yield return new WaitForSeconds(1f); 
        }
        if (ShoveCooldown == 0)
        {
            StopCoroutine(_shoveCooldown);
            _shoveCooldown = null;
        }
    }

   
}
    

