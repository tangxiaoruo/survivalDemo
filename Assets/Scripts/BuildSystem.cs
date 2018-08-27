using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour {

	private MeshFilter filter;

	public CameraFollow camFellow;
	public BackPagManager bagManeger;
	public UIController uiController;
	public float rotateSpeed=25.0f;

	//private bool isbuilding=false;
	private bool beginBuild = false;
	private bool canBuild = false;
	private Ray screenPointRay;
	private RaycastHit hit;
	private Transform hitObj;

	public GameObject[] buildModelList;
	public  List<ComponentInfo> buildingList = new List<ComponentInfo> ();
	[HideInInspector]
	public GameObject buildObj;
	private ComponentInfo objInfo;

	ComponentInfo comp1;
	ComponentInfo comp2;
	ComponentInfo comp3;

	ComponentInfo comp4;
	ComponentInfo comp5;
	ComponentInfo comp6;
	ComponentInfo comp7;
	ComponentInfo comp8;
	ComponentInfo comp9;
	ComponentInfo comp10;
	ComponentInfo comp11;
	ComponentInfo comp12;
	ComponentInfo comp13;
	ComponentInfo comp14;


	// Use this for initialization
	void Start () {
		comp1 = new ComponentInfo ("woodenHouse",new string[3]{"board","nail","rope"},new int[3]{80,60,15},null);
		comp2 = new ComponentInfo ("tent",new string[3]{"cloth","nail","rope"},new int[3]{30,10,15},null);
		comp3 = new ComponentInfo ("campFire",new string[2]{"rock","wood"},new int[2]{8,13},null);

		comp4 = new ComponentInfo ("fence1", new string[3]{ "board", "nail", "rope" }, new int[3]{ 10, 8, 5 }, null);
		comp5 = new ComponentInfo ("fence2", new string[3]{ "board", "nail", "rope" }, new int[3]{ 10, 8, 5 }, null);
		comp6 = new ComponentInfo ("fence3", new string[3]{ "board", "nail", "rope" }, new int[3]{ 10, 8, 5 }, null);
		comp7 = new ComponentInfo ("fence4", new string[3]{ "board", "nail", "rope" }, new int[3]{ 10, 8, 5 }, null);

		comp8 = new ComponentInfo ("gate1", new string[3]{ "board", "nail", "rope" }, new int[3]{ 16, 10, 8 }, null);
		comp9 = new ComponentInfo ("gate2", new string[3]{ "board", "nail", "rope" }, new int[3]{ 16, 10, 8 }, null);
		comp10 = new ComponentInfo ("gate3", new string[3]{ "board", "nail", "rope" }, new int[3]{ 16, 10, 8 }, null);
		comp11 = new ComponentInfo ("gate4", new string[3]{ "board", "nail", "rope" }, new int[3]{ 22, 15, 12 }, null);

		comp12 = new ComponentInfo ("wall1", new string[3]{ "board", "nail", "rope" }, new int[3]{ 22, 15, 12 }, null);
		comp13 = new ComponentInfo ("wall2", new string[3]{ "board", "nail", "rope" }, new int[3]{ 40, 28, 18 }, null);
		comp14 = new ComponentInfo ("gateBoard", new string[2]{ "board", "nail"}, new int[2]{ 2, 4}, null);




		buildingList.Add (comp1);
		buildingList.Add (comp2);
		buildingList.Add (comp3);

		buildingList.Add (comp4);
		buildingList.Add (comp5);
		buildingList.Add (comp6);
		buildingList.Add (comp7);
		buildingList.Add (comp8);
		buildingList.Add (comp9);
		buildingList.Add (comp10);
		buildingList.Add (comp11);
		buildingList.Add (comp12);
		buildingList.Add (comp13);
		buildingList.Add (comp14);

		uiController.InitBuildings ();
	}
	
	// Update is called once per frame
	void Update () {
		

		if (beginBuild)
			BuildObj ();

		if (Input.GetKey (KeyCode.Q) && beginBuild)
			buildObj.transform.Rotate (-Vector3.up*rotateSpeed*Time.deltaTime,Space.Self);
		if (Input.GetKey (KeyCode.E) && beginBuild)
			buildObj.transform.Rotate (Vector3.up*rotateSpeed*Time.deltaTime,Space.Self);


		if (canBuild) {
			//-----------UI show----------

			if (Input.GetKeyDown (KeyCode.Mouse0)) {

				Collider c = buildObj.GetComponent<Collider> ();
				if (c != null)
					c.enabled = true;
				foreach (Collider collider in buildObj.GetComponentsInChildren<Collider>()) {
					collider.enabled = true;
				}

				//isbuilding = false;
				beginBuild = false;
				canBuild = false;
				buildObj = null;
			

				for (int i = 0; i < objInfo.requiredGoods.Length; i++) {
					bagManeger.Reduce (objInfo.requiredGoods [i], objInfo.requiredGoodsNum [i]);
				}
			} 
		}

		if (canBuild && Input.GetKeyDown (KeyCode.Mouse1)) {
			Destroy (buildObj);
			buildObj = null;
			canBuild = false;
			beginBuild = false;
		
		}
	}




	void BuildObj(){
		screenPointRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (screenPointRay, out hit)) {
			buildObj.transform.position = hit.point;
			float angle = Vector3.Angle (buildObj.transform.up,hit.normal);
//			Debug.DrawRay (hit.point,hit.normal,Color.red);
//			Debug.DrawRay (hit.point,Vector3.up,Color.green);
//			Debug.DrawRay (hit.point,axis,Color.red);

			if (angle > 0.1f) {
				Vector3 axis = Vector3.Cross (buildObj.transform.up,hit.normal);
				buildObj.transform.Rotate (axis.normalized * angle, Space.World);
			}

			if (Vector3.Angle (hit.normal, Vector3.up) > 5.0f) {
				IFCanBuild (false);
			}else {
				if (buildObj.GetComponent<MeshFilter> () != null)
					filter = buildObj.GetComponent<MeshFilter> ();
				else
					filter = buildObj.GetComponentsInChildren<MeshFilter> () [0];

				Bounds bounds = filter.mesh.bounds;
				if (Physics.OverlapBox (hit.point + hit.normal * (bounds.extents.y + 0.015f), bounds.size*0.5f,buildObj.transform.rotation).Length !=0) {

					IFCanBuild (false);

				} else {
					IFCanBuild (true);
				}
			}
				
		}
	}


	void IFCanBuild(bool b){
		canBuild = b;

		foreach (MeshRenderer  renderer in buildObj.GetComponentsInChildren<MeshRenderer>()) {
		
			if (!canBuild)
				renderer.material.color = Color.red;
			else
				renderer.material.color = Color.white;
		}
	
	}

	public ComponentInfo GetComponentInfo(string name){
		foreach (ComponentInfo info in buildingList) {
			if (info.name == name)
				return info;
		}
		return null;
	}

	bool CheckResource(ComponentInfo info){
		bool canBuild = true;

		for (int i = 0; i < info.requiredGoods.Length; i++) {
			if (bagManeger.GetItemNum (info.requiredGoods [i]) < info.requiredGoodsNum [i]) {
				canBuild = false;
				return canBuild;
			}
		}
		return canBuild; 
	}

	IEnumerator ShowWarning(){
		bagManeger.warningLabel.GetComponent<UILabel>().text="材料不足 无法建造";
		//uiController.bg.GetComponent<UISprite> ().alpha = 0f;
		yield return new WaitForSeconds (2.0f);
		//uiController.bagPanel.GetComponent<UIPanel> ().alpha = 1.0f;
		bagManeger.warningLabel.GetComponent<UILabel>().text="背包已满";

	}

	void OnBuildClick(GameObject targetUI){
		GameObject obj = null;

		ComponentInfo targ = GetComponentInfo (targetUI.name);
		if (!CheckResource (targ)) {
			StartCoroutine (ShowWarning());
			StartCoroutine (bagManeger.AlpheAni());
			return;
		}

		objInfo = targ;

		obj = GetbuildModelPrefab (targetUI.name);
		if (obj != null) {
			beginBuild = true;
			buildObj = Instantiate (obj)as GameObject;

			Collider c = buildObj.GetComponent<Collider> ();
			if (c != null)
				c.enabled = false;
			foreach (Collider collider in buildObj.GetComponentsInChildren<Collider>()) {
				collider.enabled = false;
			}
	
		}
	}

	GameObject GetbuildModelPrefab(string name){
		for (int i = 0; i < buildModelList.Length; i++) {
			if (buildModelList [i].name == name)
				return buildModelList [i];
		}

		return null;
	}
}
