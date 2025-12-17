using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // <-- new Input System

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
}
