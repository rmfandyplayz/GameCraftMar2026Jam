using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID = 0;
    public GameObject IndicationUI;
    public bool DestroyOnPickup = true;

    private bool canInteract = false;

    Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.PlayerPickup(ID);

            if (DestroyOnPickup){
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            IndicationUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            IndicationUI.SetActive(false);
        }
    }
}
