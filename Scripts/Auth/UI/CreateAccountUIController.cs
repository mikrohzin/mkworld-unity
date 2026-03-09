using TMPro;
using UnityEngine;

public class CreateAccountUIController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private TMP_InputField loginInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField confirmPasswordInput;

    [Header("Flow")]
    [SerializeField] private LoginFlowController flowController;

    public void OnClickCreateAccount()
    {
        if (loginInput == null || passwordInput == null || confirmPasswordInput == null)
        {
            Debug.LogError("Inputs não configurados.");
            return;
        }

        string login = loginInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (password != confirmPassword)
        {
            Debug.Log("Passwords não conferem.");
            return;
        }

        bool ok = AuthServiceLocator.AuthService.Register(login, password, out string message);

        Debug.Log("CREATE ACCOUNT: " + ok + " | " + message);

        if (!ok)
            return;

        loginInput.text = "";
        passwordInput.text = "";
        confirmPasswordInput.text = "";

        if (flowController != null)
            flowController.ShowMainMenu();
    }

    public void OnClickBack()
    {
        if (flowController != null)
            flowController.ShowMainMenu();
    }
}