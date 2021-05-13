using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBody : MonoBehaviour
{

    public PlayerController pcScript;
    
    private List<Rigidbody2D> wallRigidBodies;
    private bool bodyActive;

    // Start is called before the first frame update
    void Start()
    {
        // get a hold of all rb2d in this game obejct
        bodyActive = false; // everything in wall starts inactive
        wallRigidBodies = new List<Rigidbody2D>();
        // loop through children, find all rb2d
        foreach (Transform child in transform) {
            foreach (Rigidbody2D b in child.gameObject.GetComponentsInChildren<Rigidbody2D>()) {
                wallRigidBodies.Add(b);
                b.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (wallRigidBodies == null || pcScript == null) return;
        if (pcScript.isWallMerged && !bodyActive) {
            bodyActive = true;
            foreach (Rigidbody2D b in wallRigidBodies) {
                b.bodyType = RigidbodyType2D.Dynamic;
            }
        } else if (!pcScript.isWallMerged && bodyActive) {
            bodyActive = false;
            foreach (Rigidbody2D b in wallRigidBodies) {
                b.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}
