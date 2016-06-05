using UnityEngine;
using System.Collections;
using RSG;
using System.Collections.Generic;

public class SetTrackedTrajectoryEffect : Effect
{
    public PositionTracker tracker;
    public List<TimedVector3> trajectory;
    public float timestep = 0.02f;

    public override IPromise Run() {
        List<TimedValue<Vector3>> track = new List<TimedValue<Vector3>>();
        for (int i = 0; i < trajectory.Count-1; i++) {
            for (float t = trajectory[i].time; t < trajectory[i + 1].time; t += timestep) {
                track.Add(new TimedValue<Vector3>(Vector3.Lerp(trajectory[i].value, trajectory[i + 1].value, (t - trajectory[i].time) / (trajectory[i + 1].time - trajectory[i].time)), t));
            }
        }
        track.Add(trajectory[trajectory.Count - 1]);
        tracker.Init();
        tracker.tracker.SetTrack(track);
        return Promise.Resolved();
    }
}