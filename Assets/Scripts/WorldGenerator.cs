using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private GameGrid grid;

    private void Start()
    {
        foreach(Transform child in transform)
        {
            var coordinates = grid.GetNearestCoordinate(child.position);
            grid.PositionElement(coordinates, child, new Vector3(0, -0.1f, 0));
        }
    }
}
