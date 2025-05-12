using UnityEngine;

[CreateAssetMenu(fileName = "Spawner.asset", menuName = "Spawner")]

public class SpawnData : ScriptableObject
{
   public GameObject itemToSpawn;
   
   public int minSpawnAmount;
   public int maxSpawnAmount;
}
