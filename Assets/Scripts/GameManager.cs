using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private static string skinPref;
    public string SkinPref {
        get => skinPref;
        set => skinPref = value;
    }

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
