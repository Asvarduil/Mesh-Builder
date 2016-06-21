using System.Collections.Generic;
using UnityEngine;

public class DefaultChunkGenerator : WixelChunkGenerator
{
    #region Fields

    float _stoneHeight = 0.0f;
    float _stoneBaseHeight = -24;
    float _stoneBaseNoise = 0.05f;
    float _stoneBaseNoiseHeight = 4;
    float _stoneMountainHeight = 48;
    float _stoneMountainFrequency = 0.008f;
    float _stoneMinHeight = -12;

    float _dirtHeight = 0.0f;
    float _dirtBaseHeight = 1;
    float _dirtNoise = 0.04f;
    float _dirtNoiseHeight = 3;

    float _caveChance = 0.0f;
    float _caveFrequency = 0.025f;
    int _caveSize = 7;

    float _treeFrequency = 0.2f;
    int _treeDensity = 3;

    #endregion Fields

    #region Blocks

    private Wixel _air;
    private Wixel _stone;
    private Wixel _grass;
    private Wixel _wood;
    private Wixel _leaves;

    #endregion Blocks

    #region Constructor

    public DefaultChunkGenerator(WixelRepository repository) : base(repository)
    {
        _generationCriteria = new List<WixelGenerationCriteria>
        {
            new WixelGenerationCriteria {Condition = CaveCondition, Action = StoneGenerate },
            new WixelGenerationCriteria {Condition = DirtTreeCondition, Action = DirtTreeGenerate },
            new WixelGenerationCriteria {Condition = DefaultCondition, Action = AirGenerate }
        };

        _air = _repository.GetWixelByName("Air");
        _stone = _repository.GetWixelByName("Stone");
        _grass = _repository.GetWixelByName("Grass");
        _wood = _repository.GetWixelByName("Wood");
        _leaves = _repository.GetWixelByName("Leaves");
    }

    #endregion Constructor

    #region Methods

    protected override void OnBeforeGenerateColumn(int x, int z, WixelChunk chunk)
    {
        _stoneHeight = Mathf.FloorToInt(_stoneBaseHeight);
        _stoneHeight += GetNoise(x, 0, z,_stoneMountainFrequency, Mathf.FloorToInt(_stoneMountainHeight));
        if (_stoneHeight <_stoneMinHeight)
           _stoneHeight = Mathf.FloorToInt(_stoneMinHeight);

        _stoneHeight += GetNoise(x, 0, z,_stoneBaseNoise, Mathf.FloorToInt(_stoneBaseNoiseHeight));

        _dirtHeight =_stoneHeight + Mathf.FloorToInt(_dirtBaseHeight);
        _dirtHeight += GetNoise(x, 100, z, _dirtNoise, Mathf.FloorToInt(_dirtNoiseHeight));
    }

    protected override void OnBeforeGenerateBlock(int x, int y, int z, WixelChunk chunk)
    {
        _caveChance = GetNoise(x, y, z, _caveFrequency, 100);
    }

    #endregion Methods

    #region Conditions

    public bool CaveCondition(int x, int y, int z, WixelChunk chunk)
    {
        return y <= _stoneHeight && _caveSize < _caveChance;
    }

    public bool DirtTreeCondition(int x, int y, int z, WixelChunk chunk)
    {
        return y <= _dirtHeight && _caveSize < _caveChance;
    }

    #endregion Conditions

    #region Actions

    public void StoneGenerate(int x, int y, int z, WixelChunk chunk)
    {
        SetBlock(x, y, z, _stone, chunk);
    }

    public void DirtTreeGenerate(int x, int y, int z, WixelChunk chunk)
    {
        SetBlock(x, y, z, _grass, chunk);

        bool isAtDirtHeight = y == _dirtHeight;
        bool canMakeTree = GetNoise(x, 0, z, _treeFrequency, 100) < _treeDensity;

        if (isAtDirtHeight && canMakeTree)
        {
            CreateTree(x, y + 1, z, chunk);
        }
    }

    public void AirGenerate(int x, int y, int z, WixelChunk chunk)
    {
        SetBlock(x, y, z, _air, chunk);
    }

    #endregion Actions

    #region Macros

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

    #endregion Macros
}