using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelDropDownMono : MonoBehaviour
{
    [SerializeField] protected Button openPanelBtn;
    [SerializeField] protected Button closePanelBtn;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void OnEnable()
    {
        if (openPanelBtn)
            openPanelBtn.onClick.AddListener(ShowPanel);
        if (closePanelBtn)
            closePanelBtn.onClick.AddListener(HidePanel);
    }

    protected virtual void OnDisable()
    {
        if (openPanelBtn)
            openPanelBtn.onClick.RemoveListener(ShowPanel);
        if (closePanelBtn)
            closePanelBtn.onClick.RemoveListener(HidePanel);
    }

    private void Start()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;

    }

    protected virtual void ShowPanel()
    {
        _canvasGroup.DOFade(1f, Constant.PANEL_SLIDE_SPEED);
        _canvasGroup.blocksRaycasts = true;
        _rectTransform.DOAnchorPos(new Vector2(_rectTransform.anchoredPosition.x, 0), Constant.PANEL_SLIDE_SPEED);

        EventDispatcher.Instance.PostEvent(EventID.ShowPopup);
    }

    protected virtual void HidePanel()
    {
        _canvasGroup.DOFade(0f, Constant.PANEL_SLIDE_SPEED);
        _canvasGroup.blocksRaycasts = false;
        _rectTransform.DOAnchorPos(new Vector2(_rectTransform.anchoredPosition.x, Constant.PANEL_POPUP_HIDE_POS_Y), Constant.PANEL_SLIDE_SPEED);

        EventDispatcher.Instance.PostEvent(EventID.HidePopup);
    }
}
