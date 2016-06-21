using SimpleJSON;

public class ModifiableStatMapping : SimpleJsonMapper<ModifiableStat>
{
    #region Constructor

    public ModifiableStatMapping() : base("Stats")
    {

    }

    #endregion Constructor

    #region Methods

    public override JSONClass ExportState(ModifiableStat data)
    {
        JSONClass state = new JSONClass();

        state["Name"] = data.Name;
        state["Value"] = new JSONData(data.Value);
        state["ValueCap"] = new JSONData(data.ValueCap);
        state["FixedModifier"] = new JSONData(data.FixedModifier);
        state["ScalingModifier"] = new JSONData(data.ScalingModifier);

        return state;
    }

    public override ModifiableStat MapSingle(JSONNode node)
    {
        ModifiableStat result = new ModifiableStat
        {
            Name = node["Name"],
            Value = node["Value"].AsInt,
            ValueCap = node["ValueCap"].AsInt,
            FixedModifier = node["FixedModifier"].AsInt,
            ScalingModifier = node["ScalingModifier"].AsFloat
        };

        return result;
    }

    #endregion Methods
}
