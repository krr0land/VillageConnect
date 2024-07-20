using System;
using UnityEngine;

public enum TileType
{
    Empty,
    Village,
    Road,
    Obstacle
}

public abstract class GameTile
{
    public abstract TileType TileType { get; }

    public Vector2Int Coordinate { get; set; }

    public EventHandler UpdateTile;

    public GameTile()
    {
        Coordinate = new Vector2Int();
    }

    public GameTile(Vector2Int coordinate)
    {
        Coordinate = coordinate;
    }

    public GameTile(int x, int y)
    {
        Coordinate = new Vector2Int(x, y);
    }
}

public class EmptyTile : GameTile
{
    public override TileType TileType => TileType.Empty;

    public EmptyTile() { }
    public EmptyTile(Vector2Int coordinate) : base(coordinate) { }
    public EmptyTile(int x, int y) : base(x, y) { }
}

public class VillageTile : GameTile
{
    public override TileType TileType => TileType.Village;

    public VillageTile() { }
    public VillageTile(Vector2Int coordinate) : base(coordinate) { }
    public VillageTile(int x, int y) : base(x, y) { }

    private bool connected = false;

    public bool Connected
    {
        get => connected;
        set
        {
            connected = value;
            UpdateTile?.Invoke(this, EventArgs.Empty);
        }
    }
}

public class RoadTile : GameTile
{
    public override TileType TileType => TileType.Road;

    public RoadTile() { }
    public RoadTile(Vector2Int coordinate) : base(coordinate) { }
    public RoadTile(int x, int y) : base(x, y) { }
}

public class ObstacleTile : GameTile
{
    public override TileType TileType => TileType.Obstacle;

    public ObstacleTile() { }
    public ObstacleTile(Vector2Int coordinate) : base(coordinate) { }
    public ObstacleTile(int x, int y) : base(x, y) { }
}
