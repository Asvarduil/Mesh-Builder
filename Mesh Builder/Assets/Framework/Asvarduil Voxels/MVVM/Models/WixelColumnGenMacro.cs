using System;

public class WixelColumnGenMacro
{
    #region Fields

    public string Name;
    public Action<int, int, int, WixelChunk> OnExecute;

    #endregion Fields
}
