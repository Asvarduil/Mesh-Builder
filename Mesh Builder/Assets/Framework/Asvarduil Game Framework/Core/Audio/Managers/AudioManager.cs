using System;
using UnityEngine;

public class AudioManager : ManagerBase<AudioManager>
{
    #region Variables / Properties

    public bool SoundEnabled;
    public float MasterVolume;
    public float MusicVolume;
    public float EffectVolume;

    public float EffectiveMasterVolume
    {
        get { return SoundEnabled ? MasterVolume : 0.0f; }
    }

    #endregion Variables / Properties

    #region Methods

    public AudioClip GetAudioByPath(string audioPath)
    {
        AudioClip clip = Resources.Load<AudioClip>(audioPath);
        if (clip == null)
            throw new ApplicationException("Could not find an audio clip at path " + audioPath);

        return clip;
    }

    #endregion Methods
}
