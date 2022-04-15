using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehavior : MonoBehaviour
{
    public Canvas goalCanvas;
    public GameObject prefab;
    public List<GameObject> goalList;
    public int bufferSize = 3;

    void Awake()
    {
        goalCanvas = GetComponent<Canvas>();
       // goalList = new List<GameObject>(); was reseting the list

    }
 
    public void CreateGoalData(ScriptableCombination[] Fragments)
    {
        for(int i = 0; i < bufferSize; i++)
        {
            int temp = Random.Range(0, Fragments.Length);
            GameObject go = Instantiate(prefab, goalCanvas.transform);
            go.GetComponent<FragmentBehavior>().assingCombination(Fragments[temp]);
           
            goalList.Add(go);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetGoal(ScriptableCombination[] Fragments)
    {
        for (int i = 0; i < goalList.Count; i++)
        {
            int temp = Random.Range(0, goalList.Count);
            goalList[i].GetComponent<FragmentBehavior>().assingCombination(Fragments[temp]);
        }
    }

  
}
