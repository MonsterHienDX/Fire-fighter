using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinLosePanel : PanelDropDownMono
{
    [SerializeField] private Image levelPassImg;
    [SerializeField] private Image IconImg;

    [SerializeField] private Sprite winTextImg;
    [SerializeField] private Sprite winIcon;
    [SerializeField] private Sprite loseTextImg;
    [SerializeField] private Sprite loseIcon;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventDispatcher.Instance.RegisterListener(EventID.EndLevel, SetWinLoseImgs);
        EventDispatcher.Instance.RegisterListener(EventID.EndLevel, ShowPanelWinLose);
        EventDispatcher.Instance.RegisterListener(EventID.LoadLevel, HidePanelWinLose);

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventDispatcher.Instance.RemoveListener(EventID.EndLevel, SetWinLoseImgs);
        EventDispatcher.Instance.RemoveListener(EventID.EndLevel, ShowPanelWinLose);
        EventDispatcher.Instance.RemoveListener(EventID.LoadLevel, HidePanelWinLose);
    }

    private void ShowPanelWinLose(object param = null)
    {
        ShowPanel();
    }

    private void HidePanelWinLose(object param = null)
    {
        HidePanel();
    }

    private void SetWinLoseImgs(object param = null)
    {
        bool isWin = (bool)param;

        if (isWin)
        {
            IconImg.sprite = winIcon;
            levelPassImg.sprite = winTextImg;
        }
        else
        {
            IconImg.sprite = loseIcon;
            levelPassImg.sprite = loseTextImg;
        }
    }

}
