using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	public Button[] menuItems;
	public Object prefab;

	private int selectedIndex = 0;

	private const string typeName = "TotalPwnageGameName";
	private const string gameName = "TotalPwnageRoom";

	private HostData[] hostList;

	private void StartServer()
	{
		//MasterServer.ipAddress = "127.0.0.1";
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		menuItems[selectedIndex].interactable = false;
		selectNext();
	}

	void OnServerInitialized()
	{
		foreach(Button btn in menuItems)
		{
			if(btn.name.ToLower().Contains("start"))
			{
				btn.interactable = false;
			}
		}
		//this.GetComponent<NetworkView>().viewID = Network.AllocateViewID();
		GameObject obj = (GameObject)Network.Instantiate(prefab, this.transform.position, Quaternion.identity, 0);
		obj.transform.parent = this.transform;
		print("Server Initialized");
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
		print ("searching for servers");
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if(msEvent == MasterServerEvent.HostListReceived)
		{
			hostList = MasterServer.PollHostList();
			print (hostList);
			if(hostList.Length > 0)
				menuItems[2].interactable = true;
		}
	}
	void JoinServer(HostData hostData)
	{
		print ("connecting to: " + hostData.ip);
		Network.Connect(hostData);
	}

	void Disconnect()
	{
		Network.Disconnect();
	}

	void OnConnectedToServer()
	{
		GameObject obj = (GameObject)Network.Instantiate(prefab, this.transform.position, Quaternion.identity, 0);
		obj.transform.parent = this.transform;
		print ("connected");
	}

	void OnDisconnectedFromServer()
	{
		print ("disconnected");
		foreach(GameObject obj in FindObjectsOfType<GameObject>())
		{
			print (obj.name);
			if(obj.name.ToLower().Contains("player") && obj.transform.parent == this.transform)
		   	{
				print ("destroying: " + obj.name);
				DestroyImmediate(obj);
			}
		}
	}
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if(stream.isWriting)
		{
			syncPosition = this.transform.position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			this.transform.position = syncPosition;
		}
	}
	// Use this for initialization
	void Start () {
		menuItems[selectedIndex].Select();
		menuItems[2].interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
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
			if(menuItems[selectedIndex].interactable)
				executeAction();
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
			JoinServer(hostList[0]);
		}
		else if(menuItems[selectedIndex].name.ToLower().Contains("disconnect"))
		{
			Disconnect();
		}
	}
}
