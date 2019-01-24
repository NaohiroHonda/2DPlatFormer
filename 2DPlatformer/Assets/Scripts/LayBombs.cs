using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Assets.Adds;

public class LayBombs : MonoBehaviour
{
	private int bombCount = 0;			// How many bombs the player has.
	public AudioClip bombsAway;			// Sound for when the player lays a bomb.
	public GameObject bomb;				// Prefab of the bomb.


	private GUITexture bombHUD;			// Heads up display of whether the player has a bomb or not.

    [SerializeField]
    private Text bombText;

    [SerializeField]
    private TimerUISC timerUI;

    [SerializeField]
    private float freezeTime = 0.25f;
    private float time;


	void Awake ()
	{
		// Setting up the reference.
		bombHUD = GameObject.Find("ui_bombHUD").GetComponent<GUITexture>();

        time = freezeTime;

	}

    private void Start()
    {
        timerUI.Init(1f);
        timerUI.moveTime = 0f;

        UpdateText();
    }

    public void AddBomb()
    {
        bombCount++;
        UpdateText();
    }

    private void UpdateText()
    {
        bombText.text = "Bomb:" + bombCount.ToString() + "\n" + "Type:" + PowerUpManager.BombType.ToString() + "\n" + "Power:" + PowerUpManager.GetBombDamage + "\n" + "Range:" + ((int)PowerUpManager.GetBombRadius).ToString();
    }

    void Update ()
	{
        time += Time.deltaTime;


		// If the bomb laying button is pressed, the bomb hasn't been laid and there's a bomb to lay...
		if(Input.GetButtonDown("Fire2") && bombCount > 0 && time >= freezeTime)
		{
			// Decrement the number of bombs.
			bombCount--;

            UpdateText();

			// Set bombLaid to true.
			time = 0;

			// Play the bomb laying sound.
			AudioSource.PlayClipAtPoint(bombsAway,transform.position);

			// Instantiate the bomb prefab.
			Instantiate(bomb, transform.position, transform.rotation);
		}

		// The bomb heads up display should be enabled if the player has bombs, other it should be disabled.
		bombHUD.enabled = true;

        timerUI.SetValue(bombCount == 0 ? 0f : (time < freezeTime ? time / freezeTime : 1f));
    }
}
