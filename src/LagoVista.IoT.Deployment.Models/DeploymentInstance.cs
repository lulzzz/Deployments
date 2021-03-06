﻿using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Resources;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.IoT.Deployment.Models.Resources;
using LagoVista.IoT.DeviceAdmin.Models;

namespace LagoVista.IoT.Deployment.Admin.Models
{
    [EntityDescription(DeploymentAdminDomain.DeploymentAdmin, DeploymentAdminResources.Names.Instance_Title, DeploymentAdminResources.Names.Instance_Help, DeploymentAdminResources.Names.Instance_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(DeploymentAdminResources))]
    public class DeploymentInstance : LagoVista.IoT.DeviceAdmin.Models.IoTModelBase, IOwnedEntity, IValidateable, IKeyedEntity, INoSQLEntity, IFormDescriptor
    {
        public DeploymentInstance()
        {
            Status = EntityHeader<DeploymentInstanceStates>.Create(DeploymentInstanceStates.Offline);
            InputCommandSSL = false;
            InputCommandPort = 80;
            SettingsValues = new List<AttributeValue>();
            CloudProvider = new EntityHeader() { Text = "Digital Ocean", Id = "378463ADF57B4C02B60FEF4DCB30F7E2" };
        }


        public const string Status_Offline = "offline";

        public const string Status_DeployingRuntime = "deployingruntime";

        public const string Status_StartingRuntime = "startingruntime";

        public const string Status_Initializing = "initializing";
        public const string Status_Starting = "starting";
        
        public const string Status_Running = "running";

        public const string Status_Paused = "paused";
        public const string Status_Pausing = "pausing";
        public const string Status_Stopping = "stopping";
        public const string Status_Stopped = "stopped";

        public const string Status_HostRestarting = "hostrestarting";
        public const string Status_UpdatingSolution = "updatingsolution";

        public const string Status_FatalError = "fatalerror";
        public const string Status_FailedToDeploy = "failedtodeploy";
        public const string Status_FailedToInitialize = "failedtoinitialize";
        public const string Status_FailedToStart = "failedtostart";
        public const string Status_HostFailedHealthCheck = "hostfailedhealthcheck";
        
        
        public string DatabaseName { get; set; }
        public string EntityType { get; set; }


        [FormField(LabelResource: DeploymentAdminResources.Names.Common_Key, HelpResource: DeploymentAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: DeploymentAdminResources.Names.Common_Key_Validation, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public String Key { get; set; }


        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_IsDeployed, HelpResource: DeploymentAdminResources.Names.Instance_IsDeployed_Help, FieldType: FieldTypes.Bool, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public bool IsDeployed { get; set; }


        private EntityHeader<DeploymentInstanceStates> _status;

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_Status, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public EntityHeader<DeploymentInstanceStates> Status
        {
            get { return _status; }
            set
            {
                _status = value;
                StatusTimeStamp = DateTime.UtcNow.ToJSONString();
            }
        }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_StatusTimeStamp, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string StatusTimeStamp { get; set; }


        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_Host, HelpResource: DeploymentAdminResources.Names.Instance_Host_Help, WaterMark: DeploymentAdminResources.Names.Instance_Host_Watermark, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources))]
        public EntityHeader<DeploymentHost> Host { get; set; }


