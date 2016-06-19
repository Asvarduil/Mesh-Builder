using System;

// TODO: Research how to make this concrete class obsolete.  It offends me.
[Serializable]
public class AirVoxel : VoxelBlock
{
    #region Methods

    public override VoxelMeshData BlockData(VoxelChunk chunk, int x, int y, int z, VoxelMeshData meshData)
    {
        return meshData;
    }

    public override bool IsSolid(VoxelDirection direction)
    {
        return false;
    }

    #endregion Methods
}
