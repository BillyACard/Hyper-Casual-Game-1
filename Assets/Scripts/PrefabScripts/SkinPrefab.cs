using UnityEngine;
using UnityEngine.UI;

public class SkinPrefab : MonoBehaviour
{
    [SerializeField] private Image border;
    [SerializeField] private Image skin;
    private int _index;

    public void SetBorder(bool highlight)
    {
        border.color = highlight ? Color.yellow : Color.white;
    }

    public void SetSkin(Sprite skinSprite)
    {
        skin.sprite = skinSprite;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void OnSelect()
    {
        SkinManager.Instance.SelectSkin(_index);
    }
}