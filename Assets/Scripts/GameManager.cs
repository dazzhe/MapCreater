using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	Vector3 spawnPos = new Vector3 (0,20,0);


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}



	public void Spawn() {
		GameObject player
			= PhotonNetwork.Instantiate("player", spawnPos,
			                            Quaternion.identity, 0) as GameObject;
		GameObject.Destroy (player.transform.FindChild ("Velox").gameObject);
		player.transform.FindChild ("FirstPersonCharacter").gameObject.SetActive (true);
		player.GetComponent<EditMap> ().enabled = true;
		player.GetComponent<Movement> ().enabled = true;
	}
}