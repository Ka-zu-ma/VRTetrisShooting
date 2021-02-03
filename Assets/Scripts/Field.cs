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

    void OnCollisionEnter(Collision other)
    {
        var b = other.gameObject.transform.position;
        var u = 0.1f;
        var g = new Vector3(
            (float)((int)(b.x / u)) * u,    //x座標
            (float)((int)(b.y / u)) * u,    //y座標
            transform.position.z        //z座標
            );

        //座標を反映
        other.gameObject.transform.position = g;
        //向きを一定にする
        other.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        var rb = other.gameObject.GetComponent<Rigidbody>();
        Destroy(rb);
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
}
