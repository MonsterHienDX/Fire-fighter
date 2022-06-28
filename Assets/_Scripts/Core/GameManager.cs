using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [SerializeField] private List<GameObject> levelPrefabList = new List<GameObject>();
    [SerializeField] private Transform levelRootTransform;

    private GameObject currentLevel;
    private GameObject currentLevelPrefab;
    private List<Fire> fireLevelList = new List<Fire>();
    private int fireCount;
    public bool isPlaying;

    [SerializeField] private Text winLoseText;

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.FireOut, ReduceFire);
        EventDispatcher.Instance.RegisterListener(EventID.OutOfWater, LoseByOutOfWater);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.FireOut, ReduceFire);
        EventDispatcher.Instance.RemoveListener(EventID.OutOfWater, LoseByOutOfWater);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        LoadLevel();
    }

    private void ReduceFire(object param = null)
    {
        if (fireCount > 1)
            fireCount--;
        else
        {
            _ = WinLevel();
        }
    }

    private void LoseByOutOfWater(object param = null)
    {
        _ = LoseLevel();
    }

    private async UniTask LoseLevel()
    {
        ShowPanelEndLevel(false);
        isPlaying = false;
        await UniTask.Delay(2000);
        LoadLevel();
    }

    private async UniTask WinLevel()
    {
        ShowPanelEndLevel(true);
        isPlaying = false;
        await UniTask.Delay(2000);
        LoadLevel();
    }

    private void ShowPanelEndLevel(bool isWin)
    {
        winLoseText.enabled = true;
        if (isWin)
        {
            winLoseText.text = "Congratulation!";
        }
        else
        {
            winLoseText.text = "Game over!";
        }
    }

    public void LoadLevel()
    {
        if (isPlaying) return;

        winLoseText.enabled = false;
        isPlaying = true;

        if (levelRootTransform.childCount > 0)
            Destroy(levelRootTransform.GetChild(0).gameObject);

        currentLevel = Instantiate(levelPrefabList.PickRandom());
        currentLevelPrefab = currentLevel;
        currentLevel.transform.SetParent(levelRootTransform);

        fireLevelList.Clear();
        foreach (Fire f in currentLevel.gameObject.GetComponentsInChildren<Fire>())
        {
            fireLevelList.Add(f);
        }
        fireCount = fireLevelList.Count;
        Extinguisher.instance.ReFillWater();
    }

    public void NextLevel()
    {
        while (levelRootTransform.childCount > 0)
        {
            Destroy(levelRootTransform.GetChild(0));
        }
        // currentLevelPrefab = null;
        LoadLevel();
    }

    public void ReplayLevel()
    {
        while (levelRootTransform.childCount > 0)
        {
            Destroy(levelRootTransform.GetChild(0));
        }

        // currentLevel = Instantiate(currentLevelPrefab);
        currentLevel.transform.SetParent(levelRootTransform);
    }
}
