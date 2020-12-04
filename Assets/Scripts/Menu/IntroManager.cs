using UnityEngine.SceneManagement;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [Header("Logos")]
    [SerializeField] private GameObject LogoGA = null;
    [SerializeField] private GameObject LogoGame = null;

    [Header("Logos duration ")]
    [Range(0, 5)]
    [SerializeField] private int logoDuration = 0;

    [Header("Others")]
    [SerializeField] private AudioSource[] sounds = null;


    private float timer = 0f;

    // Start is called before the first frame update
    private void Awake()
    {
        foreach (AudioSource sound in sounds)
        {
            sound.playOnAwake = false;
            sound.volume = 0.5f;
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0f && timer < logoDuration)
        {
            if (!LogoGA.activeSelf)
                LogoGA.SetActive(true);
        }
        else if (timer > logoDuration && timer < logoDuration * 2)
        {
            if (timer > logoDuration && timer < logoDuration + .5f)
                if (!sounds[1].isPlaying)
                    sounds[1].Play();

            if (LogoGA.activeSelf)
                LogoGA.SetActive(false);

            if (!LogoGame.activeSelf)
                LogoGame.SetActive(true);



        }
        else if (timer > logoDuration * 2)
        {
            PlayerPrefs.SetString("SceneNameToLoad", "Menu");
            SceneManager.LoadScene("Loader");
        }

    }
}
