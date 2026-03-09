using System;
using System.Collections.Generic;

[Serializable]
public class AccountData
{
    public string accountId;
    public string login;
    public string passwordHash;
    public List<CharacterData> characters = new List<CharacterData>();

    public AccountData()
    {
        accountId = Guid.NewGuid().ToString();
    }

    public AccountData(string login, string passwordHash)
    {
        accountId = Guid.NewGuid().ToString();
        this.login = login;
        this.passwordHash = passwordHash;
    }
}