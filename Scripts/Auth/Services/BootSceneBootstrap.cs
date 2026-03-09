using UnityEngine;
using UnityEngine.SceneManagement;

public class BootSceneBootstrap : MonoBehaviour
{
    [SerializeField] private string loginSceneName = "Login";

    private void Start()
    {
        EnsureSessionManagerExists();
        EnsureAuthServiceExists();

        SceneManager.LoadScene(loginSceneName);
    }

    private void EnsureSessionManagerExists()
    {
        if (SessionManager.Instance != null)
            return;

        GameObject go = new GameObject("SessionManager");
        go.AddComponent<SessionManager>();
    }

    private void EnsureAuthServiceExists()
    {
        _ = AuthServiceLocator.AuthService;
    }
}