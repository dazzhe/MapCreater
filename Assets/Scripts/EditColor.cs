using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditColor : MonoBehaviour {
	public Slider red, green, blue;
	public Image im;
	public int color = 1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
		Color c;
		c = new Color32 ((byte)red.value, (byte)green.value, (byte)blue.value, 255);
		im.color = c;
	}

	public void Preset1 ()
	{
		color = 1;
		red.value = 185;
		green.value = 181;
		blue.value = 162;
	}
	public void Preset2 ()
	{
		color = 2;
		red.value = 236;
		green.value = 166;
		blue.value = 131;
	}
	public void Preset3 ()
	{
		color = 3;
		red.value = 200;
		green.value = 200;
		blue.value = 200;
	}
	public void Preset4 ()
	{
		color = 4;
		red.value = 140;
		green.value = 140;
		blue.value = 200;
	}
	public void Preset5 ()
	{
		color = 5;
		red.value = 200;
		green.value = 123;
		blue.value = 0;
	}
	public void Preset6 ()
	{
		color = 6;
		red.value = 80;
		green.value = 200;
		blue.value = 80;
	}
	public void Preset7 ()
	{
		color = 7;
		red.value = 225;
		green.value = 40;
		blue.value = 40;
	}
	public void Preset8 ()
	{
		color = 8;
		red.value = 80;
		green.value = 80;
		blue.value = 100;
	}
	public void Preset9 ()
	{
		color = 9;
		red.value = 118;
		green.value = 218;
		blue.value = 86;
	}
	public void Preset10 ()
	{
		color = 10;
		red.value = 80;
		green.value = 200;
		blue.value = 195;
	}
}
