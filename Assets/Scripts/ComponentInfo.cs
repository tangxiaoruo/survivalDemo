using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentInfo {

	public string name;
	public string[] requiredGoods;
	public int[] requiredGoodsNum;

	public string[] requiredTools;

	public ComponentInfo(string name,string[] goods,int[] num,string[] tools){
	
		this.name = name;
		requiredGoods = goods;
		requiredGoodsNum = num;
		requiredTools = tools;
	}
}
