using System;
using System.Collections.Generic;
using SimpleJSON;

public abstract class SimpleJsonMapper<T> : IMapper<T>
    where T : new()
{
    #region Variables / Properties

    protected readonly string _MapKey;

    #endregion Variables / Properties

    #region Constructor

    protected SimpleJsonMapper(string mapKey)
    {
        _MapKey = mapKey;
    }

    #endregion Constructor

    #region Methods

    public virtual List<T> Map(object rawSource)
    {
        if (!(rawSource is JSONNode))
            throw new InvalidOperationException(_MapKey + " data can only be JSONNode objects.");

        return MapFromJson(rawSource as JSONNode);
    }

    public virtual object UnMap(T sourceObject)
    {
        return ExportState(sourceObject);
    }

    // Technology-specific methods
    public virtual List<T> MapFromJson(JSONNode parsed)
    {
        JSONArray jsonModels = parsed[_MapKey].AsArray;
        List<T> result = jsonModels.MapArrayWithMapper(this);
        return result;
    }

    // Single-item conversions
    public abstract T MapSingle(JSONNode rawItem);
    public abstract JSONClass ExportState(T data);

    #endregion Methods
}
