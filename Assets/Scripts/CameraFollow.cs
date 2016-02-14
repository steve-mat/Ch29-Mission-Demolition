using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public static CameraFollow S;   //singleton

    public GameObject pointOfInterest;
    private float cameraZ;
    private float easing = 0.05f;
    private Vector2 minXY;
    private Camera maincam;

    private Vector3 destination;


    void Awake() {

        S = this;
        cameraZ = this.transform.position.z;

        maincam = GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if(pointOfInterest == null) {
            destination = Vector3.zero;
        } else {

            destination = pointOfInterest.transform.position;

            if(pointOfInterest.tag == "Projectile" && pointOfInterest.GetComponent<Rigidbody>().IsSleeping() == true) {
                pointOfInterest = null;
                return;
            }

        }


        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = cameraZ;
        transform.position = destination;

        maincam.orthographicSize = destination.y + 10;

	}
}
