using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour {

    Vector3 diff;

    //追従するターゲット
    public GameObject target;
    //追従するスピード
    public float followSpeed;


	// Use this for initialization
	void Start () {
        //ターゲットとの距離を計算
        diff = target.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        //Lerp線形補完関数
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position - diff,
            Time.deltaTime * followSpeed
            );
		
	}
}
