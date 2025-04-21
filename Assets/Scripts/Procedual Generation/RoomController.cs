using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo
{
    /// This class is used to store information about the room such as if it has a specific function like being the start room, or an item room.
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{
    private Room _room;

    public static RoomController instance;

    private string CurrentWorldName = "Prison";

    private RoomInfo currentLoadRoomData;
    
    Room currentRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    private void Awake()
    {
        instance = this;
    }

 

    private void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }
        
        if(loadRoomQueue.Count == 0)
        {
            return;
        }
        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y))
        {
            return;
        }
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;
        
        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo roomInfo)
    {
        string roomName = CurrentWorldName + roomInfo.name;
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (asyncLoad != null && !asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {


            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0);
            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = CurrentWorldName + "/" + room.name;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }

            loadedRooms.Add(room);
            room.RemoveUnconnectedDoors();
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }
    
    public bool DoesRoomExist( int x, int y)
    {
        foreach (Room room in loadedRooms)
        {
            if (room.X == x && room.Y == y)
            {
                return true;
            }
        }

        return false;
    }
    public Room FindRoom( int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }
    
    public void OnPlayerEnterRoom(Room room)
    {
        if (room == null)
        {
            return;
        }
        
        CameraController.instance.currentRoom = room;
        currentRoom = room;
    }
}