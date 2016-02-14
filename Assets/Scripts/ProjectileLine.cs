using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ProjectileLine : MonoBehaviour {

    public static ProjectileLine S;

    public float minDist = 0.1f;

    public LineRenderer line;
    private GameObject _pointOfInterest;
    private List<Vector3> points;

    void Awake() {

        S = this;

        line = GetComponent<LineRenderer>();
        line.enabled = false;

        points = new List<Vector3>();

    }

    void FixedUpdate() {

        if(pointOfInterest == null) {
            if(CameraFollow.S.pointOfInterest != null) {
                if(CameraFollow.S.pointOfInterest.tag == "Projectile") {
                    pointOfInterest = CameraFollow.S.pointOfInterest;
                } else {
                    return;
                }
            } else {
                return;
            }
        }

        AddPoint();

        if(pointOfInterest.GetComponent<Rigidbody>().IsSleeping() == true) {
            pointOfInterest = null;
        }

    }

    public GameObject pointOfInterest {

        get {
            return (_pointOfInterest);
        }

        set {

            _pointOfInterest = value;
            if(_pointOfInterest != null) {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }

    }

    public void Clear() {
        _pointOfInterest = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    private void AddPoint() {

        Vector3 pt = _pointOfInterest.transform.position;
        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist) {
            return;
        }

        if(points.Count == 0) {
            Vector3 launchPos = Slingshot.S.launchPoint.transform.position;
            Vector3 launchPosDiff = pt - launchPos;

            points.Add(pt + launchPosDiff);
            points.Add(pt);

            line.SetVertexCount(2);
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);

            line.enabled = true;

        } else {
            points.Add(pt);

            line.SetVertexCount(points.Count);
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }

    }

    public Vector3 lastPoint {

        get {

            if(points == null) {
                return Vector3.zero;
            }
            return points[points.Count - 1];
        }
    }

}
