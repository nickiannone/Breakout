using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

    public float speed = 100.0f;
    public float paddleSpeed = 150.0f;

    private bool lockedToPaddle = false;
    
    private Vector2? thruPosition = null;
    private Vector2? thruVelocity = null;

    private bool gameOver = false;

	/// <summary>
    ///     Locks the ball to the paddle, builds the rows of bricks, and changes the volume.
    /// </summary>
	void Start()
    {
        LockToPaddle(0);
        GameState.GenerateBrickRows();
        GameState.ApplyVolume();
	}

    /// <summary>
    ///     Locks the ball to the paddle. Used for resetting the paddle when the game resets.
    /// </summary>
    /// <param name="yOffset">The offset from the center of the paddle.</param>
    void LockToPaddle(float yOffset)
    {
        if (!lockedToPaddle)
        {
            var ballBody = GetComponent<Rigidbody2D>();
            ballBody.velocity = new Vector2(0, 0);
            ballBody.transform.parent = GameObject.Find("paddle").transform;
            ballBody.transform.localPosition = new Vector2(yOffset, 16);
            lockedToPaddle = true;
        }
    }

    /// <summary>
    ///     Releases the ball from the paddle along the given velocity vector.
    /// </summary>
    /// <param name="normal">The release trajectory.</param>
    void ReleaseFromPaddle(Vector2 normal)
    {
        if (lockedToPaddle)
        {
            GetComponent<Rigidbody2D>().velocity = normal * speed;
            var ballBody = GetComponent<Rigidbody2D>();
            ballBody.transform.parent = null;
            lockedToPaddle = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!lockedToPaddle && col.gameObject.name != "death")
        {
            // Play audio when the ball bounces off of something.
            var audio = GameObject.Find("audio");
            if (audio != null)
            {
                var clip = Resources.Load("Sounds/forehand-close") as AudioClip;
                audio.GetComponent<AudioSource>().PlayOneShot(clip);
            }
        }

        // Bounce off the paddle.
        if (col.gameObject.name == "paddle")
        {
            float x = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.x);
            Vector2 dir = new Vector2(x, 1).normalized;
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
        
        // Set up for passing through a brick if the Thru-Ball powerup is set.
        // NOTE: This is a hack; a better solution would be to replace all bricks' box colliders with simple triggers!
        if (col.gameObject.tag == "brick" && GameState.HasThruBall)
        {
            thruVelocity = col.relativeVelocity;
            thruPosition = GetComponent<Rigidbody2D>().position;
        }
    }

    void FixedUpdate()
    {
        // If we're at game over, and the user hits space, go back to the menu scene.
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("BreakoutMenuScene");
            }
            return;
        }

        if (lockedToPaddle)
        {
            // If we're locked to the paddle, move the ball along with the paddle until it is launched.
            float h = Input.GetAxisRaw("Horizontal");
            var ballBody = GetComponent<Rigidbody2D>();
            ballBody.velocity = Vector2.right * h * paddleSpeed;

            // If the spacebar is pressed, unlock from the paddle and fire the ball.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Release upwards.
                ReleaseFromPaddle(Vector2.up);
            }
        }
        else
        {
            // If thru-ball is active, just override the position from the last frame.
            if (GameState.HasThruBall && thruPosition.HasValue)
            {
                GetComponent<Rigidbody2D>().position = thruPosition.Value;
                GetComponent<Rigidbody2D>().velocity = thruVelocity.Value;
                thruPosition = null;
                thruVelocity = null;
            }
        }
    }

    public void Die()
    {
        // Decrement lives by one
        GameState.Lives -= 1;

        // Pause ball movement.
        var ballBody = GetComponent<Rigidbody2D>();
        ballBody.velocity = new Vector2(0, 0);

        // Snap ball to paddle and constrain it.
        LockToPaddle(0);

        if (GameState.Lives <= 0)
        {
            // TODO If lives are zero, lose the game.
            GameState.GameLost();
            Destroy(gameObject);
        }
    }

    public void Win()
    {
        GameState.GameWon();
        Destroy(gameObject);
    }
        
    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth)
    {
        return (ballPos.x - racketPos.x) / racketWidth;
    }
}
