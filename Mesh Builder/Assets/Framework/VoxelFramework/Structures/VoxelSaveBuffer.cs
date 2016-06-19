using System;
using System.Collections.Generic;

[Serializable]
public class VoxelSaveBuffer
{
    #region Variables

    public Dictionary<VoxelWorldPosition, VoxelBlock> Blocks = new Dictionary<VoxelWorldPosition, VoxelBlock>();

    #endregion Variables

    #region Methods

    public VoxelSaveBuffer(VoxelChunk chunk)
    {
        for (int x = 0; x < VoxelChunk.chunkSize; x++)
        {
            for (int y = 0; y < VoxelChunk.chunkSize; y++)
            {
                for (int z = 0; z < VoxelChunk.chunkSize; z++)
                {
                    if (!chunk.Blocks[x, y, z].HasChanged)
                        continue;

                    VoxelWorldPosition pos = new VoxelWorldPosition(x, y, z);
                    Blocks.Add(pos, chunk.Blocks[x, y, z]);
                }
            }
        }
    }

    #endregion Methods
}