using System.Threading.Tasks;
using BepInEx.Logging;

namespace PeakIntiface.Triggers
{
    public class MovementTrigger
    {
        private readonly ManualLogSource logger;

        public MovementTrigger(ManualLogSource logger)
        {
            this.logger = logger;
        }

        public void Update()
        {
            if (Plugin.ToyController == null) return;
            // Get the local character data and check if it's null (Could be the case when the player is in a menu or loading screen)
            Character character = Character.localCharacter;
            if (character == null) return;
            if (character.data == null) return;

            if (character.data.isClimbing && Plugin.NormalClimbingTriggerEnabled.Value)
            {
                float currentStamina = character.data.currentStamina;
                double intensity = Plugin.NormalClimbingTriggerMinimumIntensity.Value + 
                    (1 - currentStamina) * (Plugin.NormalClimbingTriggerMaximumIntensity.Value - Plugin.NormalClimbingTriggerMinimumIntensity.Value);

                SetVibration(intensity);
            }
            else if (character.data.isRopeClimbing && Plugin.RopeClimbingTriggerEnabled.Value)
            {
                float currentStamina = character.data.currentStamina;
                double intensity = Plugin.RopeClimbingTriggerMinimumIntensity.Value + 
                    (1 - currentStamina) * (Plugin.RopeClimbingTriggerMaximumIntensity.Value - Plugin.RopeClimbingTriggerMinimumIntensity.Value);

                SetVibration(intensity);
            }
            else if (character.data.isVineClimbing && Plugin.VineClimbingTriggerEnabled.Value)
            {
                float currentStamina = character.data.currentStamina;
                double intensity = Plugin.VineClimbingTriggerMinimumIntensity.Value + 
                    (1 - currentStamina) * (Plugin.VineClimbingTriggerMaximumIntensity.Value - Plugin.VineClimbingTriggerMinimumIntensity.Value);

                SetVibration(intensity);
            }
            else if (character.data.isSprinting && Plugin.SprintingTriggerEnabled.Value)
            {
                float currentStamina = character.data.currentStamina;
                double intensity = Plugin.SprintingTriggerMinimumIntensity.Value + 
                    (1 - currentStamina) * (Plugin.SprintingTriggerMaximumIntensity.Value - Plugin.SprintingTriggerMinimumIntensity.Value);

                 SetVibration(intensity);
            }
            else
            {
                SetVibration(0.0);
            }
        }

        private void SetVibration(double intensity)
        {
            Plugin.ToyController.SetSourceIntensity("movement", intensity);
        }
    }
}
