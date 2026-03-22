using System;
using UnityEngine;
using DG.Tweening;

// controls the visibility of the map UI
public class MapUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup iconGroup;
    [SerializeField] private CanvasGroup mapGroup;
    [SerializeField] private RectTransform mapGroupTransform;

    public void Open()
    {
        ReferenceManager.lockAllInteractions = true;
        iconGroup.DOKill();
        mapGroup.DOKill();
        
        Sequence seq = DOTween.Sequence();
        
        // fade out icon and fade in main group
        seq.Insert(0, iconGroup.DOFade(0f, 0.1f).SetEase(Ease.OutQuint));
        seq.Insert(0, mapGroup.DOFade(1f, 0.2f));
        
        // move main group
        seq.Insert(0, mapGroupTransform.DOAnchorPos(new Vector2(275f, 275f), 0.3f).SetEase(Ease.OutBack));
        seq.Insert(0, mapGroupTransform.DOSizeDelta(new Vector2(400f, 400f), 0.3f).SetEase(Ease.OutBack));
        
        seq.OnComplete(() => ReferenceManager.lockAllInteractions = false);
    }

    public void Close()
    {
        ReferenceManager.lockAllInteractions = true;
        iconGroup.DOKill();
        mapGroup.DOKill();
        
        Sequence seq = DOTween.Sequence();
        
        // fade out icon and fade in main group
        seq.Insert(0, iconGroup.DOFade(1f, 0.2f).SetEase(Ease.InExpo));
        seq.Insert(0, mapGroup.DOFade(0f, 0.2f));
        
        // move main group
        seq.Insert(0, mapGroupTransform.DOAnchorPos(new Vector2(105f, 98f), 0.5f).SetEase(Ease.OutQuint));
        seq.Insert(0, mapGroupTransform.DOSizeDelta(new Vector2(100f, 100f), 0.5f).SetEase(Ease.OutQuint));
        
        seq.OnComplete(() => ReferenceManager.lockAllInteractions = false);
    }

    public void HideIcon()
    {
        iconGroup.DOFade(0, 0.2f);
    }
    
    public void ShowIcon()
    {
        iconGroup.DOFade(1, 0.2f);
    }
}
