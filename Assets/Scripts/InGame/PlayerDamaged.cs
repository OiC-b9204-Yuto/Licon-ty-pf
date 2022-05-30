using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Damaged
{
	public class PlayerDamaged : MonoBehaviour
	{
		//�_���[�W����HP�����炷������ǉ�����Ƃ��ɃR�����g�A�E�g����
		private PlayerMove playerMove;

		//�q��Renderer�̔z��
		public Renderer[] childrenRenderer;

		//childrenRenderer���L�����������̃t���O
		bool isEnabledRenderers;

		//�_���[�W���󂯂Ă��邩(�_�Œ���)�̃t���O
		public bool isDamaged { get; private set; }

		//���Z�b�g���鎞�ׂ̈ɃR���[�`����ێ�
		Coroutine blinkCoroutine;

		//�_���[�W�_�ł̒���
		float blinkDuration = 1.5f;

		//�_���[�W�_�ł̍��v�o�ߎ���
		float blinkTotalElapsedTime;

		//�_���[�W�_�ł�Renderer�̗L���E�����؂�ւ��p�̌o�ߎ���
		float blinkElapsedTime;

		//�_���[�W�_�ł�Renderer�̗L���E�����؂�ւ��p�̃C���^�[�o��
		float blinkInterval = 0.05f;

		//HP�l���p�ϐ�
		int HP;

		void Start()
		{
			playerMove = GetComponent<PlayerMove>();
			childrenRenderer = GetComponentsInChildren<Renderer>();
		}

		public void Damaged()
		{
			//�_���[�W�_�Œ��͓�d�Ɏ��s���Ȃ�
			if (isDamaged)
				return;

			//HP����(������)
			HP = playerMove.HP;
			HP -= 1;
			if (HP < 0)
            {
				HP = 0;
			}
			playerMove.HP = HP;

			//���񂾏ꍇ�̓_���[�W�_�ł����Ȃ�
			if (HP <= 0) 
			{
				return;
			}
			

			//�_�ŊJ�n
			Startblink();
		}

		void SetEnabledRenderers(bool b)
		{
			for (int i = 0; i < childrenRenderer.Length; i++)
			{
				childrenRenderer[i].enabled = b;
			}
		}

		void Startblink()
		{
			blinkCoroutine = StartCoroutine(Blink());
		}

		IEnumerator Blink()
		{

			isDamaged = true;

			blinkTotalElapsedTime = 0;
			blinkElapsedTime = 0;

			while (true)
			{

				blinkTotalElapsedTime += Time.deltaTime;
				blinkElapsedTime += Time.deltaTime;

				if (blinkInterval <= blinkElapsedTime)
				{
					//��_���[�W�_�ł̏���
					blinkElapsedTime = 0;
					//Renderer�̗L���A�����̔��]
					isEnabledRenderers = !isEnabledRenderers;
					SetEnabledRenderers(isEnabledRenderers);
				}

				if (blinkDuration <= blinkTotalElapsedTime)
				{
					//��_���[�W�_�ł̏I�����̏���
					isDamaged = false;
					//Renderer��L���ɂ���(�������ςȂ��ɂȂ�̂�h��)
					isEnabledRenderers = true;
					SetEnabledRenderers(true);

					yield break;
				}

				yield return null;
			}
		}

		//�R���[�`���̃��Z�b�g�p
		void Resetblink()
		{
			if (blinkCoroutine != null)
			{
				StopCoroutine(blinkCoroutine);
				blinkCoroutine = null;
			}
		}
	}

}