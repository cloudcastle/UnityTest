using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RSG;

namespace Terminal
{
    public class PromisesTraining
    {
        public abstract class Effect
        {
            public abstract IPromise Run();
        }

        public class DelayedEffect : Effect
        {
            string name;

            Promise promise;

            public DelayedEffect(string name) {
                this.name = name;
            }

            void Resolve() {
                promise.Resolve();
                onTick -= Resolve;
            }

            public override IPromise Run() {
                Console.WriteLine("DelayedEffect run: " + name);
                promise = new Promise();
                onTick += Resolve;
                return promise;
            }
        }

        public class ComposedEffect : Effect
        {
            IEnumerable<Effect> effects;

            public ComposedEffect(params Effect[] effects) {
                this.effects = effects;
            }

            public override IPromise Run() {
                Console.WriteLine("ComposedEffect run");
                return Promise.Sequence(effects.Select(effect => (Func<IPromise>)(() => effect.Run())));
            }
        }

        public static event Action onTick = () => {};
        public static int tick = 0;

        public static void Tick() {
            ++tick;
            Console.WriteLine("Tick " + tick);
            onTick();
        }

        public static void RunThreeContinuousEffects() {
            new ComposedEffect(new DelayedEffect("A"), new DelayedEffect("B"), new DelayedEffect("C")).Run();
            for (int i = 0; i < 10; i++) {
                Tick();
            }
        }
    }
}
