using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferBehavior : MonoBehaviour
{
    public Canvas bufferCanvas;
    public GameObject prefab;
    public List<FragmentBehavior> fragmentsInBuffer;

    public int bufferSize;
    public int buffercount;

    private void Start()
    {
        fragmentsInBuffer = new List<FragmentBehavior>();
        bufferSize = 5;
        buffercount = 0;
    }

    private void Update()
    {
        if(buffercount >= bufferSize)
        {
           AFourGameControl.Instance.GameOver();
           buffercount = 0;
        }
    }
    public void AddToBuffer(FragmentBehavior frag)
    {
        var go = Instantiate(prefab, gameObject.transform);
        go.GetComponent<FragmentBehavior>().assingCombination(frag.scriptable);
        fragmentsInBuffer.Add(go.GetComponent<FragmentBehavior>());
        buffercount++;
    }

    public void ResetBuffer()
    {
        foreach(var f in fragmentsInBuffer)
        {
            Destroy(f.gameObject);
        }

        fragmentsInBuffer.Clear();
        buffercount = 0;
    }
}
