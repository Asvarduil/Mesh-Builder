using System;

// TODO: Research how to make this concrete class obsolete.  It offends me.
[Serializable]
public class GrassVoxel : VoxelBlock
{
    #region Constructor

    #endregion Constructor

    #region Methods

    public override VoxelTile TexturePosition(VoxelDirection direction)
    {
        VoxelTile tile = new VoxelTile();
        switch (direction)
        {
            case VoxelDirection.Up:
                tile.x = 2;
                tile.y = 0;
                return tile;
            case VoxelDirection.Down:
                tile.x = 1;
                tile.y = 0;
                return tile;
        }

        tile.x = 3;
        tile.y = 0;
        return tile;
    }

    #endregion Methods
}
