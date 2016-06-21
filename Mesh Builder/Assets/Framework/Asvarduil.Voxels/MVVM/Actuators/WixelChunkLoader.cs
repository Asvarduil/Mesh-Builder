using System.Collections.Generic;
using UnityEngine;

public class WixelChunkLoader : DebuggableBehavior
{
    #region Variables

    private WixelWorld _world;
    public WixelWorld World
    {
        get
        {
            if (_world == null)
                _world = FindObjectOfType<WixelWorld>();

            return _world;
        }
    }

    private int _timer = 0;

    private List<WorldPosition> _updateList = new List<WorldPosition>();
    private List<WorldPosition> _buildList = new List<WorldPosition>();

    // Default starting location.  Definitely change this!
    private static WorldPosition[] _chunkPositions = {
        new WorldPosition( 0, 0,  0), new WorldPosition(-1, 0,  0), new WorldPosition( 0, 0, -1), new WorldPosition( 0, 0,  1), new WorldPosition( 1, 0,  0),
        new WorldPosition(-1, 0, -1), new WorldPosition(-1, 0,  1), new WorldPosition( 1, 0, -1), new WorldPosition( 1, 0,  1), new WorldPosition(-2, 0,  0),
        new WorldPosition( 0, 0, -2), new WorldPosition( 0, 0,  2), new WorldPosition( 2, 0,  0), new WorldPosition(-2, 0, -1), new WorldPosition(-2, 0,  1),
        new WorldPosition(-1, 0, -2), new WorldPosition(-1, 0,  2), new WorldPosition( 1, 0, -2), new WorldPosition( 1, 0,  2), new WorldPosition( 2, 0, -1),
        new WorldPosition( 2, 0,  1), new WorldPosition(-2, 0, -2), new WorldPosition(-2, 0,  2), new WorldPosition( 2, 0, -2), new WorldPosition( 2, 0,  2),
        new WorldPosition(-3, 0,  0), new WorldPosition( 0, 0, -3), new WorldPosition( 0, 0,  3), new WorldPosition( 3, 0,  0), new WorldPosition(-3, 0, -1),
        new WorldPosition(-3, 0,  1), new WorldPosition(-1, 0, -3), new WorldPosition(-1, 0,  3), new WorldPosition( 1, 0, -3), new WorldPosition( 1, 0,  3),
        new WorldPosition( 3, 0, -1), new WorldPosition( 3, 0,  1), new WorldPosition(-3, 0, -2), new WorldPosition(-3, 0,  2), new WorldPosition(-2, 0, -3),
        new WorldPosition(-2, 0,  3), new WorldPosition( 2, 0, -3), new WorldPosition( 2, 0,  3), new WorldPosition( 3, 0, -2), new WorldPosition( 3, 0,  2),
        new WorldPosition(-4, 0,  0), new WorldPosition( 0, 0, -4), new WorldPosition( 0, 0,  4), new WorldPosition( 4, 0,  0), new WorldPosition(-4, 0, -1),
        new WorldPosition(-4, 0,  1), new WorldPosition(-1, 0, -4), new WorldPosition(-1, 0,  4), new WorldPosition( 1, 0, -4), new WorldPosition( 1, 0,  4),
        new WorldPosition( 4, 0, -1), new WorldPosition( 4, 0,  1), new WorldPosition(-3, 0, -3), new WorldPosition(-3, 0,  3), new WorldPosition( 3, 0, -3),
        new WorldPosition( 3, 0,  3), new WorldPosition(-4, 0, -2), new WorldPosition(-4, 0,  2), new WorldPosition(-2, 0, -4), new WorldPosition(-2, 0,  4),
        new WorldPosition( 2, 0, -4), new WorldPosition( 2, 0,  4), new WorldPosition( 4, 0, -2), new WorldPosition( 4, 0,  2), new WorldPosition(-5, 0,  0),
        new WorldPosition(-4, 0, -3), new WorldPosition(-4, 0,  3), new WorldPosition(-3, 0, -4), new WorldPosition(-3, 0,  4), new WorldPosition( 0, 0, -5),
        new WorldPosition( 0, 0,  5), new WorldPosition( 3, 0, -4), new WorldPosition( 3, 0,  4), new WorldPosition( 4, 0, -3), new WorldPosition( 4, 0,  3),
        new WorldPosition( 5, 0,  0), new WorldPosition(-5, 0, -1), new WorldPosition(-5, 0,  1), new WorldPosition(-1, 0, -5), new WorldPosition(-1, 0,  5),
        new WorldPosition( 1, 0, -5), new WorldPosition( 1, 0,  5), new WorldPosition( 5, 0, -1), new WorldPosition( 5, 0,  1), new WorldPosition(-5, 0, -2),
        new WorldPosition(-5, 0,  2), new WorldPosition(-2, 0, -5), new WorldPosition(-2, 0,  5), new WorldPosition( 2, 0, -5), new WorldPosition( 2, 0,  5),
        new WorldPosition( 5, 0, -2), new WorldPosition( 5, 0,  2), new WorldPosition(-4, 0, -4), new WorldPosition(-4, 0,  4), new WorldPosition( 4, 0, -4),
        new WorldPosition( 4, 0,  4), new WorldPosition(-5, 0, -3), new WorldPosition(-5, 0,  3), new WorldPosition(-3, 0, -5), new WorldPosition(-3, 0,  5),
        new WorldPosition( 3, 0, -5), new WorldPosition( 3, 0,  5), new WorldPosition( 5, 0, -3), new WorldPosition( 5, 0,  3), new WorldPosition(-6, 0,  0),
        new WorldPosition( 0, 0, -6), new WorldPosition( 0, 0,  6), new WorldPosition( 6, 0,  0), new WorldPosition(-6, 0, -1), new WorldPosition(-6, 0,  1),
        new WorldPosition(-1, 0, -6), new WorldPosition(-1, 0,  6), new WorldPosition( 1, 0, -6), new WorldPosition( 1, 0,  6), new WorldPosition( 6, 0, -1),
        new WorldPosition( 6, 0,  1), new WorldPosition(-6, 0, -2), new WorldPosition(-6, 0,  2), new WorldPosition(-2, 0, -6), new WorldPosition(-2, 0,  6),
        new WorldPosition( 2, 0, -6), new WorldPosition( 2, 0,  6), new WorldPosition( 6, 0, -2), new WorldPosition( 6, 0,  2), new WorldPosition(-5, 0, -4),
        new WorldPosition(-5, 0,  4), new WorldPosition(-4, 0, -5), new WorldPosition(-4, 0,  5), new WorldPosition( 4, 0, -5), new WorldPosition( 4, 0,  5),
        new WorldPosition( 5, 0, -4), new WorldPosition( 5, 0,  4), new WorldPosition(-6, 0, -3), new WorldPosition(-6, 0,  3), new WorldPosition(-3, 0, -6),
        new WorldPosition(-3, 0,  6), new WorldPosition( 3, 0, -6), new WorldPosition( 3, 0,  6), new WorldPosition( 6, 0, -3), new WorldPosition( 6, 0,  3),
        new WorldPosition(-7, 0,  0), new WorldPosition( 0, 0, -7), new WorldPosition( 0, 0,  7), new WorldPosition( 7, 0,  0), new WorldPosition(-7, 0, -1),
        new WorldPosition(-7, 0,  1), new WorldPosition(-5, 0, -5), new WorldPosition(-5, 0,  5), new WorldPosition(-1, 0, -7), new WorldPosition(-1, 0,  7),
        new WorldPosition( 1, 0, -7), new WorldPosition( 1, 0,  7), new WorldPosition( 5, 0, -5), new WorldPosition( 5, 0,  5), new WorldPosition( 7, 0, -1),
        new WorldPosition( 7, 0,  1), new WorldPosition(-6, 0, -4), new WorldPosition(-6, 0,  4), new WorldPosition(-4, 0, -6), new WorldPosition(-4, 0,  6),
        new WorldPosition( 4, 0, -6), new WorldPosition( 4, 0,  6), new WorldPosition( 6, 0, -4), new WorldPosition( 6, 0,  4), new WorldPosition(-7, 0, -2),
        new WorldPosition(-7, 0,  2), new WorldPosition(-2, 0, -7), new WorldPosition(-2, 0,  7), new WorldPosition( 2, 0, -7), new WorldPosition( 2, 0,  7),
        new WorldPosition( 7, 0, -2), new WorldPosition( 7, 0,  2), new WorldPosition(-7, 0, -3), new WorldPosition(-7, 0,  3), new WorldPosition(-3, 0, -7),
        new WorldPosition(-3, 0,  7), new WorldPosition( 3, 0, -7), new WorldPosition( 3, 0,  7), new WorldPosition( 7, 0, -3), new WorldPosition( 7, 0,  3),
        new WorldPosition(-6, 0, -5), new WorldPosition(-6, 0,  5), new WorldPosition(-5, 0, -6), new WorldPosition(-5, 0,  6), new WorldPosition( 5, 0, -6),
        new WorldPosition( 5, 0,  6), new WorldPosition( 6, 0, -5), new WorldPosition( 6, 0,  5)
    };

