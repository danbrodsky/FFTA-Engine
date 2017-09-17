using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TileMap : MonoBehaviour {

    public int size_x = 100;
    public int size_z = 100;
    public float tile_size = 1.0f;

    public Texture2D terrainTiles;
    public int tileResolution = 16;

    // Use this for initialization
    void Start()
    {
        BuildMesh();
    }

    Color[][] ChopUpTiles()
    {
        int numTilesPerRow = terrainTiles.width / tileResolution;
        int numRows = terrainTiles.height / tileResolution;

        Color[][] tiles = new Color[numTilesPerRow * numRows][];

        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numTilesPerRow; x++)
            {
                tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
            }
        }
        return tiles;
    }

    void BuildTexture()
    {
        //       DTileMap map = new DTileMap(size_x, size_z);

        int numTilesPerRow = terrainTiles.width / tileResolution;
        int numRows = terrainTiles.height / tileResolution;

        int texWidth = size_x * tileResolution;
        int texHeight = size_z * tileResolution;
        Texture2D texture = new Texture2D(texWidth, texHeight);

        Color[][] tiles = ChopUpTiles();

        for (int y = 0; y < size_z; y++)
        {
            for (int x = 0; x < size_x; x++)
            {
                //              Color[] p = tiles[map.GetTileAt(x,y)];
                //             texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
            }
        }
        texture.filterMode = FilterMode.Bilinear;
        texture.Apply();

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.sharedMaterials[0].mainTexture = texture;
    }


    public void BuildMesh()
    {
        int num_tiles = size_x * size_z;
        int num_tris = num_tiles * 2;
        int vsize_x = size_x + 1;
        int vsize_z = size_z + 1;
        int num_verts = vsize_x * vsize_z;


        //generate mesh data
        Vector3[] vertices = new Vector3[num_verts];
        Vector3[] normals = new Vector3[num_verts];
        Vector2[] uv = new Vector2[num_verts];

        int[] triangles = new int[num_tris * 3];

        int x, z;
        for (z = 0; z < vsize_z; z++)
        {
            for (x = 0; x < vsize_x; x++)
            {
                vertices[z * vsize_x + x] = new Vector3(x * tile_size, Random.Range(-0f, 0f), z * tile_size);
                normals[z * vsize_x + x] = Vector3.up;
                uv[z * vsize_x + x] = new Vector2((float)x / size_x, (float)z / size_z);

            }
        }
        for (z = 0; z < size_z; z++)
        {
            for (x = 0; x < size_x; x++)
            {
                int squareIndex = z * size_x + x;
                int triIndex = squareIndex * 6;
                triangles[triIndex + 0] = z * vsize_x + x + 0;
                triangles[triIndex + 1] = z * vsize_x + x + vsize_x + 0;
                triangles[triIndex + 2] = z * vsize_x + x + vsize_x + 1;

                triangles[triIndex + 3] = z * vsize_x + x + 0;
                triangles[triIndex + 4] = z * vsize_x + x + vsize_x + 1;
                triangles[triIndex + 5] = z * vsize_x + x + 1;

            }
        }



        //creat new mesh, populate with data
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;
        ;



        // assign mesh to collider
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshCollider mc = GetComponent<MeshCollider>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        mf.mesh = mesh;
        mc.sharedMesh = mesh;

        //      BuildTexture();

    }


}
