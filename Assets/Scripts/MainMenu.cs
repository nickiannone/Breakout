using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Load values from 
	void Start () {
        var volumeSlider = GameObject.Find("volumeSlider").GetComponent<UnityEngine.UI.Slider>();
        volumeSlider.value = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : 0.5f;
        
        var livesInput = GameObject.Find("numLivesInput").GetComponent<UnityEngine.UI.InputField>();
        livesInput.text = PlayerPrefs.HasKey("NumLives") ? PlayerPrefs.GetInt("NumLives").ToString() : "3";

        var brickRowsInput = GameObject.Find("brickRowsInput").GetComponent<UnityEngine.UI.InputField>();
        brickRowsInput.text = PlayerPrefs.HasKey("BrickRows") ? PlayerPrefs.GetInt("BrickRows").ToString() : "4";
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SetLives()
    {
        var numLivesInput = GameObject.Find("numLivesInput").GetComponent<UnityEngine.UI.InputField>();
        var newValue = 0;
        if (int.TryParse(numLivesInput.text, out newValue))
        {
            PlayerPrefs.SetInt("NumLives", newValue);
        }
    }

    public void SetVolume()
    {
        var volumeSlider = GameObject.Find("volumeSlider").GetComponent<UnityEngine.UI.Slider>();
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    public void SetBrickRows()
    {
        var brickRowsInput = GameObject.Find("brickRowsInput").GetComponent<UnityEngine.UI.InputField>();
        var newValue = 0;
        if (int.TryParse(brickRowsInput.text, out newValue))
        {
            PlayerPrefs.SetInt("BrickRows", newValue);
        }
    }

    public void StartGame()
    {
        // Load the UI settings into the gamestate.
        GameState.DoomText = "Game On!";
        GameState.HasThruBall = false;
        GameState.Lives = PlayerPrefs.GetInt("NumLives");
        GameState.BrickRows = PlayerPrefs.GetInt("BrickRows");
        GameState.Volume = PlayerPrefs.GetFloat("Volume");
        GameState.GameOver = false;

        SceneManager.LoadScene("BreakoutGameScene");
    }
}
