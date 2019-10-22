using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour {

    const int MinLane = -2;             //左限界
    const int MaxLane = 2;              //右限界
    const float LaneWidth = 1.0f;       //1レーンごとの幅
    const int DefaultLife = 3;          //初期ライフ
    const float StunDuration = 0.5f;    //気絶時間

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;                 //target(nejiko)のいるレーン
    int life = DefaultLife;
    float recoverTime = 0.0f;       //復帰までの時間

    public float gravity;       //重力の力
    public float speedZ;        //前進方向の力
    public float speedX;        //横方向の力
    public float speedJump;     //ジャンプの力
    public float accelerationZ; //前進加速度

    public int Life()           //ライフ取得用関数
    {
        return life;
    }

    public bool IsStan()        //  気絶判定
    {
        //recoverTimeが発生した時　or Life が０になった時　stanをTrueにする
        return recoverTime > 0.0f || life <= 0;
    }

	// Use this for initialization
	void Start () {

        //必要なコンポーンネントを取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        //デバッグ用
        if (Input.GetKeyDown("left"))  MoveToLeft();    //キー入力があった場合MoveToLeft()を呼び出す
        if (Input.GetKeyDown("right")) MoveToRight();   //キー入力があった場合MoveToRight()を呼び出す
        if (Input.GetKeyDown("space")) Jump();          //キー入力があった場合Jump()を呼び出す

        if (IsStan())
        {
            //気絶したら動きを止め復帰カウントを進める
            moveDirection.x = 0.0f;                     //動きを止める
            moveDirection.z = 0.0f;                     //動きを止める
            recoverTime -= Time.deltaTime;              //復帰までの時間を減らす
        }
        else
        {
            //徐々に加速しZ方向に常に前進させる
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

            //X方向は目標のポジションまでの差分の割合で速度を計算
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;
        }

        //重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後接地したらY方向の力はリセットする
        if (controller.isGrounded) moveDirection.y = 0;

        //速度が0以上なら走っているフラグをtrueにする
        //Setbool(パラメーターネーム、条件)
        animator.SetBool("run", moveDirection.z > 0.0f);
	}

    //左のレーンに移動
    public void MoveToLeft()
    {
        if (IsStan()) return;           //気絶時には入力をキャンセル
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    //右のレーンに移動
    public void MoveToRight()
    {
        if (IsStan()) return;           //気絶時には入力をキャンセル
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    //ジャンプ
    public void Jump()
    {
        if (IsStan()) return;           //気絶時には入力をキャンセル
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;

            //ジャンプトリガーを設定
            animator.SetTrigger("jump");
        }
    }

    //CharacterControllerにコリジョンが生じた時
    //衝突が発生した時
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStan()) return;

        if (hit.gameObject.tag == "Robo")
        {
            //ライフを減らして気絶状態
            life--;
            recoverTime = StunDuration;

            //ダメージトリガーを設定
            animator.SetTrigger("damage");

            //ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }
    }

}
