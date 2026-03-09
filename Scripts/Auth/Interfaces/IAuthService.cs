using System.Collections.Generic;

public interface IAuthService
{
    bool Register(string login, string password, out string message);
    bool Login(string login, string password, out AccountData account, out string message);

    List<CharacterData> GetCharacters(string accountId);
    bool CreateCharacter(string accountId, string characterName, string vocation, out string message);

    CharacterData GetCharacterById(string accountId, string characterId);

    void Logout();
}