using SimpleJSON;
using UnityEngine;

public class Vector2Mapper : SimpleJsonMapper<Vector2>
{
    #region Constructor

    public Vector2Mapper() : base("Vector2")
    {
    }

    #endregion Constructor

    #region Methods

    public override JSONClass ExportState(Vector2 data)
    {
        return data.ExportAsJson();
    }

    public override Vector2 MapSingle(JSONNode rawItem)
    {
        return rawItem.ImportVector2();
    }

    #endregion Methods
}
