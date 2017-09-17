using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TileMap))]
public class TileMapInspector : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Regenerate"))
        {
            TileMap tileMap = (TileMap) target;
            //tileMap.BuildMesh();
        }
    }

   
}
