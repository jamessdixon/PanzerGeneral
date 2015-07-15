using System;


namespace Tff.Panzer.Models.Battle
{
    public enum BattleOutcomeEnum
    {
        AggressorSurvives_ProtectorHolds = 0,
        AggressorSurvives_ProtectorRetreats = 1,
        AggressorSurvives_ProtectorDestroyed = 2,
        AggressorDestroyed_ProtectorHolds = 3,
        AggressorDestroyed_ProtectorDestroyed = 4
    }
}
