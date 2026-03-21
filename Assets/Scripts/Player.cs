using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int health = 3;
    public float speed = 5;
    public int ItemId = 0;

    [Header("Items")]
    //Item 1: Coffee Mug
    //Item 2: Food
    //Item 3: Rattle
    //Item 4: Bottle
    //Item 5: Burp Cloth (if we have time)
    public SpriteRenderer ItemSprite;

    //Note for future me, First sprite should be blank as it's ID is 0!
    public Sprite[] Sprites;
    
    void Update() 
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;
    }

    public void PlayerPickup(int id){
        ItemId = id;

        ItemSprite.sprite = Sprites[id];
    }

    public void UseItem(){
        if (ItemId == 0){
            return;
        }
    }
}
