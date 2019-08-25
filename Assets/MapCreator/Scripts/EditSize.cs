using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditSize : MonoBehaviour {
	public Slider wSlider, hSlider, dSlider;
	public Text wText, hText, dText;
	public int w, h, d;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		wText.text = "" + wSlider.value;
		hText.text = "" + hSlider.value;
		dText.text = "" + dSlider.value;
		w = Mathf.FloorToInt(wSlider.value);
		h = Mathf.FloorToInt(hSlider.value);
		d = Mathf.FloorToInt(dSlider.value);
	}
}
