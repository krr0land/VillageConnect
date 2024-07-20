using QuikGraph;
using System;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    [SerializeField] private int size = 100;

    private GameTile[,] tiles;

    private UndirectedGraph<VillageTile, Edge<VillageTile>> villageGraph;

    public EventHandler RefreshAllTiles;
    public EventHandler<Vector2Int> RefreshTile;

    public int Size => size;
    public int TileCount => tiles.Length;
    public GameTile[,] Tiles => tiles;

    private void Awake()
    {
        tiles = new GameTile[size, size];
        for (var x = 0; x < size; x++)
            for (var y = 0; y < size; y++)
                tiles[x, y] = new EmptyTile(x, y);

        villageGraph = new UndirectedGraph<VillageTile, Edge<VillageTile>>();
    }

    #region Get/Set Tile
    public bool IsValid(Vector2Int coordinates) => IsValid(coordinates.x, coordinates.y);
    public bool IsValid(int x, int y) => x < 0 || x >= size || y < 0 || y >= size;

    public GameTile GetTile(Vector2Int coordinate) => GetTile(coordinate.x, coordinate.y);
    public GameTile GetTile(int x, int y)
    {
        if (x < 0 || x >= size || y < 0 || y >= size)
            return null;
        return tiles[x, y];
    }

    public void SetTile(Vector2Int coordinate, TileType tileType) => SetTile(coordinate.x, coordinate.y, tileType);

    public void SetTile(int x, int y, TileType tileType)
    {
        //if(tiles[x + y * size].TileType == TileType.Village) return;

        switch (tileType)
        {
            case TileType.Empty:
                tiles[x, y] = new EmptyTile(x, y);
                break;
            case TileType.Village:
                tiles[x, y] = new VillageTile(x, y);
                break;
            case TileType.Road:
                tiles[x, y] = new RoadTile(x, y);
                break;
            case TileType.Obstacle:
                tiles[x, y] = new ObstacleTile(x, y);
                break;
            default:
                tiles[x, y] = new EmptyTile(x, y);
                break;
        }

        if (tileType == TileType.Village)
            villageGraph.AddVertex((VillageTile)tiles[x, y]);

        if (tileType == TileType.Road || tileType == TileType.Empty)
            RecalculateConnection();

        RefreshTile?.Invoke(this, new Vector2Int(x, y));
    }
    #endregion

    public void RecalculateConnection()
    {
        // TMP CODE
        var neighbors = new Vector2Int[]
        {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1)
        };

        foreach (var village in villageGraph.Vertices)
        {
            var connected = false;
            foreach (var neighbor in neighbors)
            {
                var neighborTile = GetTile(village.Coordinate.x + neighbor.x, village.Coordinate.y + neighbor.y);
                if (neighborTile == null) continue;
                if (neighborTile.TileType == TileType.Road)
                {
                    connected = true;
                    break;
                }
            }

            village.Connected = connected;
        }

    }
}
