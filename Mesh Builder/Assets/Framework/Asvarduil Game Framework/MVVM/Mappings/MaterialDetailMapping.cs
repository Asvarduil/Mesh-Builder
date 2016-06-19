using SimpleJSON;

public class MaterialDetailMapping : SimpleJsonMapper<MaterialDetail>
{
    #region Properties

    private TextureDetailMapping _textureDetailMapping;
    private TextureDetailMapping TextureDetailMapping
    {
        get
        {
            if (_textureDetailMapping == null)
                _textureDetailMapping = new TextureDetailMapping();

            return _textureDetailMapping;
        }
    }

    #endregion Properties

    #region Constructor

    public MaterialDetailMapping() : base("Material Details")
    {
    }

    #endregion Constructor

    #region Methods

    public override JSONClass ExportState(MaterialDetail data)
    {
        throw new DataException("Material Details are read-only.");
    }

    public override MaterialDetail MapSingle(JSONNode node)
    {
        MaterialDetail model = new MaterialDetail
        {
            Name = node["Name"],
            MaterialPath = node["MaterialPath"],
            TextureDetails = node["TextureDetails"].AsArray.MapArrayWithMapper(TextureDetailMapping)
        };

        return model;
    }

    #endregion Methods
}
