using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3
};

public class PrisonCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();
    
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up },
        {Direction.left, Vector2Int.left },
        {Direction.down, Vector2Int.down },
        {Direction.right, Vector2Int.right }
    };

    public static List<Vector2Int> GeneratePrison(PrisonGenerationData data)
    {
        List<PrisonCrawler> prisonCrawlers = new List<PrisonCrawler>();

        for(int i = 0; i < data.numberOfCrawlers; i++)
        {
            prisonCrawlers.Add(new PrisonCrawler(Vector2Int.zero));
        }
        
        int iterations = Random.Range(data.iterationMin, data.iterationMax);

        for (int i = 0; i < iterations; i++)
        {
            foreach (PrisonCrawler prisonCrawler in prisonCrawlers)
            {
                Vector2Int newPosition = prisonCrawler.Move(directionMovementMap);
                positionsVisited.Add(newPosition);
            }
        }
        return positionsVisited;
    }
}
