using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject wallLeft;
    public GameObject wallRight;
    public GameObject wallTop;
    public GameObject wallBottom;

    public int Width = 20;
    
    public int Height = 10;

    public int X;

    public int Y;

    private bool UpdatedDoors = false;

    public Room(int X, int Y)
    { 
        this.X = X;
        this.Y = Y;
    }
    
    public Door leftDoor;
    public Door rightDoor;
    public Door bottomDoor;
    public Door topDoor;
    
    public List<Door> doors = new List<Door>();
    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.instance == null)
        {
            Debug.LogError("Wrong scene to press play in.");
            return;
        }
        
        
        Door[] ds = GetComponentsInChildren<Door>();
        foreach (Door d in ds)
        {
            doors.Add(d);
            switch (d.doorType)
            {
                case Door.DoorType.right: 
                    rightDoor = d;
                    break;  
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
            }
        }
        
        RoomController.instance.RegisterRoom(this);
    }

    void Update()
    {
       if(name.Contains("End") && !UpdatedDoors)
       {
           RemoveUnconnectedDoors();
           UpdatedDoors = true;
       }
    }
    
    public void RemoveUnconnectedDoors()
    {
        foreach (Door door in doors)
        {
            switch (door.doorType)
            {
                case Door.DoorType.right: 
                    if (GetRight() == null)
                        door.gameObject.SetActive(false);
                    else 
                    if (wallRight != null) wallRight.GetComponent<Collider2D>().enabled = false;
                    break;  
                case Door.DoorType.left:
                    if (GetLeft() == null)
                        door.gameObject.SetActive(false);
                    else
                    if (wallLeft != null) wallLeft.GetComponent<Collider2D>().enabled = false;
                    break;
                case Door.DoorType.bottom:
                    if (GetBottom() == null)
                        door.gameObject.SetActive(false);
                    else
                    if (wallBottom != null) wallBottom.GetComponent<Collider2D>().enabled = false;
                    break;
                case Door.DoorType.top:
                    if (GetTop() == null)
                        door.gameObject.SetActive(false);
                    else
                    if (wallTop != null) wallTop.GetComponent<Collider2D>().enabled = false;
                    break;
            }
        }
    }

    public Room GetRight()
    {
        if(RoomController.instance.DoesRoomExist(X+1, Y))
        {
            return RoomController.instance.FindRoom(X+1, Y);
        }
        return null;
    }
    public Room GetLeft()
    {
        if(RoomController.instance.DoesRoomExist(X-1, Y))
        {
            return RoomController.instance.FindRoom(X-1, Y);
        }
        return null; 
    }
    public Room GetTop()
    {
        if(RoomController.instance.DoesRoomExist(X, Y+1))
        {
            return RoomController.instance.FindRoom(X, Y+1);
        }
        return null;
    }
    public Room GetBottom()
    {
        if(RoomController.instance.DoesRoomExist(X, Y-1))
        {
            return RoomController.instance.FindRoom(X, Y-1);
        }
        return null; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3(X * Width, Y * Height);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
}
