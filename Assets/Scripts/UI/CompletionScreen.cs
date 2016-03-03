using UnityEngine;
using System.Collections;

public class CompletionScreen : MonoBehaviour {
    public void Show() {
        gameObject.SetActive(true);
        PauseManager.paused = true;
    }
}
