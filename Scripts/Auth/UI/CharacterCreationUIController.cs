using TMPro;
using UnityEngine;

public class CharacterCreationUIController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private TMP_InputField characterNameInput;
    [SerializeField] private TMP_Dropdown vocationDropdown;

    [Header("Flow")]
    [SerializeField] private LoginFlowController flowController;

    [Header("Character Selection")]
    [SerializeField] private CharacterSelectionUIController characterSelectionUI;

    public void OnClickCreateCharacter()
    {
        if (SessionManager.Instance == null || SessionManager.Instance.CurrentAccount == null)
        {
            Debug.LogError("Nenhuma conta logada.");
            return;
        }

        string name = characterNameInput.text;

        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.Log("Nome do personagem inválido.");
            return;
        }

        string vocation = GetSelectedVocation();

        bool ok = AuthServiceLocator.AuthService.CreateCharacter(
            SessionManager.Instance.CurrentAccount.accountId,
            name,
            vocation,
            out string message
        );

        Debug.Log("CREATE CHARACTER: " + ok + " | " + message);

        if (!ok)
            return;

        characterNameInput.text = "";

        if (flowController != null)
            flowController.ShowCharacterSelection();

        if (characterSelectionUI != null)
            characterSelectionUI.RefreshCharacterList();
    }

    public void OnClickBack()
    {
        if (flowController != null)
            flowController.ShowCharacterSelection();
    }

    private string GetSelectedVocation()
    {
        if (vocationDropdown == null || vocationDropdown.options.Count == 0)
            return "Knight";

        return vocationDropdown.options[vocationDropdown.value].text;
    }
}