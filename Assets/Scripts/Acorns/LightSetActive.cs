using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class LightSetActive : MonoBehaviour, ISaveManager
{
    private bool acorn5;
    public void LoadData(GameData data)
    {
        acorn5 = data.acorn5;
    }

    public void SaveData(GameData data) {}

    // Start is called before the first frame update
    void Start()
    {
        if (acorn5)
        {
            gameObject.SetActive(false);
        }
        //an example of how I should code, gameObject.SetActive(!acorn5); is shorter but much less readable
    }
}
