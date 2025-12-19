using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.IO;

public class SpellBookController : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        // Automatically find the TMP_InputField if not assigned
        if (inputField == null)
        {
            inputField = GetComponentInChildren<TMP_InputField>();
        }

        if (inputField != null)
        {
            inputField.ActivateInputField();
        }
        else
        {
            Debug.LogError("TMP_InputField not found in children!");
        }

        LoadSpellFromFile();
    }

    void Update()
    {
        // Use new Input System to check Escape key
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ExitSpellCoding();
        }
    }

    void ExitSpellCoding()
    {
        string spellText = inputField.text;
        UnityEngine.SceneManagement.SceneManager.LoadScene("AkiraTestScene");
    }

    public void SaveSpell()
    {
        if (inputField == null)
        {
            Debug.LogError("Input field is null, cannot save spell.");
            return;
        }

        string spellText = inputField.text;

        string path = Path.Combine(
            Application.persistentDataPath,
            "SavedSpell.txt"
        );

        File.WriteAllText(path, spellText);

        Debug.Log("Spell saved to" + path);
    }

    void LoadSpellFromFile()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "SavedSpell.txt"
        );

        if (File.Exists(path))
        {
            inputField.text = File.ReadAllText(path);
            inputField.ActivateInputField();
        }
    }

}
