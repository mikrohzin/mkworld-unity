using UnityEngine;

public class LoginFlowController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject createAccountPanel;
    [SerializeField] private GameObject characterSelectionPanel;
    [SerializeField] private GameObject characterCreationPanel;

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        ActivateOnly(mainMenuPanel);
    }

    public void ShowLogin()
    {
        ActivateOnly(loginPanel);
    }

    public void ShowCreateAccount()
    {
        ActivateOnly(createAccountPanel);
    }

    public void ShowCharacterSelection()
    {
        ActivateOnly(characterSelectionPanel);
    }

    public void ShowCharacterCreation()
    {
        ActivateOnly(characterCreationPanel);
    }

    private void ActivateOnly(GameObject activePanel)
    {
        mainMenuPanel.SetActive(activePanel == mainMenuPanel);
        loginPanel.SetActive(activePanel == loginPanel);
        createAccountPanel.SetActive(activePanel == createAccountPanel);
        characterSelectionPanel.SetActive(activePanel == characterSelectionPanel);
        characterCreationPanel.SetActive(activePanel == characterCreationPanel);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}