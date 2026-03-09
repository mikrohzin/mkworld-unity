using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance { get; private set; }

    public AccountData CurrentAccount { get; private set; }
    public CharacterData SelectedCharacter { get; private set; }
    public SessionData CurrentSession { get; private set; } = new SessionData();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLoggedInAccount(AccountData account)
    {
        CurrentAccount = account;
        CurrentSession.isLoggedIn = account != null;
        CurrentSession.accountId = account != null ? account.accountId : string.Empty;
    }

    public void SetSelectedCharacter(CharacterData character)
    {
        SelectedCharacter = character;
        CurrentSession.selectedCharacterId = character != null ? character.characterId : string.Empty;
    }

    public void ClearSession()
    {
        CurrentAccount = null;
        SelectedCharacter = null;
        CurrentSession = new SessionData();
    }

    public bool IsLoggedIn()
    {
        return CurrentAccount != null && CurrentSession != null && CurrentSession.isLoggedIn;
    }
}