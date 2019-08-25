using UnityEngine;
using System.Collections;

public class Synchronizer : Photon.MonoBehaviour, IPunObservable {
	private Vector3 correctPlayerPos = Vector3.zero;            // We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity;  // We lerp towards this
	// Update is called once per frame
	void Update() {
		// photonViewが自分自身ではない場合、位置と回転を反映.
		if (!photonView.isMine) {
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		}
	}
	
	/**
     * プレイヤー同士の位置/回転情報の同期をとる.
     * 自分のキャラクタの位置と回転を送信、自分以外のキャラクタの位置と回転を受信.
     */
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		} else {
			// Network player, receive data
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}
}