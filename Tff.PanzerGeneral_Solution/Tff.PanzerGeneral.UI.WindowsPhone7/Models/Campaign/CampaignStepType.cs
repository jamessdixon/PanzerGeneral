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

namespace Tff.Panzer.Models.Campaign
{
    public class CampaignStepType
    {
        public Int32 CampaignStepTypeId { get; set; }
        public String CampaignStepTypeDescription { get; set; }
        public CampaignStepTypeEnum CampaignStepTypeEnum
        {
            get
            {
                switch(CampaignStepTypeId)
                {
                    case 0:
                        return CampaignStepTypeEnum.MajorVictory;
                    case 1:
                        return CampaignStepTypeEnum.MinorVictory;
                    case 2:
                        return CampaignStepTypeEnum.Loss;
                    default:
                        return CampaignStepTypeEnum.MajorVictory;
                }
            }
        }
    }
}
