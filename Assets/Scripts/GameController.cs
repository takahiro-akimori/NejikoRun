using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public NejikoController nejiko;
    public Text scoreLabel;
    public LifePanel lifePanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //スコアラベルを更新
        int score = CalcScore();
        scoreLabel.text = "Score : " + score + "m";

        //ライフパネルを更新
        //NejikoControllerからハート数を取得
        lifePanel.UpdateLife(nejiko.Life());

        //ねじこのライフが０になったらゲームオーバー
        if (nejiko.Life() <=0)
        {
            //これ以降のUpdateはとめる
            enabled = false;

            //ハイスコアの更新
             
            if (PlayerPrefs.GetInt("HighScore") < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }

            //2秒後にReturnToTitleを呼び出す
            Invoke("ReturnToTitle", 2.0f);
        }
	}

    int CalcScore()
    {
        //ねじこの走行距離をスコアにする
        //ねじこは０地点からz軸＋方向に走るのでポジションの情報をそのまま反映
        return (int)nejiko.transform.position.z;
    }

    void ReturnToTitle()
    {
        //タイトルシーンに切替
        SceneManager.LoadScene("Title");
    }
}
