using System;
using System.IO;
using UnityEngine;

namespace UICommon
{
    public enum TitleSceneState
    {
        TSS_INTROANIM,
        TSS_FRONTMENU,
        TSS_EXITMENU,
        TSS_OPTIONMENU,
        TSS_CHANGESCENE
    }
    public enum StageSelectUIStatus
    {
        SSST_SCENESTART,
        SSST_INSELECT,
        SSST_DECIDESTAGE,
        SSST_BACKTITLE
    }
    public enum InGameStatus
    {
        IGS_INTRO,
        IGS_INGAME,
        IGS_PAUSE,
        IGS_HELP,
        IGS_GAMEOVER,
        IGS_GAMECLEAR,
        IGS_SCENECHANGE
    }

    public static class Data
    {
        private static readonly Vector2Int[] resolutions = new Vector2Int[]
        {
            new Vector2Int(1920,1080),
            new Vector2Int(1600,900),
            new Vector2Int(1366,768),
            new Vector2Int(1280,720),
            new Vector2Int(1120,630),
            new Vector2Int(960,540),
            new Vector2Int(800,450),
            new Vector2Int(640,360)
        };

        public const int MaxStageCount = 3;
        public static GameData gData;
        public static int currentStageNo { get; set; }

        public static int GetNextStageNo()
        {
            //次のステージがない場合、最初のステージに戻る
            if(currentStageNo + 1 > MaxStageCount)
            {
                return 1;
            }
            return currentStageNo + 1;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void DataLoad()
        {
            gData = new GameData();
            try
            {
                //ファイルを取得
                gData = JsonUtility.FromJson<GameData>(JsonManager.GetJsonFile("/", "GameData.json"));
                Debug.Log($"BGM: {gData.BGMVolume}, SE: {gData.SEVolume}");
            }
            catch(Exception e)
            {
                //ファイルがない場合、デフォルトのファイルを生成
                gData.ResetDefaultStatus();
                string jsonGameData = JsonUtility.ToJson(gData);
                StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/GameData.json", false);
                writer.Write(jsonGameData);
                writer.Flush();
                writer.Close();
            }

            SetResolution();
            SetQuality();
        }

        public static void SetBGMVolume(AudioSource audio)
        {
            audio.volume = gData.BGMVolume;
        }
        public static void SetSEVolume(AudioSource audio)
        {
            audio.volume = gData.SEVolume;
        }
        public static void SetResolution()
        {
            Vector2Int res = resolutions[gData.resolutionNo];
            Screen.SetResolution(res.x, res.y, gData.isFullScreen);
        }
        public static void SetQuality()
        {
            QualitySettings.SetQualityLevel(gData.quality);
        }
        public static void SaveBGMVolume(float volume)
        {
            gData.BGMVolume = volume;
        }
        public static void SaveSEVolume(float volume)
        {
            gData.SEVolume = volume;
        }
        public static void SaveResolution(int no)
        {
            gData.resolutionNo = no;
        }
        public static void SaveQuality(int no)
        {
            gData.quality = no;
        }
        public static void SaveIsFullScreen(bool flg)
        {
            gData.isFullScreen = flg;
        }

        public static void SaveAllData()
        {
            //オプション設定データを保存
            string jsonGameData = JsonUtility.ToJson(gData);
            StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/GameData.json", false);
            writer.Write(jsonGameData);
            writer.Flush();
            writer.Close();
        }
    }

    public static class AnimatorExtensions
    {
        public static bool isEndAnimation(this Animator animator, string animname)
        {
            //特定のアニメーションが終了しているか
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animname) &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f)
            {
                return true;
            }
            return false;
        }
    }
}

