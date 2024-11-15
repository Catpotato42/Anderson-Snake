using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChoiceHandler : MonoBehaviour
{
    [SerializeField] private GameObject choice;
    void Start()
    {
        Instantiate(choice, this.transform);
        Instantiate(choice, this.transform);
        Instantiate(choice, this.transform);
    }
}