    #endregion Variables

    #region Hooks

    public void Update()
    {
        if (DeleteChunks())
            return;

        FindChunksToLoad();
        LoadAndRenderChunks();
    }

    #endregion Hooks

    #region Methods

    private void FindChunksToLoad()
    {
        //Get the position of this gameobject to generate around
        WorldPosition playerPos = new WorldPosition(
            Mathf.FloorToInt(transform.position.x / WixelChunk.chunkSize) * WixelChunk.chunkSize,
            Mathf.FloorToInt(transform.position.y / WixelChunk.chunkSize) * WixelChunk.chunkSize,
            Mathf.FloorToInt(transform.position.z / WixelChunk.chunkSize) * WixelChunk.chunkSize
        );

        //If there aren't already chunks to generate
        if (_updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < _chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                WorldPosition newChunkPos = new WorldPosition(
                    _chunkPositions[i].x * WixelChunk.chunkSize + playerPos.x,
                    0,
                    _chunkPositions[i].z * WixelChunk.chunkSize + playerPos.z
                );

                //Get the chunk in the defined position
                WixelChunk newChunk = World.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);

                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk != null
                    && (newChunk.IsRendered || _updateList.Contains(newChunkPos)))
                    continue;

                //load a column of chunks in this position
                // TODO: Make the y ranges configurable!
                for (int y = -4; y < 4; y++)
                {
                    for (int x = newChunkPos.x - WixelChunk.chunkSize; x <= newChunkPos.x + WixelChunk.chunkSize; x += WixelChunk.chunkSize)
                    {
                        for (int z = newChunkPos.z - WixelChunk.chunkSize; z <= newChunkPos.z + WixelChunk.chunkSize; z += WixelChunk.chunkSize)
                        {
                            _buildList.Add(new WorldPosition(x, y * WixelChunk.chunkSize, z));
                        }
                    }

                    _updateList.Add(new WorldPosition(newChunkPos.x, y * WixelChunk.chunkSize, newChunkPos.z));
                }

