using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    public GameObject empty_unit_prefab;
    public float unit_width = 60;
    public int unit_count = 5;
    public int empty_count = 0;
    public float speed = 1.0f;
    private GameObject[] unit_prefab_list;
    private GameObject room_prefab;
    private Queue<GameObject> current_unit_list;
    private Queue<GameObject> current_room_list;
    private void Awake()
    {
        instance = this;
        unit_prefab_list = Resources.LoadAll<GameObject>("Prefabs/MapUnits");
        room_prefab = Resources.Load<GameObject>("Prefabs/Rooms/Room_001");
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
            var room = Instantiate(room_prefab, transform);
            room.transform.position = new Vector3(unit_width * (i - start_index), 0, 0);
            current_room_list.Enqueue(room);
        }
    }

    private float last_update_x = 0;
    public void Update()
    {
        var old_pos = transform.position;
        old_pos.x -= speed * Time.deltaTime;
        transform.position = old_pos;
        if (last_update_x - transform.position.x < unit_width) return;
        var deleting = current_unit_list.Dequeue();
        int rand_index = Random.Range(0, unit_prefab_list.Length);
        var prefab = unit_prefab_list[rand_index];
        var unit = Instantiate(prefab, transform);
        unit.transform.position = new Vector3(deleting.transform.position.x + unit_width * unit_count, 0, 0);
        current_unit_list.Enqueue(unit);
        GameObject.Destroy(deleting);
        last_update_x = transform.position.x;
        deleting = current_room_list.Dequeue();
        GameObject.Destroy(deleting);
        var room = Instantiate(room_prefab, transform);
        room.transform.position = unit.transform.position;
        current_room_list.Enqueue(room);
    }
}
