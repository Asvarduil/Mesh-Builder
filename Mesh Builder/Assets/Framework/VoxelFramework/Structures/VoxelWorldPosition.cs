using System;

[Serializable]
public struct VoxelWorldPosition
{
    #region Fields

    public int x;
    public int y;
    public int z;

    #endregion Fields

    #region Constructor

    public VoxelWorldPosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    #endregion Constructor

    #region Methods

    public override bool Equals(object obj)
    {
        return GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 47;
            hash = hash * 227 + x.GetHashCode();
            hash = hash * 227 + y.GetHashCode();
            hash = hash * 227 + z.GetHashCode();
            return hash;
        }
    }

    #endregion Methods
}
