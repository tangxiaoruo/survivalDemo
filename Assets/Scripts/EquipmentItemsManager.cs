using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItemsManager : MonoBehaviour {
	
	public PlayerController playerControll;
	public Transform[] weapons;
	public SkinGenerater skinGenerate;
	public UILabel label;

	[HideInInspector]
	public int gunDamageValue=0;
	[HideInInspector]
	public int knifeDamageValue=0;
	[HideInInspector]
	public int armorValue=0;
	[HideInInspector]
	public Transform currentWeapon=null;

	private Transform gun=null;
	private Transform knife=null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.CapsLock)) {
			if (currentWeapon != null) {
				if (gun == null || knife == null) {
					currentWeapon.gameObject.SetActive (true);
					if (currentWeapon.GetComponent<ItemObj>().skinType==SkinType.WEAPON) {
						playerControll.weaponType = WeaponType.KNIFE;
					} else {
						playerControll.weaponType = WeaponType.AK;
					}
				}else {

					currentWeapon.gameObject.SetActive (false);

					if (currentWeapon.name == gun.name) {
						currentWeapon = knife;
						playerControll.weaponType = WeaponType.KNIFE;
					} else {
						currentWeapon = gun;
						playerControll.weaponType = WeaponType.AK;
					}

					currentWeapon.gameObject.SetActive (true);

				}
			}else if(gun != null && knife != null){
				
				if (gun != null) {
					currentWeapon = gun;
					playerControll.weaponType = WeaponType.AK;

				} else {
					currentWeapon = knife;
					playerControll.weaponType = WeaponType.KNIFE;

				}

				currentWeapon.gameObject.SetActive (true);

			}


		} 



	}

	//由于武器没有做成蒙皮，并绑定到骨骼。因此就采用动态地禁用或激活武器（gameobject）的方法来实现切换武器
	void EquipWeapon(ItemObj info){

		for (int i = 0; i < weapons.Length; i++) {
			if (weapons [i].name == info.name) {
				if (info.type == "gun")
					gun = weapons [i];
				else
					knife = weapons [i];

				if (currentWeapon == null)
					currentWeapon = weapons [i];
				return;
			}
		}
		Debug.Log ("cant find the weapon");
	}

	//gene用于判断是装配好，还是从装备栏卸下
	public void ChangeEquipment(ItemObj objInfo,ItemObj tagInfo=null,bool gene=true){
		
		ItemObj skinInfo = objInfo;
		bool reset = false;

		if (objInfo.type == "gun") {

			if (gene) {
				EquipWeapon (objInfo);
				gunDamageValue = objInfo.damage;
			} else {
				if (tagInfo != null) {
					EquipWeapon (tagInfo);
					gunDamageValue = tagInfo.damage;
					skinInfo = tagInfo;
				} else {
					gun = null;
					gunDamageValue = 0;
					if (currentWeapon.name == objInfo.name) {
						playerControll.weaponType = WeaponType.NULL;
						currentWeapon.gameObject.SetActive (false);
						currentWeapon = null;
					}
					reset = true;
				}
			}
		} else if (objInfo.type == "knife") {
			if (gene) {
				EquipWeapon (objInfo);
				knifeDamageValue = objInfo.damage;
			} else {
				if (tagInfo != null) {
					EquipWeapon (tagInfo);
					knifeDamageValue = tagInfo.damage;
					skinInfo = tagInfo;
				} else {
					knife = null;
					knifeDamageValue = 0;

					if (currentWeapon.name == objInfo.name) {
						playerControll.weaponType = WeaponType.NULL;
						currentWeapon.gameObject.SetActive (false);
						currentWeapon = null;
					}

					reset = true;
				}
			}
		}
		else if (objInfo.type != "bag" || objInfo.type != "backpack" || objInfo.type != "mask" || objInfo.type != "bodyKit") {
			if (gene) {
				if (tagInfo != null)
					armorValue -= tagInfo.armorValue;
				armorValue += objInfo.armorValue;
			} else {
				if (tagInfo != null) {
					armorValue += tagInfo.armorValue;
					armorValue -=objInfo.armorValue;
					skinInfo = tagInfo;
				} else {
					armorValue -=objInfo.armorValue;
					reset = true;
				}

			}
		}
		//change skin
		skinGenerate.ChangeSkin (skinInfo.name,skinInfo.skinType,reset);	
		label.text = "armorValue:"+armorValue+"\ngunDamageValue:"+gunDamageValue+"\nknifeDamageValue:"+knifeDamageValue;

	}



	public void EquippedWith(Transform org,Transform tag){
		ItemObj objInfo = org.GetComponent<ItemObj> ();
		if(objInfo.type=="normal"){
			org.localPosition = Vector3.zero;
			return;
			}

		if (tag.tag == "equipmentsCells") {
			if (objInfo.type == tag.name) {
				org.parent = tag;
				ChangeEquipment (objInfo);
			} 
			org.localPosition = Vector3.zero;
		} else {
			if (objInfo.type == tag.parent.name) {
				Transform temp;
				temp = org.parent;
				org.parent = tag.parent;

				tag.parent = temp;
				tag.localPosition = Vector3.zero;

				ChangeEquipment (objInfo,tag.GetComponent<ItemObj>());
			} 
			org.localPosition = Vector3.zero;
		}
	}
}
