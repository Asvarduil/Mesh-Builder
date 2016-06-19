using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class WixelRepository : RepositoryBase<WixelRepository, Wixel>
{
    #region Variables / Properties

    public WixelDataAccessor DataAccessor;
    public string saveFolderName = "WixelWorldSaves";

    #endregion Variables / Properties

    #region Hooks

    public override void Awake()
    {
        _mapper = new WixelMapping();
        _dataAccessor = DataAccessor;

        base.Awake();
    }

    #endregion Hooks

    #region Methods

    public Wixel GetWixelByName(string wixelName)
    {
        Wixel wixel = Contents.FindItemByName(wixelName);
        if (wixel == default(Wixel))
            wixel = null;

        return wixel;
    }

    #endregion Methods

    #region Chunk Methods

    public string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    public string FileName(VoxelWorldPosition chunkLocation)
    {
        // TODO: Add a ToString to VoxelWorldPosition?
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
        return fileName;
    }

    public void SaveChunk(WixelChunk chunk)
    {
        WixelChunkSaveBuffer save = new WixelChunkSaveBuffer(chunk);
        if (save.Blocks.Count == 0)
            return;

        string saveFile = SaveLocation(chunk.World.WorldName);
        saveFile += FileName(chunk.WorldPosition);

        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            formatter.Serialize(stream, chunk.Blocks);
            stream.Close();
        }
    }

    public bool LoadChunk(WixelChunk chunk)
    {
        string saveFile = SaveLocation(chunk.World.WorldName);
        saveFile += FileName(chunk.WorldPosition);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(saveFile, FileMode.Open))
        {
            try
            {
                WixelChunkSaveBuffer save = (WixelChunkSaveBuffer)formatter.Deserialize(stream);

                foreach (var block in save.Blocks)
                {
                    chunk.Blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                stream.Close();
            }
        }

        return true;
    }

    #endregion Chunk Methods
}
