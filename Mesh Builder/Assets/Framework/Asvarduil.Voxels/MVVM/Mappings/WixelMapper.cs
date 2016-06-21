using System;
using SimpleJSON;

public class WixelMapping : SimpleJsonMapper<Wixel>
{
    #region Variables / Properties

    private WixelDirectionInformationMapper _wixelDirectionInformationMapper;
    private WixelDirectionInformationMapper WixelDirectionInformationMapper
    {
        get
        {
            if (_wixelDirectionInformationMapper == null)
                _wixelDirectionInformationMapper = new WixelDirectionInformationMapper();

            return _wixelDirectionInformationMapper;
        }
    }

    #endregion Variables / Properties

    #region Constructor

    public WixelMapping() : base("Wixels")
    {
    }

    #endregion Constructor

    #region Methods

    public override JSONClass ExportState(Wixel data)
    {
        throw new ApplicationException("Wixels are read-only!");
    }

    public override Wixel MapSingle(JSONNode rawItem)
    {
        Wixel model = new Wixel
        {
            Name = rawItem["Name"],
            FaceInformation = rawItem["FaceInformation"].AsArray.MapArrayWithMapper(WixelDirectionInformationMapper)
        };

        return model;
    }

    #endregion Methods
}
