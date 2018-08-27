using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkinType{
	BACKPACK,
	BAG,
	BODYKIT,
	HAIRSTYLE,
	HELMET,
	MASK,
	PAINTS,
	SHIRT,
	SHOES,
	VEST,
	WEAPON,
	WEAPON_GUN,
	NONE
}


public class SkinGenerater : MonoBehaviour {

	public List<GameObject> backPackList = new List<GameObject> ();
	public List<GameObject> bagList = new List<GameObject> ();
	public List<GameObject> bodyKitList = new List<GameObject> ();
	public List<GameObject> hairStyleList = new List<GameObject> ();
	public List<GameObject> helmetList = new List<GameObject> ();
	public List<GameObject> maskList = new List<GameObject> ();
	public List<GameObject> paintsList = new List<GameObject> ();
	public List<GameObject> shirtList = new List<GameObject> ();
	public List<GameObject> shoesList=new List<GameObject> ();
	public List<GameObject> vestList=new List<GameObject>();
	public List<GameObject> weaponList=new List<GameObject>();
	public List<GameObject> weapon_gunList=new List<GameObject>();

	private SkinnedMeshRenderer backPack;
	private SkinnedMeshRenderer bag;
	private SkinnedMeshRenderer bodyKit;
	private SkinnedMeshRenderer hairStyle;
	private SkinnedMeshRenderer helmet;
	private SkinnedMeshRenderer mask;
	private SkinnedMeshRenderer pants;
	private SkinnedMeshRenderer shirt;
	private SkinnedMeshRenderer shoes;
	private SkinnedMeshRenderer vest;
	private SkinnedMeshRenderer weapon;
	private SkinnedMeshRenderer weapon_gun;

	public SkinnedMeshRenderer combineSkin;

	List<Material> materials=new List<Material>();
	List<Transform> bones=new List<Transform>();
	List<CombineInstance> combineList = new List<CombineInstance> ();

	Transform[] bonesTransform;
	public GameObject root;

	public SkinnedMeshRenderer[] baseSkins;
	private SkinnedMeshRenderer temp;

	void Init(){

		bonesTransform = root.GetComponentsInChildren<Transform> ();
		Debug.Log (bonesTransform[0].name);
		//root.transform.GetChild(0).parent = transform;
		//Destroy (root);
		if (materials != null)
			materials.Clear ();

		if (bones != null)
			bones.Clear ();

		if (combineList != null)
			combineList.Clear ();

		SkinnedMeshRenderer[] skins = GetComponentsInChildren<SkinnedMeshRenderer> ();

		for (int i = 0; i < skins.Length; i++) {
		
			if (skins [i].name == "combineSkin")
				continue;

			CombineInstance combine = new CombineInstance ();
			combine.mesh = skins [i].sharedMesh;
			combineList.Add (combine);

			materials.AddRange (skins[i].materials);

			GetBone (skins[i].bones);
			Destroy (skins[i].gameObject);
		}

		Mesh mesh = new Mesh ();
		mesh.CombineMeshes (combineList.ToArray (), false, false);

		combineSkin.sharedMesh = mesh;
		combineSkin.materials = materials.ToArray ();
		combineSkin.bones = bones.ToArray ();

		backPack = null;
		bag = null;
		bodyKit = null;
		hairStyle = null;
		helmet = null;
		mask = null;
		pants = null;
		shirt = null;
		shoes = null;
		vest = null;
		weapon = null;
		weapon_gun = null;
	}

	void GetBone(Transform[] bones){
	
		for (int i = 0; i < bones.Length; i++) {

			for (int j = 0; j < bonesTransform.Length; j++) {


				if (bonesTransform[j].name.Equals (bones [i].name)) {
					this.bones.Add (bonesTransform[j]);
					break;
				}
			}

		}


	}

	public void ResetSkin(){
		
		materials.Clear ();
		bones.Clear ();
		combineList.Clear ();

		for (int i = 0; i < baseSkins.Length; i++) {
			
			CombineInstance combine = new CombineInstance ();
			combine.mesh = baseSkins [i].sharedMesh;
			combineList.Add (combine);

			materials.AddRange (baseSkins[i].materials);

			GetBone (baseSkins[i].bones);
		}

		Mesh mesh = new Mesh ();
		mesh.CombineMeshes (combineList.ToArray (), false, false);

		combineSkin.sharedMesh = mesh;
		combineSkin.materials = materials.ToArray ();
		combineSkin.bones = bones.ToArray ();



	}


