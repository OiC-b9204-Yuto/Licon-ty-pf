using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SoundEffect : MonoBehaviour
{
    //効果音の種類の列挙型
    private enum SE
    {
        FootStep,
        Damaged,
        LeafRecovery,
    }

    //使用する効果音
    [SerializeField]
    private AudioClip[] audioClip;
    //オーディオミキサー
    [SerializeField]
    private AudioMixerGroup audioMixerGroup;

    private AudioSource audioSource;

    //効果音列挙型キャスト代入用変数
    private int enumValue;
    
    private void Start()
    {
        audioSource = CreateAudioSource();
    }

    //効果音再生
    //引数：string (アニメーション：イベントのStringの内容でswitch文分岐)
    public void Play(string eventName)
    {
        switch(eventName)
        {
            case "FootStep":
                enumValue = (int)SE.FootStep;
                break;
            case "Damaged":
                enumValue = (int)SE.Damaged;
                break;
            case "LeafRecovery":
                enumValue = (int)SE.LeafRecovery;
                break;
        }
        if (audioSource.clip != audioClip[enumValue])
        { audioSource.clip = audioClip[enumValue]; }
        audioSource.Play();
    }

    //オーディオソース作成
    private AudioSource CreateAudioSource()
    {
        var audioGameObject = new GameObject();
        audioGameObject.name = "SoundEffect";
        audioGameObject.transform.SetParent(gameObject.transform);

        var audioSource = audioGameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        return audioSource;
    }
}
