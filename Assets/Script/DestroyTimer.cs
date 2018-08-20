using UnityEngine;
using System.Collections;


public class DestroyTimer : MonoBehaviour {
	public float Timer;

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject,Timer);
	}

}
