using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

// handles visual updates for a single heart
public class HeartUI : MonoBehaviour
{
    [SerializeField] Image heartImage;
    [SerializeField] List<Sprite> fullHeartFrames;
    [SerializeField] Sprite emptyHeartFrame;
    [SerializeField, Tooltip("in bpm")] float beatSpeed; // bpm

    private bool isFull = true;

    void Start()
    {
        if (isFull)
        {
            heartImage.DOPlaySprites(fullHeartFrames, 0.3f);
            PlayHeartbeat();
        }
        else
        {
            heartImage.sprite = emptyHeartFrame;
        }
    }

    // sets the state of this heart WITHOUT ANIMATIONS
    // if an animation is desired, use either Damage() or Heal()
    public void SetState(bool filled)
    {
        if (isFull == filled)
            return;

        heartImage.DOKill(true);
        
        isFull = filled;
        if (isFull)
        {
            heartImage.DOPlaySprites(fullHeartFrames, 0.3f);            
            PlayHeartbeat();
        }
        else
        {
            heartImage.sprite = emptyHeartFrame;
        }
    }

    // plays the damage animation and set this heart to empty
    public void Damage()
    {
        //TODO: damage animation?
        heartImage.DOKill(true);
        heartImage.sprite = emptyHeartFrame;
        isFull = false;
    }

    // plays the healing animation and set this heart to full animation TODO: maybe no healing animation???
    public void Heal()
    {
        heartImage.DOKill(true);
        isFull = true;
        heartImage.DOPlaySprites(fullHeartFrames, 0.3f);
        PlayHeartbeat();
    }
    
    
    private void PlayHeartbeat()
    {
        float secondsPerBeat = 60f / Mathf.Max(beatSpeed, 0.01f);

        heartImage.transform.localScale = Vector3.one;

        DOTween.Sequence()
            .SetTarget(heartImage) // so DOKill() works
            .Append(heartImage.transform.DOScale(1.15f, secondsPerBeat * 0.15f).SetEase(Ease.OutQuad))
            .Append(heartImage.transform.DOScale(0.95f, secondsPerBeat * 0.10f).SetEase(Ease.InOutQuad))
            .Append(heartImage.transform.DOScale(1.10f, secondsPerBeat * 0.12f).SetEase(Ease.OutQuad))
            .Append(heartImage.transform.DOScale(1.00f, secondsPerBeat * 0.13f).SetEase(Ease.InQuad))
            .AppendInterval(secondsPerBeat * 0.5f)
            .SetLoops(-1);
    }
    
}
