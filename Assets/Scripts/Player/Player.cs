using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Baby;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int health = 3;
    public float speed = 5;
    public int ItemId = 0;

    [Header("Items")]
    // Item 1: Coffee Mug
    // Item 2: Food
    // Item 3: Rattle
    // Item 4: Bottle
    // Item 5: Burp Cloth
    public SpriteRenderer ItemSprite;

    public GameObject CoffeeItem;

    // First sprite should be blank because ID 0 = no item
    public Sprite[] Sprites;
    private float useLockTimer = 0f;

    [Header("Events for andy lol")]
    public UnityEvent<int> OnHPChanged = new();
    public UnityEvent<Sprite> OnPickupItem = new();
    public UnityEvent OnDropItem = new();

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer playerSprite;

    [NonSerialized] public Vector2 move;

    private float lastIdleIndex = 0f; // 0 = down, 1 = side, 2 = up

    // New Input System actions
    private InputAction moveAction;
    private InputAction useAction;

    private void Awake()
    {
        // Movement: WASD + Arrow Keys + Gamepad left stick
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

        // Use item: E key + gamepad south button (A on Xbox / Cross on PS)
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
        if (FindAnyObjectByType<PlayerController>() == null)
        {
            gameObject.AddComponent<PlayerController>();
        }
    }

    private void Update()
    {
        useLockTimer -= Time.deltaTime;

        // Read movement from new Input System
        move = moveAction.ReadValue<Vector2>();

        transform.position += (Vector3)(move * (speed * Time.deltaTime));

        bool isMoving = move.magnitude > 0.01f;

        if (isMoving)
        {
            animator.SetBool("isMoving", true);

            if (Mathf.Abs(move.x) > 0.01f)
            {
                lastIdleIndex = 1f;
                animator.SetFloat("idleIndex", 1f);

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

        if (id > 0)
        {
            OnDropItem.Invoke();
        }
        else
        {
            OnPickupItem.Invoke(Sprites[id]);
        }

        useLockTimer = 0.1f;
    }

    public void UseItem()
    {
        if (ItemId == 1)
        {
            Instantiate(CoffeeItem, transform.position, Quaternion.identity);
            PlayerPickup(0);
        }

        if (ItemId == 3){
            FindFirstObjectByType<BabyController>()?.SetGoalNode(transform.position);
        }
    }

    public void TakeDamage()
    {
        health -= 1;
        OnHPChanged.Invoke(health);

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}