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
    // Start is called before the first frame update
    void Start()
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float camHalfHeight = GetComponent<Camera>().orthographicSize;
        delta = screenAspect * camHalfHeight;
        // automatic ref getters
        player = COMMON.FindMyBob();
        // consider all tilemaps
        Tilemap[] allTileMaps = GameObject.FindObjectsOfType<Tilemap>();
        // old calc. why divide by 2?
        // minDist = (tm.localBounds.min.x / 2f) + delta;
        // maxDist = (tm.localBounds.max.x / 2f) - delta;
        minDist = 999f;
        maxDist = -999f;
        foreach (Tilemap tm in allTileMaps) {
            tm.CompressBounds();
            Vector3 minP = tm.CellToWorld(tm.cellBounds.min);
            Vector3 maxP = tm.CellToWorld(tm.cellBounds.max);
            // minDist = Mathf.Min(minDist, tm.cellBounds.min.x / 2f);
            // maxDist = Mathf.Max(maxDist, tm.cellBounds.max.x / 2f);
            minDist = Mathf.Min(minDist, minP.x);
            maxDist = Mathf.Max(maxDist, maxP.x);
        }
        minDist += delta;
        maxDist -= delta;
    }

//     Cam 1.777778 | 5.604164 | 1920
// UnityEngine.Debug:Log (object)
// CameraController:Start () (at Assets/Scripts/CameraController.cs:23)
// Cam 1.777778 | 3.247474 | 1920
// UnityEngine.Debug:Log (object)
// CameraController:Start () (at Assets/Scripts/CameraController.cs:23)




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
