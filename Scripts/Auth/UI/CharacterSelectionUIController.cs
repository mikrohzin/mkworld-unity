using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionUIController : MonoBehaviour
{
    [SerializeField] private Transform listContainer;
    [SerializeField] private CharacterListItemUI itemPrefab;
    [SerializeField] private LoginFlowController flowController;
    [SerializeField] private string worldSceneName = "Main";

    private CharacterData selectedCharacter;

    public void RefreshCharacterList()
    {
        ClearList();

        if (SessionManager.Instance == null || SessionManager.Instance.CurrentAccount == null)
        {
            Debug.LogWarning("Nenhuma conta logada.");
            return;
        }

        List<CharacterData> characters = AuthServiceLocator.AuthService.GetCharacters(
            SessionManager.Instance.CurrentAccount.accountId
        );

        selectedCharacter = null;
        SessionManager.Instance.SetSelectedCharacter(null);

        foreach (CharacterData character in characters)
        {
            CharacterListItemUI item = Instantiate(itemPrefab, listContainer);
            item.Setup(character, OnCharacterSelected);
        }

        Debug.Log("Characters loaded: " + characters.Count);
    }

    public void OnCharacterSelected(CharacterData character)
    {
        selectedCharacter = character;
        SessionManager.Instance.SetSelectedCharacter(character);
        Debug.Log("SELECTED CHARACTER: " + character.name);
    }

    public void OnClickEnterWorld()
    {
        if (selectedCharacter == null)
        {
            Debug.Log("Selecione um personagem antes de entrar.");
            return;
        }

        Debug.Log("ENTER WORLD: " + selectedCharacter.name);
        SceneManager.LoadScene(worldSceneName);
    }

    public void OnClickOpenCreateCharacter()
    {
        if (flowController != null)
            flowController.ShowCharacterCreation();
    }

    public void OnClickLogout()
    {
        if (SessionManager.Instance != null)
            SessionManager.Instance.ClearSession();

        selectedCharacter = null;
        ClearList();

        if (flowController != null)
            flowController.ShowMainMenu();
    }

    private void ClearList()
    {
        if (listContainer == null)
            return;

        for (int i = listContainer.childCount - 1; i >= 0; i--)
            Destroy(listContainer.GetChild(i).gameObject);
    }
}