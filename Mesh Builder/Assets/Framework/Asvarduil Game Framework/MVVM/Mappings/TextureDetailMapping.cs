using System;
using System.Collections.Generic;
using SimpleJSON;

public class TextureDetailMapping : SimpleJsonMapper<TextureDetail>
{
    #region Constructor

    public TextureDetailMapping() : base("Texture Details")
    {
    }

    #endregion Constructor

    #region Methods

    public override JSONClass ExportState(TextureDetail data)
    {
        throw new DataException("Texture Details are read-only.");
    }

    public override TextureDetail MapSingle(JSONNode rawItem)
    {
        TextureDetailType detailType = rawItem["DetailType"].ToEnum<TextureDetailType>();

        object detailValue = null;
        switch(detailType)
        {
            case TextureDetailType.Texture:
                detailValue = rawItem["DetailValue"];
                break;

            case TextureDetailType.Tint:
                detailValue = rawItem["DetailValue"].ImportColor();
                break;

            case TextureDetailType.Offset:
                detailValue = rawItem["DetailValue"].ImportVector2();
                break;

            case TextureDetailType.Int:
                detailValue = rawItem["DetailValue"].AsInt;
                break;

            case TextureDetailType.Float:
                detailValue = rawItem["DetailValue"].AsFloat;
                break;
        }

        TextureDetail model = new TextureDetail
        {
            DetailType = detailType,
            DetailKey = rawItem["DetailKey"],
            DetailValue = detailValue
        };

        return model;
    }

    #endregion Methods
}
