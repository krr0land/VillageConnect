using UnityEngine;

public class PathBuilder : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private WorldMap worldMap;

    [SerializeField] private GameObject hoverElement;

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetCursorPosition());

        if (!Physics.Raycast(ray, out var hit))
            return;

        var position = hit.point;

        var coordinates = grid.GetNearestCoordinate(position);
        grid.PositionElement(coordinates, hoverElement.transform, new Vector3(0, -0.4f, 0));

        if (InputManager.Instance.IsBuilding())
        {
            var tileType = worldMap.GetTile(coordinates);
            if (tileType == TileType.Empty)
                worldMap.SetTile(coordinates, TileType.Road);
        }
        else if (InputManager.Instance.IsDestroying())
        {
            var tileType = worldMap.GetTile(coordinates);
            if (tileType == TileType.Road)
                worldMap.SetTile(coordinates, TileType.Empty);
        }
    }
}
