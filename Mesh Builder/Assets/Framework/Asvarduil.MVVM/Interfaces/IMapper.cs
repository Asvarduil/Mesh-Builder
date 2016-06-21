using System.Collections.Generic;

public interface IMapper<T>
{
    List<T> Map(object rawSource);
    object UnMap(T sourceObject);
}
