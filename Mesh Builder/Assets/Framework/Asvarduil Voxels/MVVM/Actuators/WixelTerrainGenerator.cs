using UnityEngine;
using SimplexNoise;

public class WixelTerrainGenerator
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

    // Study - introducing ores in the stone layer.
    float oreVeinFrequency = 0.2f;
    int oreVeinDensity = 3;

    private WixelRepository _repository;
    private Wixel _air;
    private Wixel _stone;
    private Wixel _ore;
    private Wixel _grass;
    private Wixel _wood;
    private Wixel _leaves;

    #endregion Variables

    #region Constructor

    public WixelTerrainGenerator(WixelRepository repository)
    {
        _repository = repository;
        _air = _repository.GetWixelByName("Air");
        _stone = _repository.GetWixelByName("Stone");
        _ore = _repository.GetWixelByName("Ore");
        _grass = _repository.GetWixelByName("Grass");
        _wood = _repository.GetWixelByName("Wood");
        _leaves = _repository.GetWixelByName("Leaves");
    }

    #endregion Constructor

    #region Methods

    public WixelChunk GenerateChunk(WixelChunk chunk)
    {
        // Make the 3 a configurable thing (tree radius + 1?)
        for (int x = chunk.WorldPosition.x - 3; x < chunk.WorldPosition.x + WixelChunk.chunkSize + 3; x++)
        {
            for (int z = chunk.WorldPosition.z - 3; z < chunk.WorldPosition.z + WixelChunk.chunkSize + 3; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }

        return chunk;
    }

    // TODO: IntVector3?
    public static void SetBlock(int x, int y, int z, Wixel block, WixelChunk chunk, bool replaceBlocks = false)
    {
        x -= chunk.WorldPosition.x;
        y -= chunk.WorldPosition.y;
        z -= chunk.WorldPosition.z;

        bool isInRange = WixelChunk.InRange(x); //&& WixelChunk.InRange(y) && WixelChunk.InRange(z);
        isInRange &= WixelChunk.InRange(y);
        isInRange &= WixelChunk.InRange(z);

        if (isInRange && (replaceBlocks || chunk.Blocks[x, y, z] == null))
        {
            chunk.SetBlock(x, y, z, block);
        }
    }

    public WixelChunk ChunkColumnGen(WixelChunk chunk, int x, int z)
    {
        // TODO - Figure out a way to softcode these rules.

        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));
        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.WorldPosition.y - 8; y < chunk.WorldPosition.y + WixelChunk.chunkSize; y++)
        {
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            
            if (y <= stoneHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, _stone, chunk);

                bool canMakeOre = GetNoise(x, y, z, oreVeinFrequency, 100) < oreVeinDensity;
                if (canMakeOre)
                {
                    SetBlock(x, y, z, _ore, chunk);
                }
            }
            else if (y <= dirtHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, _grass, chunk);

                bool isAtDirtHeight = y == dirtHeight;
                bool canMakeTree = GetNoise(x, 0, z, treeFrequency, 100) < treeDensity;

                if (isAtDirtHeight && canMakeTree)
                {
                    CreateTree(x, y + 1, z, chunk);
                }
            }
            else
            {
                SetBlock(x, y, z, _air, chunk);
            }
        }

        return chunk;
    }

    private void CreateTree(int x, int y, int z, WixelChunk chunk)
    {
        //create leaves
        // TODO - Make tree radius configurable/randomized?
        for (int xi = -2; xi <= 2; xi++)
        {
            for (int yi = 4; yi <= 8; yi++)
            {
                for (int zi = -2; zi <= 2; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, _leaves, chunk, true);
                }
            }
        }

        //create trunk
        // TODO - Make trunk height configurable/randomized?
        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, _wood, chunk, true);
        }
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }

    #endregion Methods
}
