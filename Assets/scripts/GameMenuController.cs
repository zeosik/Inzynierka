using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour {

	public string gameName;
	public string[] menuItems;
	//public GameObject gameMenu;
	public GameObject text;
	public float distanceToFloor;
	public Font font;
	public Material selectedFontShaderMaterial;
	public Material unselectedFontShaderMaterial;
	//public Slider loadingBar;
	//public Shader fontShader;
	GameObject[] textItems;
	TextMesh[] meshes;
	//MeshRenderer[] meshRenderers;
	int selectedIndex = 0;

	private AsyncOperation async;

	// Use this for initialization
	void Start () {
		//this.transform.
		meshes = new TextMesh[menuItems.Length];
		textItems = new GameObject[menuItems.Length];
		//float distanceToFloor = 0.5f;
		float offset = this.transform.position.y - this.transform.localScale.y / 2f + menuItems.Length * 1.5f;
		this.transform.localScale = new Vector3(this.transform.localScale.x, menuItems.Length * 1.5f + 2f, this.transform.localScale.z);
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
		
		//Vector3 position = this.transform.position;
		Quaternion q = this.transform.rotation; //new Quaternion(0f, 180f, 0f, 1f);
		for (int i = 0; i < menuItems.Length; ++i) {
			Vector3 v = new Vector3 (this.transform.position.x , offset, this.transform.position.z);

			textItems[i] = (GameObject) Instantiate (text, v, q);

			textItems[i].transform.LookAt(GameObject.FindGameObjectWithTag("camera").transform.position);

			textItems[i].renderer.material = unselectedFontShaderMaterial;

			DestroyImmediate(textItems[i].GetComponent<TextMesh>());

			//print (text.GetComponent<TextMesh>().text);
			meshes[i] = textItems[i].AddComponent<TextMesh>();

			TextMesh tmp = text.GetComponent<TextMesh>();
			meshes[i].alignment = tmp.alignment;
			meshes[i].anchor = tmp.anchor;
			meshes[i].font = font;//tmp.font;
			//meshes[i].fontSize = tmp.fontSize;
			//meshes[i].fontStyle = tmp.fontStyle;
			meshes[i].characterSize = tmp.characterSize;
			meshes[i].text = menuItems[i];

			meshes[i].color = Color.blue;
			meshes[i].offsetZ = -0.55f;
			offset -= 1.5f;
		}
		meshes [selectedIndex].color = Color.green;
		textItems[selectedIndex].renderer.material = selectedFontShaderMaterial;
		//meshes [0].color = Color.green;
		//meshes [1].color = Color.white;
		//meshes [2].color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 camera = GameObject.FindGameObjectWithTag("camera").transform.position;
		camera.y = this.transform.position.y;
		this.transform.LookAt(camera);
		this.transform.Rotate(0f, 180f, 0f);
		this.transform.rotation = new Quaternion(0f, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
		for (int i = 0; i < textItems.Length; ++i) {
			textItems[i].transform.rotation = this.transform.rotation;
		}
	}

	private void menuControl() {
		meshes [selectedIndex].color = Color.blue;
		textItems[selectedIndex].renderer.material = unselectedFontShaderMaterial;
		if (Input.GetKeyDown (KeyCode.Return)) {
			executeAction ();
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			selectedIndex = (selectedIndex == (menuItems.Length - 1) ? selectedIndex = 0 : ++selectedIndex);
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			selectedIndex = (selectedIndex == 0 ? menuItems.Length - 1 : --selectedIndex);
		} else if (Input.GetKeyDown (KeyCode.V)) {
			print ("selected: " + selectedIndex);
		}
		meshes [selectedIndex].color = Color.green;
		textItems[selectedIndex].renderer.material = selectedFontShaderMaterial;
	}

	private void executeAction() {
		if (menuItems [selectedIndex].ToLower ().Equals ("start")) {
			GlobalSettings.setMenuUserPosition();
			StartCoroutine(LoadLevel());
			//Application.LoadLevel (gameName);
		} else if (menuItems [selectedIndex].ToLower ().Equals ("exit")) {
			Application.Quit();
		}
	}
	IEnumerator LoadLevel()
	{
		async = Application.LoadLevelAsync(gameName);
		while(!async.isDone)
		{
			//loadingBar.value = async.progress;
			yield return null;
		}
	}
}
