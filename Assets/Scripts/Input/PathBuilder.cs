using UnityEngine;

public class PathBuilder : MonoBehaviour
{
    [SerializeField] private GameGrid grid;

    [SerializeField] private GameObject hoverElement;

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit))
            return;

        var position = hit.point;

        var coordinates = grid.GetNearestCoordinate(position);
        grid.PositionElement(coordinates, hoverElement.transform, new Vector3(0, 0.1f, 0));
    }
}
