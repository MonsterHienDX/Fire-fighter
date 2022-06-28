using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackGroundForPopups : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Start()
    {
        HideBackground();
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ShowPopup, ShowBackground);
        EventDispatcher.Instance.RegisterListener(EventID.HidePopup, HideBackground);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.ShowPopup, ShowBackground);
        EventDispatcher.Instance.RemoveListener(EventID.HidePopup, HideBackground);
    }

    private void ShowBackground(object param = null)
    {
        _canvasGroup.DOFade(1f, Constant.PANEL_SLIDE_SPEED);
        _canvasGroup.blocksRaycasts = true;
    }

    private void HideBackground(object param = null)
    {
        _canvasGroup.DOFade(0f, Constant.PANEL_SLIDE_SPEED);
        _canvasGroup.blocksRaycasts = false;
    }


}
