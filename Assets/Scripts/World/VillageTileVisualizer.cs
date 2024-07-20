using UnityEngine;

public class VillageTileVisualizer : MonoBehaviour
{
    private VillageTile tile;

    [SerializeField] private Sprite ConnectedTrue;
    [SerializeField] private Sprite ConnectedFalse;

    public void SetTile(VillageTile tile)
    {
        if (this.tile != null)
            this.tile.UpdateTile -= UpdateTile;
        this.tile = tile;
        tile.UpdateTile += UpdateTile;
        UpdateTile(this, null);
    }

    private void UpdateTile(object sender, System.EventArgs e)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (tile.Connected)
        {
            spriteRenderer.color = Color.green;
            spriteRenderer.sprite = ConnectedTrue;
        }
        else
        {
            spriteRenderer.color = Color.red;
            spriteRenderer.sprite = ConnectedFalse;
        }
    }
}
