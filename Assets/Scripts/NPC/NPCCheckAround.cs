using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCheckAround : MonoBehaviour
{
    // Start is called before the first frame update
    float length = 10.0f;
    private Vector3 _upPadding = new Vector3(0f,0.5f,0f);

    void Update()
    {
        CheckArroundBlock();
    }
    public void CheckArroundBlock()
    {
        Ray Rightray = new Ray(transform.position + _upPadding,Vector3.right);
        Ray Leftray = new Ray(transform.position + _upPadding,Vector3.left);
        Debug.DrawRay(transform.position + _upPadding, Vector3.right * length, Color.blue, 0.1f);
        Debug.DrawRay(transform.position + _upPadding, Vector3.left * length, Color.yellow, 0.1f);

        if (!Physics.Raycast(Rightray, out RaycastHit hit, length)) return;
    }
}
