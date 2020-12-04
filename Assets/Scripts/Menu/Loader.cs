using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private Image progressBar = null;

    public enum SceneName
    {
        Intro,
        Menu,
        Game
    }

    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadAsyncOperation()
    {
        int index = 0;
        // AsyncOperation
        //Debug.Log(SceneUtility.GetBuildIndexByScenePath("Scenes/Game"));
        //AsyncOperation load = SceneManager.LoadSceneAsync(SceneUtility.GetBuildIndexByScenePath("Scenes/Game"));

        index = SceneUtility.GetBuildIndexByScenePath("Scenes/" + PlayerPrefs.GetString("SceneNameToLoad"));
        Debug.Log(PlayerPrefs.GetString("SceneNameToLoad"));
        AsyncOperation load = SceneManager.LoadSceneAsync(index);

        while (load.progress < 1)
        {
            progressBar.fillAmount = load.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}