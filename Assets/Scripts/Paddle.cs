using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Paddle : MonoBehaviour {

    public float speed = 150.0f;

	// Use this for initialization
	void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        GetComponent<Rigidbody2D>().velocity = Vector2.right * h * speed;

        if (GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("BreakoutMenuScene");
            }
        }
    }


}
