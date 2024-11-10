using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Resources.Load("Prefabs/Choice1"), this.transform);
        Instantiate(Resources.Load("Prefabs/Choice1"), this.transform);
        Instantiate(Resources.Load("Prefabs/Choice1"), this.transform);
    }
}
