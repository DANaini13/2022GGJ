using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomCredits : MonoBehaviour
{
    public Text credits_text;
    private List<string> credits_string_list = new List<string> { "东方高数", "程序猿", "Pbbb", "Hey" };
    void Start()
    {
        var seed = System.DateTime.Now.ToString();
        Random.InitState(Animator.StringToHash(seed));
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, credits_string_list.Count);
            string text = credits_string_list[index];
            credits_string_list.RemoveAt(index);
            credits_text.text += text + " ";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
