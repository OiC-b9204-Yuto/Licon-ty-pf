using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Damaged
{
	public class PlayerDamaged : MonoBehaviour
	{
		//ダメージ時にHPを減らす処理を追加するときにコメントアウト解除
		private PlayerMove playerMove;

		//子のRendererの配列
		public Renderer[] childrenRenderer;

		//childrenRendererが有効か無効かのフラグ
		bool isEnabledRenderers;

		//ダメージを受けているか(点滅中か)のフラグ
		public bool isDamaged { get; private set; }

		//リセットする時の為にコルーチンを保持
		Coroutine blinkCoroutine;

		//ダメージ点滅の長さ
		float blinkDuration = 1.5f;

		//ダメージ点滅の合計経過時間
		float blinkTotalElapsedTime;

		//ダメージ点滅のRendererの有効・無効切り替え用の経過時間
		float blinkElapsedTime;

		//ダメージ点滅のRendererの有効・無効切り替え用のインターバル
		float blinkInterval = 0.05f;

		//HP獲得用変数
		int HP;

		void Start()
		{
			playerMove = GetComponent<PlayerMove>();
			childrenRenderer = GetComponentsInChildren<Renderer>();
		}

		public void Damaged()
		{
			//ダメージ点滅中は二重に実行しない
			if (isDamaged)
				return;

			//HP減少(未実装)
			HP = playerMove.HP;
			HP -= 1;
			if (HP < 0)
            {
				HP = 0;
			}
			playerMove.HP = HP;

			//死んだ場合はダメージ点滅させない
			if (HP <= 0) 
			{
				return;
			}
			

			//点滅開始
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
					//被ダメージ点滅の処理
					blinkElapsedTime = 0;
					//Rendererの有効、無効の反転
					isEnabledRenderers = !isEnabledRenderers;
					SetEnabledRenderers(isEnabledRenderers);
				}

				if (blinkDuration <= blinkTotalElapsedTime)
				{
					//被ダメージ点滅の終了時の処理
					isDamaged = false;
					//Rendererを有効にする(消えっぱなしになるのを防ぐ)
					isEnabledRenderers = true;
					SetEnabledRenderers(true);

					yield break;
				}

				yield return null;
			}
		}

		//コルーチンのリセット用
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