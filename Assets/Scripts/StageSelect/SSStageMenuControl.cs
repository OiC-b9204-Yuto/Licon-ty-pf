public class SSStageMenuControl : BMenuControl
{
    public int GetSelectStageNum()
    {
        //ステージ番号判定処理
        for(int i = 0; i < entryMenus.Length; i++)
        {
            if(entryMenus[i].isSelectMenu(eSystem.currentSelectedGameObject.gameObject))
            {
                return i;
            }
        }
        return -1;
    }

    public void PlayEntryOut()
    {
        for(int i = 0; i < entryMenus.Length; i ++)
        {
            entryMenus[i].SetSceneExitFlg();
        }
    }
}
