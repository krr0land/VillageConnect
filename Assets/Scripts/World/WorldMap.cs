using QuikGraph;
using QuikGraph.Algorithms.ConnectedComponents;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    [SerializeField] private int size = 100;

    private GameTile[,] tiles;

    private UndirectedGraph<GameTile, Edge<GameTile>> tileGraph;

    private List<VillageTile> villages = new List<VillageTile>();

    public EventHandler RefreshAllTiles;
    public EventHandler<Vector2Int> RefreshTile;
    public EventHandler RefreshTexts;

    public int Roads { get; private set; }
    public int ConnectedVillages { get; private set; }

    public int TotalVillages => villages.Count;
    public int Size => size;
    public int TileCount => tiles.Length;
    public GameTile[,] Tiles => tiles;

    private void Awake()
    {
        tiles = new GameTile[size, size];
        for (var x = 0; x < size; x++)
            for (var y = 0; y < size; y++)
                tiles[x, y] = new EmptyTile(x, y);

        tileGraph = new UndirectedGraph<GameTile, Edge<GameTile>>();
    }

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
        var oldTile = tiles[x, y];

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

        if (tileType == TileType.Road)
            Roads++;

        if (tileType == TileType.Village || tileType == TileType.Road)
        {
            tileGraph.AddVertex(tiles[x, y]);

            foreach (var neighbor in Neighbors.Cardinals)
            {
                var neighborTile = GetTile(x + neighbor.x, y + neighbor.y);
                if (neighborTile == null) continue;
                if (neighborTile.TileType == TileType.Road || neighborTile.TileType == TileType.Village)
                {
                    tileGraph.AddEdge(new Edge<GameTile>(tiles[x, y], neighborTile));
                    //tileGraph.AddEdge(new Edge<GameTile>(neighborTile, tiles[x, y]));
                }
            }
        }
        else if (oldTile != null && oldTile.TileType == TileType.Road && tileType != TileType.Road) // remove old
        {
            Roads--;
            tileGraph.RemoveVertex(oldTile);
        }

        if (tileType == TileType.Village)
            villages.Add((VillageTile)tiles[x, y]);
        else if (oldTile != null && oldTile.TileType == TileType.Village && tileType != TileType.Village)
            villages.Remove((VillageTile)oldTile);

        if (tileType == TileType.Road || tileType == TileType.Empty)
            RecalculateConnection();

        RefreshTile?.Invoke(this, new Vector2Int(x, y));
        RefreshTexts?.Invoke(this, new EventArgs());
    }

    public void RecalculateConnection()
    {
        // A village is connected if it is connected to any another village
        var connectedComponents = new ConnectedComponentsAlgorithm<GameTile, Edge<GameTile>>(tileGraph);
        connectedComponents.Compute();

        var components = connectedComponents.Components; // stores vertec, component id pairs
        
        // reverse the dictionary to get a list of connected vertices for each component
        var componentVertices = new Dictionary<int, List<GameTile>>();
        foreach (var component in components)
        {
            if(component.Key.TileType != TileType.Village) continue;

            if (!componentVertices.ContainsKey(component.Value))
                componentVertices[component.Value] = new List<GameTile>();
            componentVertices[component.Value].Add(component.Key);
        }

        ConnectedVillages = 0;
        foreach (var village in villages)
        {
            village.Connected = componentVertices[components[village]].Count > 1;
            if (village.Connected)
                ConnectedVillages++;
        }
    }
}
