using System.Collections.Generic;
using UnityEngine;

public class VoxelWorld : MonoBehaviour
{
    #region Variables / Properties

    // TODO: Revise this to be a data point at a later time.
    public string WorldName = "World";

    public GameObject chunkPrefab;

    public Dictionary<VoxelWorldPosition, VoxelChunk> chunks = new Dictionary<VoxelWorldPosition, VoxelChunk>();

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void CreateChunk(int x, int y, int z)
    {
        VoxelWorldPosition worldPos = new VoxelWorldPosition(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero)) as GameObject;
        VoxelChunk newChunk = newChunkObject.GetComponent<VoxelChunk>();
        newChunk.WorldPosition = worldPos;
        newChunk.World = this;

        // Parent all chunks to the world.
        newChunkObject.transform.SetParent(transform);

        //Add it to the chunks dictionary with the position as the key
        chunks.Add(worldPos, newChunk);

        var terrainGen = new VoxelTerrainGenerator();
        newChunk = terrainGen.GenerateChunk(newChunk);
        newChunk.SetBlocksUnmodified();
        VoxelSerializationService.Load(newChunk);
    }

    public void DestroyChunk(int x, int y, int z)
    {
        VoxelChunk chunk = null;
        if (chunks.TryGetValue(new VoxelWorldPosition(x, y, z), out chunk))
        {
            VoxelSerializationService.SaveChunk(chunk);

            Destroy(chunk.gameObject);
            chunks.Remove(new VoxelWorldPosition(x, y, z));
        }
    }

    private void UpdateIfEqual(int value1, int value2, VoxelWorldPosition pos)
    {
        if (value1 == value2)
        {
            VoxelChunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.PerformUpdates = true;
        }
    }

    public VoxelChunk GetChunk(int x, int y, int z)
    {
        VoxelWorldPosition pos = new VoxelWorldPosition();

        float multiple = VoxelChunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * VoxelChunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * VoxelChunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * VoxelChunk.chunkSize;

        VoxelChunk containerChunk = null;
        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }

    public VoxelBlock GetBlock(int x, int y, int z)
    {
        VoxelChunk containerChunk = GetChunk(x, y, z);

        if (containerChunk != null)
        {
            VoxelBlock block = containerChunk.GetBlock(
                x - containerChunk.WorldPosition.x,
                y - containerChunk.WorldPosition.y,
                z - containerChunk.WorldPosition.z
            );

            return block;
        }
        else
        {
            return new AirVoxel();
        }
    }

    public void SetBlock(int x, int y, int z, VoxelBlock block)
    {
        VoxelChunk chunk = GetChunk(x, y, z);
        if (chunk == null)
            return;
        
        chunk.SetBlock(x - chunk.WorldPosition.x, y - chunk.WorldPosition.y, z - chunk.WorldPosition.z, block);
        chunk.PerformUpdates = true;

        UpdateIfEqual(x - chunk.WorldPosition.x, VoxelChunk.chunkSize - 1, new VoxelWorldPosition(x + 1, y, z));
        UpdateIfEqual(y - chunk.WorldPosition.y, 0, new VoxelWorldPosition(x, y - 1, z));
        UpdateIfEqual(y - chunk.WorldPosition.y, VoxelChunk.chunkSize - 1, new VoxelWorldPosition(x, y + 1, z));
        UpdateIfEqual(z - chunk.WorldPosition.z, 0, new VoxelWorldPosition(x, y, z - 1));
        UpdateIfEqual(z - chunk.WorldPosition.z, VoxelChunk.chunkSize - 1, new VoxelWorldPosition(x, y, z + 1));
    }

    #endregion Methods
}
