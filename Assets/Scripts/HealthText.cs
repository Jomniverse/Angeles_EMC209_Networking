using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthText : MonoBehaviour
{
	public Vector3 Offset = new Vector3 (0,2,0);

	void Start()
	{
		transform.localPosition += Offset;
		
	}
}