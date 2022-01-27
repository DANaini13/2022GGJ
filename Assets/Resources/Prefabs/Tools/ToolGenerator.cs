using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGenerator : MonoBehaviour
{
    [Header("障碍prefab")]
    public Transform[] toolPrefabs;
    [Header("是否按顺序生成障碍")]
    public bool use_queue = false;
    [Header("障碍数量，为0时则由脚本自动计算")]
    public int tool_count = 0;
    [Header("障碍数量是否为偶数，tool_count手动设值时该参数无效")]
    public bool use_even_count = false;
    [Header("该道路被生成时，有多少几率使下一次生成该道路的副本(0-100)")]
    public int repeat_chance = 0;
    [Header("金币")]
    public Transform coin_prefab;
    private int difficulty_chance = 50;     //难度影响数量的几率（0-100）
    private int tool_min_count = 2;
    private int tool_max_count = 6;
    private int cur_index = 0;
    private int coin_generate_between = 1;

    void Start()
    {
        //算出此房间的障碍数量
        if (tool_count == 0)
        {
            //难度越高，障碍物越容易密集
            if (Random.Range(0, 100) < difficulty_chance)
            {
                int difficulty = MapController.instance.GetCurDifficulty();
                tool_count = Mathf.RoundToInt((difficulty / 100.0f) * (tool_max_count - tool_min_count)) + tool_min_count;
            }
            else
                tool_count = Random.Range(tool_min_count, tool_max_count + 1);
            if (use_even_count && tool_count % 2 == 1) tool_count++;
        }

        //根据障碍数量算出间距，并实例化障碍
        float unit_width = MapController.instance.unit_width;
        int tool_between = Mathf.RoundToInt(unit_width / tool_count);
        int tool_last_pos = 0;
        int tool_cur_pos = 0;
        int coin_between_tool = 3;
        int coin_between = 0;
        for (int i = 0; i < unit_width; i++)
        {
            if (i == tool_cur_pos)
            {
                tool_last_pos = tool_cur_pos;
                tool_cur_pos += tool_between;
                Transform prefab = use_queue ? toolPrefabs[cur_index] : toolPrefabs[Random.Range(0, toolPrefabs.Length)];
                Transform tool = Instantiate(prefab, this.transform).transform;
                tool.localPosition = Vector3.right * i;

                if (!use_queue) continue;
                cur_index++;
                if (cur_index >= toolPrefabs.Length)
                    cur_index = 0;
            }
            else
            {
                //用金币补充空位
                if (coin_between > 0)
                {
                    coin_between--;
                    continue;
                }
                coin_between = coin_generate_between;
                if (i > tool_last_pos + coin_between_tool && i < tool_cur_pos - coin_between_tool)
                {
                    if (!coin_prefab) continue;
                    Transform coin = Instantiate(coin_prefab, this.transform);
                    coin.localPosition = new Vector3(i + 0.5f, 1f, 0f);

                    coin = Instantiate(coin_prefab, this.transform);
                    coin.localPosition = new Vector3(i + 0.5f, -19f, 0f);
                }
            }
        }
    }
}
