using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Tff.Panzer.Services
{
    [ServiceContract]
    public interface IPanzerService
    {
        [OperationContract]
        ScenarioTile GetScenarioTile(int scenarioTileId);

        [OperationContract]
        List<ScenarioTile> GetScenarioTiles(int scenarioId);

        [OperationContract]
        List<Scenario> GetActiveScenarios();
    }
}
