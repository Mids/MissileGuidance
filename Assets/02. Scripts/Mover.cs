using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	protected Rigidbody _rigidbody;
	protected GameObject _target;

	public float Speed;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.velocity = transform.forward * Speed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected GameObject FindTarget()
	{
		GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
		GameObject target = null;

		if (asteroids.Length == 0)
		{
			return target;
		}

		// Get closest asteroid
		float minDistance = float.MaxValue;

		foreach (GameObject asteroid in asteroids)
		{
			Vector3 location = asteroid.transform.position;
			Vector3 myLocation = transform.position;

			Vector3 distance = location - myLocation;
			if (distance.magnitude < minDistance)
			{
				minDistance = distance.magnitude;
				target = asteroid;
			}
		}

		return target;
	}
}
