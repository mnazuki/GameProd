using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public struct CutsceneEntry
    {
        [Tooltip("The visual panel or background for this cutscene step.")]
        public GameObject panel;

        [Tooltip("Optional: the dialogue UI to show during this step.")]
        public GameObject dialogue;

        [Tooltip("Seconds to wait before showing the dialogue (if any).")]
        public float dialogueDelay;
    }

    [Tooltip("Define each cutscene in order: panel, optional dialogue, and delay.")]
    public CutsceneEntry[] cutscenes;

    [Tooltip("How long (in seconds) each panel stays visible (after dialogue if present).")]
    public float panelDuration = 4f;

    [Tooltip("Name of the scene to load when all cutscenes finish.")]
    public string nextSceneName = "Loading Scene";

    private IEnumerator Start()
    {
        // Ensure everything is off
        foreach (var entry in cutscenes)
        {
            if (entry.panel != null) entry.panel.SetActive(false);
            if (entry.dialogue != null) entry.dialogue.SetActive(false);
        }

        // Play each in sequence
        foreach (var entry in cutscenes)
        {
            if (entry.panel != null)
                entry.panel.SetActive(true);

            if (entry.dialogue != null)
            {
                yield return new WaitForSeconds(entry.dialogueDelay);
                entry.dialogue.SetActive(true);
            }

            yield return new WaitForSeconds(panelDuration);

            // Turn both off before next
            if (entry.panel != null) entry.panel.SetActive(false);
            if (entry.dialogue != null) entry.dialogue.SetActive(false);
        }

        // All done: load next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
