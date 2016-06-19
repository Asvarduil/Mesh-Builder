using System;
using UnityEngine;
using SimpleJSON;

public class MeshDetailMapping : SimpleJsonMapper<MeshDetail>
{
    #region Properties

    private MaterialDetailMapping _materialDetailMapper;
    private MaterialDetailMapping MaterialDetailMapper
    {
        get
        {
            return _materialDetailMapper ?? (_materialDetailMapper = new MaterialDetailMapping());
        }
    }

    #endregion Properties

    #region Constructor

    public MeshDetailMapping() : base("MeshDetails")
    {
    }

    #endregion Constructor

    #region Methods

    public override JSONClass ExportState(MeshDetail data)
    {
        throw new InvalidOperationException("MeshDetails are read-only.");
    }

    public override MeshDetail MapSingle(JSONNode node)
    {
        MeshDetail newModel = new MeshDetail();

        newModel.MeshPath = node["MeshPath"];

        // Optional parameter
        if (node["ObjectScale"] != null)
            newModel.ObjectScale = node["ObjectScale"].ImportVector3();
        else
            newModel.ObjectScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Optional parameter
        if (node["MeshOffset"] != null)
            newModel.MeshOffset = node["MeshOffset"].ImportVector3();
        else
            newModel.MeshOffset = Vector3.zero;

        // Optional parameter
        if (node["MeshRotation"] != null)
            newModel.MeshRotation = node["MeshRotation"].ImportVector3();
        else
            newModel.MeshRotation = Vector3.zero;

        // Optional parameter
        if (node["MeshScale"] != null)
            newModel.MeshScale = node["MeshScale"].ImportVector3();
        else
            newModel.MeshScale = new Vector3(1.0f, 1.0f, 1.0f);

        newModel.MaterialDetails = node["MaterialDetails"].AsArray.MapArrayWithMapper(MaterialDetailMapper);

        // Optional parameter
        if (node["AnimationControllerPath"] != null)
            newModel.AnimationControllerPath = node["AnimationControllerPath"];
        else
            newModel.AnimationControllerPath = string.Empty;

        return newModel;
    }

    #endregion Methods
}
