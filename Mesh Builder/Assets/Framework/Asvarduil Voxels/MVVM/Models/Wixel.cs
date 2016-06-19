using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wixel : INamed
{
    #region Constants

    // TODO: Extract elsewhere?
    private const float tileSize = 0.25f;
    private const float blockScale = 0.5f;

    #endregion Constants

    #region Fields

    public string Name;
    public List<WixelDirectionInformation> FaceInformation;

    public bool HasChanged;

    public string EntityName
    {
        get { return Name; }
    }

    #endregion Fields

    #region Methods

    public Vector2 TexturePosition(VoxelDirection direction)
    {
        WixelDirectionInformation faceInfo = GetInformationByDirection(direction);
        return faceInfo.Tile;
    }

    public Vector2[] FaceUVs(VoxelDirection direction)
    {
        WixelDirectionInformation faceInfo = GetInformationByDirection(direction);

        Vector2[] UVs = new Vector2[4];
        Vector2 tilePos = TexturePosition(direction);

        for(int i = 0; i < faceInfo.UVs.Count; i++)
        {
            Vector2 uvPoint = faceInfo.UVs[i];
            Vector2 actualUVPoint = new Vector2
            {
                x = (tileSize * tilePos.x) + (tileSize * uvPoint.x),
                y = (tileSize * tilePos.y) + (tileSize * uvPoint.y),
            };

            UVs[i] = actualUVPoint;
        }

        return UVs;
    }

    public WixelChunkMeshData BlockData(WixelChunk chunk, int x, int y, int z, WixelChunkMeshData meshData)
    {
        if (FaceInformation.Count == 0)
            return meshData;

        // Programmer's Note
        // -----------------
        // int x/y/z is fine due to how this is being used.
       
        Vector3 worldPosition = new Vector3(x, y, z);
        meshData.useRenderDataForCol = true;

        if (!chunk.GetBlock(x, y + 1, z).IsSolid(VoxelDirection.Down))
        {
            meshData = FaceData(VoxelDirection.Up, chunk, worldPosition, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(VoxelDirection.Up))
        {
            meshData = FaceData(VoxelDirection.Down, chunk, worldPosition, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(VoxelDirection.South))
        {
            meshData = FaceData(VoxelDirection.North, chunk, worldPosition, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(VoxelDirection.North))
        {
            meshData = FaceData(VoxelDirection.South, chunk, worldPosition, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(VoxelDirection.West))
        {
            meshData = FaceData(VoxelDirection.East, chunk, worldPosition, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(VoxelDirection.East))
        {
            meshData = FaceData(VoxelDirection.West, chunk, worldPosition, meshData);
        }

        return meshData;
    }

    public bool IsSolid(VoxelDirection direction)
    {
        if (FaceInformation.Count == 0)
            return false;

        WixelDirectionInformation faceInfo = GetInformationByDirection(direction);
        return faceInfo.IsSolid;
    }

    private WixelChunkMeshData FaceData(VoxelDirection direction, WixelChunk chunk, Vector3 worldPosition, WixelChunkMeshData meshData)
    {
        if (FaceInformation.Count == 0)
            return meshData;

        WixelDirectionInformation faceInfo = GetInformationByDirection(direction);

        for (int i = 0; i < faceInfo.Vertices.Count; i++)
        {
            Vector3 vertexPosition = faceInfo.Vertices[i] * blockScale;
            Vector3 actualPosition = vertexPosition + worldPosition;

            meshData.AddVertex(actualPosition);
        }

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(direction));
        return meshData;
    }

    private WixelDirectionInformation GetInformationByDirection(VoxelDirection direction)
    {
        WixelDirectionInformation result = null;
        for(int i = 0; i < FaceInformation.Count; i++)
        {
            WixelDirectionInformation current = FaceInformation[i];
            if (current.Direction != direction)
                continue;

            result = current;
            break;
        }

        if (result == null)
        {
            string message = string.Format("Could not find face data for Wixel {0}, Direction {1}", Name, direction.ToString());
            throw new DataException(message);
        }

        return result;
    }

    #endregion Methods
}