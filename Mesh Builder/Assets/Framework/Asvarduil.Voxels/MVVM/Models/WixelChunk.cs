using UnityEngine;

public class WixelChunk : DebuggableBehavior
{
    #region Variables / Properties

    public bool PerformUpdates = false;
    public bool IsRendered;

    // TODO: Make chunkSize a configurable property at the application level.
    public static int chunkSize = 16;

    public WixelWorld World;
    public WorldPosition WorldPosition;

    public Wixel[,,] _blocks = new Wixel[chunkSize, chunkSize, chunkSize];
    public Wixel[,,] Blocks
    {
        get { return _blocks; }
        set { _blocks = value; }
    }

    private MeshFilter _filter;
    private MeshFilter Filter
    {
        get
        {
            if (_filter == null)
                _filter = GetComponent<MeshFilter>();

            return _filter;
        }
    }

    private MeshCollider _collider;
    private MeshCollider Collider
    {
        get
        {
            if (_collider == null)
                _collider = GetComponent<MeshCollider>();

            return _collider;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    //Use this for initialization
    public void Start()
    {
    }

    public void Update()
    {
        if (!PerformUpdates)
            return;

        PerformUpdates = false;
        UpdateChunk();
    }

    #endregion Hooks

    #region Methods

    // TODO - Is a little Vector3 addition too hard?
    public Wixel GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return _blocks[x, y, z];

        return World.GetBlock(WorldPosition.x + x, WorldPosition.y + y, WorldPosition.z + z);
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    public void SetBlock(int x, int y, int z, Wixel block)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            _blocks[x, y, z] = block;
        }
        else
        {
            World.SetBlock(WorldPosition.x + x, WorldPosition.y + y, WorldPosition.z + z, block);
        }
    }

    private void UpdateChunk()
    {
        IsRendered = true;

        WixelChunkMeshData meshData = new WixelChunkMeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    var block = _blocks[x, y, z];
                    meshData = block.BlockData(this, x, y, z, meshData);
                }
            }
        }

        RenderMesh(meshData);
    }

    public void SetBlocksUnmodified()
    {
        foreach (Wixel block in Blocks)
        {
            block.HasChanged = false;
        }
    }

    // Sends the calculated mesh information
    // to the mesh and collision components
    private void RenderMesh(WixelChunkMeshData meshData)
    {
        Filter.mesh.Clear();
        Filter.mesh.vertices = meshData.vertices.ToArray();
        Filter.mesh.triangles = meshData.triangles.ToArray();

        Filter.mesh.uv = meshData.uv.ToArray();
        Filter.mesh.RecalculateNormals();

        Collider.sharedMesh = null;

        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        Collider.sharedMesh = mesh;
    }

    #endregion Methods
}
