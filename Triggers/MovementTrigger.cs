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
            // Get the character class for the player who is triggering events
            Character character = TriggerUtils.GetCharacterForStats();
            if (character == null) return;

            float currentStamina = character.data.currentStamina;
            float maxStamina = character.GetMaxStamina();

            float staminaLeft = maxStamina > 0 ? currentStamina / maxStamina : 0;

            if (character.data.isClimbing && ConfigManager.NormalClimbingTriggerEnabled.Value)
            {
                double intensity = ConfigManager.NormalClimbingTriggerMinimumIntensity.Value +
                    (1 - staminaLeft) * (ConfigManager.NormalClimbingTriggerMaximumIntensity.Value - ConfigManager.NormalClimbingTriggerMinimumIntensity.Value);

                Plugin.ToyController.SetSourceIntensity("movement", intensity);
            }
            else if (character.data.isRopeClimbing && ConfigManager.RopeClimbingTriggerEnabled.Value)
            {
                double intensity = ConfigManager.RopeClimbingTriggerMinimumIntensity.Value +
                    (1 - staminaLeft) * (ConfigManager.RopeClimbingTriggerMaximumIntensity.Value - ConfigManager.RopeClimbingTriggerMinimumIntensity.Value);
                Plugin.ToyController.SetSourceIntensity("movement", intensity);
            }
            else if (character.data.isVineClimbing && ConfigManager.VineClimbingTriggerEnabled.Value)
            {
                double intensity = ConfigManager.VineClimbingTriggerMinimumIntensity.Value +
                    (1 - staminaLeft) * (ConfigManager.VineClimbingTriggerMaximumIntensity.Value - ConfigManager.VineClimbingTriggerMinimumIntensity.Value);
                Plugin.ToyController.SetSourceIntensity("movement", intensity);
            }
            else if (character.data.isSprinting && ConfigManager.SprintingTriggerEnabled.Value)
            {
                double intensity = ConfigManager.SprintingTriggerMinimumIntensity.Value +
                    (1 - staminaLeft) * (ConfigManager.SprintingTriggerMaximumIntensity.Value - ConfigManager.SprintingTriggerMinimumIntensity.Value);
                Plugin.ToyController.SetSourceIntensity("movement", intensity);
            }
            else
            {
                TriggerUtils.SetVibration("movement", 0);
            }
        }
    }
}