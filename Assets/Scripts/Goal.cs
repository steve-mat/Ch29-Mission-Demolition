using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public static bool goalMet;

    private Renderer rend;

    void Awake() {
        rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Projectile") {
            goalMet = true;
            Color c = rend.material.color;
            c.a = 1;
            rend.material.color = c;
        }
        
    }



}
