using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsText : MonoBehaviour
{
    public Text[] credits_text_p1;
    public Text[] credits_text_p2;
    private List<string> credits_string_list = new List<string> { "东方高数", "程序猿", "Pbbb", "Hey" };

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, credits_string_list.Count);
            string text = credits_string_list[index];
            credits_string_list.RemoveAt(index);
            credits_text_p1[i].text = text;
            credits_text_p2[i].text = text;
        }
    }
}
