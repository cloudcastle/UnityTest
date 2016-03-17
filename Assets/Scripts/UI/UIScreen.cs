using UnityEngine;
using System.Collections;

public class UIScreen : MonoBehaviour {
    public virtual void Show() {
        gameObject.SetActive(true);
    }

    public virtual void Hide() {
        gameObject.SetActive(false);
    }
}
