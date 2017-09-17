using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(TileMap))]
public class CharGrid : MonoBehaviour
{
    public TileType[] tileTypes;
    public TileMap _tileMap;
    //public GameObject selectedUnit;
    public List<Unit> units = new List<Unit>();
    public List<Unit> selectedUnits = new List<Unit>();
    public Unit selectedUnit;
    public GameObject highlight;
    public int[,] tiles;
    public int[,] elevation;
    Node[,] graph;

    void Start()
    {
        _tileMap = GetComponent<TileMap>();

        GenerateMapData();
        GeneratePathfindingGraph();
      //  GenerateMapVisual();


    }

    public void charSelected()
    {
        selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().tileZ = (int)selectedUnit.transform.position.z;
		selectedUnit.GetComponent<Unit> ().map = this;
    }
    void GenerateMapData()
    {
        _tileMap = GetComponent<TileMap>();
        tiles = new int[_tileMap.size_x, _tileMap.size_z];

        for (int x = 0; x < _tileMap.size_x; x++)
        {
            for (int z = 0; z < _tileMap.size_z; z++)
            {
                tiles[x, z] = 0;
            }
        }
        tiles[4, 4] = 1;
        tiles[5, 4] = 1;
        tiles[6, 4] = 1;
        tiles[7, 4] = 1;
        tiles[8, 4] = 1;
        tiles[8, 5] = 1;
        tiles[8, 6] = 1;
        tiles[4, 5] = 1;
        tiles[4, 6] = 1;

        elevation = new int[_tileMap.size_x, _tileMap.size_z];
        for (int x = 0; x < _tileMap.size_x; x++)
        {
            for (int z = 0; z < _tileMap.size_z; z++)
            {
                elevation[x, z] = 0;
            }
        }

        elevation[0, 2] = 1;
        elevation[0, 3] = 2;
        elevation[0, 4] = 3;
        elevation[0, 5] = 4;
        elevation[1, 5] = 5;
        elevation[1, 6] = 6;


    }

    public float CostToEnterTile(int sourceX, int sourceZ, int targetX, int targetZ)
    {

        TileType tt = tileTypes[tiles[targetX, targetZ]];

        if (UnitCanEnterTile(targetX, targetZ) == false)
            return Mathf.Infinity;

        float cost = tt.movementCost;

        return cost;
    }


    void GeneratePathfindingGraph()
    {
        _tileMap = GetComponent<TileMap>();
        graph = new Node[_tileMap.size_x, _tileMap.size_z];

        for (int x = 0; x < _tileMap.size_x; x++)
        {
            for (int z = 0; z < _tileMap.size_z; z++)
            {
                graph[x, z] = new Node();
                graph[x, z].x = x;
                graph[x, z].z = z;
            }
        }

        for (int x = 0; x < _tileMap.size_x; x++)
        {
            for (int z = 0; z < _tileMap.size_z; z++)
            {
                if (x > 0)
                    graph[x, z].neighbours.Add(graph[x - 1, z]);
                if (x < _tileMap.size_x - 1)
                    graph[x, z].neighbours.Add(graph[x + 1, z]);
                if (z > 0)
                    graph[x, z].neighbours.Add(graph[x, z - 1]);
                if (z < _tileMap.size_z - 1)
                    graph[x, z].neighbours.Add(graph[x, z + 1]);

            }
        }

        for (int x = 0; x < _tileMap.size_x; x++)
        {
            for (int z = 0; z < _tileMap.size_z; z++)
            {
                if (elevation[x, z] >= 1)
                {
                    foreach (Node n in graph[x, z].neighbours)
                    {
                        if (elevation[n.x, n.z] + 1 != elevation[x, z] && elevation[n.x, n.z] - 1 != elevation[x, z])
                        {
                            n.neighbours.Remove(graph[x, z]);

                        }
                    }
                    graph[x, z].neighbours.Clear();
                    if (x > 0 && (elevation[x - 1, z] + 1 == elevation[x, z] || elevation[x - 1, z] - 1 == elevation[x, z]))
                        graph[x, z].neighbours.Add(graph[x - 1, z]);
                    if (x < _tileMap.size_x - 1 && (elevation[x + 1, z] + 1 == elevation[x, z] || elevation[x + 1, z] - 1 == elevation[x, z]))
                        graph[x, z].neighbours.Add(graph[x + 1, z]);
                    if (z > 0 && (elevation[x, z - 1] + 1 == elevation[x, z] || elevation[x, z - 1] - 1 == elevation[x, z]))
                        graph[x, z].neighbours.Add(graph[x, z - 1]);
                    if (z < _tileMap.size_z - 1 && (elevation[x, z + 1] + 1 == elevation[x, z] || elevation[x, z + 1] - 1 == elevation[x, z]))
                        graph[x, z].neighbours.Add(graph[x, z + 1]);
                }

            }
        }


    }



