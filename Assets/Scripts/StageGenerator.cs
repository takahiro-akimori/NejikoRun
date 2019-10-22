using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour {

    const int StageTipSize = 30;

    int currentTipIndex;

    public Transform character;         //ターゲットのキャラクター
    public GameObject[] stageTips;      //ステージチップスの配列
    public int startTipsIndex;          //自動生成開始インデックス
    public int preInstantiate;          //生成先読み個数
    public List<GameObject> generatedStageList = new List<GameObject>();    //生成済ステージチップ保存リスト

	// Use this for initialization
	void Start () {

        currentTipIndex = startTipsIndex - 1;
        UpdateStage(preInstantiate);
		
	}
	
	// Update is called once per frame
	void Update () {
        //キャラクターの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z / StageTipSize);

        //次のステージチップに入ったらステージの更新処理
        if (charaPositionIndex + preInstantiate > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    //指定のIndexまでのステージチップを生成して管理下に置く
    void UpdateStage(int toTipIndex)
    {
        if (toTipIndex <= currentTipIndex) return;
        
            //指定のステージチップまでを生成
            for (int i = currentTipIndex + 1; i <= toTipIndex; i++)
            {
                GameObject stageObject = GenerateStage(i);

                //生成したステージチップを管理リストに追加
                generatedStageList.Add(stageObject);
            }

            //ステージ保持上限内になるまで古いステージを削除
            while (generatedStageList.Count > preInstantiate + 2) DestroyOldStage();

            currentTipIndex = toTipIndex;
    }

    //指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int tipIndex)
    {
        int nextStageTip = Random.Range(0, stageTips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageTips[nextStageTip],
            new Vector3(0, 0, tipIndex * StageTipSize),
            Quaternion.identity);

        return stageObject;
    }

    //一番古いステージを削除
    void DestroyOldStage()
    {
        //oldStageに古い配列を代入
        GameObject oldStage = generatedStageList[0];
        //RemoveAtで要素を取り除き詰める
        generatedStageList.RemoveAt(0);
        //oldStageを削除
        Destroy(oldStage);
    }
}
