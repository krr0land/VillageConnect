using UnityEditor;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private int size = 100;

    [SerializeField] private float cellSize = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Handles.color = Color.blue;
        for (var x = 0; x < size; x++)
        {
            for (var z = 0; z < size; z++)
            {
                var position = new Vector3(x * cellSize, 0, z * cellSize);

                Gizmos.DrawWireCube(transform.position + position, new Vector3(cellSize, 0, cellSize));
                var coord = GetNearestCoordinate(transform.position + position);
                Handles.Label(transform.position + position, $"{coord.x}, {coord.y}");
            }
        }
    }

    public Vector2 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        var xCount = Mathf.RoundToInt(position.x / cellSize);
        var x = xCount * cellSize;

        var zCount = Mathf.RoundToInt(position.z / cellSize);
        var z = zCount * cellSize;

        return new Vector2(x, z);
    }

    public Vector2Int GetNearestCoordinate(Vector3 position)
    {
        position -= transform.position;
        var xCount = Mathf.RoundToInt(position.x / cellSize);
        var zCount = Mathf.RoundToInt(position.z / cellSize);

        return new Vector2Int(xCount, zCount);
    }

    public Vector3 GetWorldPosition(Vector2Int coordinate)
    {
        var position = new Vector3(coordinate.x * cellSize, 0, coordinate.y * cellSize);
        return transform.position + position;
    }

    public void PositionElement(Vector2Int coordinate, Transform element, Vector3 offset = new Vector3())
    {
        element.position = GetWorldPosition(coordinate) + offset;
    }
}
