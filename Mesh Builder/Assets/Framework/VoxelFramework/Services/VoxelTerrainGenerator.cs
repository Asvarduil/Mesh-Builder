using UnityEngine;
using SimplexNoise;

public class VoxelTerrainGenerator
{
    #region Variables

    // TODO: Make these variables configurable through data.
    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.05f;
    float stoneBaseNoiseHeight = 4;
    float stoneMountainHeight = 48;
    float stoneMountainFrequency = 0.008f;
    float stoneMinHeight = -12;

    float dirtBaseHeight = 1;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 3;

    float caveFrequency = 0.025f;
    int caveSize = 7;

    float treeFrequency = 0.2f;
    int treeDensity = 3;

    #endregion Variables

    #region Methods

    public VoxelChunk GenerateChunk(VoxelChunk chunk)
    {
        // Make the 3 a configurable thing (tree radius + 1?)
        for (int x = chunk.WorldPosition.x - 3; x < chunk.WorldPosition.x + VoxelChunk.chunkSize + 3; x++)
        {
            for (int z = chunk.WorldPosition.z - 3; z < chunk.WorldPosition.z + VoxelChunk.chunkSize + 3; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }

        return chunk;
    }

    // TODO: IntVector3?
    public static void SetBlock(int x, int y, int z, VoxelBlock block, VoxelChunk chunk, bool replaceBlocks = false)
    {
        x -= chunk.WorldPosition.x;
        y -= chunk.WorldPosition.y;
        z -= chunk.WorldPosition.z;

        bool isInRange = VoxelChunk.InRange(x); //&& VoxelChunk.InRange(y) && VoxelChunk.InRange(z);
        isInRange &= VoxelChunk.InRange(y);
        isInRange &= VoxelChunk.InRange(z);

        if (isInRange && (replaceBlocks || chunk.Blocks[x, y, z] == null))
        {
            chunk.SetBlock(x, y, z, block);
        }
    }

    public VoxelChunk ChunkColumnGen(VoxelChunk chunk, int x, int z)
    {
        // TODO - Figure out a way to softcode these rules.

        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));
        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.WorldPosition.y - 8; y < chunk.WorldPosition.y + VoxelChunk.chunkSize; y++)
        {
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);

            if (y <= stoneHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new VoxelBlock(), chunk);
            }
            else if (y <= dirtHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new GrassVoxel(), chunk);

                bool isAtDirtHeight = y == dirtHeight;
                bool canMakeTree = GetNoise(x, 0, z, treeFrequency, 100) < treeDensity;

                if (isAtDirtHeight && canMakeTree)
                {
                    CreateTree(x, y + 1, z, chunk);
                }
            }
            else 
            {
                SetBlock(x, y, z, new AirVoxel(), chunk);
            }
        }

        return chunk;
    }

    private void CreateTree(int x, int y, int z, VoxelChunk chunk)
    {
        //create leaves
        // TODO - Make tree radius configurable/randomized?
        for (int xi = -2; xi <= 2; xi++)
        {
            for (int yi = 4; yi <= 8; yi++)
            {
                for (int zi = -2; zi <= 2; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, new LeavesVoxel(), chunk, true);
                }
            }
        }

        //create trunk
        // TODO - Make trunk height configurable/randomized?
        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, new WoodVoxel(), chunk, true);
        }
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }

    #endregion Methods
}
