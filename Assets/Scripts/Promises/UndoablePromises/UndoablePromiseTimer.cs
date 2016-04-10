using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSG
{
    public class UndoablePromiseTimer : IPromiseTimer
    {
        public UndoablePromiseTimer(Func<float> getCurrentTime) {
            this.getCurrentTime = getCurrentTime;
            new ListShallowTracker<PredicateWait>((v) => waiting = v, () => waiting);
        }

        /// <summary>
        /// The current running total for time that this PromiseTimer has run for
        /// </summary>
        private Func<float> getCurrentTime;

        /// <summary>
        /// Currently pending promises
        /// </summary>
        private List<PredicateWait> waiting = new List<PredicateWait>();

        /// <summary>
        /// Resolve the returned promise once the time has elapsed
        /// </summary>
        public IPromise WaitFor(float seconds) {
            return WaitUntil(t => t.elapsedTime >= seconds);
        }

        /// <summary>
        /// Resolve the returned promise once the predicate evaluates to false
        /// </summary>
        public IPromise WaitWhile(Func<TimeData, bool> predicate) {
            return WaitUntil(t => !predicate(t));
        }

        /// <summary>
        /// Resolve the returned promise once the predicate evalutes to true
        /// </summary>
        public IPromise WaitUntil(Func<TimeData, bool> predicate) {
            var promise = new UndoablePromise();

            var wait = new PredicateWait()
            {
                timeStarted = getCurrentTime(),
                pendingPromise = promise,
                timeData = new TimeData(),
                predicate = predicate
            };

            waiting.Add(wait);

            return promise;
        }

        /// <summary>
        /// Update all pending promises. Must be called for the promises to progress and resolve at all.
        /// </summary>
        public void Update(float deltaTime) {
            int i = 0;
            while (i < waiting.Count) {
                var wait = waiting[i];

                var newElapsedTime = getCurrentTime() - wait.timeStarted;
                wait.timeData.deltaTime = newElapsedTime - wait.timeData.elapsedTime;
                wait.timeData.elapsedTime = newElapsedTime;

                bool result;
                try {
                    result = wait.predicate(wait.timeData);
                } catch (Exception ex) {
                    wait.pendingPromise.Reject(ex);
                    waiting.RemoveAt(i);
                    continue;
                }

                if (result) {
                    wait.pendingPromise.Resolve();
                    waiting.RemoveAt(i);
                } else {
                    i++;
                }
            }
        }
    }
}