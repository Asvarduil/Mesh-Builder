using System;

public class WorldPosition
{
  #region Fields/Properties
  
  public int x;
  public int y;
  public int z;
  
  #endregion Fields/Properties
  
  #region Methods
  
  public override string ToString()
  {
    return string.Format("{0}, {1}, {2}", x, y, z);
  }
  
  #endregion Methods
}
