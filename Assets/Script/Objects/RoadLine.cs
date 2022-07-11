using System;
using UnityEngine;

public class RoadLine
{
    public Vector3 startPoint;
    public Vector3 direction;
    public float length;
    public RoadLine nextRoadLine;

    public Vector3 endPoint
    {
        get
        {
            return this.startPoint + this.direction * this.length;
        }
    }

    public RoadLine(Vector3 startPoint, Vector3 direction, float length, RoadLine nextRoadLine) {
        this.startPoint = startPoint;
        this.direction = direction;
        this.length = length;
        this.nextRoadLine = nextRoadLine;
    }

}
