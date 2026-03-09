using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private Button button;

    private CharacterData characterData;
    private Action<CharacterData> onSelected;

    public void Setup(CharacterData data, Action<CharacterData> onSelectedCallback)
    {
        characterData = data;
        onSelected = onSelectedCallback;

        if (labelText != null)
            labelText.text = $"{data.name} - {data.vocation} - Level {data.level}";

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(HandleClick);
        }
    }

    private void HandleClick()
    {
        Debug.Log("ITEM CLICK: " + characterData.name);
        onSelected?.Invoke(characterData);
    }
}