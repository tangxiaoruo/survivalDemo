using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsInfo : MonoBehaviour {

	public string name;
	public bool randomGenerate;
	public int randomLevel;
	public  bool opened=false;
	public bool nonRandomGoods;
	public string[] goodsName;
	public int[] goodsNum;

	public Hashtable itemTable=new Hashtable();

}
