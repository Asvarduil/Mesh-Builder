using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public enum BlobSource
{
    File,
    Local,
    Remote,
    Raw
}

public class SimpleJsonDataAccessor<T> : DebuggableBehavior, IDataAccessor<T>
{
    #region Variables / Properties

    public BlobSource Source;
    public string FilePath;
    public TextAsset LocalBlob;
    public string SaveBlobUrl;
    public string RemoteBlobUrl;
    public string RawBlob;

    private JSONNode _mappedJson;

    public bool HasLoaded { get; protected internal set; }

    #endregion Variables / Properties

    #region Hooks

    public List<T> ReadData(IMapper<T> mapper)
    {
        ReadBlobFromSource();

        List<T> results = mapper.Map(_mappedJson);
        return results;
    }

    public void SaveData(T data, IMapper<T> mapper)
    {
        JSONNode node = mapper.UnMap(data) as JSONNode;
        SaveBlobToSource(node);
    }

    #endregion Hooks

    #region Methods

    protected void ReadBlobFromSource()
    {
        switch (Source)
        {
            case BlobSource.File:
                RawBlob = ReadBlobFromFile();
                _mappedJson = JSON.Parse(RawBlob);
                break;

            case BlobSource.Local:
                RawBlob = FetchLocalBlob();
                _mappedJson = JSON.Parse(RawBlob);
                break;

            case BlobSource.Remote:
                StartCoroutine(DownloadBlob());
                break;

            case BlobSource.Raw:
                DebugMessage("Softcoded blob is already loaded.", LogLevel.Warn);
                break;

            default:
                throw new InvalidOperationException("Unexpected Blob Source: " + Source);
        }
    }

    protected void SaveBlobToSource(JSONNode node)
    {
        switch (Source)
        {
            case BlobSource.File:
                SaveBlobToFile(node);
                break;

            case BlobSource.Remote:
                StartCoroutine(SendBlob(SaveBlobUrl, node));
                break;

            case BlobSource.Local:
            case BlobSource.Raw:
                throw new InvalidOperationException("Cannot save to a local fileasset or a raw blob.");

            default:
                throw new InvalidOperationException("Unexpected Blob Source: " + Source);
        }
    }

    protected IEnumerator SendBlob(string url, JSONNode node)
    {
        WWWForm form = new WWWForm();
        form.AddField("Data", node.ToString());

        WWW dataInterface = new WWW(url, form);
        yield return dataInterface;

        if (!string.IsNullOrEmpty(dataInterface.error))
        {
            Exception inner = new Exception(dataInterface.error);
            throw new InvalidOperationException("Could not download from the remote source.", inner);
        }
    }

    protected IEnumerator DownloadBlob()
    {
        HasLoaded = false;
        using (WWW dataInterface = new WWW(RemoteBlobUrl))
        {
            while (!dataInterface.isDone)
                yield return 0;

            if (!string.IsNullOrEmpty(dataInterface.error))
            {
                Exception inner = new Exception(dataInterface.error);
                throw new InvalidOperationException("Could not download from the remote source.", inner);
            }

            RawBlob = dataInterface.text;
            _mappedJson = JSON.Parse(RawBlob);
            HasLoaded = true;
        }
    }

    protected void SaveBlobToFile(JSONNode node)
    {
        if (LocalBlob == null)
            return;

        node.SaveToFile(FilePath);
    }

    protected string ReadBlobFromFile()
    {
        if (string.IsNullOrEmpty(FilePath))
            return string.Empty;

        // This line is more concise, but requires functions that don't exist
        // in Unity's version of Mono.NET.
        //string result = File.ReadAllText(FilePath);

        // This should brute-force the text of the file into the buffer.
        string result = string.Empty;
        using (StreamReader reader = new StreamReader(FilePath))
        {
            string line = string.Empty;
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    result += line;
                }
            }
            finally
            {
                reader.Close();
            }
        }

        HasLoaded = true;
        return result;
    }

    protected string FetchLocalBlob()
    {
        if (LocalBlob == null)
            return string.Empty;

        HasLoaded = true;
        return LocalBlob.text;
    }

    #endregion Methods
}
