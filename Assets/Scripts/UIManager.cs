using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;


public class UIManager : MonoBehaviour
{
    public GameObject panel;
    public AI ai;
   public Image grabButton;

    public float grabScore;
    // Start is called before the first frame update
    void Start()
    {
        grabScore = 10;
    }

    // Update is called once per frame
    void Update()
    {
    
        if(ai.IsGrabbed)
        {
            panel.SetActive(true);
        }
        if(ai.IsGrabbed == false)
        {
            panel.SetActive(false);
            grabScore = 10;
        }

        // When E is pressed, take one away from the current grab score
        if(panel.activeSelf)
        {
            HandleButtonSize();
            if (Input.GetKeyDown(KeyCode.E))
            {
                grabScore -= 1;
            }
        }
        if(grabScore == 0)
        {
            ai.IsGrabbed = false;
        }
        //Function used to shrink and rezise the button when E is pressed. Used for better visual feedback.
        void HandleButtonSize()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                grabButton.transform.localScale = new Vector2(0.95f, 0.95f);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                grabButton.transform.localScale = new Vector2(1f, 1f);
            }
        }
    }
   
}
