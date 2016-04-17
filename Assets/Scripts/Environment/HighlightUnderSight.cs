using UnityEngine;

public class HighlightUnderSight : Highlight
{
    bool UnderSight() {
        return Player.instance.current.eye.underSight == gameObject;
    }

    void Update() {
        Switch(UnderSight());
    }
}