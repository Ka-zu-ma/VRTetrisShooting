using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
	// ブロックが配置されていればtrue、そうでなければfalse
	bool[,] blocks = new bool[10, 20];

	[SerializeField] GameObject scoreViewObj;

	public List<GameObject> blockList;

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
		if (other.gameObject.tag != "blockUnits")
			return;

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

		//範囲外ならエラーを帰す
		if (bx < 0 || by < 0 || bx >= 10 || by >= 20 || CheckExistBlock(other.gameObject, bx, by))
		{
			//衝突したブロックが配置できない場合、子オブジェクトのブロックのBoxColliderを削除
			for (int i = 0; i < other.gameObject.transform.childCount; i++)
			{
				Destroy(other.gameObject.transform.GetChild(i).transform.GetComponent<BoxCollider>());
			}
		}
		else
		{
			//座標を反映
			other.gameObject.transform.position = new Vector3(bx * u + px, by * u + py, transform.position.z);

			//配置された位置の配列をtrueにする
			ApplyBlockUnits(other.gameObject, bx, by);

			// 位置を表示
			GameObject sv = Instantiate(scoreViewObj);
			sv.transform.position = other.gameObject.transform.position;
			sv.GetComponent<ScoreView>().textMesh.text = $"({bx},{by})";

			// 親オブジェクトを削除
			int cnt = other.transform.childCount;
			for (int i = 0; i < cnt; i++)
			{
				other.transform.GetChild(0).parent = null;
			}
			other.transform.tag = "Untagged";
			other.transform.position = Vector3.forward * 1000f;

		}
	}

	bool CheckExistBlock(GameObject target, int x, int y)
	{
		for (int i = 0; i < target.transform.childCount; i++)
		{
			Vector3 g = target.transform.GetChild(i).localPosition;
			int bx = (int)(g.x * 10);
			int by = (int)(g.y * 10);
			//枠外にはブロックが存在することにしておく
			if (x + bx < 0 || y + by < 0 || x + bx >= 10 || y + by >= 20)
			{
				return true;
			}
			// （x+bx,y+by）の位置にブロックが存在しているかどうかチェック
			if (!(x + bx < 0 || x + bx >= 10 || y + by < 0 || y + by >= 20) && blocks[x + bx, y + by])
			{
				return true;
			}
		}
		return false;
	}

	//ブロックを登録する関数
	void ApplyBlockUnits(GameObject target, int x, int y)
	{
		blockList = new List<GameObject>();
		for (int i = 0; i < target.transform.childCount; i++)
		{
			Vector3 g = target.transform.GetChild(i).localPosition;
			int bx = (int)(g.x * 10);
			int by = (int)(g.y * 10);
			blocks[x + bx, y + by] = true;

			//判定用の座標を設定
			target.transform.GetChild(i).GetComponent<Block>().x = x + bx;
			target.transform.GetChild(i).GetComponent<Block>().y = y + by;

			//参照できるように名前を設定
			target.transform.GetChild(i).name = $"name:{x + bx},{y + by}";

			//落下用ゲームオブジェクトに設定
			blockList.Add(target.transform.GetChild(i).gameObject);
		}
	}

	void SortBlockList()
	{
		for (int i = 0; i < blockList.Count; i++)
		{
			for (int j = i; j < blockList.Count; j++)
			{
				if (blockList[i].GetComponent<Block>().y > blockList[j].GetComponent<Block>().y)
				{
					var tmp = blockList[i];
					blockList[i] = blockList[j];
					blockList[j] = tmp;
				}
			}
		}

	}
}
