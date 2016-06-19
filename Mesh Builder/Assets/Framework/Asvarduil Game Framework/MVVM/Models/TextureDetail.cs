using System;

public enum TextureDetailType
{
    Texture,
    Tint,
    Int,
    Float,
    Offset
}

[Serializable]
public class TextureDetail
{
    #region Fields

    public TextureDetailType DetailType;
    public string DetailKey;
    public object DetailValue;

    #endregion Fields
}