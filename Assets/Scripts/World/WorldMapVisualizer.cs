using UnityEngine;

public class WorldMapVisualizer : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private WorldMap worldMap;
    [SerializeField] private Transform parent;

    [Header("Prefabs")]
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private GameObject villagePrefab;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject obstaclePrefab;

    private GameObject[] gridElements;

    public int Size => worldMap.Size;

    private void Awake()
    {
        worldMap.RefreshAllTiles += OnRefreshAllTiles;
        worldMap.RefreshTile += OnRefreshTile;
        gridElements = new GameObject[worldMap.Size * worldMap.Size];

        // Spawn empty grass
        for (var x = 0; x < Size; x++)
            for (var y = 0; y < Size; y++)
            {
                var obj = Instantiate(grassPrefab, grid.GetWorldPosition(new Vector2Int(x, y)), Quaternion.identity, parent);
                gridElements[x + y * Size] = obj;
            }
    }

    private void OnRefreshAllTiles(object sender, System.EventArgs e)
    {
        for (var x = 0; x < Size; x++)
            for (var y = 0; y < Size; y++)
                OnRefreshTile(sender, new Vector2Int(x, y));
    }

    private void OnRefreshTile(object sender, Vector2Int coordinate)
    {
        var tileType = worldMap.GetTile(coordinate);
        var pos = grid.GetWorldPosition(coordinate);

        if (gridElements[coordinate.x + coordinate.y * Size] != null)
            Destroy(gridElements[coordinate.x + coordinate.y * Size]);

        GameObject obj;
        switch (tileType)
        {
            case TileType.Empty:
                obj = Instantiate(grassPrefab, pos, Quaternion.identity, parent);
                break;
            case TileType.Village:
                obj = Instantiate(villagePrefab, pos, Quaternion.identity, parent);
                break;
            case TileType.Road:
                obj = Instantiate(roadPrefab, pos, Quaternion.identity, parent);
                break;
            case TileType.Obstacle:
                obj = Instantiate(obstaclePrefab, pos, Quaternion.identity, parent);
                break;
            default:
                obj = Instantiate(grassPrefab, pos, Quaternion.identity, parent);
                break;
        }
        gridElements[coordinate.x + coordinate.y * Size] = obj;
    }
}
