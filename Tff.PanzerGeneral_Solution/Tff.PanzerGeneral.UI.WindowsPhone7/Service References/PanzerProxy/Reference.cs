﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.Phone.ServiceReference, version 3.7.0.0
// 
namespace Tff.Panzer.PanzerProxy {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ScenarioTile", Namespace="http://schemas.datacontract.org/2004/07/Tff.Panzer.Services")]
    public partial class ScenarioTile : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int ColumnNumberField;
        
        private bool DeployIndicatorField;
        
        private int NationIdField;
        
        private int RowNumberField;
        
        private int ScenarioIdField;
        
        private int ScenarioTileIdField;
        
        private int SideIdField;
        
        private bool SupplyIndicatorField;
        
        private int TerrainIdField;
        
        private int TileNameIdField;
        
        private bool VictoryIndicatorField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ColumnNumber {
            get {
                return this.ColumnNumberField;
            }
            set {
                if ((this.ColumnNumberField.Equals(value) != true)) {
                    this.ColumnNumberField = value;
                    this.RaisePropertyChanged("ColumnNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool DeployIndicator {
            get {
                return this.DeployIndicatorField;
            }
            set {
                if ((this.DeployIndicatorField.Equals(value) != true)) {
                    this.DeployIndicatorField = value;
                    this.RaisePropertyChanged("DeployIndicator");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int NationId {
            get {
                return this.NationIdField;
            }
            set {
                if ((this.NationIdField.Equals(value) != true)) {
                    this.NationIdField = value;
                    this.RaisePropertyChanged("NationId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RowNumber {
            get {
                return this.RowNumberField;
            }
            set {
                if ((this.RowNumberField.Equals(value) != true)) {
                    this.RowNumberField = value;
                    this.RaisePropertyChanged("RowNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ScenarioId {
            get {
                return this.ScenarioIdField;
            }
            set {
                if ((this.ScenarioIdField.Equals(value) != true)) {
                    this.ScenarioIdField = value;
                    this.RaisePropertyChanged("ScenarioId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ScenarioTileId {
            get {
                return this.ScenarioTileIdField;
            }
            set {
                if ((this.ScenarioTileIdField.Equals(value) != true)) {
                    this.ScenarioTileIdField = value;
                    this.RaisePropertyChanged("ScenarioTileId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SideId {
            get {
                return this.SideIdField;
            }
            set {
                if ((this.SideIdField.Equals(value) != true)) {
                    this.SideIdField = value;
                    this.RaisePropertyChanged("SideId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool SupplyIndicator {
            get {
                return this.SupplyIndicatorField;
            }
            set {
                if ((this.SupplyIndicatorField.Equals(value) != true)) {
                    this.SupplyIndicatorField = value;
                    this.RaisePropertyChanged("SupplyIndicator");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TerrainId {
            get {
                return this.TerrainIdField;
            }
            set {
                if ((this.TerrainIdField.Equals(value) != true)) {
                    this.TerrainIdField = value;
                    this.RaisePropertyChanged("TerrainId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TileNameId {
            get {
                return this.TileNameIdField;
            }
            set {
                if ((this.TileNameIdField.Equals(value) != true)) {
                    this.TileNameIdField = value;
                    this.RaisePropertyChanged("TileNameId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool VictoryIndicator {
            get {
                return this.VictoryIndicatorField;
            }
            set {
                if ((this.VictoryIndicatorField.Equals(value) != true)) {
                    this.VictoryIndicatorField = value;
                    this.RaisePropertyChanged("VictoryIndicator");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Scenario", Namespace="http://schemas.datacontract.org/2004/07/Tff.Panzer.Services")]
    public partial class Scenario : object, System.ComponentModel.INotifyPropertyChanged {
        
        private bool ActiveIndField;
        
        private int DaysPerTurnField;
        
        private int MaxUnitExperienceField;
        
        private int MaxUnitStrengthField;
        
        private int NumberOfTurnsField;
        
        private string ScenarioDescriptionField;
        
        private int ScenarioIdField;
        
        private string ScenarioNameField;
        
        private System.DateTime ScenarioStartField;
        
        private int StartingWeatherIdField;
        
        private int TurnsPerDayField;
        
        private int WeatherZoneIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ActiveInd {
            get {
                return this.ActiveIndField;
            }
            set {
                if ((this.ActiveIndField.Equals(value) != true)) {
                    this.ActiveIndField = value;
                    this.RaisePropertyChanged("ActiveInd");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int DaysPerTurn {
            get {
                return this.DaysPerTurnField;
            }
            set {
                if ((this.DaysPerTurnField.Equals(value) != true)) {
                    this.DaysPerTurnField = value;
                    this.RaisePropertyChanged("DaysPerTurn");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MaxUnitExperience {
            get {
                return this.MaxUnitExperienceField;
            }
            set {
                if ((this.MaxUnitExperienceField.Equals(value) != true)) {
                    this.MaxUnitExperienceField = value;
                    this.RaisePropertyChanged("MaxUnitExperience");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MaxUnitStrength {
            get {
                return this.MaxUnitStrengthField;
            }
            set {
                if ((this.MaxUnitStrengthField.Equals(value) != true)) {
                    this.MaxUnitStrengthField = value;
                    this.RaisePropertyChanged("MaxUnitStrength");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int NumberOfTurns {
            get {
                return this.NumberOfTurnsField;
            }
            set {
                if ((this.NumberOfTurnsField.Equals(value) != true)) {
                    this.NumberOfTurnsField = value;
                    this.RaisePropertyChanged("NumberOfTurns");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ScenarioDescription {
            get {
                return this.ScenarioDescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.ScenarioDescriptionField, value) != true)) {
                    this.ScenarioDescriptionField = value;
                    this.RaisePropertyChanged("ScenarioDescription");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ScenarioId {
            get {
                return this.ScenarioIdField;
            }
            set {
                if ((this.ScenarioIdField.Equals(value) != true)) {
                    this.ScenarioIdField = value;
                    this.RaisePropertyChanged("ScenarioId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ScenarioName {
            get {
                return this.ScenarioNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ScenarioNameField, value) != true)) {
                    this.ScenarioNameField = value;
                    this.RaisePropertyChanged("ScenarioName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ScenarioStart {
            get {
                return this.ScenarioStartField;
            }
            set {
                if ((this.ScenarioStartField.Equals(value) != true)) {
                    this.ScenarioStartField = value;
                    this.RaisePropertyChanged("ScenarioStart");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StartingWeatherId {
            get {
                return this.StartingWeatherIdField;
            }
            set {
                if ((this.StartingWeatherIdField.Equals(value) != true)) {
                    this.StartingWeatherIdField = value;
                    this.RaisePropertyChanged("StartingWeatherId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TurnsPerDay {
            get {
                return this.TurnsPerDayField;
            }
            set {
                if ((this.TurnsPerDayField.Equals(value) != true)) {
                    this.TurnsPerDayField = value;
                    this.RaisePropertyChanged("TurnsPerDay");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int WeatherZoneId {
            get {
                return this.WeatherZoneIdField;
            }
            set {
                if ((this.WeatherZoneIdField.Equals(value) != true)) {
                    this.WeatherZoneIdField = value;
                    this.RaisePropertyChanged("WeatherZoneId");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PanzerProxy.IPanzerService")]
    public interface IPanzerService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IPanzerService/GetScenarioTile", ReplyAction="http://tempuri.org/IPanzerService/GetScenarioTileResponse")]
        System.IAsyncResult BeginGetScenarioTile(int scenarioTileId, System.AsyncCallback callback, object asyncState);
        
        Tff.Panzer.PanzerProxy.ScenarioTile EndGetScenarioTile(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IPanzerService/GetScenarioTiles", ReplyAction="http://tempuri.org/IPanzerService/GetScenarioTilesResponse")]
        System.IAsyncResult BeginGetScenarioTiles(int scenarioId, System.AsyncCallback callback, object asyncState);
        
        System.Collections.Generic.List<Tff.Panzer.PanzerProxy.ScenarioTile> EndGetScenarioTiles(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IPanzerService/GetActiveScenarios", ReplyAction="http://tempuri.org/IPanzerService/GetActiveScenariosResponse")]
        System.IAsyncResult BeginGetActiveScenarios(System.AsyncCallback callback, object asyncState);
        
        System.Collections.Generic.List<Tff.Panzer.PanzerProxy.Scenario> EndGetActiveScenarios(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPanzerServiceChannel : Tff.Panzer.PanzerProxy.IPanzerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetScenarioTileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetScenarioTileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Tff.Panzer.PanzerProxy.ScenarioTile Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Tff.Panzer.PanzerProxy.ScenarioTile)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetScenarioTilesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetScenarioTilesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.Generic.List<Tff.Panzer.PanzerProxy.ScenarioTile> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.Generic.List<Tff.Panzer.PanzerProxy.ScenarioTile>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetActiveScenariosCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetActiveScenariosCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.Generic.List<Tff.Panzer.PanzerProxy.Scenario> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.Generic.List<Tff.Panzer.PanzerProxy.Scenario>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PanzerServiceClient : System.ServiceModel.ClientBase<Tff.Panzer.PanzerProxy.IPanzerService>, Tff.Panzer.PanzerProxy.IPanzerService {
        
        private BeginOperationDelegate onBeginGetScenarioTileDelegate;
        
        private EndOperationDelegate onEndGetScenarioTileDelegate;
        
        private System.Threading.SendOrPostCallback onGetScenarioTileCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetScenarioTilesDelegate;
        
        private EndOperationDelegate onEndGetScenarioTilesDelegate;
        
        private System.Threading.SendOrPostCallback onGetScenarioTilesCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetActiveScenariosDelegate;
        
        private EndOperationDelegate onEndGetActiveScenariosDelegate;
        
        private System.Threading.SendOrPostCallback onGetActiveScenariosCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public PanzerServiceClient() {
        }
        
        public PanzerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PanzerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PanzerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PanzerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetScenarioTileCompletedEventArgs> GetScenarioTileCompleted;
        
        public event System.EventHandler<GetScenarioTilesCompletedEventArgs> GetScenarioTilesCompleted;
        
        public event System.EventHandler<GetActiveScenariosCompletedEventArgs> GetActiveScenariosCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Tff.Panzer.PanzerProxy.IPanzerService.BeginGetScenarioTile(int scenarioTileId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetScenarioTile(scenarioTileId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Tff.Panzer.PanzerProxy.ScenarioTile Tff.Panzer.PanzerProxy.IPanzerService.EndGetScenarioTile(System.IAsyncResult result) {
            return base.Channel.EndGetScenarioTile(result);
        }
        
        private System.IAsyncResult OnBeginGetScenarioTile(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int scenarioTileId = ((int)(inValues[0]));
            return ((Tff.Panzer.PanzerProxy.IPanzerService)(this)).BeginGetScenarioTile(scenarioTileId, callback, asyncState);
        }
        
        private object[] OnEndGetScenarioTile(System.IAsyncResult result) {
            Tff.Panzer.PanzerProxy.ScenarioTile retVal = ((Tff.Panzer.PanzerProxy.IPanzerService)(this)).EndGetScenarioTile(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetScenarioTileCompleted(object state) {
            if ((this.GetScenarioTileCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetScenarioTileCompleted(this, new GetScenarioTileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetScenarioTileAsync(int scenarioTileId) {
            this.GetScenarioTileAsync(scenarioTileId, null);
        }
        
        public void GetScenarioTileAsync(int scenarioTileId, object userState) {
            if ((this.onBeginGetScenarioTileDelegate == null)) {
                this.onBeginGetScenarioTileDelegate = new BeginOperationDelegate(this.OnBeginGetScenarioTile);
            }
            if ((this.onEndGetScenarioTileDelegate == null)) {
                this.onEndGetScenarioTileDelegate = new EndOperationDelegate(this.OnEndGetScenarioTile);
            }
            if ((this.onGetScenarioTileCompletedDelegate == null)) {
                this.onGetScenarioTileCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetScenarioTileCompleted);
            }
            base.InvokeAsync(this.onBeginGetScenarioTileDelegate, new object[] {
                        scenarioTileId}, this.onEndGetScenarioTileDelegate, this.onGetScenarioTileCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Tff.Panzer.PanzerProxy.IPanzerService.BeginGetScenarioTiles(int scenarioId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetScenarioTiles(scenarioId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.Generic.List<Tff.Panzer.PanzerProxy.ScenarioTile> Tff.Panzer.PanzerProxy.IPanzerService.EndGetScenarioTiles(System.IAsyncResult result) {
            return base.Channel.EndGetScenarioTiles(result);
        }
        
        private System.IAsyncResult OnBeginGetScenarioTiles(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int scenarioId = ((int)(inValues[0]));
            return ((Tff.Panzer.PanzerProxy.IPanzerService)(this)).BeginGetScenarioTiles(scenarioId, callback, asyncState);
        }
        
        private object[] OnEndGetScenarioTiles(System.IAsyncResult result) {
            System.Collections.Generic.List<Tff.Panzer.PanzerProxy.ScenarioTile> retVal = ((Tff.Panzer.PanzerProxy.IPanzerService)(this)).EndGetScenarioTiles(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetScenarioTilesCompleted(object state) {
            if ((this.GetScenarioTilesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetScenarioTilesCompleted(this, new GetScenarioTilesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetScenarioTilesAsync(int scenarioId) {
            this.GetScenarioTilesAsync(scenarioId, null);
        }
        
        public void GetScenarioTilesAsync(int scenarioId, object userState) {
            if ((this.onBeginGetScenarioTilesDelegate == null)) {
                this.onBeginGetScenarioTilesDelegate = new BeginOperationDelegate(this.OnBeginGetScenarioTiles);
            }
            if ((this.onEndGetScenarioTilesDelegate == null)) {
                this.onEndGetScenarioTilesDelegate = new EndOperationDelegate(this.OnEndGetScenarioTiles);
            }
            if ((this.onGetScenarioTilesCompletedDelegate == null)) {
                this.onGetScenarioTilesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetScenarioTilesCompleted);
            }
            base.InvokeAsync(this.onBeginGetScenarioTilesDelegate, new object[] {
                        scenarioId}, this.onEndGetScenarioTilesDelegate, this.onGetScenarioTilesCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Tff.Panzer.PanzerProxy.IPanzerService.BeginGetActiveScenarios(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetActiveScenarios(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.Generic.List<Tff.Panzer.PanzerProxy.Scenario> Tff.Panzer.PanzerProxy.IPanzerService.EndGetActiveScenarios(System.IAsyncResult result) {
            return base.Channel.EndGetActiveScenarios(result);
        }
        
        private System.IAsyncResult OnBeginGetActiveScenarios(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((Tff.Panzer.PanzerProxy.IPanzerService)(this)).BeginGetActiveScenarios(callback, asyncState);
        }
        
        private object[] OnEndGetActiveScenarios(System.IAsyncResult result) {
            System.Collections.Generic.List<Tff.Panzer.PanzerProxy.Scenario> retVal = ((Tff.Panzer.PanzerProxy.IPanzerService)(this)).EndGetActiveScenarios(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetActiveScenariosCompleted(object state) {
            if ((this.GetActiveScenariosCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetActiveScenariosCompleted(this, new GetActiveScenariosCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetActiveScenariosAsync() {
            this.GetActiveScenariosAsync(null);
        }
        
        public void GetActiveScenariosAsync(object userState) {
            if ((this.onBeginGetActiveScenariosDelegate == null)) {
                this.onBeginGetActiveScenariosDelegate = new BeginOperationDelegate(this.OnBeginGetActiveScenarios);
            }
            if ((this.onEndGetActiveScenariosDelegate == null)) {
                this.onEndGetActiveScenariosDelegate = new EndOperationDelegate(this.OnEndGetActiveScenarios);
            }
            if ((this.onGetActiveScenariosCompletedDelegate == null)) {
                this.onGetActiveScenariosCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetActiveScenariosCompleted);
            }
            base.InvokeAsync(this.onBeginGetActiveScenariosDelegate, null, this.onEndGetActiveScenariosDelegate, this.onGetActiveScenariosCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override Tff.Panzer.PanzerProxy.IPanzerService CreateChannel() {
            return new PanzerServiceClientChannel(this);
        }
        
        private class PanzerServiceClientChannel : ChannelBase<Tff.Panzer.PanzerProxy.IPanzerService>, Tff.Panzer.PanzerProxy.IPanzerService {
            
            public PanzerServiceClientChannel(System.ServiceModel.ClientBase<Tff.Panzer.PanzerProxy.IPanzerService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetScenarioTile(int scenarioTileId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = scenarioTileId;
                System.IAsyncResult _result = base.BeginInvoke("GetScenarioTile", _args, callback, asyncState);
                return _result;
            }
            
            public Tff.Panzer.PanzerProxy.ScenarioTile EndGetScenarioTile(System.IAsyncResult result) {
                object[] _args = new object[0];
                Tff.Panzer.PanzerProxy.ScenarioTile _result = ((Tff.Panzer.PanzerProxy.ScenarioTile)(base.EndInvoke("GetScenarioTile", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetScenarioTiles(int scenarioId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = scenarioId;
                System.IAsyncResult _result = base.BeginInvoke("GetScenarioTiles", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.Generic.List<Tff.Panzer.PanzerProxy.ScenarioTile> EndGetScenarioTiles(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.Generic.List<Tff.Panzer.PanzerProxy.ScenarioTile> _result = ((System.Collections.Generic.List<Tff.Panzer.PanzerProxy.ScenarioTile>)(base.EndInvoke("GetScenarioTiles", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetActiveScenarios(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("GetActiveScenarios", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.Generic.List<Tff.Panzer.PanzerProxy.Scenario> EndGetActiveScenarios(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.Generic.List<Tff.Panzer.PanzerProxy.Scenario> _result = ((System.Collections.Generic.List<Tff.Panzer.PanzerProxy.Scenario>)(base.EndInvoke("GetActiveScenarios", _args, result)));
                return _result;
            }
        }
    }
}
