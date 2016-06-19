using UnityEngine;

public static class VoxelTerrainService
{
    // TODO: Convert to extension method on Vector3?
    public static VoxelWorldPosition GetBlockPosition(Vector3 pos)
    {
        VoxelWorldPosition blockPos = new VoxelWorldPosition(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z)
        );

        return blockPos;
    }

    public static VoxelWorldPosition GetBlockPosition(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
        );

        return GetBlockPosition(pos);
    }

    public static bool SetBlock(RaycastHit hit, VoxelBlock block, bool adjacent = false)
    {
        VoxelChunk chunk = hit.collider.GetComponent<VoxelChunk>();
        if (chunk == null)
            return false;

        VoxelWorldPosition pos = GetBlockPosition(hit, adjacent);

        chunk.World.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }

    public static VoxelBlock GetBlock(RaycastHit hit, bool adjacent = false)
    {
        VoxelChunk chunk = hit.collider.GetComponent<VoxelChunk>();
        if (chunk == null)
            return null;

        VoxelWorldPosition pos = GetBlockPosition(hit, adjacent);

        VoxelBlock block = chunk.World.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }

    private static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
            if (adjacent)
            {
                pos += (norm / 2);
            }
            else
            {
                pos -= (norm / 2);
            }
        }

        return pos;
    }
}
