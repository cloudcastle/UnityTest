using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TrajectoryMover : TimedMover {
    public List<TimedTransform> trajectory;

    protected override Vector3 PositionByTime(float time) {
        int i = 0;
        while (i < trajectory.Count - 2 && trajectory[i + 1].time < time) {
            i++;
        }
        return Vector3.Lerp(trajectory[i].value.position, trajectory[i + 1].value.position, (time - trajectory[i].time) / (trajectory[i + 1].time - trajectory[i].time));
    }

}
