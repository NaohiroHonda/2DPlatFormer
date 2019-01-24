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

        timer = spawnTimeDefault;
        spawnTime = spawnTimeDefault;
        spawnDelay = spawnDelayDefault;
	}

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    private void TimerInit()
    {
        timer = Random.Range(spawnTimeDefault, spawnTimeDefault + spawnDelayDefault);
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
	}
}
