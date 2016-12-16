using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float XMin, XMax, ZMin, ZMax;
}

public class PlayerCtrl : MonoBehaviour
{
	private Rigidbody _rigidbody;

	public float Speed;
	public float Tilt;
	public Boundary BoundaryRect;

	public GameObject Shot;
	public Transform ShotSpawn;
	public float FireRate = 0.5f;
	private float _nextFire = 0;

	public void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void Update()
	{
		if (Input.GetButton("Fire1") && Time.time > _nextFire)
		{
			_nextFire = Time.time + FireRate;
			Instantiate(Shot, ShotSpawn.position, ShotSpawn.rotation);
		}
	}

	public void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

		_rigidbody.velocity = movement * Speed;

		_rigidbody.position = new Vector3(
			Mathf.Clamp(_rigidbody.position.x, BoundaryRect.XMin, BoundaryRect.XMax),
			0,
			Mathf.Clamp(_rigidbody.position.z, BoundaryRect.ZMin, BoundaryRect.ZMax)
			);

		_rigidbody.rotation = Quaternion.Euler(0, 0, _rigidbody.velocity.x * -Tilt);
	}
}
