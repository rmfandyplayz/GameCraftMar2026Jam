using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int health = 3;
    public float speed = 5;
    public int ItemId = 0;

    private bool canTakeDamage = true;

    [Header("Damage")]
    public float iFrameDuration = 0.3f;
    public float flashInterval = 0.1f;

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

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer playerSprite; // assign your player sprite here


    [NonSerialized] public Vector2 move;

    private float lastIdleIndex = 0f; // 0 = down, 1 = side, 2 = up

    private void Start()
    {
        if (FindAnyObjectByType<PlayerController>() == null)
        {
            gameObject.AddComponent<PlayerController>();
        }
    }

    void Update() 
    {
        useLockTimer -= Time.deltaTime;
        
        transform.position += (Vector3)(move * (speed * Time.deltaTime));

        bool isMoving = move.magnitude > 0;

        if (isMoving)
        {
            animator.SetBool("isMoving", true);

            if (move.x != 0)
            {
                lastIdleIndex = 1f;
                animator.SetFloat("idleIndex", 1f);

                // Flip character
                if (move.x > 0)
                {
                    playerSprite.flipX = true;
                }
                else if (move.x < 0)
                {
                    playerSprite.flipX = false;
                }
            }
            else if (move.y > 0)
            {
                lastIdleIndex = 2f;
                animator.SetFloat("idleIndex", 2f);
            }
            else if (move.y < 0)
            {
                lastIdleIndex = 0f;
                animator.SetFloat("idleIndex", 0f);
            }
        } 
        else 
        {
            animator.SetBool("isMoving", false);
            animator.SetFloat("idleIndex", lastIdleIndex);
        }

        //can't use an item if bro has none lol
        //this is nessasary so it doesn't override pickup input!

        if (ItemId == 0){
            return;
        }
        if (useLockTimer <= 0f && Input.GetKeyDown(KeyCode.E))
        {
            UseItem();
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
        if (canTakeDamage == false)
        {
            return;
        }

        canTakeDamage = false;
        
        health -= 1;
        
        OnHPChanged.Invoke(health);

        StartCoroutine(DamageIFrames());

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private System.Collections.IEnumerator DamageIFrames()
{
    float timer = 0f;

    while (timer < iFrameDuration)
    {
        // flash red
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(flashInterval);

        // back to normal
        playerSprite.color = Color.white;
        yield return new WaitForSeconds(flashInterval);

        timer += flashInterval * 2f;
    }

    playerSprite.color = Color.white;
    canTakeDamage = true;
}

    // public static Sprite GetItemSprite(int id)
    // {
    //     return Sprites[id];
    // }
}