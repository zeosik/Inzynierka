using UnityEngine;
using System.Collections;

public class GameMenuController : MonoBehaviour {

	public string gameName;
	public string[] menuItems;
	//public GameObject gameMenu;
	public GameObject text;
	GameObject[] textItems;
	TextMesh[] meshes;
	int selectedIndex = 0;


	// Use this for initialization
	void Start () {
		//this.transform.
		meshes = new TextMesh[menuItems.Length];
		textItems = new GameObject[menuItems.Length];
		float distanceToFloor = 0.5f;
		float offset = distanceToFloor + menuItems.Length * 1.5f + 0.5f;
		this.transform.localScale = new Vector3(this.transform.localScale.x, menuItems.Length * 1.5f + 2f, this.transform.localScale.z);
		this.transform.position = new Vector3(this.transform.position.x, this.transform.localScale.y / 2f + distanceToFloor, this.transform.position.z);
		
		//Vector3 position = this.transform.position;
		Quaternion q = this.transform.rotation; //new Quaternion(0f, 180f, 0f, 1f);
		for (int i = 0; i < menuItems.Length; ++i) {
			Vector3 v = new Vector3 (this.transform.position.x , offset, this.transform.position.z);

			//print (text.GetComponent<TextMesh>().text);
			textItems[i] = (GameObject) Instantiate (text, v, q);

			textItems[i].transform.LookAt(GameObject.FindGameObjectWithTag("camera").transform.position);

			DestroyImmediate(textItems[i].GetComponent<TextMesh>());

			//print (text.GetComponent<TextMesh>().text);
			meshes[i] = textItems[i].AddComponent<TextMesh>();

			TextMesh tmp = text.GetComponent<TextMesh>();
			meshes[i].alignment = tmp.alignment;
			meshes[i].anchor = tmp.anchor;
			meshes[i].font = tmp.font;
			meshes[i].fontSize = tmp.fontSize;
			meshes[i].fontStyle = tmp.fontStyle;
			meshes[i].characterSize = tmp.characterSize;
			meshes[i].text = menuItems[i];

			meshes[i].color = Color.gray;

			offset -= 1.5f;
		}
		meshes [selectedIndex].color = Color.white;
		//meshes [0].color = Color.green;
		//meshes [1].color = Color.white;
		//meshes [2].color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt(GameObject.FindGameObjectWithTag ("camera").transform.position);
		this.transform.Rotate(0f, 180f, 0f);
		for (int i = 0; i < textItems.Length; ++i) {
			textItems[i].transform.rotation = this.transform.rotation;
		}
	}

	private void menuControl() {
		meshes [selectedIndex].color = Color.gray;
		if (Input.GetKeyDown (KeyCode.Return)) {
			executeAction ();
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			selectedIndex = (selectedIndex == (menuItems.Length - 1) ? selectedIndex = 0 : ++selectedIndex);
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			selectedIndex = (selectedIndex == 0 ? menuItems.Length - 1 : --selectedIndex);
		} else if (Input.GetKeyDown (KeyCode.V)) {
			print ("selected: " + selectedIndex);
		}
		meshes [selectedIndex].color = Color.white;
	}

	private void executeAction() {
		print (menuItems [selectedIndex]);
		if (menuItems [selectedIndex].ToLower ().Equals ("start")) {
			Application.LoadLevel (gameName);
		} else if (menuItems [selectedIndex].ToLower ().Equals ("exit")) {
			Application.Quit();
		}
	}
}
