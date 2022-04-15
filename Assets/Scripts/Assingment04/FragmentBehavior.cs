using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FragmentBehavior : MonoBehaviour
{
    public TextMeshProUGUI text;
    public ScriptableCombination scriptable;
    public Button buttom;

    public int positionX;
    public int PositionY;

    public bool isSelected = false;
    void Awake()
    {
        buttom = GetComponent<Button>();
        buttom.onClick.AddListener(OnClick);
    }
    public void Initialize(ScriptableCombination combination, int y, int x)
    {
        scriptable = combination;
        text.text = scriptable.letter + scriptable.number;

        positionX = x;
        PositionY = y;

        ChangeColor(Color.black);
    }

    public void assingCombination(ScriptableCombination combination)
    {
        scriptable = combination;
        text.text = scriptable.letter + scriptable.number;
        ChangeColor(Color.black);
    } 
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        AFourGameControl.Instance.OnFragmentCliked(this);
    }

    public void SetPositionXY(int x, int y)
    {
        positionX = x;
        PositionY = y;
    }
    public void ChangeColor(Color color)
    {
        buttom.image.color = color;
    }
}
