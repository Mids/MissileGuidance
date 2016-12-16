using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour
{
	public void OnTriggerExit(Collider other)
	{
		Destroy(other.gameObject);
	}
}
