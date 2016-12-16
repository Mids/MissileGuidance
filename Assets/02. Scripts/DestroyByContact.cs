using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
	public GameObject Explosion;
	public GameObject PlayerExplosion;

	public void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Boundary")
		{
			return;
		}
		Destroy(Instantiate(Explosion, transform.position, transform.rotation), 2.0f);
		if(other.tag == "Player")
		{
			Instantiate(PlayerExplosion, other.transform.position, other.transform.rotation);
		}
		Destroy(other.gameObject);
		Destroy(gameObject);
	}
}
