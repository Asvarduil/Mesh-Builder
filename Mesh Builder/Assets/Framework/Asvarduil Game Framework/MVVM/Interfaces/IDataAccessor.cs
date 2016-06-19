using System.Collections.Generic;

public interface IDataAccessor<T>
{
    bool HasLoaded { get; }

    List<T> ReadData(IMapper<T> mapper);
    void SaveData(T data, IMapper<T> mapper);
}