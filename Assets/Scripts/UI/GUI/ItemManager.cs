using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Image backgroundSprite;
    [SerializeField] private Image itemSprite;
    [SerializeField] private List<Sprite> backgroundSpriteAnimFrames;

    private Player playerRef;
    
    private void Awake()
    {
        playerRef = FindFirstObjectByType<Player>().GetComponent<Player>();
    }

    private void OnEnable()
    {
        if (playerRef != null)
        {
            playerRef.OnPickupItem.AddListener(PickupItem);
            playerRef.OnDropItem.AddListener(DropItem);
        }
    }

    private void OnDisable()
    {
        if (playerRef != null)
        {
            playerRef.OnPickupItem.RemoveListener(PickupItem);
            playerRef.OnDropItem.RemoveListener(DropItem);
        }
    }

    public void PickupItem(Sprite newSprite)
    {
        itemSprite.gameObject.SetActive(true);
        backgroundSprite.gameObject.SetActive(true);
        
        itemSprite.DOKill(true);
        backgroundSprite.DOKill(true);
        
        Sequence seq =  DOTween.Sequence();
        
        itemSprite.sprite = newSprite;

        seq.Insert(0.15f, itemSprite.DOFade(1, 0.2f));
        seq.Insert(0.15f, itemSprite.rectTransform.DOAnchorPos(new Vector2(-180, 173), 0.2f));
        seq.Insert(0.15f, itemSprite.rectTransform.DOScale(1f, 0.2f));
        
        seq.Insert(0f, backgroundSprite.DOFade(1, 0.2f));
        seq.Insert(0f, backgroundSprite.rectTransform.DOAnchorPos(new Vector2(-67, 88), 0.2f));
        seq.Insert(0f, backgroundSprite.rectTransform.DORotate(Vector3.zero, 0.2f));
        backgroundSprite.DOPlaySprites(backgroundSpriteAnimFrames, 0.2f);
        
    }

    public void DropItem()
    {
        itemSprite.DOKill(true);
        backgroundSprite.DOKill(true);
        
        itemSprite.DOFade(0, 0.2f);
        itemSprite.rectTransform.DOAnchorPos(new Vector2(-150, 150), 0.2f);
        itemSprite.rectTransform.DOScale(1.1f, 0.2f);
        
        backgroundSprite.DOFade(0, 0.2f);
        backgroundSprite.rectTransform.DORotate(new Vector3(0, 0, -9), 0.2f);
        backgroundSprite.rectTransform.DOAnchorPos(new Vector2(-20, 33), 0.2f).OnComplete(() =>
        {
            itemSprite.gameObject.SetActive(false);
            backgroundSprite.gameObject.SetActive(false);
        });
    }
}
