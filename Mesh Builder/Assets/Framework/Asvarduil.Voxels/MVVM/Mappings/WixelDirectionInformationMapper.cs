using System;
using SimpleJSON;

public class WixelDirectionInformationMapper : SimpleJsonMapper<WixelDirectionInformation>
{
    #region Properties

    private Vector2Mapper _vector2Mapper;
    private Vector2Mapper Vector2Mapper
    {
        get
        {
            if (_vector2Mapper == null)
                _vector2Mapper = new Vector2Mapper();

            return _vector2Mapper;
        }
    }

    private Vector3Mapper _vector3Mapper;
    private Vector3Mapper Vector3Mapper
    {
        get
        {
            if (_vector3Mapper == null)
                _vector3Mapper = new Vector3Mapper();

            return _vector3Mapper;
        }
    }

    #endregion Properties

    #region Constructor

    public WixelDirectionInformationMapper() : base("WixelDirectionInformation")
    {
    }

    #endregion Constructor

    #region Methods

    public override JSONClass ExportState(WixelDirectionInformation data)
    {
        throw new ApplicationException("WixelDirectionInformation is read-only!");
    }

    public override WixelDirectionInformation MapSingle(JSONNode rawItem)
    {
        WixelDirectionInformation model = new WixelDirectionInformation
        {
            Direction = rawItem["Direction"].ToEnum<WixelDirection>(),
            IsSolid = rawItem["IsSolid"].AsBool,
            Vertices = rawItem["Vertices"].AsArray.MapArrayWithMapper(Vector3Mapper),
            UVs = rawItem["UVs"].AsArray.MapArrayWithMapper(Vector2Mapper),
            Tile = rawItem["Tile"].ImportVector2()
        };

        return model;
    }

    #endregion Methods
}
