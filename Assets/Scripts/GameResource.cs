using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class GameResource : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}

public class GameStatus
{
    // ゲームの状態を保持
    // 「ブロックを発射する状態」	: Shot
    // 「発射後の待機状態」		: Wait
    // 「ブロックが落下する状態」	: Fall
    // 「ブロックが消える状態」		: Delete
    static public string status = "Shot";
}