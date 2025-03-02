using UnityEngine;

public class ChoiceHandler : MonoBehaviour, ISaveManager
{
    private int extraChoices = 0;
    public void LoadData (GameData data) {
        extraChoices = data.extraChoices;
    }
    public void SaveData (GameData data) {
    }
    [SerializeField] private GameObject choice;
    void Start()
    {
        Instantiate(choice, this.transform);
        Instantiate(choice, this.transform);
        Instantiate(choice, this.transform);
        for (int i = 0; i < extraChoices; i++) {
            Instantiate(choice, this.transform);
        }
    }
}
