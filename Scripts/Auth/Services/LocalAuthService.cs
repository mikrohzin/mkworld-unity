using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class LocalAuthService : IAuthService
{
    [Serializable]
    private class AccountsDatabase
    {
        public List<AccountData> accounts = new List<AccountData>();
    }

    private readonly string filePath;
    private AccountsDatabase database;

    public LocalAuthService()
    {
        filePath = Path.Combine(Application.persistentDataPath, "accounts.json");
        LoadDatabase();
    }

    public bool Register(string login, string password, out string message)
    {
        login = SanitizeLogin(login);
        password = password?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(login))
        {
            message = "Login inválido.";
            return false;
        }

        if (password.Length < 4)
        {
            message = "A senha deve ter pelo menos 4 caracteres.";
            return false;
        }

        if (database.accounts.Any(a => a.login.Equals(login, StringComparison.OrdinalIgnoreCase)))
        {
            message = "Esse login já existe.";
            return false;
        }

        string passwordHash = ComputeSha256(password);
        AccountData newAccount = new AccountData(login, passwordHash);

        database.accounts.Add(newAccount);
        SaveDatabase();

        message = "Conta criada com sucesso.";
        return true;
    }

    public bool Login(string login, string password, out AccountData account, out string message)
    {
        account = null;

        login = SanitizeLogin(login);
        password = password?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            message = "Informe login e senha.";
            return false;
        }

        string passwordHash = ComputeSha256(password);

        account = database.accounts.FirstOrDefault(a =>
            a.login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
            a.passwordHash == passwordHash);

        if (account == null)
        {
            message = "Login ou senha inválidos.";
            return false;
        }

        message = "Login realizado com sucesso.";
        return true;
    }

    public List<CharacterData> GetCharacters(string accountId)
    {
        AccountData account = database.accounts.FirstOrDefault(a => a.accountId == accountId);

        if (account == null)
            return new List<CharacterData>();

        return new List<CharacterData>(account.characters);
    }

    public bool CreateCharacter(string accountId, string characterName, string vocation, out string message)
    {
        AccountData account = database.accounts.FirstOrDefault(a => a.accountId == accountId);

        if (account == null)
        {
            message = "Conta não encontrada.";
            return false;
        }

        characterName = SanitizeCharacterName(characterName);
        vocation = string.IsNullOrWhiteSpace(vocation) ? "Adventurer" : vocation.Trim();

        if (!IsValidCharacterName(characterName))
        {
            message = "Nome de personagem inválido.";
            return false;
        }

        bool nameAlreadyExists = database.accounts
            .SelectMany(a => a.characters)
            .Any(c => c.name.Equals(characterName, StringComparison.OrdinalIgnoreCase));

        if (nameAlreadyExists)
        {
            message = "Esse nome de personagem já existe.";
            return false;
        }

        CharacterData character = new CharacterData(characterName, vocation)
        {
            level = 1,
            floor = 0,
            x = 100,
            y = 100,
            hp = 100,
            mana = 50
        };

        account.characters.Add(character);
        SaveDatabase();

        message = "Personagem criado com sucesso.";
        return true;
    }

    public CharacterData GetCharacterById(string accountId, string characterId)
    {
        AccountData account = database.accounts.FirstOrDefault(a => a.accountId == accountId);

        if (account == null)
            return null;

        return account.characters.FirstOrDefault(c => c.characterId == characterId);
    }

    public void Logout()
    {
        // Sem estado interno de sessão aqui.
        // A sessão ficará centralizada no SessionManager.
    }

    private void LoadDatabase()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                database = new AccountsDatabase();
                SaveDatabase();
                return;
            }

            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                database = new AccountsDatabase();
                SaveDatabase();
                return;
            }

            database = JsonUtility.FromJson<AccountsDatabase>(json);

            if (database == null)
                database = new AccountsDatabase();

            if (database.accounts == null)
                database.accounts = new List<AccountData>();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LocalAuthService] Erro ao carregar database: {ex.Message}");
            database = new AccountsDatabase();
        }
    }

    private void SaveDatabase()
    {
        try
        {
            string json = JsonUtility.ToJson(database, true);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LocalAuthService] Erro ao salvar database: {ex.Message}");
        }
    }

    private string SanitizeLogin(string login)
    {
        return login?.Trim().ToLower() ?? string.Empty;
    }

    private string SanitizeCharacterName(string characterName)
    {
        return characterName?.Trim() ?? string.Empty;
    }

    private bool IsValidCharacterName(string characterName)
    {
        if (string.IsNullOrWhiteSpace(characterName))
            return false;

        if (characterName.Length < 3 || characterName.Length > 16)
            return false;

        foreach (char c in characterName)
        {
            if (!(char.IsLetter(c) || c == ' '))
                return false;
        }

        return true;
    }

    private string ComputeSha256(string rawData)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }
    }
}