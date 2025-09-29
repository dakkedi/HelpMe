using TMPro;
using UnityEngine;

public class LevelUpDescription : MonoBehaviour
{
    private TextMeshProUGUI _descriptionText;

    private void Awake()
    {
        _descriptionText = GetComponent<TextMeshProUGUI>();
    }

    public void SetDescriptionText(string text)
    {
        _descriptionText.text = text;
    }
}
