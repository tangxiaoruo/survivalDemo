using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackScript : MonoBehaviour {

	public GameObject deadObj;
	public PlayerController controller;
	//public Transform beginPos;
	public EquipmentItemsManager equipManager;
	public UIController uiController;
	public Transform healthBar;
	public int gunRate = 3;

	bool endShoot=true;
	//public int knifeRate = 1;
	RaycastHit hit;
	float timer=0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (controller.weaponType == WeaponType.AK && controller.shooting) {
			endShoot = false;
			GunAttack ();
		} else if (controller.weaponType == WeaponType.KNIFE) {
			
		} else {
			if (!endShoot)
				uiController.ShowHealthBar (false);
			
			endShoot = true;
		}
	}


	void GunAttack(){
		timer += Time.deltaTime;
		if (timer >= (1f / gunRate)) {
			timer = 0f;
			if (Physics.Raycast (transform.position + Vector3.up * 1.5f, transform.forward, out hit)) {
				Debug.DrawLine (transform.position + Vector3.up * 1.5f,transform.position + Vector3.up * 1.5f+transform.forward*20f,Color.black);
				Debug.Log (hit.transform.name);
				if (hit.transform.tag == "enemy") {
					uiController.ShowHealthBar (true);
					float dis = (hit.point - transform.position + Vector3.up * 1.5f).magnitude;
					Debug.DrawLine (transform.position + Vector3.up * 1.5f, transform.position + Vector3.up * 1.5f + transform.forward * dis, Color.red);
					float inverseLerp = 1.0f - Mathf.InverseLerp (10.0f, 20.0f, dis);

					int health = hit.transform.GetComponent<EnemyInfo> ().health;
					int currenthealth = (hit.transform.GetComponent<EnemyInfo> ().currentHealth -= (int)(inverseLerp * equipManager.gunDamageValue));
					Debug.Log (hit.transform.name);
					healthBar.GetComponent<UISlider> ().value = Mathf.InverseLerp (0f, health, currenthealth);

					if (currenthealth<= 0) {
						
						StartCoroutine (EnemyDead(hit.transform.gameObject));
					}

				} else
					uiController.ShowHealthBar (false);
			} else
				uiController.ShowHealthBar (false);
		}
	}


	IEnumerator EnemyDead(GameObject obj){
//		obj.GetComponent<CapsuleCollider> ().enabled = false;
//		obj.GetComponent<NavMeshAgent> ().enabled = false;
//		hit.transform.GetComponent<NavMeshAgent> ().enabled = false;
//		yield return new WaitForSeconds(0.5f);
//		obj.GetComponent<Animator> ().stop
//		obj.GetComponent<Animator> ().enabled = false;
		obj.SetActive(false);
		Destroy (obj);

		GameObject deadZombie=Instantiate(deadObj);
		deadZombie.transform.position = obj.transform.position;
		deadZombie.transform.rotation = obj.transform.rotation;
		yield return new WaitForSeconds (8f);
		Destroy (deadZombie);
	}
}
