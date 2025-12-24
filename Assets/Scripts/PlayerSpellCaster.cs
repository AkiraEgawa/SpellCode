using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;

public class PlayerSpellCaster : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            // Load spell from persistent file
            string path = Path.Combine(Application.persistentDataPath, "SavedSpell.txt");

            if (!File.Exists(path))
            {
                Debug.LogError("Saved spell file not found at: " + path);
                return;
            }

            string spellText = File.ReadAllText(path);

            // Use singleton instance of GameController
            GameController.Instance.CastSpell(spellText);

            Debug.Log("Casting spell:\n" + spellText);
        }
    }
}