using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty
{
    EASY,
    MEDIUM,
    HARD
}
public class DificultyControll : MonoBehaviour
{
    private Canvas dificultySelectCanvas;

    public Difficulty currentDifficulty;

    private LockPickingControll game;
    // Start is called before the first frame update
    void Start()
    {
        dificultySelectCanvas = GetComponent<Canvas>();
        game = GameObject.Find("LockPickHolder").GetComponent<LockPickingControll>();
    }

    public void OnEasyButtonClick()
    {
        currentDifficulty = Difficulty.EASY;

        game.SetDifficulty(currentDifficulty);
        game.setLives(3);

        hideCanvas();
    }
    public void OnMediumButtonClick()
    {
        currentDifficulty = Difficulty.MEDIUM;

        game.SetDifficulty(currentDifficulty);
        game.setLives(3);

        hideCanvas();
    }
    public void OnHardButtonClick()
    {
        currentDifficulty = Difficulty.HARD;

        game.SetDifficulty(currentDifficulty);
        game.setLives(3);

        hideCanvas();
    }

    public void hideCanvas()
    {
        game.pause = false;
        dificultySelectCanvas.gameObject.SetActive(false);
    }
}
