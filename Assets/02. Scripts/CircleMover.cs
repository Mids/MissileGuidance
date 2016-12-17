using UnityEngine;
using System.Collections;

public class CircleMover : Mover
{
	public float RotationRadius;

	private float _targetSpeed;
	private Vector3 _velocity;
	private Vector3 _circleDirection;
	private float _spinningTime;

	public void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void Update()
	{
		_velocity = transform.forward * Speed;
		_target = FindTarget();
		if (_target == null)
		{
			transform.position += _velocity * Time.deltaTime;
		}
		else
		{
			Vector3 targetVelocity = _target.GetComponent<Rigidbody>().velocity;
			_targetSpeed = targetVelocity.magnitude;
			Vector3 distance = _target.transform.position - transform.position;

			Vector3 targetPoint = _target.transform.position + (targetVelocity * distance.magnitude / _velocity.magnitude);
			float totalTime = CalculateTime(targetPoint, false);

			// 2nd
			for (int i = 0; i < 1; i++)
			{
				targetPoint = _target.transform.position + (targetVelocity * totalTime);
				totalTime = CalculateTime(targetPoint, false);
			}

			// 3rd
			targetPoint = _target.transform.position + (targetVelocity * totalTime);
			CalculateTime(targetPoint, true);


			Debug.DrawLine(_target.transform.position, targetPoint, Color.cyan);


			if (_spinningTime < 0.01f && _spinningTime > -0.01f)
			{
				transform.position += _velocity * Time.deltaTime;
			}
			else
			{
				float rotateAngle = Speed * Time.deltaTime / RotationRadius;
				if (_circleDirection.x < 0) rotateAngle = -rotateAngle;
				Quaternion rotation = Quaternion.Euler(0f, rotateAngle * 180 / Mathf.PI, 0f);

				Vector3 newPosition = _velocity * Time.deltaTime;
				newPosition = (rotation * newPosition);

				newPosition += transform.position;

				transform.LookAt(newPosition);
				transform.position = newPosition;
			}
		}
	}

	float CalculateTime(Vector3 target, bool isEnd)
	{
		Vector3 circleDirection = isLeft(target) ? Vector3.left : Vector3.right;
		Vector3 circleCenter = transform.TransformPoint(circleDirection * RotationRadius);
		if (isEnd) Debug.DrawLine(transform.position, circleCenter, Color.red);

		Vector3 circleToTarget = target - circleCenter;
		float cicleToTargetAngle = Mathf.Acos(RotationRadius / circleToTarget.magnitude) * circleDirection.x;

		Quaternion v3Rotation = Quaternion.Euler(0f, (-cicleToTargetAngle) * 180 / Mathf.PI, 0f);
		Vector3 v3RotatedDirection = (v3Rotation * circleToTarget).normalized * RotationRadius;
		if (isEnd)
		{
			Debug.DrawLine(circleCenter, circleCenter + v3RotatedDirection, Color.red);
			Debug.DrawLine(circleCenter, circleToTarget + circleCenter, Color.blue);
		}

		Vector3 straightStart = circleCenter + v3RotatedDirection;
		if (isEnd) Debug.DrawLine(straightStart, target, Color.green);
		Vector3 straightLine = target - straightStart;
		float straightTime = straightLine.magnitude / Speed;

		float spinningAngle = getAngle360(straightLine, circleDirection.x < 0);
		float spinningLength = (RotationRadius * spinningAngle) * (Mathf.PI / 180);
		float spinningTime = spinningLength / Speed;

		_spinningTime = spinningTime;
		_circleDirection = circleDirection;

		Debug.Log("straight : " + straightTime + ", spin : " + spinningTime);

		return straightTime + (spinningTime < 0 ? -spinningTime : spinningTime);
	}

	bool isLeft(Vector3 target)
	{
		return transform.InverseTransformPoint(target).x < 0;
	}

	float getAngle360(Vector3 to, bool isCircleOnLeft)
	{
		float angle = Vector3.Angle(_velocity, to);
		if (isCircleOnLeft)
		{
			if (isLeft(transform.position + to))
				return -angle;
			else
				return angle - 360;
		}
		else
		{
			if (isLeft(transform.position + to))
				return 360 - angle;
			else
				return angle;
		}
	}
}