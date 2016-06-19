using System;
using SimpleJSON;

[Serializable]
public class ModifiableStat : INamed, ICloneable
{
	#region Variables / Properties

	public string Name;
	public int Value;
    public int ValueCap;

	public int FixedModifier = 0;
	public float ScalingModifier = 1.0f;

    public string EntityName { get { return Name; } }

    public int Current
    {
        get { return ModifiedValue; }
        set { Value = value; }
    }

    public int Maximum
    {
        get { return ValueCap; }
        set { ValueCap = value; }
    }

	public int ModifiedValue
	{
		get 
        { 
            int modified = ((int)(Value * ScalingModifier)) + FixedModifier;
            return modified > ValueCap
                ? ValueCap
                : modified;
        }
	}

    public bool IsAtMax
    {
        get { return ModifiedValue == ValueCap; }
    }

    public bool IsAtZero
    {
        get { return ModifiedValue == 0; }
    }

    #endregion Variables / Properties

    #region Methods

    public object Clone()
    {
        ModifiableStat clone = new ModifiableStat
        {
            Name = Name,
            Value = Value,
            ValueCap = ValueCap,
            ScalingModifier = ScalingModifier,
            FixedModifier = FixedModifier
        };

        return clone;
    }

    public void Alter(int amount)
    {
        Value += amount;

        if (Value > ValueCap)
            Value = ValueCap;
        else if (Value < 0)
            Value = 0;
    }

    public void Increase(int amount)
    {
        Value += amount;

        if (Value > ValueCap)
            Value = ValueCap;
    }

    public void Decrease(int amount)
    {
        Value -= amount;

        if (Value < 0)
            Value = 0;
    }

    public void RaiseMax(int amount)
    {
        ValueCap += amount;
    }

    #endregion Methods
}
