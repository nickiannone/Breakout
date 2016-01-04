using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Destroy(gameObject);
        GameState.Score += 20;
        GameState.DoomText = "Awesome!";

        if (GameState.Random.NextDouble() >= 0.95)
        {
            // TODO Generate a ThruBall powerup!
            GameState.HasThruBall = true;
        }

        // DEBUG Write the number of remaining bricks to the UI!
        GameObject.Find("debugText").GetComponent<UnityEngine.UI.Text>().text = "Bricks left: " + (GameObject.FindGameObjectsWithTag("brick").Length - 1);

        if (GameObject.FindGameObjectsWithTag("brick").Length <= 1)
        {
            // Tell the ball that we won.
            GameObject.Find("ball").SendMessage("Win");
        }
    }
}
