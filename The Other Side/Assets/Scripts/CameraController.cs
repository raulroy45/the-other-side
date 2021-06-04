using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{

    private GameObject player;
    // private Tilemap tilemap;
    public Vector3 position;
    public float smoothing = 2;
    public float shift = 2;
    public float minDist;
    public float maxDist;
    public float delta;
    public bool manualMinMax = false;
    // Start is called before the first frame update
    void Start()
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float camHalfHeight = GetComponent<Camera>().orthographicSize;
        delta = screenAspect * camHalfHeight;

        // automatic ref getters
        player = COMMON.FindMyBob();
        if (!manualMinMax) {
            // consider all tilemaps
            Tilemap[] allTileMaps = GameObject.FindObjectsOfType<Tilemap>();
            bool first = true;
            foreach (Tilemap tm in allTileMaps) {
                if (first) {
                    minDist = (tm.localBounds.min.x / 2f) + delta;
                    maxDist = (tm.localBounds.max.x / 2f) - delta;
                    first = false;
                }
                minDist = Mathf.Min(minDist, (tm.localBounds.min.x / 2f) + delta);
                maxDist = Mathf.Max(maxDist, (tm.localBounds.max.x / 2f) - delta);
            }
        }
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
