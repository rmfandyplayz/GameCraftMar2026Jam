using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;

// controls the visibility and contents of the objects stuff
public class ObjectivesUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup iconGroup;
    [SerializeField] private CanvasGroup objectiveGroup;
    [SerializeField] private RectTransform mapGroupTransform;

    [SerializeField] private List<TextMeshProUGUI> objectiveText; // max 2 ig
    
    public void Open()
    {
        ReferenceManager.lockAllInteractions = true;
        iconGroup.DOKill();
        objectiveGroup.DOKill();
        
        Sequence seq = DOTween.Sequence();
        
        // fade out icon and fade in main group
        seq.Insert(0, iconGroup.DOFade(0f, 0.1f).SetEase(Ease.OutQuint));
        seq.Insert(0, objectiveGroup.DOFade(1f, 0.2f));
        
        // move main group
        seq.Insert(0, mapGroupTransform.DOAnchorPos(new Vector2(-345f, -300f), 0.3f).SetEase(Ease.OutBack));
        seq.Insert(0, mapGroupTransform.DOSizeDelta(new Vector2(1040f, 305f), 0.3f).SetEase(Ease.OutBack));
        
        seq.OnComplete(() => ReferenceManager.lockAllInteractions = false);
    }

    public void Close()
    {
        ReferenceManager.lockAllInteractions = true;
        iconGroup.DOKill();
        objectiveGroup.DOKill();
        
        Sequence seq = DOTween.Sequence();
        
        // fade out icon and fade in main group
        seq.Insert(0, iconGroup.DOFade(1f, 0.2f).SetEase(Ease.InExpo));
        seq.Insert(0, objectiveGroup.DOFade(0f, 0.2f));
        
        // move main group
        seq.Insert(0, mapGroupTransform.DOAnchorPos(new Vector2(-700f, -435f), 0.5f).SetEase(Ease.OutQuint));
        seq.Insert(0, mapGroupTransform.DOSizeDelta(new Vector2(100f, 100f), 0.5f).SetEase(Ease.OutQuint));
        
        seq.OnComplete(() => ReferenceManager.lockAllInteractions = false);
    }

    public void HideIcon()
    {
        Debug.Log("fading");
        iconGroup.DOFade(0, 0.2f);
    }
    
    public void ShowIcon()
    {
        iconGroup.DOFade(1, 0.2f);
    }
    
    // replace the last text with new objective. all other text will be replaced with
    // the next one on the list, where it's also strikethroughed
    public void UpdateObjectiveText(string newObjective)
    {
        // no objectives yet
        if (string.IsNullOrEmpty(objectiveText[0].text))
        {
            objectiveText[0].text = newObjective;
            return;
        }

        // shift everything and strikethrough
        for (int i = 0; i < objectiveText.Count - 1; i++)
        {
            string nextText = objectiveText[i + 1].text;
            
            if (!string.IsNullOrEmpty(nextText) && !nextText.StartsWith("<s>"))
                nextText = $"<s>{nextText}</s>";

            objectiveText[i].text = nextText;
        }
        
        objectiveText[^1].text = newObjective; // newest thing no strikethrough
    }
}
