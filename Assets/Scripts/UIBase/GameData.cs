using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public int resolutionNo;
    public bool isFullScreen;
    public int quality;
    public float BGMVolume;
    public float SEVolume;

    public void ResetDefaultStatus()
    {
        resolutionNo = 0;
        isFullScreen = false;
        quality = 1;
        BGMVolume = 0.5f;
        SEVolume = 0.5f;
    }
}
