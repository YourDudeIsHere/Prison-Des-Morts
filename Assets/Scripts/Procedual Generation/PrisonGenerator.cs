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
      
      RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);
      
      
    }
  }
}
