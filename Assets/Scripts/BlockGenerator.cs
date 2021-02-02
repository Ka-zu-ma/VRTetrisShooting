using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    // ブロックのゲームオブジェクト
    [SerializeField]GameObject blockObj;

    // 左右どちらのコントローラかを取得するために使用するControllerクラス
    OVRInput.Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        // 左右どちらのコントローラかを取得 
        controller = GetComponent<OVRControllerHelper>().m_controller;
    }

    // Update is called once per frame
    void Update()
    {//A/Xボタンを押したらブロックを発射
        if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            Fire(transform.position, transform.forward, blockObj);
        }

    }

    //ブロックを発射する関数
    void Fire(Vector3 startPos, Vector3 direction, GameObject target)
    {
        //ブロックのコピーを生成
        GameObject go = Instantiate(target);
        //ブロックの発射位置を設定
        go.transform.position = startPos;
        //ブロックをコントローラ正面方向に放つ
        go.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
    }
}
