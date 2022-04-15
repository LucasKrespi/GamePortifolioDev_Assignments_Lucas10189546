using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AFourGameControl : MonoBehaviour
{
    public GoalBehavior goal;
    public BoardControl board;
    public BufferBehavior buffer;
    public ScriptableCombination[] fragments;

    public Canvas GameOverCanvas;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    public static AFourGameControl Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameOverCanvas.gameObject.SetActive(false);
        goal.CreateGoalData(fragments);
        board.CreateBoard(fragments);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetGame()
    {
        goal.resetGoal(fragments);
        board.ResetBoard(fragments);
        buffer.ResetBuffer();
        GameOverCanvas.gameObject.SetActive(false);
    }

    public void OnFragmentCliked(FragmentBehavior frag)
    {
        if (frag.isSelected) return;

        board.OnFragmentCliked(frag);
        buffer.AddToBuffer(frag);
    }

    public void GameOver()
    {
        if (checkSquence())
        {
            Debug.Log("win");
            winGame();
        }
        else
        {
            loseGame();
            Debug.Log("lose");
        }
    }

    public bool checkSquence()
    {
        int match = 0;
        foreach(var frag in buffer.fragmentsInBuffer)
        {
            if (match >= 3) break;
            if (frag.text.text == goal.goalList[match].GetComponent<FragmentBehavior>().text.text)
            {
                match++;
            }
            else
            {
                match = 0;
            }
        }
       
        if(match >= 3)
        {
            return true;
        }

        return false;
    }

    public void winGame()
    {
        GameOverCanvas.gameObject.SetActive(true);
        loseText.gameObject.SetActive(false);
    } 
    public void loseGame()
    {
        GameOverCanvas.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
    }

    public void EasyButtonClick()
    {
        buffer.bufferSize = 5;
    }  
    public void MediumButtonClick()
    {
        buffer.bufferSize = 4;
    } 
    public void HardButtonClick()
    {
        buffer.bufferSize = 3;
    }

}
