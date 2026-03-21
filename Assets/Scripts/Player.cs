using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

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

    public GameObject CoffeeItem;

    //Note for future me, First sprite should be blank as it's ID is 0!
    public Sprite[] Sprites;
    private float useLockTimer = 0f;

    //Events
    [Header("Events for andy lol")]
    public UnityEvent<int> OnHPChanged = new();
    public UnityEvent<Sprite> OnPickupItem = new();
    public UnityEvent OnDropItem = new();

    // 🔥 NEW: Animation
    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer playerSprite; // assign your player sprite here

    void Update() 
    {
        useLockTimer -= Time.deltaTime;
             
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;

        // 🔥 ANIMATION LOGIC START

        bool isMoving = moveX != 0 || moveY != 0;

        // Reset all first
        animator.SetBool("Side", false);
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);

        if (isMoving)
        {
            if (moveX != 0)
            {
                animator.SetBool("Side", true);

                // Flip character (left = default, right = flipped)
                if (moveX > 0)
                {
                    playerSprite.flipX = true;
                }
                else if (moveX < 0)
                {
                    playerSprite.flipX = false;
                }
            }
            else if (moveY > 0)
            {
                animator.SetBool("Up", true);
            }
            else if (moveY < 0)
            {
                animator.SetBool("Down", true);
            }
        }

        // 🔥 ANIMATION LOGIC END

        //can't use an item if bro has none lol
        //this is nessasary so it doesn't override pickup input!

        if (ItemId == 0){
            return;
        } else 
        {
            if (useLockTimer <= 0f && Input.GetKeyDown(KeyCode.E))
            {
                UseItem();
            }
        }
    }

    public void PlayerPickup(int id){
        ItemId = id;

        ItemSprite.sprite = Sprites[id];

        if (id > 0)
        {
            OnDropItem.Invoke(); 
        } else 
        {
            OnPickupItem.Invoke(Sprites[id]); 
        }

        useLockTimer = 0.1f;
    }

    public void UseItem(){

        if (ItemId == 1){
            Instantiate(CoffeeItem, transform.position, Quaternion.identity);
            PlayerPickup(0);
        }
    }

    public void TakeDamage(){
        health -= 1;
        
        OnHPChanged.Invoke(health);

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // public static Sprite GetItemSprite(int id)
    // {
    //     return Sprites[id];
    // }
}