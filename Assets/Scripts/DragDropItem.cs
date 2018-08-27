using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropItem :UIDragDropItem {

	protected override void OnDragDropStart ()
	{
		base.OnDragDropStart ();
		UISprite sprite = transform.GetComponent<UISprite> ();
		sprite.width = 45;
		sprite.height = 45;
		sprite.depth = 10;
	}

	protected override void OnDragDropRelease (GameObject surface)
	{
		base.OnDragDropRelease (surface);

		UISprite sprite = transform.GetComponent<UISprite> ();
		sprite.width = 40;
		sprite.height = 40;
		sprite.depth = 2;

		if (surface == null) {
			transform.localPosition = Vector3.zero;
			return;
		}
		Debug.Log(surface.name);

		GameObject equippedItemsUI = GameObject.Find ("equippedItemsUI");
		if (equippedItemsUI == null) {
			Debug.Log ("cant find the equippedItemsUI!");
			return;
		}
		
		if (surface.tag == "cells") {
			
			if (transform.parent.tag == "equipmentsCells")
				equippedItemsUI.GetComponent<EquipmentItemsManager> ().ChangeEquipment (transform.GetComponent<ItemObj>(),null,false);
			
			transform.parent = surface.transform;
			transform.localPosition = Vector3.zero;

		} else if (surface.tag == "item") {
			if (surface.transform.parent.name.StartsWith ("cell")) {
				//drag item from equipmentItemUI to backpackUI or goodsUI
				if (transform.parent.tag=="equipmentsCells") {
					if (transform.GetComponent<ItemObj> ().type == surface.transform.GetComponent<ItemObj> ().type) {
						Transform temp;
						temp = transform.parent;
						transform.parent = surface.transform.parent;
					
						surface.transform.parent = temp;
						surface.transform.localPosition = Vector3.zero;

						equippedItemsUI.GetComponent<EquipmentItemsManager> ().ChangeEquipment (transform.GetComponent<ItemObj>(),surface.transform.GetComponent<ItemObj>(),false);

					}
					transform.localPosition = Vector3.zero;
					return;
				}

				if (surface.name == transform.name) {
					//----------------------------------------
					GameObject bagUI = GameObject.Find ("backpackUI");
					if (bagUI == null)
						Debug.Log ("cant find the backpackUI");
					else {

						bagUI.GetComponent<BackPagManager> ().CombineItemsInside (transform, surface.transform);
					}

				} else {
					Transform temp;
					temp = transform.parent;
					transform.parent = surface.transform.parent;
					transform.localPosition = Vector3.zero;

					surface.transform.parent = temp;
					surface.transform.localPosition = Vector3.zero;

				}
			} else {
				Debug.Log ("sssssssdfdfdf");
				equippedItemsUI.GetComponent<EquipmentItemsManager> ().EquippedWith (transform,surface.transform);
			}
					

		} else if (surface.tag == "equipmentsCells") {
			if(transform.parent.tag=="equipmentsCells")
				transform.localPosition = Vector3.zero;
			
			equippedItemsUI.GetComponent<EquipmentItemsManager> ().EquippedWith (transform,surface.transform);
		} else {
			transform.localPosition = Vector3.zero;
		}
			
	}

}
