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
    //�P�ł��邱�Ƃ�ۏ؂��邽�߁��O���[�o���A�N�Z�X�p
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
        else if (Instance != this) //���g�����ɂ�����悤�Ȃ�
        {
            Destroy(gameObject); //�폜
            return;
        }
        //soundDictionary�ɃZ�b�g
        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.name, soundData);
        }
        audioSource = GetComponent<AudioSource>();
        Data.SetSEVolume(audioSource);
    }

    //�w�肳�ꂽ�ʖ��œo�^���ꂽAudioClip���Đ�
    public void Play(string name)
    {
        //�Ǘ��pDictionary ����A�ʖ��ŒT��
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            //����������A�Đ�
            audioSource.PlayOneShot(soundData.audioClip); 
        }
        else
        {
            Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���:{name}");
        }
    }
}
