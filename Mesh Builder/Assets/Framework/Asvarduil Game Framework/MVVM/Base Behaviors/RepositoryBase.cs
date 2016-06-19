using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

public class RepositoryBase<T, T1> 
    : ManagerBase<T>
    where T : UnityObject
{
    #region Variables / Properties

    public List<T1> Contents;

    protected IDataAccessor<T1> _dataAccessor;
    protected IMapper<T1> _mapper;

    public bool HasLoaded
    {
        get
        {
            return _dataAccessor.HasLoaded;
        }
    }

    #endregion Variables / Properties

    #region Constructor

    #endregion Constructor

    #region Hooks

    public virtual void Awake()
    {
        Contents = _dataAccessor.ReadData(_mapper);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
