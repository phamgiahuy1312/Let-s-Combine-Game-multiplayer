using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Line : MonoBehaviour
{
    
    LineRenderer rend;
    EdgeCollider2D col;

    //add list vector2 to store line points
    public List<Vector2> linePoints = new List<Vector2>();

    private void Start()
    {
        rend = GetComponent<LineRenderer>();
        col = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        //set line position, locol player position
        linePoints[0] = rend.GetPosition(0);
        linePoints[1] = rend.GetPosition(1);
        //set collider points
        col.SetPoints(linePoints);
    }
}
