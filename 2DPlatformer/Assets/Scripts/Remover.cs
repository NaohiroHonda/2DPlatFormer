using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Assets.Adds;

public class Remover : MonoBehaviour
{
	public GameObject splash;
    public Score score;

    private void Start()
    {
        score = GameObject.Find("Score").GetComponent<Score>();
    }

    void OnTriggerEnter2D(Collider2D col)
	{
        if (col.gameObject.tag == "Trigger") return;
		// If the player hits the trigger...
		if(col.gameObject.tag == "Player")
		{
			// .. stop the camera tracking the player
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;

			// .. stop the Health Bar following the player
			if(GameObject.FindGameObjectWithTag("HealthBar").activeSelf)
			{
				GameObject.FindGameObjectWithTag("HealthBar").SetActive(false);
			}

			// ... instantiate the splash where the player falls in.
			Instantiate(splash, col.transform.position, transform.rotation);
			// ... destroy the player.
			Destroy (col.gameObject);
            Score.GameOver = true;
			// ... reload the level.
			StartCoroutine("ReloadGame");
		}
		else
		{
            if(col.gameObject.tag == "Enemy")
            {
                //生きたエネミーの選別
                Enemy enemy = col.GetComponent<Enemy>();
                if(!enemy.Dead) Score.life--;
            }

			// ... instantiate the splash where the enemy falls in.
			Instantiate(splash, col.transform.position, transform.rotation);

			// Destroy the enemy.
			Destroy (col.gameObject);	
		}
	}

	IEnumerator ReloadGame()
	{			
		// ... pause briefly
		yield return new WaitForSeconds(2);
		// ... and then reload the level.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        score.Init();
        PowerUpManager.Init();
	}
}
