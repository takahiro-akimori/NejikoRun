using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePanel : MonoBehaviour {

    //ライフ情報格納
	public GameObject[] icons;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    //ライフに応じてスプライトを出し分ける
    public void UpdateLife (int life) {

        for (int i =0; i < icons.Length; i++)
		{
            //SetActiveでiconの表示と非表示を区別する
			if (i < life) icons[i].SetActive(true);
			else icons[i].SetActive(false);
		}
		
	}
}
