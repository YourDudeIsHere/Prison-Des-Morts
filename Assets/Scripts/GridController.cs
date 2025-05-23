
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    [System.Serializable]

    public struct Grid
    {
        public int columns, rows;
        
        public float verticalOffset, horizontalOffset;
    }
    public Grid grid;
    public GameObject gridTile;
    public List<Vector2> availablePoints = new List<Vector2>();
    
    void Start()
    {
        if (room == null) // Only assign if room is not already set
        {
            room = GetComponentInParent<Room>();
            if (room == null)
            {
                Debug.LogError($"Room component not found on parent or ancestor of {gameObject.name}. Parent: {transform.parent?.name ?? "None"}");
                return;
            }
        }

        grid.columns = room.Width - 2;
        grid.rows = room.Height - 2;
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid.verticalOffset += room.transform.localPosition.y;
        grid.horizontalOffset += room.transform.localPosition.x;
        
        for (int y = 0; y < grid.rows; y++)
        {
            for (int x = 0; x < grid.columns; x++)
            {
                GameObject go = Instantiate(gridTile, transform);
                go.GetComponent<Transform>().position = new Vector2(x - (grid.columns - grid.horizontalOffset), y - (grid.rows - grid.verticalOffset));
                go.name = "X "+ x + " Y " + y;
                availablePoints.Add(go.transform.position);
            }
        }
    }
}
