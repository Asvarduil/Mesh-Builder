using System;
using UnityEngine;

[Serializable]
public class VoxelBlock
{
    #region Constants

    // TODO: Make block scale a configurable property at the application level.
    const float blockScale = 0.5f;

    // TODO: Make tile texture size a configurable property at the application level.
    const float tileSize = 0.25f;

    #endregion Constants

    #region Fields

    public bool HasChanged = true;

    #endregion Fields

    #region Constructor

    #endregion Constructor

    #region Methods

    // TODO: Figure out how to make these into data as well.

    public virtual VoxelTile TexturePosition(VoxelDirection direction)
    {
        VoxelTile tile = new VoxelTile
        {
            x = 0,
            y = 0
        };

        return tile;
    }

    public virtual Vector2[] FaceUVs(VoxelDirection direction)
    {
        Vector2[] UVs = new Vector2[4];
        VoxelTile tilePos = TexturePosition(direction);
        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y);
        return UVs;
    }

    public virtual VoxelMeshData BlockData(VoxelChunk chunk, int x, int y, int z, VoxelMeshData meshData)
    {
        meshData.useRenderDataForCol = true;

        if (!chunk.GetBlock(x, y + 1, z).IsSolid(VoxelDirection.Down))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(VoxelDirection.Up))
        {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(VoxelDirection.South))
        {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(VoxelDirection.North))
        {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(VoxelDirection.West))
        {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(VoxelDirection.East))
        {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
        }

        return meshData;
    }

    // TODO: Research a 'blocks' file that has these relationships set up; refer to those by name-value pair
    //       to have data-driven blocks.

    protected virtual VoxelMeshData FaceDataUp(VoxelChunk chunk, int x, int y, int z, VoxelMeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - blockScale, y + blockScale, z + blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y + blockScale, z + blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y + blockScale, z - blockScale));
        meshData.AddVertex(new Vector3(x - blockScale, y + blockScale, z - blockScale));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(VoxelDirection.Up));
        return meshData;
    }

    protected virtual VoxelMeshData FaceDataDown(VoxelChunk chunk, int x, int y, int z, VoxelMeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - blockScale, y - blockScale, z - blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y - blockScale, z - blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y - blockScale, z + blockScale));
        meshData.AddVertex(new Vector3(x - blockScale, y - blockScale, z + blockScale));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(VoxelDirection.Down));
        return meshData;

    }

    protected virtual VoxelMeshData FaceDataNorth(VoxelChunk chunk, int x, int y, int z, VoxelMeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + blockScale, y - blockScale, z + blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y + blockScale, z + blockScale));
        meshData.AddVertex(new Vector3(x - blockScale, y + blockScale, z + blockScale));
        meshData.AddVertex(new Vector3(x - blockScale, y - blockScale, z + blockScale));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(VoxelDirection.North));
        return meshData;
    }

    protected virtual VoxelMeshData FaceDataEast(VoxelChunk chunk, int x, int y, int z, VoxelMeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + blockScale, y - blockScale, z - blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y + blockScale, z - blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y + blockScale, z + blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y - blockScale, z + blockScale));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(VoxelDirection.East));
        return meshData;
    }

    protected virtual VoxelMeshData FaceDataSouth(VoxelChunk chunk, int x, int y, int z, VoxelMeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - blockScale, y - blockScale, z - blockScale));
        meshData.AddVertex(new Vector3(x - blockScale, y + blockScale, z - blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y + blockScale, z - blockScale));
        meshData.AddVertex(new Vector3(x + blockScale, y - blockScale, z - blockScale));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(VoxelDirection.South));
        return meshData;
    }

    protected virtual VoxelMeshData FaceDataWest(VoxelChunk chunk, int x, int y, int z, VoxelMeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - blockScale, y - blockScale, z + blockScale));
        meshData.AddVertex(new Vector3(x - blockScale, y + blockScale, z + blockScale));
        meshData.AddVertex(new Vector3(x - blockScale, y + blockScale, z - blockScale));
        meshData.AddVertex(new Vector3(x - blockScale, y - blockScale, z - blockScale));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(VoxelDirection.West));
        return meshData;
    }

    // TODO: Research a blocks file, where blocks has face-by-face solidity set up; refer to those
    //       by name-value pair to have data-driven blocks.

    public virtual bool IsSolid(VoxelDirection direction)
    {
        switch (direction)
        {
            case VoxelDirection.Up:
            case VoxelDirection.Down:
            case VoxelDirection.North:
            case VoxelDirection.East:
            case VoxelDirection.South:
            case VoxelDirection.West:
                return true;

            default:
                return false;
        }
    }

    #endregion Methods
}
