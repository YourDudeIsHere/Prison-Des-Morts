using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool AbleToShove;
    public bool canSwing = true;
    public bool AxeDamage;
    public AI ai;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && !ai.IsGrabbed)
        {
            animator.SetTrigger("AxeShove");
            Debug.Log("Axe Shove is true");
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && canSwing)
        {
            if (!ai.IsGrabbed)
            {
                animator.SetTrigger("AxeSwing");
                Debug.Log("Axe Swing is true");
            }
        }
    }

    
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();
    public void OnTriggerStay2D(Collider2D other)
    {
        if (AxeDamage && other.CompareTag("AI"))
        {
            if (!hitEnemies.Contains(other))
            {
                //Only allows for Ai to be hit once 
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
