using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public static RoomController instance;

    private string CurrentWorldName = "Prison";

    private RoomInfo[] currentLoadRoomData;
    
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    
    public List<Room> loadedRooms = new List<Room>();
    
    bool isLoadingRoom = false;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        LoadRoom("StartRoom", 0, 0);
        LoadRoom("Empty", 1, 0);
        LoadRoom("Empty", 2, 0);
    }

    public void Update()
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
        
    }

    public void LoadRoom(string name, int x, int y)
    {
        //Prevents Overlapping Rooms
        if (DoesRoomExist(x, y))
        {
            return;
        }
        
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;
        
        //Adds the room to the queue
        loadRoomQueue.Enqueue(newRoomData);
        
        if (!isLoadingRoom)
        {
            StartCoroutine(LoadRoomCoroutine(loadRoomQueue.Dequeue()));
        }
    }

    IEnumerator LoadRoomCoroutine(RoomInfo info)
    {
        string roomName = CurrentWorldName + info.name;
        
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }
    
    public void RegisterRoom(Room room)
    {
        room.transform.position = new Vector3(
            currentLoadRoomData[0].X * room.Width,
            currentLoadRoomData[0].Y * room.Height,
            0
            );

        room.X = currentLoadRoomData[0].X;
        room.Y = currentLoadRoomData[0].Y;
        room.name = CurrentWorldName+ "-" + currentLoadRoomData[0].name +" " + room.X + "," + room.Y;
        room.transform.parent = transform;

        isLoadingRoom = false;

        loadedRooms.Add(room);
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y) != null;
    }
}
