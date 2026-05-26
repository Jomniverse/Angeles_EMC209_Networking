using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageText : MonoBehaviour
{
	public float DestroyTime = 1f;
	public Vector3 Offset = new Vector3 (0,2,0);
	public Vector3 RandomAlignment = new Vector3(1,0,0);

	void Start()
	{
		Destroy(gameObject, DestroyTime);

		transform.localPosition += Offset;
		transform.localPosition += new Vector3(
			Random.Range(-RandomAlignment.x, RandomAlignment.x),
			Random.Range(-RandomAlignment.y, RandomAlignment.y),
			Random.Range(-RandomAlignment.z, RandomAlignment.z));
	}
}