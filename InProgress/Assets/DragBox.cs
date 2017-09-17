 using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
 public class DragBox : MonoBehaviour
{
    // Draggable inspector reference to the Image GameObject's RectTransform.

    // This variable will store the location of wherever we first click before dragging.

    public GameObject prefab;
    public GameObject border;
    public CharGrid map;
    public GameObject box;
    GameObject bottom;
    GameObject top;
    GameObject right;
    GameObject left;
    private Vector2 initialClickPosition = Vector2.zero;
    int[,] tiles;

    bool once = true;

    float xi;
    float zi;
    float xf;
    float zf;

    void Update()
    {
        int[,] tiles;
        tiles = new int[map._tileMap.size_x, map._tileMap.size_z];

        // Click somewhere in the Game View.
        if (Input.GetMouseButtonDown(0))
        {
            // Get the initial click position of the mouse. No need to convert to GUI space
            // since we are using the lower left as anchor and pivot.
            box = (GameObject)Instantiate(prefab, new Vector3(-100f, 0.01f,-100f), Quaternion.identity);
            box.transform.Rotate(new Vector3(0,135,0));
            box.GetComponent<Renderer>().material.renderQueue = 4000;

            bottom = (GameObject)Instantiate(border, new Vector3(-100f, 0.01f, -100f), Quaternion.identity);
            bottom.transform.Rotate(new Vector3(0, 135, 135));
            bottom.GetComponent<Renderer>().material.renderQueue = 4000;
 

            top = (GameObject)Instantiate(border, new Vector3(-100f, 0.01f, -100f), Quaternion.identity);
            top.transform.Rotate(new Vector3(0, 135, 135));
            top.GetComponent<Renderer>().material.renderQueue = 4000;



            right = (GameObject)Instantiate(border, new Vector3(-100f, 0.01f, -100f), Quaternion.identity);
            right.transform.Rotate(new Vector3(135, 135, 0));
            right.GetComponent<Renderer>().material.renderQueue = 4000;
  

            left = (GameObject)Instantiate(border, new Vector3(-100f, 0.01f, -100f), Quaternion.identity);
            left.transform.Rotate(new Vector3(135, 135, 0));
            left.GetComponent<Renderer>().material.renderQueue = 4000;


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                xi = hitInfo.point.x;
                zi = hitInfo.point.z;

                box.transform.position = new Vector3(xi,0.01f, zi);

            }

        }

