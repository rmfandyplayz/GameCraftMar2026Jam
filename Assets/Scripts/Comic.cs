using UnityEngine;

public class Comic : MonoBehaviour
{
    public GameObject[] comicStrips 

    public int currentComicStrip = 0;

    private int comicAmount = 4;
    
    void Update()
    {
        
    }

    void NextComic(){
        currentComicStrip += 1;
        
        if (currentComicStrip == comicAmount){
            
        }

        comicStrips[currentComicStrip].SetActive(true);
    }
}
