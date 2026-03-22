using UnityEngine;

public class BabyDetector : MonoBehaviour
{
    public Player player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Baby"))
        {
            player.CanInteractWithBaby();
        }
    }

        void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Baby"))
        {
            player.CantInteractWithBaby();
        }
    }
}