	public void ChangeSkin(string skinName,SkinType type,bool reset=false){
	
		materials.Clear ();
		bones.Clear ();
		combineList.Clear ();


		if (type == SkinType.BACKPACK) {
			if (reset)
				backPack = null;
			else {
				GetMeshRenderer (skinName, backPackList);
				if (temp != null)
					backPack = temp;
			}
		} else if (type == SkinType.BAG) {
			if (reset)
				bag = null;
			else {
				GetMeshRenderer (skinName, bagList);
				if (temp != null)
					bag = temp;
			}
		} else if (type == SkinType.BODYKIT) {
			if (reset)
				bodyKit = null;
			else {
				GetMeshRenderer (skinName, bodyKitList);
				if (temp != null)
					bodyKit = temp;
			}
		} else if (type == SkinType.HAIRSTYLE) {
			if (reset)
				hairStyle = null;
			else {
				GetMeshRenderer (skinName, hairStyleList);
				if (temp != null)
					hairStyle = temp;
			}
		} else if (type == SkinType.HELMET) {
			if (reset)
				helmet = null;
			else {
				GetMeshRenderer (skinName, helmetList);
				if (temp != null)
					helmet = temp;
			}
		} else if (type == SkinType.MASK) {
			if (reset)
				mask = null;
			else {
				GetMeshRenderer (skinName, maskList);
				if (temp != null)
					mask = temp;
			}
		} else if (type == SkinType.PAINTS) {
			if (reset)
				pants = null;
			else {
				GetMeshRenderer (skinName, paintsList);
				if (temp != null)
					pants = temp;
			}
		} else if (type == SkinType.SHIRT) {
			if (reset)
				shirt = null;
			else {
				GetMeshRenderer (skinName, shirtList);
				if (temp != null)
					shirt = temp;
			}
		} else if (type == SkinType.SHOES) {
			if (reset)
				shoes = null;
			else {
				GetMeshRenderer (skinName, shoesList);
				if (temp != null)
					shoes = temp;
			}
		} else if (type == SkinType.VEST) {
			if (reset)
				vest = null;
			else {
				GetMeshRenderer (skinName, vestList);
				if (temp != null)
					vest = temp;
			}
		}
		else if (type == SkinType.WEAPON)
			//***********************************
			;
		else if (type == SkinType.WEAPON_GUN)
			//***********************************
			;
		else
			Debug.Log ("unknow skinType!");

		if (temp != null) {

			CombineBaseMesh ();

			CombineMesh ();


			Mesh mesh = new Mesh ();
			mesh.CombineMeshes (combineList.ToArray (), false, false);

			combineSkin.sharedMesh = mesh;
			combineSkin.materials = materials.ToArray ();
			combineSkin.bones = bones.ToArray ();
		}
	
	}



	void CombineBaseMesh(){

		for (int i = 0; i < baseSkins.Length; i++) {

			CombineInstance combine = new CombineInstance ();
			combine.mesh = baseSkins [i].sharedMesh;
			combineList.Add (combine);

			materials.AddRange (baseSkins[i].materials);

			GetBone (baseSkins[i].bones);
		}

	}

	void CombineMesh(){


		AddMeshToCombineLsit (backPack);
		AddMeshToCombineLsit (bag);
		AddMeshToCombineLsit (bodyKit);
		AddMeshToCombineLsit (hairStyle);
		AddMeshToCombineLsit (helmet);
		AddMeshToCombineLsit (mask);
		AddMeshToCombineLsit (pants);
		AddMeshToCombineLsit (shirt);
		AddMeshToCombineLsit (shoes);
		AddMeshToCombineLsit (vest);
		AddMeshToCombineLsit (weapon);
		AddMeshToCombineLsit (weapon_gun);

	}

	void AddMeshToCombineLsit (SkinnedMeshRenderer skin){

		if (skin == null) {
			Debug.Log ("skin is null");
			return;
		}

		CombineInstance com = new CombineInstance ();
		com.mesh = skin.sharedMesh;

		combineList.Add (com);
		materials.AddRange (skin.materials);

		GetBone (skin.bones);
	}

	void GetMeshRenderer(string name,List<GameObject> skinList){
	
		temp = null;
		foreach (GameObject obj in skinList) {
			if (obj.name == name)
				temp = obj.GetComponent<SkinnedMeshRenderer> ();
		}

		if (temp == null) {
			Debug.Log ("can't find the skinnedMesh!!!");
		}
	}


	// Use this for initialization
	void Start () {
		Init ();
	}

}
