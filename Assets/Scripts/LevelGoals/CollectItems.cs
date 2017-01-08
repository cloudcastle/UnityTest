using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CollectItems : MonoBehaviour
{
    public int collected;
    public int required;

    public Text text;

    void Start() {
        required = FindObjectsOfType<Bonus>().Count();
        new ValueTracker<int>(x => collected = x, () => collected);
        FindObjectsOfType<Bonus>().ForEach(b => b.onPicked.AddListener(() => {
            collected++;
            if (collected == required) {
                GameManager.instance.CompleteLevel();
            }
        }));
    }

    void Update() {
        text.text = "{0}/{1}".i(collected, required);
    }
}
