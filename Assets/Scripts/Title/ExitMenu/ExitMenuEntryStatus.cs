using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitMenuEntryStatus : MonoBehaviour
{
    private Button button;
    [SerializeField] Sprite[] sprites = new Sprite[2];
    public void Initialize()
    {
        button = GetComponent<Button>();
    }

    public void SpriteUpdate(bool isselect)
    {
        if (isselect)
        { GetComponent<Image>().sprite = sprites[1]; }
        else
        { GetComponent<Image>().sprite = sprites[0]; }
    }

    public void FocusButton() { button.Select(); }
}
