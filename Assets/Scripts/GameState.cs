using System;
using UnityEngine;

public class GameState : MonoBehaviour {

    private static int lives = 3;
    private static int score = 0;
    private static bool hasThruBall = false;
    private static string doomText = "Game On!";
    private static System.Random r = new System.Random();
    private static int brickRows = 4;
    private static bool gameOver = false;

    public static T PickRandomElement<T>(T[] array)
    {
        return array[r.Next(array.Length)];
    }

    // These collections indicate which sprites are selected when Doomgirl is updated.
    public static int[] DOOMGIRL_HAPPY = { 0, 1, 5, 9 };

    public static bool GameOver
    {
        get { return gameOver; }
        set { gameOver = value; }
    }

    public static void ApplyVolume()
    {
        var audio = GameObject.Find("audio");
        if (audio != null)
        {
            audio.GetComponent<AudioSource>().volume = GameState.Volume;
        }
    }

    public static int[] DOOMGIRL_IDLE = { 9 };
    public static int[] DOOMGIRL_ANGRY = { 3, 4, 7, 8, 10, 11 };
    public static int[] DOOMGIRL_IMPRESSED = { 2, 6 };

    public static System.Random Random
    {
        get { return r; }
        private set { }
    }

    public static float Volume
    {
        get; set;
    }

    public static int Lives
    {
        get { return lives; }
        set {
            if (value < lives)
            {
                UpdateFaceDisplay(DOOMGIRL_ANGRY);
                UpdateDoomText("Fiddle Sticks!");
            }
            lives = value;
            UpdateScoreDisplay();
        }
    }

    public static int Score
    {
        get { return score; }
        set {
            if (value > score)
            {
                UpdateFaceDisplay(DOOMGIRL_HAPPY);
                UpdateDoomText("Awesome!");
            }
            score = value;
            UpdateScoreDisplay();
        }
    }

    public static bool HasThruBall
    {
        get { return hasThruBall; }
        set {
            hasThruBall = value;
            if (hasThruBall)
            {
                UpdateFaceDisplay(DOOMGIRL_IMPRESSED);
                UpdateDoomText("Thru Ball Enabled!");
            }
        }
    }

    public static int BrickRows
    {
        get { return brickRows; }
        set { brickRows = value; }
    }

    public static void GenerateBrickRows()
    {
        var brickTemplate = GameObject.Find("brickTemplate");
        if (brickTemplate != null)
        {
            // Create N rows of bricks from the template.
            for (int i = 0; i < brickRows; i++)
            {
                // Calculate the X and Y coordinates.
                var y = 88 - (i * 24);
                int[] x = { -96, -48, 0, 48, 96 };

                // Create five bricks per row.
                for (int j = 0; j < 5; j++)
                {
                    // Create a brick.
                    var brick = Instantiate(brickTemplate);
                    brick.transform.position = new Vector2(x[j], y);
                    brick.tag = "brick";
                }
            }
        }
    }

    public static string DoomText
    {
        get { return doomText; }
        set
        {
            doomText = value;
            UpdateDoomText(doomText);
        }
    }

    void Awake()
    {
        // Make this object persistent.
        DontDestroyOnLoad(this);
    }

    public static void UpdateScoreDisplay()
    {
        var scoreLivesText = GameObject.Find("scoreLivesText");
        if (scoreLivesText != null)
        {
            scoreLivesText.GetComponent<UnityEngine.UI.Text>().text = "Score: " + GameState.score + "\nLives: " + GameState.lives;
        }
    }

    private static void UpdateFaceDisplay(int[] spriteIndexGroup)
    {
        var gameObj = GameObject.Find("doomgirl");
        if (gameObj != null)
        {
            var spriteIndex = PickRandomElement(spriteIndexGroup);
            var sprite = Resources.LoadAll<Sprite>("Sprites/doomgirl")[spriteIndex];
            gameObj.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    private static void UpdateDoomText(string text)
    {
        var doomText = GameObject.Find("doomText");
        if (doomText != null)
        {
            doomText.GetComponent<UnityEngine.UI.Text>().text = text;
        }
    }

    public static void GameLost()
    {
        UpdateFaceDisplay(DOOMGIRL_ANGRY);
        if (GameObject.FindGameObjectsWithTag("brick").Length > 2)
        {
            DoomText = "Oh well.";
        }
        else
        {
            DoomText = "I can do it!";
        }
        GameOver = true;
    }

    public static void GameWon()
    {
        UpdateFaceDisplay(DOOMGIRL_IMPRESSED);
        DoomText = "Wicked cool!";
        GameOver = true;
    }
}
