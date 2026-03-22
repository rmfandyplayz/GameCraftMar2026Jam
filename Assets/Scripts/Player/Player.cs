using Baby;
using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int health = 3;
    public float speed = 5;

    // 0 = none
    // 1 = coffee
    // 2 = bottle
    // 4 = filled bottle
    // 3 = rattle
    //5 = book
    public int ItemId = 0;

    [HideInInspector]
    public bool canMove = true;

    [Header("Items")]
    public SpriteRenderer ItemSprite;
    public GameObject[] ItemObjects;
    public GameObject RattleEffect;

    public Sprite[] Sprites;
    private float useLockTimer = 0f;

    [Header("I-Frames")]
    public float iFrameDuration = 1f;
    public float flashSpeed = 12f;
    private float iFrameTimer = 0f;
    private bool isInvincible = false;

    [Header("Events for andy lol")]
    public UnityEvent<int> OnHPChanged = new();
    public UnityEvent<Sprite> OnPickupItem = new();
    public UnityEvent OnDropItem = new();

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer playerSprite;

    [NonSerialized] public Vector2 move;

    private float lastIdleIndex = 0f;

    private InputAction moveAction;
    private InputAction useAction;

    private Rigidbody2D rb;

    private SceneTransition sceneTrans;

    [Header("Sounds")]
    public GameObject hurtSound;
    public AudioSource DrinkSound;
    public GameObject Music;

    [Header("Visuals")]
    public GameObject indicator;
    public GameObject DeathScreen;

    [Header("Misc.")]
    public string winScene;

    private bool canUseBottle = false;

    [Header("Objectives")]
    public GameObject[] itemObjectives;
    public bool isFireQuest = false;

    public bool crib = false;
    public bool dark = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");

        moveAction.AddBinding("<Gamepad>/leftStick");

        useAction = new InputAction("Use", InputActionType.Button);
        useAction.AddBinding("<Keyboard>/e");
        useAction.AddBinding("<Gamepad>/buttonSouth");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        useAction.Enable();
        useAction.performed += OnUsePerformed;
    }

    private void OnDisable()
    {
        useAction.performed -= OnUsePerformed;
        moveAction.Disable();
        useAction.Disable();
    }

    private void Start()
    {
        sceneTrans = FindFirstObjectByType<SceneTransition>();
    }

    private void Update()
    {
        useLockTimer -= Time.deltaTime;

        if (canMove){
            move = moveAction.ReadValue<Vector2>().normalized;
        }

        // I-FRAME TIMER + FLASH
        if (isInvincible)
        {
            iFrameTimer -= Time.deltaTime;

            // Flash effect
            playerSprite.enabled = Mathf.FloorToInt(Time.time * flashSpeed) % 2 == 0;

            if (iFrameTimer <= 0f)
            {
                isInvincible = false;
                playerSprite.enabled = true;
            }
        }

        bool isMoving = move.magnitude > 0.01f;

        if (isMoving)
        {
            animator.SetBool("isMoving", true);

            if (Mathf.Abs(move.x) > 0.01f)
            {
                lastIdleIndex = 1f;
                animator.SetFloat("idleIndex", 1f);
                playerSprite.flipX = move.x > 0;
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

        if (isFireQuest == true)
        {
            GameObject[] fires = GameObject.FindGameObjectsWithTag("fire");

            if (fires.Length == 0)
            {
                sceneTrans.LoadScene(winScene);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    private void OnUsePerformed(InputAction.CallbackContext context)
    {
        if (ItemId == 0)
            return;

        if (useLockTimer <= 0f)
        {
            UseItem();
        }
    }

    public void PlayerPickup(int id)
    {
        ItemId = id;
        ItemSprite.sprite = Sprites[id];

        if (id == 0)
        {
            OnDropItem.Invoke();
        }
        else
        {
            OnPickupItem.Invoke(Sprites[id]);
            Instantiate(itemObjectives[id], transform.position, Quaternion.identity);
        }

        useLockTimer = 0.1f;
    }

    public void UseItem()
    {
        if (ItemId == 1)
        {
            Instantiate(ItemObjects[1], transform.position, Quaternion.identity);
            PlayerPickup(0);
        }

        if (ItemId == 3)
        {
            FindFirstObjectByType<BabyController>()?.SetGoalNode(transform.position);
            Instantiate(RattleEffect, transform.position, Quaternion.identity);
        }

        if (ItemId == 4)
        {
            if (canUseBottle == true)
            {
                DrinkSound.Play();
                sceneTrans.LoadScene(winScene);

                PlayerPickup(2);
            }
        }

        if (ItemId == 5)
        {
            if ((crib == true) || (dark == true))
            {
                sceneTrans.LoadScene(winScene);
                PlayerPickup(0);
            }
        }
    }

    public void TakeDamage()
    {
        if (isInvincible)
            return;

        Instantiate(hurtSound, transform.position, Quaternion.identity);

        health -= 1;
        OnHPChanged.Invoke(health);

        isInvincible = true;
        iFrameTimer = iFrameDuration;

        if (health <= 0)
        {
            DeathScreen.SetActive(true);
            Music.SetActive(false);
            canMove = false;
        }
    }

    public void DropCurrentItem(){
        if (ItemId == 0){
            return;
        }

        Instantiate(ItemObjects[ItemId], transform.position, Quaternion.identity);
    }

    public void CanInteractWithBaby()
    {
        canUseBottle = true;
        if (ItemId == 4)
        {
            indicator.SetActive(true);
        }
    }

    public void CantInteractWithBaby()
    {
        canUseBottle = false;

        if (ItemId == 4)
        {
            indicator.SetActive(true);
        }
    }

    public void darkToggle()
    {
        if (dark == true)
        {
            dark = false;
            return;
        } else
        {
            dark = true;
        }
    }
}