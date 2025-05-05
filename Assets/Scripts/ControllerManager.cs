using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public Player Player;
    public Animation controllerImage; // Reference to the Animation component
    public AnimationClip animationClip; // Reference to the animation clip

    void Start()
    {
        // Add the animation clip to the Animation component
        controllerImage.AddClip(animationClip, "PlayOnce");
    }

    void Update()
    {
        //if (Player != null && Player.IsControllerInputDetected())
        {
            // Play the animation
            controllerImage.Play("ControllerConnected");
        }
    }

    
    public void AfterConnection()
    {
        // Play the animation when the controller is connected
        Player.DisableControllerConnectedAnimation = true;
    }
}
