using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonGenerator : MonoBehaviour
{
  public PrisonGenerationData prisonGenerationData;
  private List<Vector2Int> prisonRooms;

  private void Start()
  {
    prisonRooms = PrisonCrawlerController.GeneratePrison(prisonGenerationData);
    SpawnRooms(prisonRooms);
  }

  private void SpawnRooms(IEnumerable<Vector2Int> rooms)
  {
    RoomController.instance.LoadRoom("Start", 0, 0);
    foreach (Vector2Int roomLocation in rooms)
    {
      // This is used to check if the rooms list is at the last room and it is not at 0,0 which ends up spawing the end room.
      if (roomLocation == prisonRooms[prisonRooms.Count - 1] && !(roomLocation == Vector2Int.zero))
      {
        RoomController.instance.LoadRoom("End", roomLocation.x, roomLocation.y);
      }
      else
      {
        RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);
      }
      
    }
  }
}
