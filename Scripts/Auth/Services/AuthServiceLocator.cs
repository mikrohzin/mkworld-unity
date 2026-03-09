public static class AuthServiceLocator
{
    private static IAuthService authService;

    public static IAuthService AuthService
    {
        get
        {
            if (authService == null)
                authService = new LocalAuthService();

            return authService;
        }
    }

    public static void SetAuthService(IAuthService service)
    {
        authService = service;
    }

    public static void Reset()
    {
        authService = null;
    }
}