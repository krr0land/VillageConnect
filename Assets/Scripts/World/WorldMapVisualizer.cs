using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapVisualizer : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private WorldMap worldMap;
    [SerializeField] private Transform parent;

    [Header("Prefabs")]
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private GameObject villagePrefab;
    [SerializeField] private GameObject obstaclePrefab;

    [SerializeField] private GameObject[] roadPrefabs;

    private GameObject[,] gridElements;

    public int Size => worldMap.Size;

    private static Vector2Int[] neighbors8 = new Vector2Int[]
    {
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1)
    };

    private static Vector2Int[] neighbors4 = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
    };

    private void Awake()
    {
        worldMap.RefreshAllTiles += OnRefreshAllTiles;
        worldMap.RefreshTile += OnRefreshTile;
        gridElements = new GameObject[worldMap.Size, worldMap.Size];

        // Spawn empty grass
        for (var x = 0; x < Size; x++)
            for (var y = 0; y < Size; y++)
            {
                var obj = Instantiate(grassPrefab, grid.GetWorldPosition(new Vector2Int(x, y)), Quaternion.identity, parent);
                gridElements[x, y] = obj;
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
        var tile = worldMap.GetTile(coordinate);
        var pos = grid.GetWorldPosition(coordinate);

        if (gridElements[coordinate.x, coordinate.y] != null)
        {
            Destroy(gridElements[coordinate.x, coordinate.y]);
            gridElements[coordinate.x, coordinate.y] = null;
        }

        switch (tile.TileType)
        {
            case TileType.Empty:
                gridElements[coordinate.x, coordinate.y] = Instantiate(grassPrefab, pos, Quaternion.identity, parent);
                break;
            case TileType.Village:
                gridElements[coordinate.x, coordinate.y] = Instantiate(villagePrefab, pos, Quaternion.identity, parent);
                var visualizer = gridElements[coordinate.x, coordinate.y].GetComponentInChildren<VillageTileVisualizer>();
                visualizer.SetTile((VillageTile)tile);
                break;
            case TileType.Road:
                InstantiateRoad(coordinate, pos);
                break;
            case TileType.Obstacle:
                gridElements[coordinate.x, coordinate.y] = Instantiate(obstaclePrefab, pos, Quaternion.identity, parent);
                break;
            default:
                gridElements[coordinate.x, coordinate.y] = Instantiate(grassPrefab, pos, Quaternion.identity, parent);
                break;
        }

        RefreshNeighborRoads(coordinate);
    }

    // Determines which road prefab to place based on the surrounding tiles
    // 1 2 3
    // 4 . 5
    // 6 7 8
    // Representation: 12345678 where 0 is empty/obstacle/village, 1 is road
    // Only 2457 are used, due to too many cases (this is 16 instead of 256 xd)
    private void InstantiateRoad(Vector2Int coordinate, Vector3 position)
    {
        if (gridElements[coordinate.x, coordinate.y] != null)
            Destroy(gridElements[coordinate.x, coordinate.y]);

        var directNeighbors = new HashSet<int>();
        for (var i = 0; i < 4; i++)
        {
            var neighbor = coordinate + neighbors4[i];
            var tile = worldMap.GetTile(neighbor);
            if (tile != null && tile.TileType == TileType.Road)
                directNeighbors.Add(Neigbor4to8converter[i]);
        }

        var roadPrefab = roadPrefabs[11];
        var rotationY = 0f;

        var configuration = GenerateConfiguration(directNeighbors);
        byte configurationByte = Convert.ToByte(configuration, 2);

        // Debug.Log(configuration + " " + coordinate);

        if (RoadConfiguration.configuration.ContainsKey(configurationByte))
        {
            int index;
            (index, rotationY) = RoadConfiguration.configuration[configurationByte];
            roadPrefab = roadPrefabs[index];
        }
        
        var rotation = Quaternion.Euler(new Vector3(0, rotationY, 0));
        gridElements[coordinate.x, coordinate.y] = Instantiate(roadPrefab, position, rotation, parent);
    }

    private void RefreshNeighborRoads(Vector2Int coordinate)
    {
        foreach (var neighbor in neighbors8)
        {
            var neighborCoordinate = coordinate + neighbor;
            var pos = grid.GetWorldPosition(neighborCoordinate);
            var tile = worldMap.GetTile(neighborCoordinate);
            if (tile != null && tile.TileType == TileType.Road)
                InstantiateRoad(neighborCoordinate, pos);
        }
    }

    private static string GenerateConfiguration(HashSet<int> indexes)
    {
        var configuration = "00000000";
        foreach (var index in indexes)
            configuration = configuration.Substring(0, index) + "1" + configuration.Substring(index + 1);
        return configuration;
    }

    private Dictionary<int, int> Neigbor4to8converter = new Dictionary<int, int>
    {
        { 0, 1 }, 
        { 1, 3 },
        { 2, 4 },
        { 3, 6 }
    };
}
