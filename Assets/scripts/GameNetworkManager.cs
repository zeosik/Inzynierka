using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameNetworkManager : MonoBehaviour {

	public Button[] menuItems;

	private int selectedIndex = 0;

	private const string typeName = "TotalPwnageGameName";
	private const string gameName = "TotalPwnageRoom";
	
	private HostData[] hostList;

	static GameNetworkManager menu;
	private bool networkMenuActive = false;
	private bool justEntered = false;

	private int discButtIndex = 0;
	
	private void StartServer()
	{
		GameController.newPopupInfo ("starting server...");
		//MasterServer.ipAddress = "127.0.0.1";
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		menuItems[selectedIndex].interactable = false;
		selectNext();
	}
	
	void OnServerInitialized()
	{
		menu.menuItems[discButtIndex].interactable = true;
		foreach(Button btn in menuItems)
		{
			if(btn.name.ToLower().Contains("start"))
			{
				btn.interactable = false;
			}
		}
		//this.GetComponent<NetworkView>().viewID = Network.AllocateViewID();
		//GameObject obj = (GameObject)Network.Instantiate(prefab, this.transform.position, Quaternion.identity, 0);
		//obj.transform.parent = this.transform;
		print("Server Initialized");
		GameController.newPopupInfo ("server started");
	}
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
		print ("searching for servers");
		GameController.newPopupInfo ("searching...");
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if(msEvent == MasterServerEvent.HostListReceived)
		{
			hostList = MasterServer.PollHostList();
			print (hostList.Length);
			if(hostList.Length > 0)
			{
				menuItems[2].interactable = true;
				GameController.newPopupInfo ("found");
			}
			else
			{
				menuItems[2].interactable = false;
				GameController.newPopupInfo ("not found");
			}

		}
	}
	void JoinServer(HostData hostData)
	{
		print ("connecting to: " + hostData.ip);
		GameController.newPopupInfo ("connecting...");
		Network.Connect(hostData);
	}
	
	void Disconnect()
	{
		GameController.newPopupInfo ("disconnecting...");
		Network.Disconnect();
		hostList = null;
		menuItems[2].interactable = false;
		menuItems[0].interactable = true;
	}
	
	void OnConnectedToServer()
	{
		menu.menuItems[discButtIndex].interactable = true;
		menuItems[2].interactable = false;
		if(Network.isClient)
		{
			GameObject.Find("OVRCameraController").transform.parent = GameObject.Find("Player2").transform;
			GameObject.Find("Menus").transform.localPosition = new Vector3(-2.5f, 0f, 1.43f);
		}
		else
		{
			GameObject.Find("OVRCameraController").transform.parent = GameObject.Find("Player1").transform;
			GameObject.Find("Menus").transform.localPosition = new Vector3(-1.4f, 0f, 1.43f);
		}
		GameObject.Find("OVRCameraController").transform.localPosition = new Vector3(-0.212f, 0.1f, 0.1f);
		print ("connected");
		GameController.newPopupInfo ("connected");
	}

	void OnDisconnectedFromServer()
	{
		GameObject.Find("OVRCameraController").transform.parent = GameObject.Find("Player1").transform;
		GameObject.Find("Menus").transform.localPosition = new Vector3(-1.4f, 0f, 1.43f);
		GameObject.Find("OVRCameraController").transform.localPosition = new Vector3(-0.212f, 0.1f, 0.1f);
		GameObject.Find ("Player2").transform.localRotation = Quaternion.identity;
		GameController.newPopupInfo ("disconnected");
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncAngularVelocity = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		Vector3 syncPosition = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		Quaternion syncPlayerRotation = Quaternion.identity;
		if(stream.isWriting)
		{
			syncAngularVelocity = this.rigidbody.angularVelocity;
			syncVelocity = this.rigidbody.velocity;
			syncPosition = this.rigidbody.position;
			syncRotation = this.rigidbody.rotation;
			syncPlayerRotation = GameObject.Find ("Player1").transform.localRotation;

			stream.Serialize(ref syncAngularVelocity);
			stream.Serialize(ref syncVelocity);
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncRotation);
			stream.Serialize(ref syncPlayerRotation);
		}
		else
		{
			object[] args = new object[2];
			args[0] = "Player2";
			args[1] = GameObject.Find ("Player2").transform.localRotation;
			this.networkView.RPC("updatePlayerRotation", RPCMode.Others, args);

			stream.Serialize(ref syncAngularVelocity);
			stream.Serialize(ref syncVelocity);
				
			if(!this.rigidbody.isKinematic)
			{
				this.rigidbody.angularVelocity = syncAngularVelocity;
				this.rigidbody.velocity = syncVelocity;
			}

			stream.Serialize(ref syncPosition);
			this.rigidbody.position = syncPosition;

			stream.Serialize(ref syncRotation);
			this.rigidbody.rotation = syncRotation;

			stream.Serialize(ref syncPlayerRotation);
			GameObject.Find ("Player1").transform.localRotation = syncPlayerRotation;
		}
	}
	// Use this for initialization
	void Start () 
	{
		menu = gameObject.transform.GetComponent<GameNetworkManager>();
		menuItems[selectedIndex].Select();
		menuItems[2].interactable = false;
		if(Network.isClient)
		{
			GameObject.Find("OVRCameraController").transform.parent = GameObject.Find("Player2").transform;
			GameObject.Find("Menus").transform.localPosition = new Vector3(-2.5f, 0f, 1.43f);
			//GameObject.Find("Menus").transform.parent = GameObject.Find("Player2").transform;
		}
		else
		{
			GameObject.Find("OVRCameraController").transform.parent = GameObject.Find("Player1").transform;
			GameObject.Find("Menus").transform.localPosition = new Vector3(-1.4f, 0f, 1.43f);
			//GameObject.Find("Menus").transform.parent = GameObject.Find("Player1").transform;
		}

		//GameObject.Find("OVRCameraController").transform.localPosition = new Vector3(-1f, -0.6f,1f);
		GameObject.Find("OVRCameraController").transform.localPosition = new Vector3(-0.212f, 0.1f, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		if(networkMenuActive)
		{
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				selectNext();
				
			} 
			else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				selectPrev();
				
			} 
			else if(Input.GetKeyDown(KeyCode.Return))
			{
				if(justEntered)
				{
					justEntered = false;
					return;
				}
				if(menuItems[selectedIndex].interactable)
					executeAction();
			}
		}
	}

	void selectNext()
	{
		selectedIndex = (selectedIndex == (menuItems.Length - 1) ? selectedIndex = 0 : ++selectedIndex);
		if(menuItems[selectedIndex].interactable)
			menuItems[selectedIndex].Select();
		else
			selectNext();
	}
	
	void selectPrev()
	{
		selectedIndex = (selectedIndex == 0 ? menuItems.Length - 1 : --selectedIndex);
		if(menuItems[selectedIndex].interactable)
			menuItems[selectedIndex].Select();
		else
			selectPrev();
	}
	
	void executeAction()
	{
		if(menuItems[selectedIndex].name.ToLower().Contains("start"))
		{
			StartServer();
		}
		else if(menuItems[selectedIndex].name.ToLower().Contains("search"))
		{
			RefreshHostList();
		}
		else if(menuItems[selectedIndex].name.ToLower().Contains("join"))
		{
			if(hostList.Length > 0)
			{
				GameController.newPopupInfo ("joining...");
				JoinServer(hostList[0]);
			}

		}
		else if(menuItems[selectedIndex].name.ToLower().Contains("disconnect"))
		{
			Disconnect();
		}
	}
	public static void toggleNetworkMenu()
	{
		menu.networkMenuActive = !menu.networkMenuActive;
		if(menu.networkMenuActive)
		{

			menu.discButtIndex = 0;
			foreach(Button btn in menu.menuItems)
			{
				if(btn.name.ToLower().Contains("disconnect"))
				{
					break;
				}
				++menu.discButtIndex;
			}
			if(Network.isClient || Network.isServer)
				menu.menuItems[menu.discButtIndex].interactable = true;
			else
				menu.menuItems[menu.discButtIndex].interactable = false;

			menu.justEntered = true;
			menu.selectedIndex = 0;
			if(!menu.menuItems[menu.selectedIndex].interactable)
				menu.selectNext();
			else
				menu.menuItems[menu.selectedIndex].Select();
		}
		else
			menu.justEntered = false;
	}
	[RPC]
	public void togglePauseGame()
	{
		GameController.togglePauseGame();
	}
	[RPC]
	public void restartGame()
	{
		BobsleighController.restartGame();
	}
	[RPC]
	public void updatePlayerRotation(string playerName, Quaternion rotation )
	{
		GameObject.Find (playerName).transform.localRotation = rotation;
	}
}
