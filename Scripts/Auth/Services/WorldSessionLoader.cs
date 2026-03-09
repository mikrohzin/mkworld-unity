using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSessionLoader : MonoBehaviour
{
    [SerializeField] private string loginSceneName = "Login";

    private void Start()
    {
        if (SessionManager.Instance == null || !SessionManager.Instance.IsLoggedIn())
        {
            Debug.LogWarning("[WorldSessionLoader] Sessão inválida. Voltando para Login.");
            SceneManager.LoadScene(loginSceneName);
            return;
        }

        if (SessionManager.Instance.SelectedCharacter == null)
        {
            Debug.LogWarning("[WorldSessionLoader] Nenhum personagem selecionado. Voltando para Login.");
            SceneManager.LoadScene(loginSceneName);
            return;
        }

        CharacterData character = SessionManager.Instance.SelectedCharacter;

        Debug.Log($"[WorldSessionLoader] Personagem carregado: {character.name} | Floor={character.floor} X={character.x} Y={character.y}");
    }
}