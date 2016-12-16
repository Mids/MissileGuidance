using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	private Rigidbody _rigidbody;

	public float Speed;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.velocity = transform.forward * Speed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
