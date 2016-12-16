using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {
	private Rigidbody _rigidbody;

	public float tumble;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody>();

		_rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
