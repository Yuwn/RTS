using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            ResourcesManager.instance.AddFood(100);
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            ResourcesManager.instance.AddWood(100);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
