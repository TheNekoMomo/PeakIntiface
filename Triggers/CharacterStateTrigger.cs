using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BepInEx.Logging;

namespace PeakIntiface.Triggers
{
    public class CharacterStateTrigger
    {
        private readonly ManualLogSource logger;

        private bool alreadyDead = false;
        private bool alreadyPassedOut = false;
        private bool alreadyRagdoll = false;

        private readonly Dictionary<string, CancellationTokenSource> timedVibration = new Dictionary<string, CancellationTokenSource>();

        public CharacterStateTrigger(ManualLogSource logger)
        {
            this.logger = logger;
        }

        public void Update()
        {
            Character character = Character.localCharacter;
            if (character == null || character.data == null) return;

            bool isDead = character.data.dead;
            bool isPassedOut = character.data.passedOut || character.data.fullyPassedOut;
            bool isRagdoll = character.data.fallSeconds > 0f;

            if (isDead && !alreadyDead && ConfigManager.DeadEnabled.Value) OnDeath();
            if (isPassedOut && !alreadyPassedOut && ConfigManager.PassedOutEnabled.Value) OnPassOut();
            if (isRagdoll && !alreadyRagdoll && ConfigManager.RagdollEnabled.Value) OnRagdoll();

            alreadyDead = isDead;
            alreadyPassedOut = isPassedOut;
            alreadyRagdoll = isRagdoll;
        }

        private void OnDeath()
        {
            _ = StartTimedVibration("Death", ConfigManager.DeadIntensity.Value, ConfigManager.DeadTime.Value);
        }

        private void OnPassOut()
        {
            _ = StartTimedVibration("PassOut", ConfigManager.PassedOutIntensity.Value, ConfigManager.PassedOutTime.Value);
        }

        private void OnRagdoll()
        {
            _ = StartTimedVibration("Ragdoll", ConfigManager.RagdollIntensity.Value, ConfigManager.RagdollTime.Value);
        }

        private async Task StartTimedVibration(string VibrationSource, double intensity, float durationMilliseconds)
        {
            if (timedVibration.TryGetValue(VibrationSource, out CancellationTokenSource oldTimer))
            {
                oldTimer.Cancel();
                oldTimer.Dispose();
            }

            CancellationTokenSource timer = new CancellationTokenSource();
            timedVibration[VibrationSource] = timer;

            string source = $"CharacterState-{VibrationSource}";

            TriggerUtils.SetVibration(source, intensity);

            try
            {
                await Task.Delay((int)durationMilliseconds, timer.Token);
            }
            catch (System.Exception)
            {
                return;
            }

            TriggerUtils.SetVibration(source, 0);

            timer.Dispose();
            timedVibration.Remove(VibrationSource);
        }
    }
}
