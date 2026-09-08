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
            if (character == null)
            {
                TriggerUtils.SetVibration("movement", 0);
                return;
            }

            float currentStamina = character.data.currentStamina;
            float maxStamina = character.GetMaxStamina();

            float staminaLeft = maxStamina > 0 ? currentStamina / maxStamina : 0;

            TriggerUtils.SetVibration("movement", 1 - staminaLeft);
        }
    }
}