        // While we are dragging.
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                xf = hitInfo.point.x;
                zf = hitInfo.point.z;
                // Debug.Log("x: " + xf + "z: " + zf);
            }
            // Store the current mouse position in screen space.
            //    Vector2 currentMousePosition = new Vector2(Input.mousePosition.x/100, Input.mousePosition.y/100);

            // How far have we moved the mouse?
            Vector2 difference = new Vector2(xf, zf) - new Vector2(xi, zi);
            //difference = difference * Mathf.Sqrt(2);

            // Copy the initial click position to a new variable. Using the original variable will cause
            // the anchor to move around to wherever the current mouse position is,
            // which isn't desirable.

            // The following code accounts for dragging in various directions.


            box.GetComponent<Transform>().localScale = new Vector3((0.707107f * difference.x + 0.707107f * difference.y), 0.01f, ((0.707107f * difference.x - 0.707107f * difference.y)));
            box.transform.position = new Vector3(xi + (0.5f * difference.x), 0.01f, zi + (0.5f * difference.y));

            // bottom border
            bottom.GetComponent<Transform>().localScale = new Vector3(0.01f, 0.01f, ((0.707107f * difference.x - 0.707107f * difference.y + 0.01f)));
            bottom.transform.position = new Vector3(xi + (0.5f * difference.x) + (0.707107f * difference.x + 0.707107f * difference.y) * 0.353535f, 0.01f, zi + (0.5f * difference.y) + (0.707107f * difference.x + 0.707107f * difference.y) * 0.353535f);

            // top border
            top.GetComponent<Transform>().localScale = new Vector3(0.01f, 0.01f, ((0.707107f * difference.x - 0.707107f * difference.y + 0.01f)));
            top.transform.position = new Vector3(xi + (0.5f * difference.x) - (0.707107f * difference.x + 0.707107f * difference.y) * 0.353535f, 0.01f, zi + (0.5f * difference.y) - (0.707107f * difference.x + 0.707107f * difference.y) * 0.353535f);

            // right border
            right.GetComponent<Transform>().localScale = new Vector3((0.707107f * difference.x + 0.707107f * difference.y) + 0.01f, 0.01f, 0.01f);
            right.transform.position = new Vector3(xi + (0.5f * difference.x) + (0.707107f * difference.x - 0.707107f * difference.y) * 0.353535f, 0.01f, zi + (0.5f * difference.y) - (0.707107f * difference.x - 0.707107f * difference.y) * 0.353535f);
            // left border
            left.GetComponent<Transform>().localScale = new Vector3((0.707107f * difference.x + 0.707107f * difference.y) + 0.01f, 0.01f, 0.01f);
            left.transform.position = new Vector3(xi + (0.5f * difference.x) - (0.707107f * difference.x - 0.707107f * difference.y) * 0.353535f, 0.01f, zi + (0.5f * difference.y) + (0.707107f * difference.x - 0.707107f * difference.y) * 0.353535f);

            int sx = (int)Mathf.Floor(xi);
            int sz = (int)Mathf.Floor(zi);
 
            float height = left.GetComponent<Transform>().localScale.x;
            float length = top.GetComponent<Transform>().localScale.z;
            bool lnegative = false;
            bool hnegative = false;
            bool swap = false;

            if (top.GetComponent<Transform>().localScale.z < 0)
                lnegative = true;
            if (box.GetComponent<Transform>().localScale.x < 0)
                hnegative = true;

            if(!lnegative && hnegative)
            {
                for (float i = 0; i >= height; i -= 0.7071f)
                {
                    int ix = sx;
                    int iz = sz;

                    foreach (Unit u in map.units)
                    {
                        if (u.tileX == ix && u.tileZ == iz)
                            if (!alreadyIn(ix, iz))
                                map.selectedUnits.Add(u);
                    }
                    while (true)
                    {
                        foreach (Unit u in map.units)
                        {
                            if (u.tileX == ix && u.tileZ == iz)
                                if (!alreadyIn(ix, iz))
                                    map.selectedUnits.Add(u);
                        }
                        

                        length -= 1.4142f;
                            ix++;
                            iz--;
                        if ((length) <= 0)
                            break;

                    }
                    if (swap)
                    {
                        sx--;
                        length = top.GetComponent<Transform>().localScale.z;
                        swap = false;
                    }
                    else
                    {
                        sz--;
                        length = top.GetComponent<Transform>().localScale.z - 1.4142f;
                        swap = true;
                    }
                }
            }

            if (!lnegative && !hnegative)
            {
                for (float i = 0; i <= height; i += 0.7071f)
                {
                    int ix = sx;
                    int iz = sz;

                    foreach (Unit u in map.units)
                    {
                        if (u.tileX == ix && u.tileZ == iz)
                            if (!alreadyIn(ix, iz))
                                map.selectedUnits.Add(u);
                    }
                    while (true)
                    {
                        foreach (Unit u in map.units)
                        {
                            if (u.tileX == ix && u.tileZ == iz)
                                if (!alreadyIn(ix, iz))
                                    map.selectedUnits.Add(u);

                        }

                            length -= 1.4142f;
                            ix++;
                            iz--;
                            if ((length) <= 0)
                                break;
                    }
                    if (swap)
                    {
                        sz++;
                        length = top.GetComponent<Transform>().localScale.z;
                        swap = false;
                    }
                    else
                    {
                        sx++;
                        length = top.GetComponent<Transform>().localScale.z - 1.4142f;
                        swap = true;
                    }
                }
            }

            if (lnegative && !hnegative)
            {
                for (float i = 0; i <= height; i += 0.7071f)
                {
                    int ix = sx;
                    int iz = sz;

                    foreach (Unit u in map.units)
                    {
                        if (u.tileX == ix && u.tileZ == iz)
                            if (!alreadyIn(ix, iz))
                                map.selectedUnits.Add(u);
                    }
                    while (true)
                    {
                        foreach (Unit u in map.units)
                        {
                            if (u.tileX == ix && u.tileZ == iz)
                                if (!alreadyIn(ix, iz))
                                    map.selectedUnits.Add(u);
                        }

                            length += 1.4142f;
                            ix--;
                            iz++;
                            if ((length) >= 0)
                                break;
                    }
                    if (swap)
                    {
                        sx++;
                        length = top.GetComponent<Transform>().localScale.z;
                        swap = false;
                    }
                    else
                    {
                        sz++;
                        length = top.GetComponent<Transform>().localScale.z - 1.4142f;
                        swap = true;
                    }
                }
            }

            if (lnegative && hnegative)
            {
                for (float i = 0; i >= height; i -= 0.7071f)
                {
                    int ix = sx;
                    int iz = sz;

                    foreach (Unit u in map.units)
                    {
                        if (u.tileX == ix && u.tileZ == iz)
                            if (!alreadyIn(ix, iz))
                                map.selectedUnits.Add(u);
                    }
                    while (true)
                    {
                        //   Debug.Log("x: " + ix + "z: " + iz);
                        // if (ix >= 0 && ix < map._tileMap.size_x && iz < map._tileMap.size_z && iz >= 0)
                        foreach (Unit u in map.units)
                        {
                            if (u.tileX == ix && u.tileZ == iz)
                                if (!alreadyIn(ix, iz))
                                    map.selectedUnits.Add(u);
                        }

                            length += 1.4142f;
                            ix--;
                            iz++;
                            if ((length) >= 0)
                                break;   
                    }
                    if (swap)
                    {
                        sz--;
                        length = top.GetComponent<Transform>().localScale.z;
                        swap = false;
                    }
                    else
                    {
                        sx--;
                        length = top.GetComponent<Transform>().localScale.z - 1.4142f;
                        swap = true;
                    }
                }
            }
        }

        // After we release the mouse button.
        if (Input.GetMouseButtonUp(0))
        {
            // Reset
            initialClickPosition = Vector2.zero;

            DestroyObject(box);
            DestroyObject(top);
            DestroyObject(bottom);
            DestroyObject(right);
            DestroyObject(left);
            foreach(Unit u in map.selectedUnits)
            Debug.Log("x: " + u.tileX + "z: " + u.tileZ);

        }
    }

    public bool alreadyIn(int x, int z)
    {
        foreach (Unit u in map.selectedUnits)
            if (u.tileX == x && u.tileZ == z)
                return true;

        return false;
    }
}