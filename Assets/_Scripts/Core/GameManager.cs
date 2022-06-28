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
    private int fireCountInLevel;

    public bool isPlaying { get => _isPlaying; private set => _isPlaying = value; }
    private bool _isPlaying;

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
        if (fireCountInLevel > 1)
            fireCountInLevel--;
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
        EventDispatcher.Instance.PostEvent(EventID.EndLevel, false);
        isPlaying = false;
        await UniTask.Delay(2000);
        LoadLevel();
    }

    private async UniTask WinLevel()
    {
        EventDispatcher.Instance.PostEvent(EventID.EndLevel, true);
        isPlaying = false;
        await UniTask.Delay(2000);
        LoadLevel();
    }

    public void LoadLevel()
    {
        if (isPlaying) return;

        EventDispatcher.Instance.PostEvent(EventID.LoadLevel);

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
        fireCountInLevel = fireLevelList.Count;
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
