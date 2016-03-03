using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
    public static UI instance;

    public CompletionScreen completionScreen;

    void Awake() {
        instance = this;
    }
}
