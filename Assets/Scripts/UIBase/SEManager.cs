using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICommon;

public  class SEManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
    }

    [SerializeField] private SoundData[] soundDatas;
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

    private AudioSource audioSource;
    //１つであることを保証するため＆グローバルアクセス用
    public static SEManager Instance
    {
        private set;
        get;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this) //自身が他にもあるようなら
        {
            Destroy(gameObject); //削除
            return;
        }
        //soundDictionaryにセット
        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.name, soundData);
        }
        audioSource = GetComponent<AudioSource>();
        Data.SetSEVolume(audioSource);
    }

    //指定された別名で登録されたAudioClipを再生
    public void Play(string name)
    {
        //管理用Dictionary から、別名で探索
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            //見つかったら、再生
            audioSource.PlayOneShot(soundData.audioClip); 
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }
}
