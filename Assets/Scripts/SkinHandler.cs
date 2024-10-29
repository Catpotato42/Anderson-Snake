using UnityEngine;

public class SkinHandler : MonoBehaviour
{
    void Awake()
    {
        SpriteRenderer skin = gameObject.GetComponent<SpriteRenderer>();
        if (GameManager.instance.SkinPref == "everett") {
            skin.sprite = Resources.Load<Sprite>("Skins/EverettHead");
        } else {
            skin.sprite = Resources.Load<Sprite>("Skins/Square");
        }
    }
}
