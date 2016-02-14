using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {

    public static Slingshot S;

    public GameObject projectilePrefab;
    public float velocityMult = 10.0f;
    public bool ______________________;


    public GameObject launchPoint;
    private Transform launchPointTrans;
    private Vector3 launchPos = Vector3.zero;
    private GameObject projectile;
    private bool aimingMode;
    private Rigidbody projectileRigidbody;
    public bool projectileAlreadyCreated;

    private SphereCollider sCollider;


    void Awake() {

        S = this;

        launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        sCollider = GetComponent<SphereCollider>();

	}

    void Update() {


        if(aimingMode == false) {
            return;
        }

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;

        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;

        float maxMagnitude = sCollider.radius;
        if(mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta = mouseDelta * maxMagnitude;
        }

        
        Vector3 projectilePos = launchPos + mouseDelta;
        projectile.transform.position = projectilePos;
        
        if(Input.GetMouseButtonUp(0)) {
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            CameraFollow.S.pointOfInterest = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
        }


    }

    void OnMouseEnter() {

        launchPoint.SetActive(true);

    }

    void OnMouseExit() {

        launchPoint.SetActive(false);

    }

    void OnMouseDown() {

        aimingMode = true;

        projectile = Instantiate(projectilePrefab);
        projectile.transform.position = launchPos;

        projectileRigidbody = projectile.GetComponent<Rigidbody>();

        projectileRigidbody.isKinematic = true;                    


    }
}
