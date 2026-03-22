using UnityEngine;
using UnityEngine.InputSystem;

public class Comic : MonoBehaviour
{
    public GameObject[] comicStrips;

    public int currentComicStrip = 0;
    public int comicAmount = 4;

    public SceneTransition sceneTrans;
    public string SceneName;

    public GameObject tutorial;



    void Update()
    {
        // Space (keyboard) OR A button (gamepad)
        if (
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        )
        {
            NextComic();
        }
    }

    void NextComic()
    {
        if (currentComicStrip == 0){
            tutorial.SetActive(false);
        }
        
        currentComicStrip += 1;

        if (currentComicStrip == comicAmount)
        {
            sceneTrans.LoadScene(SceneName);
            return;
        }

        comicStrips[currentComicStrip].SetActive(true);
    }
}