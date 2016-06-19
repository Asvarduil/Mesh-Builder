using UnityEngine;

public class DebugCameraModify : MonoBehaviour
{
    #region Fields

    public float CameraSpeed = 3.0f;

    Vector2 rot;

    private WixelManager _manager;
    private WixelManager Manager
    {
        get
        {
            if (_manager == null)
                _manager = WixelManager.Instance;

            return _manager;
        }
    }

    private WixelRepository _repository;
    private WixelRepository Repository
    {
        get
        {
            if (_repository == null)
                _repository = WixelRepository.Instance;

            return _repository;
        }
    }

    private Wixel _air;
    private Wixel Air
    {
        get
        {
            if (_air == null)
                _air = Repository.GetWixelByName("Air");

            return _air;
        }
    }

    #endregion Fields

    #region Hooks

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                //VoxelTerrainService.SetBlock(hit, new AirVoxel());
                Manager.SetBlock(hit, Air);
            }
        }

        rot = new Vector2(rot.x + Input.GetAxis("Mouse X") * 3, rot.y + Input.GetAxis("Mouse Y") * 3);

        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

        transform.position += transform.forward * CameraSpeed * Input.GetAxis("Vertical");
        transform.position += transform.right * CameraSpeed * Input.GetAxis("Horizontal");
    }

    #endregion Hooks
}
