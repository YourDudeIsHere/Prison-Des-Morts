
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class Player : MonoBehaviour
{
    public float speed = 8.5f;
    private float _directionHorizontal = 0f;
    private float _directionVertical = 0f;
    private Rigidbody2D _rigidbody;
    public AI ai;
    public bool enableInput = true;
    public float playerHealth;
    public float AI_Speed_Recharge;
    public GameObject ShoveZone;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100f;
        //rb = RigidBody
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        print(playerHealth);
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
                }

                else if (_directionHorizontal < 0f)
                {
                    _rigidbody.velocity = new Vector2(_directionHorizontal * speed, _rigidbody.velocity.x);
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
                }
                else if (_directionVertical < 0f)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _directionVertical * speed);
                }
                else
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    ShoveZone.SetActive(true);
                    StartCoroutine(DisableAfterDelay());
                }
            }
            
         





        }
    }
    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        ShoveZone.SetActive(false);
    }

    private Coroutine _stunTime;
    
    private IEnumerator ShoveTime()
    {
        ai.speed += 1;
        AI_Speed_Recharge += 1;
        yield return new WaitForSeconds(1f);
        
        if (AI_Speed_Recharge == 4)
        {
            StopCoroutine(_stunTime);
            _stunTime = null;
        }
    }
}
    

