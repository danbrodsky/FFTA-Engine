using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour
{

    TileMap _tileMap;

    public Vector3 currentTileCoord;
    public CharGrid grid;

    public Transform selectionCube;
    public int x;
    public int z;

    void Start()
    {
        _tileMap = GetComponent<TileMap>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            x = (int) (hitInfo.point.x / _tileMap.tile_size);
            z = (int) (hitInfo.point.z / _tileMap.tile_size);
            //Debug.Log ("Tile: " + x + ", " + z);

            currentTileCoord.x = x + 0.5f;
            currentTileCoord.z = z + 0.5f;
            currentTileCoord.y = 0.001f;
            currentTileCoord.y = grid.getElevation(x, z) * 0.25f;

            selectionCube.transform.position = currentTileCoord * 1f;
        }
        else
        {
            // Hide selection cube?
        }

        if (Input.GetMouseButtonDown(0))
        {
        }
    }
}

