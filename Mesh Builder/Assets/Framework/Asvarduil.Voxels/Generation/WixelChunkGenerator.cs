using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;

public abstract class WixelChunkGenerator
{
    #region Fields

    protected List<WixelGenerationCriteria> _generationCriteria;
    protected WixelRepository _repository;

    // Metrics!
    protected bool _runMetrics = false;
    protected float _startGenerateTime;
    protected float _endGenerateTime;

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
        OnBeforeGenerateChunk(chunk);

        // Make the 3 a configurable thing (tree radius + 1?)
        for (int x = chunk.WorldPosition.x - 3; x < chunk.WorldPosition.x + WixelChunk.chunkSize + 3; x++)
        {
            for (int z = chunk.WorldPosition.z - 3; z < chunk.WorldPosition.z + WixelChunk.chunkSize + 3; z++)
            {
                chunk = GenerateColumn(chunk, x, z);
            }
        }

        OnAfterGenerateChunk(chunk);

        return chunk;
    }

    /// <summary>
    /// Generates a column of wixels in the given chunk.
    /// </summary>
    /// <param name="chunk">Chunk in which to generate wixels</param>
    /// <param name="x">X chunk coordinate</param>
    /// <param name="z">Z chunk coordinate</param>
    /// <returns>Chunk with modifications.</returns>
    protected WixelChunk GenerateColumn(WixelChunk chunk, int x, int z)
    {
        OnBeforeGenerateColumn(x, z, chunk);

        // TODO: Implement
        for (int y = chunk.WorldPosition.y - 8; y < chunk.WorldPosition.y + WixelChunk.chunkSize; y++)
        {
            OnBeforeGenerateBlock(x, y, z, chunk);

            for(int i = 0; i < _generationCriteria.Count; i++)
            {
                WixelGenerationCriteria current = _generationCriteria[i];
                current.CheckCriteria(x, y, z, chunk);
            }

            OnAfterGenerateBlock(x, y, z, chunk);
        }

        OnAfterGenerateColumn(x, z, chunk);

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
    protected static void SetBlock(int x, int y, int z, Wixel block, WixelChunk chunk, bool replaceBlocks = false)
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
    protected static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }

    /// <summary>
    /// Default condition for Wixel Chunk Generation.
    /// </summary>
    /// <returns>True always</returns>
    public bool DefaultCondition(int x, int y, int z, WixelChunk chunk)
    {
        return true;
    }

    #endregion Primary Methods

    #region Customization Methods

    protected virtual void OnBeforeGenerateChunk(WixelChunk chunk)
    {
        if(! _runMetrics)
            return;
            
        _startGenerateTime = Time.time;
    }

    protected virtual void OnAfterGenerateChunk(WixelChunk chunk)
    {
        if(! _runMetrics)
            return;
            
        _endGenerateTime = Time.time;

        float elapsedTime = _endGenerateTime - _startGenerateTime;
        string message = string.Format("Chunk at {0} was generated in {1} seconds", chunk.WorldPosition, elapsedTime);
        Debug.Log(message);
    }

    protected virtual void OnBeforeGenerateColumn(int x, int z, WixelChunk chunk)
    {
    }

    protected virtual void OnAfterGenerateColumn(int x, int z, WixelChunk chunk)
    {
    }

    protected virtual void OnBeforeGenerateBlock(int x, int y, int z, WixelChunk chunk)
    {
    }

    protected virtual void OnAfterGenerateBlock(int x, int y, int z, WixelChunk chunk)
    {
    }

    #endregion Customization Methods
}
