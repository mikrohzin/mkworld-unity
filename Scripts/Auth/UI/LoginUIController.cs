using TMPro;
using UnityEngine;

public class LoginUIController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private TMP_InputField loginInput;
    [SerializeField] private TMP_InputField passwordInput;

    [Header("Flow")]
    [SerializeField] private LoginFlowController flowController;

    [Header("Character Selection")]
    [SerializeField] private CharacterSelectionUIController characterSelectionUI;

    public void OnClickLogin()
    {
        if (loginInput == null || passwordInput == null)
        {
            Debug.LogError("Inputs não configurados.");
            return;
        }

        bool ok = AuthServiceLocator.AuthService.Login(
            loginInput.text,
            passwordInput.text,
            out AccountData account,
            out string message
        );

        Debug.Log("LOGIN: " + ok + " | " + message);

        if (!ok)
            return;

        if (SessionManager.Instance == null)
        {
            Debug.LogError("SessionManager.Instance está null. Abra o jogo pela cena Boot.");
            return;
        }

        SessionManager.Instance.SetLoggedInAccount(account);

        if (flowController == null)
        {
            Debug.LogError("flowController está null no LoginUIController.");
            return;
        }

        flowController.ShowCharacterSelection();

        if (characterSelectionUI == null)
        {
            Debug.LogError("characterSelectionUI está null no LoginUIController.");
            return;
        }

        characterSelectionUI.RefreshCharacterList();
    }
}