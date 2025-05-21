using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Player playerscript;
    public GameObject panel;
    public AI ai;
    public Image grabButton;

    public float grabScore;

    private bool isPanelActive = false; // Track the panel's state

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Update()
    {
        if (isPanelActive && Input.GetKeyDown(KeyCode.E))
        {
            grabScore -= 1;
            if (Input.GetKeyDown(KeyCode.E))
            {
                grabButton.transform.localScale = new Vector2(0.95f, 0.95f);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                grabButton.transform.localScale = new Vector2(1f, 1f);
            }

            if (grabScore <= 0)
            {
                HideGrabUI();
                ai.Release();
            }
        }
    }

    void Start()
    {

        // Find the panel (GrabbedUI)
        if (panel == null)
        {
            panel = GameObject.Find("GrabbedUI");
            if (panel == null)
            {
                Debug.LogError("Panel (GrabbedUI) not found!");
            }
        }
        if (playerscript == null)
        {
            playerscript = FindObjectOfType<Player>();
            if (playerscript == null)
            {
                Debug.LogError("Player script not found!");
            }
        }
        if (ai == null)
        {
            ai = FindObjectOfType<AI>();
            if (ai == null)
            {
                Debug.LogError("AI script not found!");
            }
        }

        // Find the grabButton as a child of the panel
        if (panel != null && grabButton == null)
        {
            Transform grabButtonTransform = panel.transform.Find("GrabButton");
            if (grabButtonTransform != null)
            {
                grabButton = grabButtonTransform.GetComponent<Image>();
            }
            else
            {
                Debug.LogError("GrabButton not found as a child of GrabbedUI!");
            }
        }

        if (panel != null)
        {
            panel.SetActive(false);
        }
        grabScore = 10;
    }


    public void ShowGrabbedUI()
    {
        if (panel != null)
        {
            grabScore = 10;
            panel.SetActive(true);
            isPanelActive = true;
        }
    }

    public void HideGrabUI()
    {
        isPanelActive = false;
        panel.SetActive(false);
        ai.IsGrabbing = false;
        playerscript.isGrabbed = false;
    }

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