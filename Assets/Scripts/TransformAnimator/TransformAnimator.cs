﻿using UnityEngine;
using System.Collections;
using RSG;
using System;

public class TransformAnimator : Script
{
    const float eps = 1e-5f;

    public TimedValue<TransformState> previous;
    public TimedValue<TransformState> target;

    public bool animating = false;
    public UndoablePromise finishAnimation = null;

    public Func<float> getTime = () => TimeManager.GameTime;

    public override void InitInternal() {
        new ValueTracker<TimedValue<TransformState>>(v => previous = v, () => previous);
        new ValueTracker<TimedValue<TransformState>>(v => target = v, () => target);
        new BoolTracker(v => animating = v, () => animating);
    }

    public float phase() {
        return Mathf.Clamp((getTime() - previous.time) / (target.time - previous.time), 0, 1);
    }

    void FixedUpdate() {
        if (animating) {
            if (phase() > 1 - eps) {
                ToAnimationEnd();
            } else {
                transform.localPosition = Vector3.Lerp(previous.value.position, target.value.position, phase());
                transform.localRotation = Quaternion.Lerp(previous.value.rotation, target.value.rotation, phase());
                transform.localScale = Vector3.Lerp(previous.value.scale, target.value.scale, phase());
            }
        }
    }

    public IPromise Animate(TimedValue<TransformState> target) {
        this.previous = new TimedValue<TransformState>(new TransformState(transform.localPosition, transform.localRotation, transform.localScale), getTime());
        this.target = target;
        finishAnimation = new UndoablePromise();
        animating = true;
        return finishAnimation.WithName(string.Format("Animate {0}", name));
    }

    void ToAnimationEnd() {
        transform.localPosition = target.value.position;
        transform.localRotation = target.value.rotation;
        transform.localScale = target.value.scale;
        animating = false;
        finishAnimation.Resolve();
    }

    public void SkipAnimation() {
        ToAnimationEnd();
    }
}