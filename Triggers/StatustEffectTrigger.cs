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
            // Get the character class for the player who is triggering events
            Character character = TriggerUtils.GetCharacterForStats();
            if (character == null) return;

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
                    if (!ConfigManager.InjuryTriggerEnabled.Value) break;
                    double InjuryIntensity = TriggerUtils.CalculateIntensity(amount, 
                        ConfigManager.InjuryTriggerMaximumIntensity.Value, ConfigManager.InjuryTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, InjuryIntensity, ConfigManager.InjuryTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Cold:
                    if (!ConfigManager.ColdTriggerEnabled.Value) break;
                    double coldIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.ColdTriggerMaximumIntensity.Value, ConfigManager.ColdTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, coldIntensity, ConfigManager.ColdTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Poison:
                    if (!ConfigManager.PoisonTriggerEnabled.Value) break;
                    double poisonIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.PoisonTriggerMaximumIntensity.Value, ConfigManager.PoisonTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, poisonIntensity, ConfigManager.PoisonTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Curse:
                    if (!ConfigManager.CurseTriggerEnabled.Value) break;
                    double curseIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.CurseTriggerMaximumIntensity.Value, ConfigManager.CurseTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, curseIntensity, ConfigManager.CurseTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Drowsy:
                    if (!ConfigManager.DrowsyTriggerEnabled.Value) break;
                    double drowsyIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.DrowsyTriggerMaximumIntensity.Value, ConfigManager.DrowsyTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, drowsyIntensity, ConfigManager.DrowsyTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Hot:
                    if (!ConfigManager.HotTriggerEnabled.Value) break;
                    double hotIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.HotTriggerMaximumIntensity.Value, ConfigManager.HotTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, hotIntensity, ConfigManager.HotTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Thorns:
                    if (ConfigManager.ThornsTriggerEnabled.Value) break;
                    double thornsIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.ThornsTriggerMaximumIntensity.Value, ConfigManager.ThornsTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, thornsIntensity, ConfigManager.ThornsTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Spores:
                    if (!ConfigManager.SporesTriggerEnabled.Value) break;
                    double sporesIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.SporesTriggerMaximumIntensity.Value, ConfigManager.SporesTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, sporesIntensity, ConfigManager.SporesTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Web:
                    if (!ConfigManager.WebTriggerEnabled.Value) break;
                    double webIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.WebTriggerMaximumIntensity.Value, ConfigManager.WebTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, webIntensity, ConfigManager.WebTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Arrow:
                    if (!ConfigManager.ArrowTriggerEnabled.Value) break;
                    double arrowIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.ArrowTriggerMaximumIntensity.Value, ConfigManager.ArrowTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, arrowIntensity, ConfigManager.ArrowTriggerDuration.Value);
                    break;

                case CharacterAfflictions.STATUSTYPE.Petrify:
                    if (ConfigManager.PetrifyTriggerEnabled.Value) break;
                    double PetrifyIntensity = TriggerUtils.CalculateIntensity(amount,
                        ConfigManager.PetrifyTriggerMaximumIntensity.Value, ConfigManager.PetrifyTriggerMinimumIntensity.Value, 2);
                    _ = HandleAfflictionIntensity(sTATUSTYPE, PetrifyIntensity, ConfigManager.PetrifyTriggerDuration.Value);
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
            afflictionIntensities.Remove(sTATUSTYPE);
        }
    }
}