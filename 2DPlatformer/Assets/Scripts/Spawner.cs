using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float spawnTimeDefault = 5f;		// The amount of time between each spawn.
	public float spawnDelayDefault = 3f;		// The amount of time before spawning starts.
	public GameObject[] enemies;		// Array of enemy prefabs.
    private float timer;
    private float spawnTime;
    private float spawnDelay;

	void Start ()
	{
        // Start calling the Spawn function repeatedly after a delay .
        // InvokeRepeating("Spawn", spawnDelay, spawnTime);

        TimerInit();
        spawnTime = spawnTimeDefault;
        spawnDelay = spawnDelayDefault;
	}

    private void Update()
    {
        if (Score.GameOver) return;
        timer -= Time.deltaTime;

        if(timer <= 0.0f)
        {
            Spawn();
        }
    }

    private static float Ratio()
    {
        float val = (1.0f - (float)Score.score / 50000f);
        return (val < 0.0f) ? 0.0f : val * val;
    }

    private void TimerInit()
    {
        timer = Random.Range(spawnTime, spawnTime + spawnDelay);
    }

    void Spawn ()
	{
		// Instantiate a random enemy.
		int enemyIndex = Random.Range(0, enemies.Length);
		GameObject enemy = Instantiate(enemies[enemyIndex], transform.position, transform.rotation);
        enemy.transform.localScale = this.transform.localScale;

		// Play the spawning effect from all of the particle systems.
		foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
		{
			p.Play();
		}

        spawnTime = spawnTimeDefault * Ratio();
        //spawn速度上限を1flameに抑制
        if (spawnTime < 1.0f / 60f) spawnTime = 1.0f / 60f;

        spawnDelay = spawnDelayDefault * Ratio();

        TimerInit();
    }
}
