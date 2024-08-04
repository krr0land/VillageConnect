using UnityEngine;
using UnityEngine.EventSystems;

public class PathBuilder : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private WorldMap worldMap;

    [SerializeField] private GameObject hoverElement;

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
           hoverElement.SetActive(false);
            return;
        }

        var ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetCursorPosition());

        if (!Physics.Raycast(ray, out var hit))
        {
            hoverElement.SetActive(false);
            return;
        }

        var position = hit.point;

        var coord3D = grid.WorldToCell(position);
        var coordinates = new Vector2Int(coord3D.x, coord3D.z);

        if (!worldMap.IsValid(coordinates))
        {
            hoverElement.SetActive(false);
            return;
        }

        hoverElement.SetActive(true);
        hoverElement.transform.position = grid.GetCellCenterWorld(coord3D) + new Vector3(0, -0.4f, 0);
        //grid.PositionElement(coordinates, hoverElement.transform, new Vector3(0, -0.4f, 0));

        if (InputManager.Instance.IsBuilding())
        {
            var tileType = worldMap.GetTile(coordinates).TileType;
            if (tileType == TileType.Empty)
                worldMap.SetTile(coordinates, TileType.Road);
        }
        else if (InputManager.Instance.IsDestroying())
        {
            var tileType = worldMap.GetTile(coordinates).TileType;
            if (tileType == TileType.Road)
                worldMap.SetTile(coordinates, TileType.Empty);
        }
    }
}
