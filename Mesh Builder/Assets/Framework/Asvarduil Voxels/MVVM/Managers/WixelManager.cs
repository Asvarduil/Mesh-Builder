using UnityEngine;

public class WixelManager : ManagerBase<WixelManager>
{
    #region Variables / Properties

    // TODO: Store all worldgen parameters.

    #endregion Variables / Properties

    #region Methods

    public VoxelWorldPosition GetBlockPosition(Vector3 pos)
    {
        VoxelWorldPosition blockPos = new VoxelWorldPosition(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z)
        );

        return blockPos;
    }

    public VoxelWorldPosition GetBlockPosition(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
        );

        return GetBlockPosition(pos);
    }

    public bool SetBlock(RaycastHit hit, Wixel block, bool adjacent = false)
    {
        WixelChunk chunk = hit.collider.GetComponent<WixelChunk>();
        if (chunk == null)
            return false;

        VoxelWorldPosition pos = GetBlockPosition(hit, adjacent);

        chunk.World.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }

    public Wixel GetBlock(RaycastHit hit, bool adjacent = false)
    {
        WixelChunk chunk = hit.collider.GetComponent<WixelChunk>();
        if (chunk == null)
            return null;

        VoxelWorldPosition pos = GetBlockPosition(hit, adjacent);

        Wixel block = chunk.World.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }

    private float MoveWithinBlock(float pos, float norm, bool adjacent = false)
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

    #endregion Methods
}