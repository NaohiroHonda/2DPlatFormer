using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour
{
	public static int score = 0;					// The player's score.

    public static int life = 50;

	private PlayerControl playerControl;	// Reference to the player control script.
	private int previousScore = 0;			// The score in the previous frame.

    public static bool GameOver { get; set; }
    private bool goUpdate;

    [SerializeField]
    private Text gameOver;

	void Awake ()
	{
		// Setting up the reference.
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        Init();
    }
    
    public void Init()
    {
        score = 0;
        life = 50;

        gameOver.text = "";
        GameOver = false;
        goUpdate = false;
    }


	void Update ()
	{
        if (life <= 0) GameOver = true;

        if (GameOver) GameOverUpdate();
		// Set the score text.
		GetComponent<GUIText>().text = "Score: " + score + "  Life: " + life;

		// If the score has changed...
		if(previousScore != score)
			// ... play a taunt.
			playerControl.StartCoroutine(playerControl.Taunt());

		// Set the previous score to this frame's score.
		previousScore = score;
	}

    private void GameOverUpdate()
    {
        if (goUpdate) return;

        goUpdate = true;
        gameOver.text = "Game Over";
    }
}
