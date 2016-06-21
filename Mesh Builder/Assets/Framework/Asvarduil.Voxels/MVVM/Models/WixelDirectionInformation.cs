using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WixelDirectionInformation
{
    #region Variables

    public WixelDirection Direction;
    public Vector2 Tile;
    public bool IsSolid;
    public List<Vector3> Vertices;
    public List<Vector2> UVs;

    #endregion Variables
}
