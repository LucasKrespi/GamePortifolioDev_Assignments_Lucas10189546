using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpot : MonoBehaviour
{
    public int positionX;
    public int positionY;
    public GameObject prefab;
    public GameObject currentTile;
    public TileSpot(GameObject TileObject, int pX, int pY)
    {
        positionX = pY;
        positionY = pY;
        prefab = TileObject;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentTile = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentTile = null;
    }
}
