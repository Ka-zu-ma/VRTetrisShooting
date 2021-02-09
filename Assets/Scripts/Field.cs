using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    // ブロックが配置されていればtrue、そうでなければfalse
    bool[,] blocks = new bool[10, 20];

    [SerializeField]GameObject scoreViewObj;

    // Start is called before the first frame update
    void Start()
    {
        InitBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitBlocks()
    {
        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                blocks[i, j] = false;
            }
        }
    }

    int GetPosition(float value, float distance, int minRange, int maxRange)
    {
        for (int i = minRange; i < maxRange; i++)
        {
            if (Mathf.Abs(value - (float)(i)) < distance)
            {
                return i;
            }
        }
        return minRange - 1;
    }

    void OnCollisionEnter(Collision other)
    {
        var b = other.gameObject.transform.position;
        var u = 0.1f;
        var g = new Vector3(
            ((int)(b.x / u)) * u,           //x座標
            ((int)(b.y / u)) * u,           //y座標
            transform.position.z        //z座標
            );

        //向きを一定にする
        other.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        //左下座標を定義
        float px = -0.4f;
        float py = 0.1f;

        //blocksの要素番号を取得
        int bx = GetPosition((g.x - px) / u, 0.01f, 0, 10);
        int by = GetPosition((g.y - py) / u, 0.01f, 0, 20);

        //配置された位置の配列をtrueにする
        blocks[bx, by] = true;

        //参照できるように名前を設定
        other.gameObject.name = $"name:{bx},{by}";
        //座標を反映
        other.gameObject.transform.position = new Vector3(bx * u + px, by * u + py, transform.position.z);

        var rb = other.gameObject.GetComponent<Rigidbody>();
        Destroy(rb);

        // 位置を表示
        GameObject sv = Instantiate(scoreViewObj);
        sv.transform.position = other.gameObject.transform.position;
        sv.GetComponent<ScoreView>().textMesh.text = $"({bx},{by})";
    }
}
