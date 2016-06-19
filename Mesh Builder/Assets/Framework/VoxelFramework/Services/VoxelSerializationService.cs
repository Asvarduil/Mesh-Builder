using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class VoxelSerializationService
{
    #region Variables / Properties

    public static string saveFolderName = "voxelGameSaves";

    #endregion Variables / Properties

    #region Methods

    public static string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    public static string FileName(VoxelWorldPosition chunkLocation)
    {
        // TODO: Add a ToString to VoxelWorldPosition?
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
        return fileName;
    }

    public static void SaveChunk(VoxelChunk chunk)
    {
        VoxelSaveBuffer save = new VoxelSaveBuffer(chunk);
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

    public static bool Load(VoxelChunk chunk)
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
                VoxelSaveBuffer save = (VoxelSaveBuffer)formatter.Deserialize(stream);

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

    #endregion Methods
}