        /// <summary>
        /// This is the primary host that will be be used as an access point into the instance, if the instance consists of many machines, this will manage all the other hosts for a clustered version of an instance.
        /// </summary>
        EntityHeader<DeploymentHost> _primaryHost;
        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_Host, HelpResource: DeploymentAdminResources.Names.Instance_Host_Help, WaterMark: DeploymentAdminResources.Names.Instance_Host_Watermark, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources))]
        public EntityHeader<DeploymentHost> PrimaryHost
        {
            get
            {
                if(EntityHeader.IsNullOrEmpty(_primaryHost))
                {
                    _primaryHost = Host;
                    return Host;
                }
                else
                {
                    return _primaryHost;
                }
            }
            set { _primaryHost = value; }
        }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_LocalUsageStatistics, FieldType: FieldTypes.Bool, ResourceType: typeof(DeploymentAdminResources))]
        public bool LocalUsageStatistics { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_LocalLogs, FieldType: FieldTypes.Bool, ResourceType: typeof(DeploymentAdminResources))]
        public bool LocalLogs { get; set; }


        [FormField(LabelResource: DeploymentAdminResources.Names.Common_IsPublic, FieldType: FieldTypes.Bool, ResourceType: typeof(DeploymentAdminResources))]
        public bool IsPublic { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }

        public EntityHeader ToEntityHeader()
        {
            return new EntityHeader()
            {
                Id = Id,
                Text = Name,
            };
        }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_UpSince,  FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsRequired: false, IsUserEditable:false)]
        public string UpSince { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Host_Subscription, WaterMark: DeploymentAdminResources.Names.Host_SubscriptionSelect, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: true, IsRequired: true)]
        public EntityHeader Subscription { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Host_Size, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), WaterMark: DeploymentAdminResources.Names.Host_SelectSize, IsRequired: true)]
        public EntityHeader Size { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_DeviceRepo, HelpResource: DeploymentAdminResources.Names.Instance_DeviceRepo_Help, WaterMark: DeploymentAdminResources.Names.Instance_DeviceRepo_Select, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public EntityHeader<DeviceRepository> DeviceRepository { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_SettingsValues, FieldType: FieldTypes.ChildList, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public List<AttributeValue> SettingsValues { get; set; }

        public Dictionary<string, object> PropertyBag { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Host_ContainerRepository, WaterMark: DeploymentAdminResources.Names.Host_ContainerRepository_Select, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public EntityHeader ContainerRepository { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Host_ContainerTag, WaterMark: DeploymentAdminResources.Names.Host_ContainerTag_Select, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public EntityHeader ContainerTag { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_LastPing, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string LastPing { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Host_DNSName, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string DnsHostName { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_InputCommandSSL, FieldType: FieldTypes.CheckBox, HelpResource:DeploymentAdminResources.Names.Instance_InputCommandSSL_Help, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: true)]
        public bool InputCommandSSL { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_InputCommandPort, FieldType: FieldTypes.Integer, HelpResource: DeploymentAdminResources.Names.Instance_InputCommandPort_Help, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: true)]
        public int InputCommandPort { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Host_CloudProvider, HelpResource: DeploymentAdminResources.Names.Host_CloudProvider_Help, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false, IsRequired: true)]
        public EntityHeader CloudProvider { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_DebugMode, HelpResource: DeploymentAdminResources.Names.Instance_DebugMode_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: true)]
        public bool DebugMode { get; set; }


        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_Solution, WaterMark: DeploymentAdminResources.Names.Instance_Solution_Select, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public EntityHeader<Solution> Solution { get; set; }
        public DeploymentInstanceSummary CreateSummary()
        {
            return new DeploymentInstanceSummary()
            {
                Description = Description,
                Name = Name,
                Key = Key,
                Id = Id,
                IsPublic = IsPublic,
                IsDeployed = IsDeployed,
                Status = Status
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(DeploymentInstance.Name),
                nameof(DeploymentInstance.Key),
                nameof(DeploymentInstance.DnsHostName),
                nameof(DeploymentInstance.Status),
                nameof(DeploymentInstance.IsDeployed),
                nameof(DeploymentInstance.Subscription),
                nameof(DeploymentInstance.Size),
                nameof(DeploymentInstance.CloudProvider),
                nameof(DeploymentInstance.ContainerRepository),
                nameof(DeploymentInstance.ContainerTag),
                nameof(DeploymentInstance.DeviceRepository),
                nameof(DeploymentInstance.Solution),
            };
        }
    }

    public class DeploymentInstanceSummary : SummaryData
    {
        public EntityHeader<DeploymentInstanceStates> Status { get; set; }
        public bool IsDeployed { get; set; }
    }
}
