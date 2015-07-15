using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Resources;
using System.Collections.Generic;
using Tff.Panzer.Models.Campaign;
using Tff.Panzer.Models;
using Tff.Panzer.Models.VictoryCondition;

namespace Tff.Panzer.Factories.VictoryCondition
{
    public class VictoryConditionFactory
    {
        public CampaignVictoryConditionFactory CampaignVictoryConditionFactory { get; private set; }
        public StandAloneVictoryConditionFactory StandAloneVictoryConditionFactory { get; private set; }
        
        public VictoryConditionFactory()
        {
            CampaignVictoryConditionFactory = new CampaignVictoryConditionFactory();
            StandAloneVictoryConditionFactory = new StandAloneVictoryConditionFactory();
        }

        public CampaignVictoryCondition GetCampaignVictoryConditionForAScenario(int scenarioId)
        {
            return CampaignVictoryConditionFactory.GetCampaignVictoryConditionForAScenario(scenarioId);
        }

        public StandAloneVictoryCondition GetStandAloneVictoryConditionForAScenario(int scenarioId)
        {
            return StandAloneVictoryConditionFactory.GetStandAloneVictoryConditionForAScenario(scenarioId);
        }

        public CampaignStepTypeEnum CalculateCampaignStepResult(int scenarioId, int turnsRemaining, int numberOfAxisVictoryTiles, int numberOfAlliedVictoryTiles)
        {
            CampaignVictoryCondition campaignVictoryCondition = GetCampaignVictoryConditionForAScenario(scenarioId);
            if (campaignVictoryCondition.ObjectiveTypeEnum == ObjectiveTypeEnum.AxisAttacks)
            {
                if (numberOfAlliedVictoryTiles == 0 && turnsRemaining >= campaignVictoryCondition.MajorVictoryTurnsRemain)
                {
                    return CampaignStepTypeEnum.MajorVictory;
                }
                else if (numberOfAlliedVictoryTiles == 0 && turnsRemaining >= campaignVictoryCondition.MinorVactoryTurnsRemain)
                {
                    return CampaignStepTypeEnum.MinorVictory;
                }
                else
                {
                    return CampaignStepTypeEnum.Loss;
                }

            }
            else
            {
                if (numberOfAxisVictoryTiles >= campaignVictoryCondition.MajorVictoryObjectviesToHold)
                {
                    return CampaignStepTypeEnum.MajorVictory;
                }
                else if (numberOfAxisVictoryTiles >= campaignVictoryCondition.MinorVictoryObjectviesToHold)
                {
                    return CampaignStepTypeEnum.MinorVictory;
                }
                else
                {
                    return CampaignStepTypeEnum.Loss;
                }
            }

        }

        public SideEnum CalculateWinningSide(int scenarioId, int numberOfAxisVictoryTiles, int numberOfAlliedVictoryTiles)
        {
            StandAloneVictoryCondition standAloneVictoryCondition = GetStandAloneVictoryConditionForAScenario(scenarioId);

            if (standAloneVictoryCondition.ObjectiveTypeEnum == ObjectiveTypeEnum.AxisAttacks)
            {
                if (numberOfAlliedVictoryTiles == 0)
                {
                    return SideEnum.Axis;
                }
                else
                {
                    return SideEnum.Allies;
                }
            }
            else
            {
                if (numberOfAxisVictoryTiles >= standAloneVictoryCondition.LossAmount)
                {
                    return SideEnum.Axis;
                }
                else
                {
                    return SideEnum.Allies;
                }
            }
        }

    }
}
