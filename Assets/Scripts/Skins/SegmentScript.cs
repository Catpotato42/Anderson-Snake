using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentScript : MonoBehaviour
{
    void Awake () {
        SpriteRenderer skin = gameObject.GetComponent<SpriteRenderer>();
        if (GameManager.instance.SkinPref == "everett") {
            skin.sprite = Resources.Load<Sprite>("Skins/EverettHead");
        } else {
            skin.sprite = Resources.Load<Sprite>("Skins/Square");
        }
    }
}
