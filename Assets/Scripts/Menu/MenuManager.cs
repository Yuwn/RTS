using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    #region Variables
    // MENU
    [Header("Main Menu Overflow")]
    [SerializeField] private GameObject menu_screen = null;
    [SerializeField] private GameObject options_screen = null;
    [SerializeField] private GameObject credits_screen = null;

    // MUSIC
    [Header("Toggle mute")]
    [SerializeField] private Toggle toggleMusics = null;

    [Header("Musics")]
    [SerializeField] private Slider volMusicSlider = null;
    [SerializeField] private Text volMusicValueText = null;
    [Header("Musics sources")]
    [SerializeField] private AudioSource[] musics = null;

    // SFX
    [Header("Toggle mute")]
    [SerializeField] private Toggle toggleSFX = null;
    [Header("SFX")]
    [SerializeField] private Slider volSFXSlider = null;
    [SerializeField] private Text volSFXValueText = null;
    [Header("Sounds sources")]
    [SerializeField] private AudioSource[] sounds = null;

    private bool musicsON = true;
    private bool sfxON = true;

    #endregion

    #region Getters and Setters

    public static int volMusicValue
    {
        get { return PlayerPrefs.GetInt("volMusicValue", 100); }
        set { PlayerPrefs.SetInt("volMusicValue", value); }
    }

    public static int volSFXValue
    {
        get { return PlayerPrefs.GetInt("volSFXValue", 100); }
        set { PlayerPrefs.SetInt("volSFXValue", value); }
    }

    #endregion

    public enum eSounds
    {
        confirm = 0,
        back
    }


    // Start is called before the first frame update
    void Start()
    {
        //// Musics
        //volMusicSlider.value = volMusicValue;
        //volMusicValueText.text = volMusicValue.ToString() + " %";

        //foreach (AudioSource music in musics)
        //    music.volume = volMusicValue / 100f;

        //musics[0].Play();

        //// Sounds
        //volSFXSlider.value = volSFXValue;
        //volSFXValueText.text = volSFXValue.ToString() + " %";

        //foreach (AudioSource sound in sounds)
        //    sound.volume = volSFXValue / 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (options_screen.activeInHierarchy || credits_screen.activeInHierarchy))
        {
            //PlaySound(1);
            BackToMenu();
        }

        //if (!musics[0].isPlaying && !musics[1].isPlaying)
        //    musics[1].Play();


        MusicsAndSoundToggleGestion();
    }

    public void OnVolumeSliderValueMusicChanged()
    {
        volMusicValue = (int)volMusicSlider.value;
        volMusicValueText.text = volMusicValue.ToString() + " %";

        foreach (AudioSource music in musics)
            music.volume = volMusicValue / 100f;
    }

    public void OnVolumeSliderValueSFXChanged()
    {
        //volSFXValue = (int)volSFXSlider.value;
        //volSFXValueText.text = volSFXValue.ToString() + " %";

        //foreach (AudioSource sound in sounds)
        //    sound.volume = volSFXValue / 100f;
    }

    public void LoadScene_Game()
    {
        CloseEveryScreens(); //only for debug
        //Debug.Log("Launch game");
        PlayerPrefs.SetString("SceneNameToLoad", "Game");
        SceneManager.LoadScene("Loader");
    }

    public void LoadScene_Menu()
    {
        Debug.Log("Launch menu");
        SceneManager.LoadScene("Menu");
    }

    public void Options()
    {
        CloseEveryScreens();
        options_screen.SetActive(true);
    }

    public void Credits()
    {
        CloseEveryScreens();
        credits_screen.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void BackToMenu()
    {
        CloseEveryScreens();
        menu_screen.SetActive(true);
    }

    private void CloseEveryScreens()
    {
        menu_screen.SetActive(false);
        options_screen.SetActive(false);
        credits_screen.SetActive(false);
    }

    public void PlaySound(int _sound)
    {
        //switch (_sound)
        //{
        //    case (int)eSounds.confirm:
        //        if (!sounds[0].isPlaying)
        //            sounds[0].Play();
        //        break;
        //    case (int)eSounds.back:
        //        if (!sounds[1].isPlaying)
        //            sounds[1].Play();
        //        break;
        //    default:
        //        break;
        //}
    }

    public void MuteMusics(bool _truth)
    {
        //foreach (AudioSource music in musics)
        //    music.mute = _truth;
    }

    public void MuteSFX(bool _truth)
    {
        foreach (AudioSource sound in sounds)
            sound.mute = _truth;
    }

    private void MusicsAndSoundToggleGestion()
    {
        if (toggleMusics.isOn)
        {
            if (!musicsON)
            {
                MuteMusics(false);
                musicsON = true;
            }
        }
        else
        {
            if (musicsON)
            {
                MuteMusics(true);
                musicsON = false;
            }
        }

        if (toggleSFX.isOn)
        {
            if (!sfxON)
            {
                MuteSFX(false);
                sfxON = true;
            }
        }
        else
        {
            if (sfxON)
            {
                MuteSFX(true);
                sfxON = false;
            }
        }
    }
}
