using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomTile : Tile
{
    public bool hasCollider;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        if (hasCollider)
        {
            tileData.colliderType = Tile.ColliderType.Sprite;
        }
        else
        {
            tileData.colliderType = Tile.ColliderType.None;
        }
    }
}
