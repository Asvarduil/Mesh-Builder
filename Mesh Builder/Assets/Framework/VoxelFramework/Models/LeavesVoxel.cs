using System;

[Serializable]
public class LeavesVoxel : VoxelBlock
{
    public override VoxelTile TexturePosition(VoxelDirection direction)
    {
        VoxelTile tile = new VoxelTile();
        tile.x = 0;
        tile.y = 1;

        return tile;
    }

    public override bool IsSolid(VoxelDirection direction)
    {
        return false;
    }
}
