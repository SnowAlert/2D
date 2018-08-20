using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Shield : NetworkBehaviour
{
	
	public GameObject sheild;
	public GameObject sheildBlock;
	public AnimationClip[] anim;
	//public GameObject movementSpeed;
	private float movementSpeedBufer;
	public GameObject player;

	//[SyncVar (hook = "Walk2")]
	//[SyncVar]
	public int state = 0;
	//[SyncVar(hook="FixedUpdate")]
	public int state2 = 0;

	void Start ()
	{
		//movementSpeed=GameObject.Find("Player(Clone)");
		movementSpeedBufer = player.GetComponent<PlayerController> ().movementSpeed;
	}

	void Update ()
	{
		if (!player.GetComponent<NetworkIdentity>().isLocalPlayer)
			return;
		
		state = 0;
		if ((Input.GetKey (KeyCode.W)) || (Input.GetKey (KeyCode.S)) || (Input.GetKey (KeyCode.A)) || (Input.GetKey (KeyCode.D))||(state==1)) {
			state = 1;
			sheild.GetComponent<Animation> ().Play (anim [1].name);
			sheildBlock.GetComponent<Animation> ().Play (anim [2].name);
			Walk (state);
		} else {
			sheild.GetComponent<Animation> ().Play (anim [0].name);
			sheildBlock.GetComponent<Animation> ().Play (anim [0].name);
			Walk (state);
		}
		if (Input.GetKeyDown (KeyCode.Space)||(state==2)) {
			state = 2;
			sheild.GetComponent<Renderer> ().enabled = false;
			sheildBlock.GetComponent<Renderer> ().enabled = true;
			Walk (state);
		}
		if (Input.GetKeyUp (KeyCode.Space)||(state==3)) {
			player.GetComponent<PlayerController> ().movementSpeed = movementSpeedBufer;

			sheild.GetComponent<Renderer> ().enabled = true;
			sheildBlock.GetComponent<Renderer> ().enabled = false;
			state = 3;
			Walk (state);
		}

		//if (isServer)
		//RpcWalk (state);
		//if(!isServer)
		//	TargetWalk2 (state);
		
			
	}


	[Command]
	void CmdWalk (int state){
		WalkClient (state);
	}
	[ClientRpc]
	void RpcWalk(int state){
		WalkClient (state);
	}

	void WalkClient (int state)
	{
		switch (state) {
		case 1:
			sheild.GetComponent<Animation> ().Play (anim [1].name);
			sheildBlock.GetComponent<Animation> ().Play (anim [2].name);
			break;

		case 2:
			sheild.GetComponent<Renderer> ().enabled = false;
			sheildBlock.GetComponent<Renderer> ().enabled = true;

			//movementSpeedBufer=movementSpeed.GetComponent<PlayerController>().movementSpeed;
			player.GetComponent<PlayerController> ().movementSpeed = movementSpeedBufer / 2;
			break;

		case 3:
			player.GetComponent<PlayerController> ().movementSpeed = movementSpeedBufer;

			sheild.GetComponent<Renderer> ().enabled = true;
			sheildBlock.GetComponent<Renderer> ().enabled = false;

			break;

		case 0:
			sheild.GetComponent<Animation> ().Play (anim [0].name);
			sheildBlock.GetComponent<Animation> ().Play (anim [0].name);
			break;
		}
	}


	void Walk (int state)
	{
		if (isLocalPlayer)
			CmdWalk (state);
		if(isServer)
			RpcWalk (state);
		WalkClient (state);
	}
}
