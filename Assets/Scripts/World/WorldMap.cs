using System;
using UnityEngine;

public enum TileType
{
    Empty,
    Village,
    Road,
    Obstacle
}

public class WorldMap : MonoBehaviour
{
    [SerializeField] private int size = 100;

    private TileType[] tiles;

    public EventHandler RefreshAllTiles;
    public EventHandler<Vector2Int> RefreshTile;

    public int Size => size;
    public int TileCount => tiles.Length;
    public TileType[] Tiles => tiles;

    private void Awake()
    {
        tiles = new TileType[size * size];
        for (var x = 0; x < size; x++)
            for (var y = 0; y < size; y++)
                tiles[x + y * size] = TileType.Empty;
    }

    #region Get/Set Tile
    public TileType GetTile(Vector2Int coordinate)
    {
        return tiles[coordinate.x + coordinate.y * size];
    }

    public TileType GetTile(int x, int y)
    {
        return tiles[x + y * size];
    }

    public void SetTile(Vector2Int coordinate, TileType tileType)
    {
        tiles[coordinate.x + coordinate.y * size] = tileType;
        RefreshTile?.Invoke(this, coordinate);
    }

    public void SetTile(int x, int y, TileType tileType)
    {
        tiles[x + y * size] = tileType;
        RefreshTile?.Invoke(this, new Vector2Int(x, y));
    }
    #endregion
}
