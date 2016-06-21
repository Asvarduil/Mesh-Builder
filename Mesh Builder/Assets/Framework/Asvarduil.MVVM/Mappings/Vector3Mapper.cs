using SimpleJSON;
using UnityEngine;

public class Vector3Mapper : SimpleJsonMapper<Vector3>
{
    #region Constructor

    public Vector3Mapper() : base("Vector3")
    {
    }

    #endregion Constructor

    #region Methods

    public override JSONClass ExportState(Vector3 data)
    {
        return data.ExportAsJson();
    }

    public override Vector3 MapSingle(JSONNode rawItem)
    {
        return rawItem.ImportVector3();
    }

    #endregion Methods
}