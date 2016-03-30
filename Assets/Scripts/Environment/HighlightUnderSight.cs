using UnityEngine;

public class HighlightUnderSight : Highlight
{
    bool UnderSight() {
        return Player.current.eye.underSight == gameObject;
    }

    void Update() {
        Switch(UnderSight());
    }
}