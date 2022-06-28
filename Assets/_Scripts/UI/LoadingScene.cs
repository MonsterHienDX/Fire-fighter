using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public bool sceneLoading = false;
    public Image slideBar;

    private string sceneMain = "PersistantScene";

    private string sceneLoad = "Loading";

    private void Awake()
    {
        Application.targetFrameRate = 30;
    }

    // Start is called before the first frame update
    void Start()
    {
        // DontDestroyOnLoad(this);
        LoadScene();
    }

    public void LoadScene()
    {
        StartCoroutine(LoadAyncScene());
    }

    IEnumerator LoadAyncScene()
    {
        sceneLoading = true;

        float loadCount = 0.05f;
        var sizeDelta = slideBar.rectTransform.sizeDelta;
        float maxSize = sizeDelta.x;
        WaitForSeconds seconds = new WaitForSeconds(0.05f);
        Vector2 size = new Vector2(loadCount * maxSize, sizeDelta.y);
        while (loadCount < 0.7f)
        {
            loadCount += 0.02f;
            // slideBar.rectTransform.sizeDelta = new Vector2(loadCount * maxSize, sizeDelta.y);
            slideBar.fillAmount = loadCount;
            yield return seconds;
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneMain);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Progress :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar

        while (asyncOperation.progress < .9f)
        {
            //Output the current progress
            if (loadCount < 1f)
            {
                loadCount = 1f;
                // slideBar.image.fillAmount = loadCount;
                slideBar.rectTransform.sizeDelta = new Vector2(loadCount * maxSize, sizeDelta.y);
            }

            // Check if the load has finished
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
        DelayLoadCanvas();
    }

    public async void DelayLoadCanvas()
    {
        await Task.Delay(1000);
        sceneLoading = false;
    }
}