                return;
            }
        }
    }

    private void BuildChunk(WorldPosition pos)
    {
        if (World.GetChunk(pos.x, pos.y, pos.z) == null)
            World.CreateChunk(pos.x, pos.y, pos.z);
    }

    private bool DeleteChunks()
    {
        // TODO: Make deletion tick configurable.
        if (_timer < 10)
        {
            _timer++;
            return false;
        }

        var chunksToDelete = new List<WorldPosition>();

        foreach (var chunk in World.chunks)
        {
            float distance = Vector3.Distance(
                new Vector3(chunk.Value.WorldPosition.x, 0, chunk.Value.WorldPosition.z),
                new Vector3(transform.position.x, 0, transform.position.z)
            );

            // TODO - Make max distance configurable.
            if (distance > 256)
                chunksToDelete.Add(chunk.Key);
        }

        foreach (var chunk in chunksToDelete)
        {
            World.DestroyChunk(chunk.x, chunk.y, chunk.z);
        }

        _timer = 0;
        return true;
    }

    private void LoadAndRenderChunks()
    {
        if (_buildList.Count != 0)
        {
            for (int i = 0; i < _buildList.Count && i < 8; i++)
            {
                BuildChunk(_buildList[0]);
                _buildList.RemoveAt(0);
            }

            //If chunks were built return early
            return;
        }

        if (_updateList.Count != 0)
        {
            WixelChunk chunk = World.GetChunk(_updateList[0].x, _updateList[0].y, _updateList[0].z);
            if (chunk != null)
                chunk.PerformUpdates = true;

            _updateList.RemoveAt(0);
        }
    }

    #endregion Methods
}
