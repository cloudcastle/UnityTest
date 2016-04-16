using UnityEngine;
using System.Collections;

public class TransformAnimator : Script
{
    const float eps = 1e-5f;

    public TimedValue<TransformState> previous;
    public TimedValue<TransformState> target;

    public bool animating = false;

    public override void InitInternal() {
        new ValueTracker<TimedValue<TransformState>>(v => previous = v, () => previous);
        new ValueTracker<TimedValue<TransformState>>(v => target = v, () => target);
        new BoolTracker(v => animating = v, () => animating);
    }

    public float phase() {
        return Mathf.Clamp((TimeManager.GameTime - previous.time) / (target.time - previous.time), 0, 1);
    }

    void FixedUpdate() {
        if (animating) {
            transform.localPosition = Vector3.Lerp(previous.value.position, target.value.position, phase());
            transform.localScale = Vector3.Lerp(previous.value.scale, target.value.scale, phase());
            if (phase() > 1 - eps) {
                animating = false;
            }
        }
    }

    public void Animate(TimedValue<TransformState> target) {
        this.previous = new TimedValue<TransformState>(new TransformState(transform.localPosition, transform.localScale), TimeManager.GameTime);
        this.target = target;
        animating = true;
    }

    public void SkipAnimation() {
        transform.localPosition = target.value.position;
        transform.localScale = target.value.scale;
        animating = false;
    }
}