
using UnityEngine;

[CreateAssetMenu(fileName = "ProcedualGenerationData.asset", menuName = "ProcedualGenerationData/Prison Data")]
public class PrisonGenerationData : ScriptableObject
{
    public int numberOfCrawlers;
    
    //This is the number of rooms that will be generated in the world at minimum.
    public int iterationMin;
    //Same as above just instead of minimum it is maximum.
    public int iterationMax;
}
