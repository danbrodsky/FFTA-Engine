using UnityEngine;
using System.Collections;



public class Move : MonoBehaviour
{

    public CharGrid map;
    public TileMapMouse point;
    public DragBox dragbox;
    bool dragging = false;

    public int x = -1;
    public int z = -1;

    void Update()
    {

            if (Input.GetMouseButton(0))
            {

                if (x == -1)
                    x = (int)(point.currentTileCoord.x - 0.5f);
                if (z == -1)
                    z = (int)(point.currentTileCoord.z - 0.5f);

            if (dragbox.box != null)
                    if (Mathf.Abs(dragbox.box.transform.localScale.x) > 0.5f && Mathf.Abs(dragbox.box.transform.localScale.z) > 0.5f)
                    {
                        dragging = true;
                        map.selectedUnits.Clear();
                    }
                    else dragging = false;
            }
        if (Input.GetMouseButtonUp(0) && !dragging) {
            
             
            bool downRight = false;

            // Only 1 unit selected
            if (map.selectedUnits.Count == 1)
            {
                map.selectedUnit = map.selectedUnits[0];
               // map.charSelected();
                map.GeneratePathTo((int)(point.currentTileCoord.x - 0.5f), (int)(point.currentTileCoord.z - 0.5f));
                Unit unit = map.selectedUnits[0].GetComponent<Unit>();


                if (unit.currentPath != null)
                {

                    foreach (GameObject go in unit.highlightPath)
                    {
                        DestroyObject(go);
                    }
                    int currNode = 0;
                    while (currNode < unit.currentPath.Count)
                    {
                        map.highlightPath(unit.currentPath[currNode].x, unit.currentPath[currNode].z);
                        currNode++;
                    }
                }
            }

            else
            {
                if (map.isWalkable((int)(point.currentTileCoord.x - 0.5f), (int)(point.currentTileCoord.z - 0.5f)))
                {

                    int c = 0;
                    foreach (Unit selectedUnit in map.selectedUnits)
                    {
                        c++;
                    }
                    if (c > 0)
                    {
                        if (map.selectedUnits[c / 2].tileX <= (int)point.currentTileCoord.x - 0.5f && map.selectedUnits[c / 2].tileZ == (int)(point.currentTileCoord.z - 0.5f))
                        {
                            downRight = true;

                        }
                        if (map.selectedUnits[c / 2].tileX == (int)(point.currentTileCoord.x - 0.5f) && map.selectedUnits[c / 2].tileZ != (int)(point.currentTileCoord.z - 0.5f))
                        {
                            downRight = false;

                        }

                        if (map.selectedUnits[c / 2].tileX <= point.currentTileCoord.x - 0.5f)
                        {
                            downRight = true;

                        }
                        if (map.selectedUnits[c / 2].tileX >= point.currentTileCoord.x - 0.5f && Mathf.Abs(map.selectedUnits[c / 2].tileZ - (int)(point.currentTileCoord.z - 0.5f)) < 1.01)
                        {
                            downRight = true;

                        }

                        if (map.selectedUnits[c / 2].tileX >= point.currentTileCoord.x - 0.5f && map.selectedUnits[c / 2].tileZ != (int)(point.currentTileCoord.z - 0.5f))
                        {
                            downRight = false;

                        }
                    }
                    

                    if (downRight)
                    {
                        downRight = false;
                        int i = -map.selectedUnits.Count / 2;
                        int n = map.selectedUnits.Count / 2 + 1;

                        foreach(Unit selectedUnit in map.selectedUnits)
                            map.setTile(selectedUnit.tileX, selectedUnit.tileZ, true);

                        foreach (Unit selectedUnit in map.selectedUnits)
                        {
                            
                            map.selectedUnit = selectedUnit;
                        //    map.charSelected();
                            if (point.currentTileCoord.x - 0.5f >= 0 && point.currentTileCoord.z - 0.5f -i>= 0 && point.currentTileCoord.x - 0.5f < map._tileMap.size_x && point.currentTileCoord.z - 0.5f -i< map._tileMap.size_z)
                                map.GeneratePathTo((int)(point.currentTileCoord.x - 0.5f), (int)(point.currentTileCoord.z - 0.5f - i));
                            else
                            {
                                if (point.currentTileCoord.x - 0.5f >= 0 && point.currentTileCoord.z - 0.5f + n >= 0 && point.currentTileCoord.x - 0.5f < map._tileMap.size_x && point.currentTileCoord.z - 0.5f + n < map._tileMap.size_z)
                                    map.GeneratePathTo((int)(point.currentTileCoord.x - 0.5f), (int)(point.currentTileCoord.z - 0.5f + n));
                                n++;
                            }
                            i++;

                            Unit unit = selectedUnit.GetComponent<Unit>();


                            if (unit.currentPath != null)
                            {

                                foreach (GameObject go in unit.highlightPath)
                                {
                                    DestroyObject(go);
                                }
                                int currNode = 0;
                                while (currNode < unit.currentPath.Count)
                                {
                                    map.highlightPath(unit.currentPath[currNode].x, unit.currentPath[currNode].z);
                                    currNode++;
                                }
                            }
                        }
                    }
                    else
                    {
                        int i = -map.selectedUnits.Count / 2;
                        int n = map.selectedUnits.Count / 2 + 1;

                        foreach (Unit selectedUnit in map.selectedUnits)
                            map.setTile(selectedUnit.tileX, selectedUnit.tileZ, true);

                        foreach (Unit selectedUnit in map.selectedUnits)
                        {
                            
                            map.selectedUnit = selectedUnit;
                    //        map.charSelected();
                            map.setTile(selectedUnit.tileX, selectedUnit.tileZ, true);
                            if (point.currentTileCoord.x - 0.5f -i>= 0 && point.currentTileCoord.z - 0.5f >= 0 && point.currentTileCoord.x - 0.5f -i < map._tileMap.size_x && point.currentTileCoord.z - 0.5f < map._tileMap.size_z)
                                map.GeneratePathTo((int)(point.currentTileCoord.x - 0.5f - i), (int)(point.currentTileCoord.z - 0.5f));
                            else
                            {
                                if (point.currentTileCoord.x - 0.5f + n >= 0 && point.currentTileCoord.z - 0.5f >= 0 && point.currentTileCoord.x - 0.5f + n < map._tileMap.size_x && point.currentTileCoord.z - 0.5f < map._tileMap.size_z)
                                    map.GeneratePathTo((int)(point.currentTileCoord.x - 0.5f + n), (int)(point.currentTileCoord.z - 0.5f));
                                n++;
                            }
                            i++;

                            Unit unit = selectedUnit.GetComponent<Unit>();


                            if (unit.currentPath != null)
                            {

                                foreach (GameObject go in unit.highlightPath)
                                {
                                    DestroyObject(go);
                                }
                                int currNode = 0;
                                while (currNode < unit.currentPath.Count)
                                {
                                    map.highlightPath(unit.currentPath[currNode].x, unit.currentPath[currNode].z);
                                    currNode++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
