using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    public Tilemap tilemap;
    public Vector3 position;
    public float smoothing = 2;
    public float shift = 2;
    public float minDist;
    public float maxDist;
    public float delta;
    // Start is called before the first frame update
    void Start()
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float camHalfHeight = GetComponent<Camera>().orthographicSize;
        delta = screenAspect * camHalfHeight;
        minDist = (tilemap.localBounds.min.x / 2f) + delta;
        maxDist = (tilemap.localBounds.max.x / 2f) - delta;
        
    }

    // Update is called once per frame
    void Update()
    {
        position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        if (player.transform.localScale.x > 0f) {
            position = new Vector3(position.x + shift, position.y, position.z);
        } else {
            position = new Vector3(position.x - shift, position.y, position.z);
        }

        position.x = Mathf.Clamp(position.x, minDist, maxDist);

        transform.position = Vector3.Lerp(transform.position, position, smoothing * Time.deltaTime);
    }
}