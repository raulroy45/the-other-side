using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextLevel : MonoBehaviour
{
    
    public string nextLevelName;
    // support multiple keys
    public int lockCount;
    public SpriteRenderer targetRenderer;
    // sprite of opened door
    public Sprite targetSprite;

    // dirty fix
    void Update() {
        if (lockCount == 0) {
            targetRenderer.sprite = targetSprite;
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Bob" && lockCount == 0) {
            // open the door a bit?
            // next lv
            SceneManager.LoadScene(nextLevelName);
        }
    }


}
