using UnityEngine;
using System.Collections;

public class elevatedTile : MonoBehaviour
{

    public int tileX;
    public int tileZ;
    public Move move;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        move.x = tileX;
        move.z = tileZ;
    }


}