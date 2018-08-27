using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public UISlider slider;
	public Transform panel;
	public Transform panel2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPlayClicked(){
	
		StartCoroutine (LoadAsync(1));

	}

	IEnumerator LoadAsync(int index){
	
		panel.GetComponent<UIPanel> ().alpha = 0f;
		panel2.GetComponent<UIPanel> ().alpha = 1.0f;

		AsyncOperation operation = SceneManager.LoadSceneAsync (index);

		while (!operation.isDone) {
			slider.value = operation.progress;	
			yield return null;
		}
		
	}

	public void ExitClicked(){
		Application.Quit ();
	}
}
