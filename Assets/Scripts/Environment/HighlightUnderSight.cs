using UnityEngine;

public class HighlightUnderSight : Highlight
{
    bool UnderSight() {
        return Eye.instance.underSight == gameObject;
    }

    void Update() {
        Switch(UnderSight());
    }
}