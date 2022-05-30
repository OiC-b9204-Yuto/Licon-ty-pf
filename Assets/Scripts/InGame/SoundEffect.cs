using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SoundEffect : MonoBehaviour
{
    //���ʉ��̎�ނ̗񋓌^
    private enum SE
    {
        FootStep,
        Damaged,
        LeafRecovery,
    }

    //�g�p������ʉ�
    [SerializeField]
    private AudioClip[] audioClip;
    //�I�[�f�B�I�~�L�T�[
    [SerializeField]
    private AudioMixerGroup audioMixerGroup;

    private AudioSource audioSource;

    //���ʉ��񋓌^�L���X�g����p�ϐ�
    private int enumValue;
    
    private void Start()
    {
        audioSource = CreateAudioSource();
    }

    //���ʉ��Đ�
    //�����Fstring (�A�j���[�V�����F�C�x���g��String�̓��e��switch������)
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

    //�I�[�f�B�I�\�[�X�쐬
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
