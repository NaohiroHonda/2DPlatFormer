﻿using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
	public GameObject[] pickups;				// Array of pickup prefabs with the bomb pickup first and health second.
	public float pickupDeliveryTime = 2f;		// Delay on delivery.
	public float dropRangeLeft;					// Smallest value of x in world coordinates the delivery can happen at.
	public float dropRangeRight;				// Largest value of x in world coordinates the delivery can happen at.
	public float highHealthThreshold = 75f;		// The health of the player, above which only bomb crates will be delivered.
	public float lowHealthThreshold = 25f;		// The health of the player, below which only health crates will be delivered.

    public float dropRangeTop;
    public float dropRangeBottom;

    private float timer;

	private PlayerHealth playerHealth;			// Reference to the PlayerHealth script.


	void Awake ()
	{
		// Setting up the reference.
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}


	void Start ()
	{
		// Start the first delivery.
		// StartCoroutine(DeliverPickup());
        timer = pickupDeliveryTime;
	}

    public void Update()
    {
        if (Score.GameOver) return;

        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            Spawn();
            timer = pickupDeliveryTime;
        }
    }


    public IEnumerator DeliverPickup()
	{
		// Wait for the delivery delay.
		yield return new WaitForSeconds(pickupDeliveryTime);

        Spawn();
	}

    private void Spawn()
    {
        // Create a random x coordinate for the delivery in the drop range.

        float dropPosX = Random.Range(dropRangeLeft, dropRangeRight);
        float dropPosY = Random.Range(dropRangeBottom, dropRangeTop);

        // Create a position with the random x coordinate.
        Vector3 dropPos = new Vector3(dropPosX, dropPosY, 1f);

        //// If the player's health is above the high threshold...
        //if(playerHealth.health >= highHealthThreshold)
        //	// ... instantiate a bomb pickup at the drop position.
        //	Instantiate(pickups[0], dropPos, Quaternion.identity);
        //// Otherwise if the player's health is below the low threshold...
        //else if(playerHealth.health <= lowHealthThreshold)
        //	// ... instantiate a health pickup at the drop position.
        //	Instantiate(pickups[1], dropPos, Quaternion.identity);
        if (false)
        {
            //本来はhealthの条件判断ifに対して下のelse
        }
        // Otherwise...
        else
        {
            // ... instantiate a random pickup at the drop position.
            int pickupIndex = Random.Range(0, pickups.Length);
            Instantiate(pickups[pickupIndex], dropPos, Quaternion.identity);
        }
    }
}
