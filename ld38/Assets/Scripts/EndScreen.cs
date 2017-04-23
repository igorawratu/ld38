using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {
    public Text text;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < Levels.Instance.scores_.Length; ++i)
        {
            text.text += "Level " + i + ": " + Levels.Instance.scores_[i] + "\n";
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
