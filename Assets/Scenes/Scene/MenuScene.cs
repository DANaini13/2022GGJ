using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 1;
        loading = false;
    }

    private bool loading = false;
    void Update()
    {
        if (loading) return;
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
            loading = true;
        }
    }
}
