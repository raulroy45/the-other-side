using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class FallingPlat : MonoBehaviour
{

    private Rigidbody2D body2D;
    
    // Start is called before the first frame update
    void Start()
    {
        // Add all colliders automatically
        TilemapCollider2D tc = gameObject.AddComponent<TilemapCollider2D>() as TilemapCollider2D;
        CompositeCollider2D cc = gameObject.AddComponent<CompositeCollider2D>() as CompositeCollider2D;
        tc.usedByComposite = true;
        // cc.attachedRigidbody.bodyType = RigidbodyType2D.Static;
        cc.geometryType = CompositeCollider2D.GeometryType.Polygons;
        body2D = cc.attachedRigidbody;
        body2D.bodyType = RigidbodyType2D.Static;
        body2D.mass = 20;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other) {
        body2D.bodyType = RigidbodyType2D.Dynamic;
    }
    
}

