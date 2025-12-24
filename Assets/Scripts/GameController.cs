using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public SpellRuntime spellRuntime;

    Dictionary<string, string> lexicon = new();
    Dictionary<string, Action> commandTable = new();

    // Loads lexicon.txt in Assets/SteamingAssets/lexicon.txt
    void LoadLexicon()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "lexicon.txt");

        foreach (string line in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(' ');
            lexicon[parts[0]] = parts[1];
        }
    }

    void BuildCommandTable()
    {
        commandTable["Add"] = spellRuntime.Add;
        commandTable["Subtract"] = spellRuntime.Subtract;
        commandTable["Multiply"] = spellRuntime.Multiply;
        commandTable["Divide"] = spellRuntime.Divide;
        commandTable["Power"] = spellRuntime.Power;
        commandTable["Sin"] = spellRuntime.Sin;
        commandTable["Cos"] = spellRuntime.Cos;
        commandTable["Tan"] = spellRuntime.Tan;

        // Numbers 1–9 (as “commands” that push the number)
        commandTable["1"] = () => spellRuntime.PushNumber(1);
        commandTable["2"] = () => spellRuntime.PushNumber(2);
        commandTable["3"] = () => spellRuntime.PushNumber(3);
        commandTable["4"] = () => spellRuntime.PushNumber(4);
        commandTable["5"] = () => spellRuntime.PushNumber(5);
        commandTable["6"] = () => spellRuntime.PushNumber(6);
        commandTable["7"] = () => spellRuntime.PushNumber(7);
        commandTable["8"] = () => spellRuntime.PushNumber(8);
        commandTable["9"] = () => spellRuntime.PushNumber(9);
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (spellRuntime == null)
        {
            spellRuntime = GetComponent<SpellRuntime>();
            if (spellRuntime == null)
            {
                spellRuntime = gameObject.AddComponent<SpellRuntime>();
            }
        }

        LoadLexicon();
        BuildCommandTable();
    }

    public void CastSpell(string spellText)
    {
        spellRuntime.ClearStack();

        string[] tokens = spellText.Split(
            new[] { ' ', '\n', '\t' },
            StringSplitOptions.RemoveEmptyEntries
        );

        foreach (string token in tokens)
        {
            ExecuteToken(token);
        }
    }

    void ExecuteToken(string token)
    {
        // Number?
        if (float.TryParse(token, out float number))
        {
            spellRuntime.PushNumber(number);
            return;
        }

        // Eldritch word?
        if (!lexicon.TryGetValue(token, out string commandName))
            throw new Exception($"Unknown word: {token}");

        // Execute
        if (!commandTable.TryGetValue(commandName, out Action command))
            throw new Exception($"Unimplemented command: {commandName}");

        command.Invoke();

        // Debug stack after command
        Debug.Log($"After '{token}', stack: {spellRuntime.StackToString()}");
    }
}