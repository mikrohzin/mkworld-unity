using System;

[Serializable]
public class CharacterData
{
    public string characterId;
    public string name;
    public string vocation;

    public int level = 1;

    public int floor = 0;
    public int x = 100;
    public int y = 100;

    public int hp = 100;
    public int mana = 50;

    public CharacterData()
    {
        characterId = Guid.NewGuid().ToString();
    }

    public CharacterData(string name, string vocation)
    {
        characterId = Guid.NewGuid().ToString();
        this.name = name;
        this.vocation = vocation;
    }
}