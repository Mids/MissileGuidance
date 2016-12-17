using UnityEngine;
using System.Collections;

public class BezierMover : Mover
{
	private Vector3 _velocityForBezier;
	
	// Use this for initialization
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_velocityForBezier = transform.forward * Speed;
	}

	public void Update()
	{
		if(_target == null)
		{
			_target = FindTarget();
			transform.position += _velocityForBezier * Time.deltaTime;
		}
		else
		{
			_velocityForBezier = GetGuidedDirection(_target);
			_rigidbody.rotation = Quaternion.LookRotation(_velocityForBezier);
			_rigidbody.velocity = transform.forward * Speed;
		}
	}
	
	float getBezierLength(GameObject target)
	{
		float totalLength = 0;
		Vector3 last = transform.position;

		Vector3[] points = {
			transform.position,
			transform.position + _velocityForBezier * 20,
			target.transform.position,
			target.transform.position + target.GetComponent<Rigidbody>().velocity
		};
		
		Debug.DrawLine(points[0], points[1], Color.green);
		Debug.DrawLine(points[2], points[1], Color.green);

		for (int i=1;i<=1000;i++)
		{
			float t = i / 1000f;
			float u = 1f - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;

			Vector3 p = uu * points[0]; //first term  
			p += 2 * u * t * points[1]; //second term  
			p += tt * points[2]; //fourth term  

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

		Vector3 p = uu * transform.position; //first term  
		p += 2 * u * t * (transform.position + _velocityForBezier*20); //second term  
		p += tt * (target.transform.position); //fourth term  

		return p;
	}

	Vector3 GetGuidedDirection(GameObject target)
	{
		Vector3 guidedDirection = transform.forward;
		
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
		float estimatedTime = targetDirection.magnitude / _velocityForBezier.magnitude;

		float bezierLength = getBezierLength(target);
		Vector3 bezierPoint = GetBezier(target, Time.deltaTime);			

		guidedDirection = bezierPoint - transform.position;

		return guidedDirection;
	}
}
