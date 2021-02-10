using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BlockType
{
    public int[,] shape;　//形状を定義する配列
}

public class BlockGenerator : MonoBehaviour
{
    // ブロックのゲームオブジェクト
    [SerializeField]GameObject blockObj;

    // ブロックタイプの作成
    [SerializeField]BlockType[] blockTypes;


    // 左右どちらのコントローラかを取得するために使用するControllerクラス
    OVRInput.Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        // 左右どちらのコントローラかを取得 
        controller = GetComponent<OVRControllerHelper>().m_controller;
        CreateBlockType();
    }

    // Update is called once per frame
    void Update()
    {//A/Xボタンを押したらブロックを発射
        if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            Fire(transform.position, transform.forward, CreateBlock(Random.Range(0, 7)));
        }
    }

    //ブロックを発射する関数
    void Fire(Vector3 startPos, Vector3 direction, GameObject target)
    {
        //ブロックのコピーを生成
        //GameObject go = Instantiate(target);
        //ブロックの発射位置を設定
        target.transform.position = startPos;
        //ブロックをコントローラ正面方向に放つ
        target.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
    }

    // ブロックの種類を定義
    void CreateBlockType()
    {
        blockTypes = new BlockType[7];
        blockTypes[0].shape = new int[,] {
            {0,0,0,0},
            {0,0,0,0},
            {1,1,1,1},
            {0,0,0,0},
        };
        blockTypes[1].shape = new int[,] {
            {1,1},
            {1,1},
        };
        blockTypes[2].shape = new int[,] {
            {0,1,0},
            {1,1,1},
            {0,0,0},
        };
        blockTypes[3].shape = new int[,] {
            {0,0,1},
            {1,1,1},
            {0,0,0},
        };
        blockTypes[4].shape = new int[,] {
            {1,0,0},
            {1,1,1},
            {0,0,0},
        };
        blockTypes[5].shape = new int[,] {
            {1,1,0},
            {0,1,1},
            {0,0,0},
        };
        blockTypes[6].shape = new int[,] {
            {0,1,1},
            {1,1,0},
            {0,0,0},
        };
    }

    //ブロックを生成
    GameObject CreateBlock(int typeNum)
    {
        int size = blockTypes[typeNum].shape.GetLength(0);

        GameObject blockUnits = new GameObject("BlockUnits");
        blockUnits.tag = "blockUnits";
        // 物理演算を適用させるためRigidbodyを追加
        blockUnits.AddComponent<Rigidbody>();
        // 二次元配列をループで処理
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // ブロックを配置する位置であればブロックを生成
                if (blockTypes[typeNum].shape[j, i] == 1)
                {
                    GameObject go = Instantiate(blockObj);
                    go.transform.parent = blockUnits.transform;
                    // ブロックを生成する位置は親オブジェクトの相対位置で決定
                    go.transform.localPosition = new Vector3(i - size / 2, size / 2 - j, 0) * 0.1f;
                }
            }
        }
        return blockUnits;
    }
}
