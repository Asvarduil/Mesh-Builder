using System;

public static class StringExtensions
{
    #region Methods

    public static T ToEnum<T>(this string value)
    {
        T result = (T)Enum.Parse(typeof(T), value);
        return result;
    }

    #endregion Methods
}
