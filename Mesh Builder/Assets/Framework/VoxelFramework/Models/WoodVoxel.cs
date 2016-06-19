using System;

// TODO: Research how to make this concrete class obsolete.  It offends me.
[Serializable]
public class WoodVoxel : VoxelBlock
{
    public override VoxelTile TexturePosition(VoxelDirection direction)
    {
        VoxelTile tile = new VoxelTile();
        switch (direction)
        {
            case VoxelDirection.Up:
                tile.x = 2;
                tile.y = 1;
                return tile;

            case VoxelDirection.Down:
                tile.x = 2;
                tile.y = 1;
                return tile;
        }

        tile.x = 1;
        tile.y = 1;

        return tile;
    }
}

