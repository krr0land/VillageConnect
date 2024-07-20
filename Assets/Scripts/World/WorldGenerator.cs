using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private WorldMap grid;

    private void Start()
    {
        var res = FastPoissonDiskSampling.FastPoissonDiskSampling.Sampling(new Vector2(1, 1), new Vector2(grid.Size-1, grid.Size-1), 4);

        foreach (var point in res)
        {
            var random = Random.Range(0f, 1f);
            var tileType = random > 0.6f ? TileType.Village : TileType.Obstacle;
            grid.SetTile((int)point.x, (int)point.y, tileType);
        }
    }
}
