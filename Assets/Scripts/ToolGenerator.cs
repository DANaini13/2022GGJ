using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGenerator : MonoBehaviour
{
    [Header("障碍prefab")]
    public Transform[] toolPrefabs;
    [Header("是否按顺序生成")]
    public bool useQueue = false;
    [Header("障碍数量，为0时则由脚本自动计算")]
    public int tool_count = 0;
    private int difficulty_chance = 50;     //难度影响数量的几率（0-100）
    private int tool_min_count = 2;
    private int tool_max_count = 6;
    private int cur_index = 0;

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
        }

        //根据障碍数量算出间距，并实例化障碍
        float unit_width = MapController.instance.unit_width;
        int tool_between = Mathf.RoundToInt(unit_width / tool_count);
        for (int i = 0; i < tool_count; i++)
        {
            Transform prefab = useQueue ? toolPrefabs[cur_index] : toolPrefabs[Random.Range(0, toolPrefabs.Length)];
            Transform tool = Instantiate(prefab).transform;
            tool.SetParent(this.transform);
            tool.localPosition = Vector3.right * (tool_between * i);
            Debug.Log(tool.localPosition.x);

            cur_index++;
            if (cur_index >= toolPrefabs.Length)
                cur_index = 0;
        }
    }
}
