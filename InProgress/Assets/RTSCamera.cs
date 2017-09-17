// Original: https://www.youtube.com/watch?v=48xnDi0c7FA / http://pastebin.com/guuw0wgj

using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour
{

    public float CamSpeed = 0.1f;
    public int GUISize = 25;

    void Update()
    {
        var recdown = new Rect(0, 0, Screen.width, GUISize);

        var recup = new Rect(0, Screen.height - GUISize, Screen.width, GUISize);

        var recleft = new Rect(0, 0, GUISize, Screen.height);

        var recright = new Rect(Screen.width - GUISize, 0, GUISize, Screen.height);

        if (recdown.Contains(Input.mousePosition))
        {
            transform.Translate(-CamSpeed, 0, -CamSpeed, Space.World);
        }

        if (recup.Contains(Input.mousePosition))
        {
            transform.Translate(CamSpeed, 0, CamSpeed, Space.World);
        }

        if (recleft.Contains(Input.mousePosition))
        {
            transform.Translate(-CamSpeed, 0, CamSpeed, Space.World);
        }

        if (recright.Contains(Input.mousePosition))
        {
            transform.Translate(CamSpeed, 0, -CamSpeed, Space.World);
        }

        GameObject Eye = GameObject.Find("Main Camera");

        //
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Eye.GetComponent<Camera>().orthographicSize > 1)
        {
            Eye.GetComponent<Camera>().orthographicSize = Eye.GetComponent<Camera>().orthographicSize - 1;
        }

        //
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Eye.GetComponent<Camera>().orthographicSize < 7)
        {
            Eye.GetComponent<Camera>().orthographicSize = Eye.GetComponent<Camera>().orthographicSize + 1;
        }

        //default zoom
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Eye.GetComponent<Camera>().orthographicSize = 5;
        }

    }
}