        void GenerateMapVisual()
    {
        for (int x = 0; x < _tileMap.size_x; x++)
        {
            for (int z = 0; z < _tileMap.size_z; z++)
            {
                TileType tt = tileTypes[tiles[x, z]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x +0.5f, 0, z +0.5f), Quaternion.identity);
            }
        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int z)
    {
        return new Vector3(x + 0.25f, 0f, z + 0.25f);
    }

    public bool UnitCanEnterTile(int x, int z)
    {

        return tileTypes[tiles[x, z]].isWalkable;
    }

    public void GeneratePathTo(int x, int z)
    {
        if (selectedUnit!= null)
        {

            // Clear out our unit's old path.
            //selectedUnit.GetComponent<Unit>().currentPath = null;
            if (selectedUnit.currentPath != null)
                setTile(selectedUnit.currentPath[selectedUnit.currentPath.Count - 1].x, selectedUnit.currentPath[selectedUnit.currentPath.Count - 1].z, true);

            setTile(selectedUnit.tileX, selectedUnit.tileZ, true);

            if (UnitCanEnterTile(x, z) == false)
            {
                // We probably clicked on a mountain or something, so just quit out.
                return;
            }
            Dictionary<Node, float> dist = new Dictionary<Node, float>();
            Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

            // Setup the "Q" -- the list of nodes we haven't checked yet.
            List<Node> unvisited = new List<Node>();
            
            

            Node source = graph[
                                selectedUnit.GetComponent<Unit>().tileX,
                                selectedUnit.GetComponent<Unit>().tileZ
                                ];

            Node target = graph[
                                x,
                                z
                                ];

            dist[source] = 0;
            prev[source] = null;

            // Initialize everything to have INFINITY distance, since
            // we don't know any better right now. Also, it's possible
            // that some nodes CAN'T be reached from the source,
            // which would make INFINITY a reasonable value
            foreach (Node v in graph)
            {
                if (v != source)
                {
                    dist[v] = Mathf.Infinity;
                    prev[v] = null;
                }

                unvisited.Add(v);
            }
            while (unvisited.Count > 0)
            {
                // "u" is going to be the unvisited node with the smallest distance.
                Node u = null;

                foreach (Node possibleU in unvisited)
                {
                    if (u == null || dist[possibleU] < dist[u])
                    {
                        u = possibleU;
                    }
                }

                if (u == target)
                {
                    break;  // Exit the while loop!
                }

                unvisited.Remove(u);

                foreach (Node v in u.neighbours)
                {
                    //float alt = dist[u] + u.DistanceTo(v);
                    float alt = dist[u] + CostToEnterTile(u.x, u.z, v.x, v.z);
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }

            // If we get there, then either we found the shortest route
            // to our target, or there is no route at ALL to our target.

            if (prev[target] == null)
            {
                // No route between our target and the source
                return;
            }

            List<Node> currentPath = new List<Node>();

            Node curr = target;

            // Step through the "prev" chain and add it to our path
            while (curr != null)
            {
                currentPath.Add(curr);
                curr = prev[curr];
            }

            // Right now, currentPath describes a route from our target to our source
            // So we need to invert it!

            currentPath.Reverse();
            if (selectedUnit.currentPath != null)
            {
                Debug.Log(currentPath.Count);
                setTile(selectedUnit.GetComponent<Unit>().currentPath[selectedUnit.GetComponent<Unit>().currentPath.Count - 1].x,
                selectedUnit.GetComponent<Unit>().currentPath[selectedUnit.GetComponent<Unit>().currentPath.Count - 1].z, true);
            }
                selectedUnit.currentPath = currentPath;

            foreach (GameObject go in selectedUnit.highlightPath)
                {
                    DestroyObject(go);
                }
        }
    }

    public void setTile(int x, int z, bool passable)
    {
        if (!passable)
            tiles[x, z] = 2;

        if (passable)
            tiles[x, z] = 0;
    }

    public bool getTile(int x, int z)
    {
        if (tiles[x, z] == 2)
            return true;
        return false;
    }

    public bool isWalkable(int x, int z)
    {
        return tileTypes[tiles[x, z]].isWalkable;
    }

    public void highlightPath(int x, int z)
    {
        GameObject go = (GameObject)Instantiate(highlight, new Vector3(x + 0.5f, 0.001f, z + 0.5f), Quaternion.identity);


        selectedUnit.GetComponent<Unit>().highlightPath.Add(go);
    }

    public float getElevation(int x, int z)
    {
        return elevation[x, z];
    }
}
