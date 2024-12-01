using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    //Ok so I know the way I'm doing it is dumb in a readability way but I saw online that like the Mathf lerp is different or something?
    //and that was enough to convince me to just write some quick fake code. I'll fix it later fs.
    private Vector3 xpValue = new Vector3(0, 0, 0);
    private Vector3 xpMax = new Vector3(5, 0, 0);
    private Vector3 xpFirst = new Vector3(0, 0, 0);
    [SerializeField] private const float duration = .5f;
    private float elapsedTime = 0f;
    private Slider xpBar;
    private Vector3 barVal = new Vector3(0f, 0, 0);
    void Start()
    {
        xpBar = gameObject.GetComponent<Slider>();
        xpBar.value = 0f;
        Player.instance.OnXPIncrease += UpdateXPValues;
        Player.instance.OnReset += UpdateXPValues;
    }

    public void UpdateXPValues()
    {
        xpMax.x = Player.instance.growThreshold[Player.instance.UpgradeNumber];
        xpValue.x = Player.instance.XpScore;
    }

    void Update () {
        elapsedTime += Time.deltaTime;
        float percentComplete = elapsedTime/duration;
        barVal = Vector3.Lerp(xpFirst, xpValue, percentComplete);
        if (percentComplete > .95f) {
            xpFirst.x = barVal.x;
            elapsedTime = 0f;
        }
        xpBar.maxValue = xpMax.x;
        xpBar.value = barVal.x;
    }
}
