using UnityEngine;
using UnityEngine.UI;
using UICommon;

public class BMenuEntry : MonoBehaviour
{
    public Animator animator { get; set; }
    [SerializeField] protected Text text;
    protected bool isSelect;

    public virtual void Initialize()
    {
        //�R���|�[�l���g�̎擾
        animator = GetComponent<Animator>();

        isSelect = false;
    }
    public void PlayInAnimation() { animator.SetTrigger("In"); }
    public void PlayOutAnimation() { animator.SetTrigger("Out"); }
    public void SetSceneExitFlg() { animator.SetBool("isExitScene", true); }

    public bool SpriteUpdate(GameObject esys)
    {
        //�I�𒆁E��I�𒆂̃X�v���C�g�̐؂�ւ�����
        if (esys == this.gameObject)
        { SelectMenu(); return true; }
        else
        { DeselectMenu(); return false; }
    }
    public virtual void Focus() { }
    public virtual void ValueChange(float f1) { }
    public virtual void ValueChange(int i1) { }
    public virtual void ValueChange(bool b1) { }
    public virtual float GetValueF() { return 0; }
    public virtual int GetValueI() { return 0; }
    public virtual bool GetValueB() { return false; }

    public bool isSelectMenu(GameObject esys)
    {
        return esys == gameObject;
    }

    public virtual void SelectMenu() { }
    public virtual void DeselectMenu() { }
}
