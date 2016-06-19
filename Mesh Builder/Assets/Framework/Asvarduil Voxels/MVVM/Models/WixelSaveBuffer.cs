using System.Collections.Generic;

public class WixelChunkSaveBuffer
{
    #region Variables

    public Dictionary<VoxelWorldPosition, Wixel> Blocks = new Dictionary<VoxelWorldPosition, Wixel>();

    #endregion Variables

    #region Constructor

    public WixelChunkSaveBuffer(WixelChunk chunk)
    {
        for (int x = 0; x < WixelChunk.chunkSize; x++)
        {
            for (int y = 0; y < WixelChunk.chunkSize; y++)
            {
                for (int z = 0; z < WixelChunk.chunkSize; z++)
                {
                    if (!chunk.Blocks[x, y, z].HasChanged)
                        continue;

                    VoxelWorldPosition pos = new VoxelWorldPosition(x, y, z);
                    Blocks.Add(pos, chunk.Blocks[x, y, z]);
                }
            }
        }
    }

    #endregion Constructor
}
