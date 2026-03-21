using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;

// plays sprite animations for each button state
public class UIButtonFrameAnims : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float timePerFrame;

    [SerializeField] private List<Sprite> normalFrames;
    [SerializeField] private List<Sprite> hoverFrames;
    [SerializeField] private Sprite disabledFrame;

    private bool isHovering = false;
    private bool wasInteractable;
    private Image targetImage;
    private Coroutine currentAnimation;
    private Button button;

    void Start() // grab refs, plays normal animations if allowed. otherwise, set to disabled
    {
        if(targetImage == null)
            targetImage = GetComponent<Image>();
        
        button = GetComponent<Button>();
        wasInteractable = button.IsInteractable();

        if (wasInteractable)
            PlayAnimation(normalFrames);
        else
            SetDisabled();
    }

    void Update() // check if button is supposed to be interactable via parent canvasgroup
    {
        if (button.IsInteractable() != wasInteractable)
        {
            wasInteractable = button.IsInteractable();
            
            if(wasInteractable)
                PlayAnimation(isHovering ? hoverFrames : normalFrames);
            else
                SetDisabled();
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (!wasInteractable)
            return;
        
        PlayAnimation(hoverFrames);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;

        if (!wasInteractable)
            return;
        
        PlayAnimation(normalFrames);
    }
    
    private void SetDisabled() // stop animation and set to disabled sprite
    {
        if(currentAnimation != null)
            StopCoroutine(currentAnimation);
        if(disabledFrame != null)
            targetImage.sprite = disabledFrame;
    }
    
    private void PlayAnimation(List<Sprite> frames)
    {
        if (frames == null || frames.Count == 0)
            return;
        
        if(currentAnimation != null)
            StopCoroutine(currentAnimation);
        
        currentAnimation = StartCoroutine(Animate(frames));
    }

    IEnumerator Animate(List<Sprite> frames)
    {
        int index = 0;
        while (true)
        {
            targetImage.sprite = frames[index];
            index = (index + 1) % frames.Count;
            yield return new WaitForSeconds(timePerFrame);
        }
    }
}
