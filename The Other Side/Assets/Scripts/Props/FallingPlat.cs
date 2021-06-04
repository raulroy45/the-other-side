using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class FallingPlat : MonoBehaviour
{

    // switches
    public bool onlyBobCanTrigger = false;

    private Rigidbody2D body2D;

	private Vector3 originPosition;
	private Quaternion originRotation;
    Tilemap tilemap;
    // shake when true
    private bool isShaking;
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

        // 
        isShaking = false;
        originPosition = transform.position;
        originRotation = transform.rotation;
        tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
        Debug.Log(tilemap.localBounds.center);
    }

    // magic param
    private float a = 25;
    private float b = 10;
    private float c = 15;
    private float maxG = 1;
    private float shakeGain = 0f;
    // Update is called once per frame

    void Update()
    {
        
        Vector3 maxP = tilemap.CellToWorld(tilemap.cellBounds.max);
        Vector3 minP = tilemap.CellToWorld(tilemap.cellBounds.min);

        if (isShaking) {
            // go!
            Vector3 dp = new Vector3(
                shakeGain * Mathf.Sin(Time.time * a) / b,
                shakeGain * (Random.value - 0.5f) / c, 0);
            Debug.Log(transform.position + " | " + originRotation);
            transform.position = originPosition + dp;

            transform.RotateAround((maxP + minP) / 2, Vector3.forward, (Random.value - 0.5f) * 1f);
            shakeGain += Time.deltaTime * 2;
            shakeGain = Mathf.Min(maxG, shakeGain);
            
        }
    }

    // oneshot
    private bool one_shot = false;
    void OnCollisionEnter2D(Collision2D other) {
        if (onlyBobCanTrigger && !other.transform.CompareTag("Bob")) return;
        if (one_shot) return;
        one_shot = true;
        // shart shaking
        isShaking = true;
        StartCoroutine(FallAfter(0.3f));
    }

    IEnumerator FallAfter(float time) {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay
        body2D.bodyType = RigidbodyType2D.Dynamic;
        // end shaking
        isShaking = false;
        transform.position = originPosition;
        transform.rotation = originRotation;
    }
    
}

