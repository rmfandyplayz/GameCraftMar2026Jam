using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DG.Tweening; 

public class UIButtonFrameAnims : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<Sprite> normalFrames;
    [SerializeField] private List<Sprite> hoverFrames;
    [SerializeField] private Sprite disabledFrame;
    [SerializeField] private float timePerFrame = 0.1f;

    private bool isHovering;
    private bool wasInteractable;
    private Button button;
    private Image targetImage;

    private void Start()
    {
        button = GetComponent<Button>();
        targetImage = GetComponent<Image>();
        wasInteractable = button.IsInteractable();
        
        UpdateVisuals();
    }

    void Update()
    {
        bool currentInteractable = button.IsInteractable();
        if (currentInteractable != wasInteractable)
        {
            wasInteractable = currentInteractable;
            UpdateVisuals();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        Debug.Log("enter");
        isHovering = true;
        if (wasInteractable)
        {
            targetImage.DOPlaySprites(hoverFrames, timePerFrame);
        }
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        Debug.Log("exit");
        isHovering = false;
        if (wasInteractable)
        {
            targetImage.DOPlaySprites(normalFrames, timePerFrame);
        }
    }

    private void UpdateVisuals()
    {
        if (!wasInteractable)
        {
            targetImage.DOKill();
            if (disabledFrame != null) 
                targetImage.sprite = disabledFrame;
        }
        else
        {
            targetImage.DOPlaySprites(isHovering ? hoverFrames : normalFrames, timePerFrame);
        }
    }
}