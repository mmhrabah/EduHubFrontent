using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectManager : MonoBehaviour
{
    [SerializeField]
    private ButtonManager[] buttons;

    [SerializeField]
    private Color selectedIconColor;
    [SerializeField]
    private Color selectedTextColor;
    [SerializeField]
    private Color selectedBGColor;

    [SerializeField]
    private Color unselectedIconColor;
    [SerializeField]
    private Color unselectedTextColor;
    [SerializeField]
    private Color unselectedBGColor;

    private Image[] buttonBgImages;

    private void Awake()
    {
        buttonBgImages = new Image[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].normalLayout.transform.childCount > 0)
            {
                buttonBgImages[i] = buttons[i].normalLayout.transform.GetChild(0).GetComponent<Image>();
            }
        }
    }
    public void SelectButton(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == index)
            {
                buttons[i].normalText.color = selectedTextColor;
                buttons[i].normalImage.color = selectedIconColor;
                buttonBgImages[i].enabled = true;
                buttonBgImages[i].color = selectedBGColor;
            }
            else
            {
                buttons[i].normalText.color = unselectedTextColor;
                buttons[i].normalImage.color = unselectedIconColor;
                buttonBgImages[i].enabled = true;
                buttonBgImages[i].color = unselectedBGColor;
            }
        }
    }

}
