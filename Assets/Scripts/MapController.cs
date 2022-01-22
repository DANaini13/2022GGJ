using UnityEngine;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    
    
    public float unit_width = 60;
    public float speed = 1.0f;
    private GameObject[] map_unit_list;
    private void Awake()
    {
        instance = this;
        map_unit_list = Resources.LoadAll<GameObject>("Prefabs/MapUnits");
        int rand_index = Random.Range(0, map_unit_list.Length);
        var prefab = map_unit_list[rand_index];
        var first_unit = Instantiate(prefab, transform);
        first_unit.transform.position = new Vector3(unit_width / 2.0f, 0, 0);
        current_room = first_unit;
    }

    private GameObject current_room = null;
    private GameObject next_room = null;

    public void Update()
    {
        var old_pos = transform.position;
        old_pos.x -= speed * Time.deltaTime;
        transform.position = old_pos;
        if (next_room == null && current_room.transform.position.x <= 30)
        {
            int rand_index = Random.Range(0, map_unit_list.Length);
            var prefab = map_unit_list[rand_index];
            var unit = Instantiate(prefab, transform);
            unit.transform.position = current_room.transform.position + new Vector3(60, 0, 0);
            next_room = unit;
        }

        if (next_room != null && next_room.transform.position.x <= 0)
        {
            GameObject.Destroy(current_room);
            current_room = next_room;
            next_room = null;
            
        }
    }
}
