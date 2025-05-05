using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public bool AbleToShove;
    public bool canSwing = true;
    public bool AxeDamage;
    public AI ai;
    Animator animator;

    private PlayerControls controls;

    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    private void Awake()
    {
        controls = new PlayerControls();

        // Bind inputs
        controls.Gameplay.Shove.performed += ctx => TryShove();
        controls.Gameplay.Swing.performed += ctx => TrySwing();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void TryShove()
    {
        if (!ai.IsGrabbing && AbleToShove)
        {
            animator.SetTrigger("AxeShove");
            Debug.Log("Axe Shove is true");
        }
    }

    private void TrySwing()
    {
        if (canSwing && !ai.IsGrabbing)
        {
            animator.SetTrigger("AxeSwing");
            Debug.Log("Axe Swing is true");
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (AxeDamage && other.CompareTag("AI"))
        {
            if (!hitEnemies.Contains(other))
            {
                other.GetComponent<AI>().AIhealth -= 5f;
                hitEnemies.Add(other);
            }
        }
    }

    public void AxeStopSwinging()
    {
        canSwing = true;
        AbleToShove = true;
    }

    public void AxeStartSwinging()
    {
        canSwing = false;
        AxeDamage = true;
        hitEnemies.Clear();
    }

    public void AxeHitbox()
    {
        AxeDamage = true;
    }

    public void AxeHitboxEnd()
    {
        AxeDamage = false;
    }

    public void AxeStartShove()
    {
        canSwing = false;
        AbleToShove = false;
    }

    public void AxeEndShove()
    {
        AbleToShove = true;
        canSwing = true;
    }
}