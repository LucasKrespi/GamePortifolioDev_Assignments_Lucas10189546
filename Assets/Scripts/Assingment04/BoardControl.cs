using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardControl : MonoBehaviour
{
    [Header("Board creation")]
    public int height;
    public int width;
    public GameObject prefab;

    private FragmentBehavior[,] FragmenteListBoard;

    private int clickCount = 0;
    // Start is called before the first frame update
    void Awake()
    {
        
        FragmenteListBoard = new FragmentBehavior[height, width];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBoard(ScriptableCombination[] Fragments)
    {
     

        for (int i = 0; i < height; i++)
        {
            for (int k = 0; k < width; k++)
            {
                int temp = Random.Range(0, Fragments.Length);

                var go = Instantiate(prefab, gameObject.transform);
                FragmenteListBoard[i, k] = go.GetComponent<FragmentBehavior>();
                FragmenteListBoard[i, k].Initialize(Fragments[temp], i, k);
            }
        }
    }

    public void ResetBoard(ScriptableCombination[] Fragments)
    {
        if (FragmenteListBoard[0, 0] == null) return;

        for (int i = 0; i < height; i++)
        {
            for (int k = 0; k < width; k++)
            {
                int temp = Random.Range(0, Fragments.Length);
                FragmenteListBoard[i,k].assingCombination(Fragments[temp]);
                FragmenteListBoard[i, k].isSelected = false;
            }
        }

        clickCount = 0;
    }

    public void OnFragmentCliked(FragmentBehavior frag)
    {

        if (frag.isSelected) return;

        if(clickCount % 2 == 0)
        {
            foreach(var f in ReturnFragmentLine(frag))
            {
                f.ChangeColor(Color.green);
                f.isSelected = false;
            }

            frag.ChangeColor(Color.blue);
            frag.isSelected = true;

       
            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < width; k++)
                {
                    if(frag.PositionY != i)
                    {
                        FragmenteListBoard[i, k].isSelected = true;
                    }
                }

            }
        }
        else
        {
            foreach (var f in ReturnFragmentColum(frag))
            {
                f.ChangeColor(Color.green);
                f.isSelected = false;
            }

            frag.ChangeColor(Color.blue);
            frag.isSelected = true;


            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < width; k++)
                {
                    if (frag.positionX != k)
                    {
                        FragmenteListBoard[i, k].isSelected = true;
                    }
                }

            }
        }

        clickCount++;
    }
    public List<FragmentBehavior> ReturnFragmentLine(FragmentBehavior frag)
    {
        List<FragmentBehavior> tempList = new List<FragmentBehavior>();
        
        for (int i = 0; i < height; i++)
        {
            for (int k = 0; k < width; k++)
            {
                if(frag.PositionY == i)
                {
                    tempList.Add(FragmenteListBoard[i, k]);
                }
            }
        }

        return tempList;
    }
    public List<FragmentBehavior> ReturnFragmentColum(FragmentBehavior frag)
    {
        List<FragmentBehavior> tempList = new List<FragmentBehavior>();
        
        for (int i = 0; i < height; i++)
        {
            for (int k = 0; k < width; k++)
            {
                if(frag.positionX == k)
                {
                    tempList.Add(FragmenteListBoard[i, k]);
                }
            }
        }

        return tempList;
    }
}
