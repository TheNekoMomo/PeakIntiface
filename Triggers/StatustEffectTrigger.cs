using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BepInEx.Logging;

namespace PeakIntiface.Triggers
{
    public class StatustEffectTrigger
    {
        private readonly ManualLogSource logger;

        private Character subscribedCharacter = null;
        private readonly Dictionary<CharacterAfflictions.STATUSTYPE, CancellationTokenSource> afflictionIntensities = 
            new Dictionary<CharacterAfflictions.STATUSTYPE, CancellationTokenSource>();

        public StatustEffectTrigger(ManualLogSource logger)
        {
            this.logger = logger;
        }

        internal void Update()
        {
            // Check if the local character is valid and has afflictions
            Character character = Character.localCharacter;
            if (character == null || character.refs.afflictions == null) return;
            // Check if the character has changed
            if (subscribedCharacter == character) return;

            // Unsubscribe from the previous character's affliction events
            if (subscribedCharacter != null)
            {
                subscribedCharacter.refs.afflictions.OnAddedIncrementalStatus -= OnAfflictionAdded;
            }

            subscribedCharacter = character;
            subscribedCharacter.refs.afflictions.OnAddedIncrementalStatus += OnAfflictionAdded;
            logger.LogInfo("Subscribed to status effects.");
        }

        private void OnAfflictionAdded(CharacterAfflictions.STATUSTYPE sTATUSTYPE, float amount)
        {
            switch (sTATUSTYPE)
            {
                case CharacterAfflictions.STATUSTYPE.Injury:
                    double InjuryIntensity = ConfigManager.InjuryTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.InjuryTriggerMaximumIntensity.Value - ConfigManager.InjuryTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, InjuryIntensity, ConfigManager.InjuryTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Hunger:
                    double hungerIntensity = ConfigManager.HungerTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.HungerTriggerMaximumIntensity.Value - ConfigManager.HungerTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, hungerIntensity, ConfigManager.HungerTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Cold:
                    double coldIntensity = ConfigManager.ColdTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.ColdTriggerMaximumIntensity.Value - ConfigManager.ColdTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, coldIntensity, ConfigManager.ColdTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Poison:
                    double poisonIntensity = ConfigManager.PoisonTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.PoisonTriggerMaximumIntensity.Value - ConfigManager.PoisonTriggerMinimumIntensity.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Curse:
                    double curseIntensity = ConfigManager.CurseTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.CurseTriggerMaximumIntensity.Value - ConfigManager.CurseTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, curseIntensity, ConfigManager.CurseTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Drowsy:
                    double drowsyIntensity = ConfigManager.DrowsyTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.DrowsyTriggerMaximumIntensity.Value - ConfigManager.DrowsyTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, drowsyIntensity, ConfigManager.DrowsyTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Weight:
                    break;
                case CharacterAfflictions.STATUSTYPE.Hot:
                    double hotIntensity = ConfigManager.HotTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.HotTriggerMaximumIntensity.Value - ConfigManager.HotTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, hotIntensity, ConfigManager.HotTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Thorns:
                    double thornsIntensity = ConfigManager.ThornsTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.ThornsTriggerMaximumIntensity.Value - ConfigManager.ThornsTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, thornsIntensity, ConfigManager.ThornsTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Spores:
                    double sporesIntensity = ConfigManager.SporesTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.SporesTriggerMaximumIntensity.Value - ConfigManager.SporesTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, sporesIntensity, ConfigManager.SporesTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Web:
                    double webIntensity = ConfigManager.WebTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.WebTriggerMaximumIntensity.Value - ConfigManager.WebTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, webIntensity, ConfigManager.WebTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Arrow:
                    double arrowIntensity = ConfigManager.ArrowTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.ArrowTriggerMaximumIntensity.Value - ConfigManager.ArrowTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, arrowIntensity, ConfigManager.ArrowTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.Petrify:
                    double PetrifyIntensity = ConfigManager.PetrifyTriggerMinimumIntensity.Value +
                        amount * (ConfigManager.PetrifyTriggerMaximumIntensity.Value - ConfigManager.PetrifyTriggerMinimumIntensity.Value);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, PetrifyIntensity, ConfigManager.PetrifyTriggerDuration.Value);
                    break;
                case CharacterAfflictions.STATUSTYPE.FlyTrap:
                    break;
            }
        }

        private async Task HandleAfflictionIntensity(CharacterAfflictions.STATUSTYPE sTATUSTYPE, double intensity, float durationMilliseconds)
        {
            if (afflictionIntensities.TryGetValue(sTATUSTYPE, out CancellationTokenSource oldTimer))
            {
                oldTimer.Cancel();
                oldTimer.Dispose();
            }

            CancellationTokenSource timer = new CancellationTokenSource();
            afflictionIntensities[sTATUSTYPE] = timer;

            string source = $"StatustEffectTrigger-{sTATUSTYPE}";

            Plugin.ToyController.SetSourceIntensity(source, intensity);

            try
            {
                await Task.Delay((int)durationMilliseconds, timer.Token);
            }
            catch (System.Exception)
            {
                return;
            }

            Plugin.ToyController.SetSourceIntensity(source, 0);

            timer.Dispose();
            afflictionIntensities.Remove(sTATUSTYPE);
        }
    }
}
