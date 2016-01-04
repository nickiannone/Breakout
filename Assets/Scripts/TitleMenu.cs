using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("BreakoutMenuScene");
        }
    }
}
