using UnityEngine;
using System.Collections;

public class GuidedMover : MonoBehaviour
{
	private Rigidbody _rigidbody;

	public float Speed;
	
	// Use this for initialization
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void Update()
	{
		transform.LookAt(transform.position + GetGuidedDirection());
		//_rigidbody.velocity = transform.forward * Speed;

		transform.position += transform.forward * Speed * Time.deltaTime;


	}

	// Only for debug line.
	float getBezierLength(GameObject target)
	{
		float totalLength = 0;
		Vector3 last = transform.position;

		Vector3[] points = {
			transform.position,
			transform.position + _rigidbody.velocity,
			target.transform.position,
			target.transform.position + target.GetComponent<Rigidbody>().velocity
		};

		Debug.DrawLine(points[0], points[1], Color.green);
		Debug.DrawLine(points[2], points[3], Color.green);


		for (int i=1;i<=1000;i++)
		{
			float t = i / 1000f;
			float u = 1f - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;

			Vector3 p = uuu * points[0]; //first term  
			p += 3 * uu * t * points[1]; //second term  
			p += 3 * u * tt * points[2]; //third term  
			p += ttt * points[3]; //fourth term  

			totalLength += (p - last).magnitude;
			Debug.DrawLine(last, p, Color.magenta);
			last = p;
		}

		return totalLength;
	}

	Vector3 GetBezier(GameObject target, float t)
	{
		float u = 1f - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;

		Vector3 p = uuu * transform.position; //first term  
		p += 3 * uu * t * (transform.position + _rigidbody.velocity); //second term  
		p += 3 * u * tt * target.transform.position; //third term  
		p += ttt * (target.transform.position + target.GetComponent<Rigidbody>().velocity); //fourth term  

		return p;
	}

	Vector3 GetGuidedDirection()
	{
		Vector3 guidedDirection = transform.forward;

		GameObject target = FindTarget();
		if(target == null)
		{
			return guidedDirection;
		}

		Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
		if(targetRigidbody == null)
		{
			Debug.LogWarning("The target doesn't have rigidbody!");
			return guidedDirection;
		}

		Vector3 targetDirection = target.transform.position - transform.position;
		float estimatedTime = targetDirection.magnitude / _rigidbody.velocity.magnitude;

		float bezierLength = getBezierLength(target);
		float ratio = Speed * Time.deltaTime / bezierLength;
		Vector3 bezierPoint = GetBezier(target, ratio);		

		guidedDirection = bezierPoint - transform.position;

		return guidedDirection;
	}

	GameObject FindTarget()
	{
		GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
		GameObject target = null;

		if (asteroids.Length == 0)
		{
			return target;
		}

		// Get closest asteroid
		float minDistance = float.MaxValue;
		Vector3 minVector = Vector3.zero;

		foreach (GameObject asteroid in asteroids)
		{
			Vector3 location = asteroid.transform.position;
			Vector3 myLocation = transform.position;

			Vector3 distance = location - myLocation;
			if(distance.magnitude < minDistance)
			{
				minDistance = distance.magnitude;
				minVector = distance;
				target = asteroid;
			}
		}
		
		return target;
	}
}
