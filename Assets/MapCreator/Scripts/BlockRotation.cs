using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlockRotation : MonoBehaviour {
	public float x = 0;
	public float y = 0;
	public float z = 0;
	public Slider xS, yS, zS;
	public Text xT, yT, zT;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		x = xS.value;
		y = yS.value;
		z = zS.value;
		xT.text = "" + x;
		yT.text = "" + y;
		zT.text = "" + z;
	}
}
