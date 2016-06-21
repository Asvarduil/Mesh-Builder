using UnityEngine;

public struct WorldPosition
{
    #region Fields/Properties

    public int x;
    public int y;
    public int z;

    #endregion Fields/Properties

    #region Constructor

    public WorldPosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    #endregion Constructor

    #region Methods

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}", x, y, z);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    #endregion Methods
}
