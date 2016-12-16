using UnityEngine;
using System.Collections;

public class GameCtrl : MonoBehaviour {
	public GameObject Hazard;
	public Vector3 SpawnValue;
	public int HazardCount;
	public float spawnWait;

	IEnumerator SpawnWaves()
	{
		while(true)
		{
			yield return new WaitForSeconds(spawnWait);

			Vector3 spawnPosition = new Vector3(Random.Range(-SpawnValue.x, SpawnValue.x), SpawnValue.y, SpawnValue.z);

			Instantiate(Hazard, spawnPosition, Quaternion.identity);
		}
	}


	// Use this for initialization
	void Start () {
		StartCoroutine(SpawnWaves());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
