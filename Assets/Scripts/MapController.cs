using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    public GameObject empty_unit_prefab;
    public float unit_width = 60;
    public int unit_count = 5;
    public int empty_count = 0;
    public float speed = 5.0f;
    public float speed_min = 5.0f;
    public float speed_max = 30.0f;
    public float speed_hurt_multiplier = 0.75f;      //受伤后速度乘数
    public float map_add_speed_duration = 1.0f;     //加速频率
    public float map_add_speed_value = 0.5f;        //每次速度增加的值
    public GameObject ps_room_1;
    public GameObject ps_room_2;
    private GameObject[] unit_prefab_list;
    private GameObject[] room_prefab_list;
    private Queue<GameObject> current_unit_list;
    private Queue<GameObject> current_room_list;
    private void Awake()
    {
        instance = this;
        speed = speed_min;
        //第一次以竹林关启动
        string folder_name = "room_" + (Random.Range(1, 6) > 1 ? 2 : 1).ToString();
        if (PlayerPrefs.GetInt("first_game", 1) == 1)
        {
            PlayerPrefs.SetInt("first_game", 0);
            folder_name = "room_2";
        }
        if (folder_name.Equals("room_1"))
        {
            ps_room_1.gameObject.SetActive(false);
            ps_room_2.gameObject.SetActive(false);
        }
        unit_prefab_list = Resources.LoadAll<GameObject>("Prefabs/MapUnits");
        room_prefab_list = Resources.LoadAll<GameObject>("Prefabs/Rooms/" + folder_name);
        current_unit_list = new Queue<GameObject>();
        current_room_list = new Queue<GameObject>();
        // 先把list填满
        int start_index = 1;
        for (int i = 0; i < unit_count; i++)
        {
            GameObject prefab = null;
            if (i < empty_count)
            {
                prefab = empty_unit_prefab;
            }
            else
            {
                int rand_index = Random.Range(0, unit_prefab_list.Length);
                prefab = unit_prefab_list[rand_index];
            }
            var unit = Instantiate(prefab, transform);
            unit.transform.position = new Vector3(unit_width * (i - start_index), 0, 0);
            current_unit_list.Enqueue(unit);
            int rand_room_index = Random.Range(0, room_prefab_list.Length);
            var room_prefab = room_prefab_list[rand_room_index];
            var room = Instantiate(room_prefab, transform);
            room.transform.position = new Vector3(unit_width * (i - start_index), 0, 0);
            current_room_list.Enqueue(room);
        }
    }

    private float last_update_x = 0;
    private int unit_repeat_index = -1;
    public void Update()
    {
        CheckAddSpeed();
        //地图滚动
        var old_pos = transform.position;
        old_pos.x -= speed * Time.deltaTime;
        transform.position = old_pos;

        //超出距离，销毁旧道路，生成新的道路
        if (last_update_x - transform.position.x < unit_width) return;
        var deleting = current_unit_list.Dequeue();
        int unit_index = unit_repeat_index >= 0 ? unit_repeat_index:Random.Range(0, unit_prefab_list.Length);
        var prefab = unit_prefab_list[unit_index];
        var unit = Instantiate(prefab, transform);
        unit.transform.position = new Vector3(deleting.transform.position.x + unit_width * unit_count, 0, 0);
        current_unit_list.Enqueue(unit);
        GameObject.Destroy(deleting);
        //房间
        deleting = current_room_list.Dequeue();
        int rand_map_index = Random.Range(0, room_prefab_list.Length);
        var room_prefab = room_prefab_list[rand_map_index];
        var room = Instantiate(room_prefab, transform);
        room.transform.position = unit.transform.position;
        current_room_list.Enqueue(room);
        GameObject.Destroy(deleting);

        //重新记录当前距离
        last_update_x = transform.position.x;

        //判断道路是否重复生成
        ToolGenerator toolGenerator = unit.GetComponent<ToolGenerator>();
        if (!toolGenerator) return;
        if (Random.Range(0, 100) > toolGenerator.repeat_chance)
            unit_repeat_index = -1;
        else
            unit_repeat_index = unit_index;
    }

    private float last_add_speed_time = 0;
    private void CheckAddSpeed()
    {
        if (Time.fixedTime - last_add_speed_time < map_add_speed_duration) return;
        last_add_speed_time = Time.fixedTime;
        speed += map_add_speed_value;
        if (speed > speed_max) speed = speed_max;
    }

    public void SlowDown()
    {
        speed *= speed_hurt_multiplier;
        if (speed < speed_min) speed = speed_min;
    }

    public int GetCurDifficulty()
    {
        return Mathf.RoundToInt((speed - speed_min) / (speed_max - speed_min) * 100.0f);
    }
}
