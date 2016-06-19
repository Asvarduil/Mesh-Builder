using UnityEngine;
using SimplexNoise;

public class WixelChunkGenerator
{
    #region Fields

    // Programmer's Notes:
    // -------------------
    // I want to devise an easy way to add the following things:
    //
    // A) Column Generation conditions, in descending order of rarity
    // B) Chunk- and Column- specific values, callable from conditions
    // C) Macros that start at a given position and do their own thing.

    private WixelRepository _repository;

    #endregion Fields

    #region Constructor

    public WixelChunkGenerator(WixelRepository repository)
    {
        _repository = repository;
    }

    #endregion Constructor

    #region Primary Methods

    /// <summary>
    /// Generates wixels into the given chunk.
    /// </summary>
    /// <param name="chunk">Chunk in which to generate</param>
    /// <returns>Chunk with modifications.</returns>
    public WixelChunk GenerateChunk(WixelChunk chunk)
    {
        // Make the 3 a configurable thing (tree radius + 1?)
        for (int x = chunk.WorldPosition.x - 3; x < chunk.WorldPosition.x + WixelChunk.chunkSize + 3; x++)
        {
            for (int z = chunk.WorldPosition.z - 3; z < chunk.WorldPosition.z + WixelChunk.chunkSize + 3; z++)
            {
                chunk = GenerateColumn(chunk, x, z);
            }
        }

        return chunk;
    }

    /// <summary>
    /// Generates a column of wixels in the given chunk.
    /// </summary>
    /// <param name="chunk">Chunk in which to generate wixels</param>
    /// <param name="x">X chunk coordinate</param>
    /// <param name="z">Z chunk coordinate</param>
    /// <returns>Chunk with modifications.</returns>
    private WixelChunk GenerateColumn(WixelChunk chunk, int x, int z)
    {
        // TODO: Implement
        return chunk;
    }

    /// <summary>
    /// Sets the given block at the X/Y/Z chunk coordinates for a chunk.
    /// </summary>
    /// <param name="x">Chunk X coordinate</param>
    /// <param name="y">Chunk Y coordinate</param>
    /// <param name="z">Chunk Z coordinate</param>
    /// <param name="block">Block to set</param>
    /// <param name="chunk">Chunk in which to set the block</param>
    /// <param name="replaceBlocks">Optional; if true, overwrite any block at the current chunk coordinate.</param>
    private static void SetBlock(int x, int y, int z, Wixel block, WixelChunk chunk, bool replaceBlocks = false)
    {
        x -= chunk.WorldPosition.x;
        y -= chunk.WorldPosition.y;
        z -= chunk.WorldPosition.z;

        bool isInRange = WixelChunk.InRange(x) && WixelChunk.InRange(y) && WixelChunk.InRange(z);
        if (isInRange && (replaceBlocks || chunk.Blocks[x, y, z] == null))
        {
            chunk.SetBlock(x, y, z, block);
        }
    }

    /// <summary>
    /// Generates randomized integer noise for a given X/Y/Z world coordinate.
    /// </summary>
    /// <param name="x">World X position</param>
    /// <param name="y">World Y position</param>
    /// <param name="z">World Z position</param>
    /// <param name="scale">Amplitude of the noise</param>
    /// <param name="max">Max cutoff</param>
    /// <returns>Noise as an integer</returns>
    private static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }

    #endregion Primary Methods
}
