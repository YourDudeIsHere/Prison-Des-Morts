using UnityEngine;

public class CameraController : MonoBehaviour
{
    public AI ai;
    public GameObject player;
    public static CameraController instance;
    public Room currentRoom;
    
    public float moveSpeedWhenRoomChange;
    
    public float zoomSpeed = 1f; // Speed of zooming
    public float defaultZoom = 7f; // Default zoom level
    public float grabZoom = 3f; // Zoom level when the player is grabbed
    
    private float targetZoom; // The zoom level the camera is moving towards


    private void Awake()
    {
        instance = this;
        //Intialize with the default zoom level
        targetZoom = defaultZoom; 
    }
    

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        //Method that is used to zoom in when the player is grabbed
        UpdateZoom();
    }

    void UpdatePosition()
    {
        if(currentRoom == null)
        {
            return;
        }
        
        Vector3 targetPos = GetCameraTargetPosition();
        
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
    }
    
  void UpdateZoom()
  {
      if (ai.IsGrabbing)
      {
          // Smoothly increases the camera's zoom level towards the grab zoom level
          targetZoom = grabZoom;
          //The focus of the camera is set to the camera's position 
          Vector3 targetPosition = player.transform.position;
          targetPosition.z = transform.position.z; // Keep the camera's Z position unchanged
          transform.position = targetPosition; //Puts the transform of the camera to the player 
          transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * zoomSpeed); //Uses a timer to slowly zoom into the player
      }
      else
      {
          //Method used to keep the camera
          ResetCaneraZoom();
      }
  
      // Smoothly interpolate the camera's zoom level towards the target zoom level
      Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
  }

    void ResetCaneraZoom()
    {
        targetZoom = defaultZoom;
        // Smoothly interpolate the camera's zoom level back to the default zoom
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, defaultZoom, Time.deltaTime * zoomSpeed);
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null)
        {
            return Vector3.zero;
        }
        Vector3 targetPos = currentRoom.GetRoomCentre();
        targetPos.z = transform.position.z;
        
        return targetPos;
    }
    
    public bool IsSwitchingScene()
    {
        return transform.position.Equals( GetCameraTargetPosition()) == false;
    }
}
