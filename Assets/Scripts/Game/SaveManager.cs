using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Serializable]
    private class Entity
    {
        public int health;
        public Vector3 position;
    }

    [Serializable]
    private class GameInfos
    {
        public Entity[] entities;
    }
    GameInfos gameInfos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            LoadSave();
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            Save();
        }
    }


    private void LoadSave()
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/save.json");
        Debug.Log(json);
        gameInfos = JsonUtility.FromJson<GameInfos>(json);
    }

    private void Save()
    {
        foreach (Entity entity in gameInfos.entities)
        {
            entity.health = 100;
            entity.position = new Vector3(0, 0, 0);
        }
        string toJson = JsonUtility.ToJson(gameInfos, true);
        File.WriteAllText(Application.dataPath + "/Resources/save.json", toJson);
    }
}
