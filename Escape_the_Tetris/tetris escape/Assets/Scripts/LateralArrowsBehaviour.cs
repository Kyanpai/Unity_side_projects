using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralArrowsBehaviour : MonoBehaviour {

	public bool left;
	private Vector3 origin;
	public float speed;
	public float offsetX;
	private bool originLeft;

	// Use this for initialization
	void Start () {
		origin = transform.position;
		originLeft = left;
	}
	
	// Update is called once per frame
	void Update () {
		if ((originLeft && left && Mathf.Abs (transform.position.x - origin.x) > offsetX) || originLeft && !left && transform.position.x - origin.x > 0)
			left = !left;
		else if ((!originLeft && !left && Mathf.Abs (transform.position.x - origin.x) > offsetX) || !originLeft && left && transform.position.x - origin.x < 0)
			left = !left;
//		if ((transform.position.x - origin.x > offsetX) || (transform.position.x - origin.x < 0 && left))
//			left = !left;
		if (left) {
			transform.Translate (-transform.right * Time.deltaTime * speed);
		} else {
			transform.Translate (transform.right * Time.deltaTime * speed);
		}
	}
}
