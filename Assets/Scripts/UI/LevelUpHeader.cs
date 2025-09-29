using TMPro;
using UnityEngine;

public class LevelUpHeader : MonoBehaviour
{
    private TextMeshProUGUI _headerText;

    private void Awake()
    {
        _headerText = GetComponent<TextMeshProUGUI>();
    }

    public void SetHeaderText(string text)
    {
        _headerText.text = text;
    }
}
