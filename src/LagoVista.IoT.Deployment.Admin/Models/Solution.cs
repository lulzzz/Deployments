﻿using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Resources;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Deployment.Admin.Models
{
    /*
var credentials = new TokenCloudCredentials("", "");
var client = new Microsoft.Azure.Management.Resources.ResourceManagementClient(credentials);
var result = c.ResourceGroups.CreateOrUpdateAsync("MyResourceGroup", new Microsoft.Azure.Management.ResourceGroup("West US"), new System.Threading.CancellationToken()).Result */


    [EntityDescription(DeploymentAdminDomain.DeploymentAdmin, DeploymentAdminResources.Names.Solution_Title, Resources.DeploymentAdminResources.Names.Solution_Help, Resources.DeploymentAdminResources.Names.Solution_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(DeploymentAdminResources))]
    public class Solution : LagoVista.IoT.DeviceAdmin.Models.IoTModelBase, IOwnedEntity, IKeyedEntity, INoSQLEntity, IValidateable
    {
        public string DatabaseName { get; set; }
        public string EntityType { get; set; }

        public Solution()
        {
            DeviceConfigurations = new List<EntityHeader<DeviceConfiguration>>();
            Listeners = new List<EntityHeader<ListenerConfiguration>>();
        }

        public EntityHeader Environment
        {
            get;
            set;
        }

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

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Deployment_Listeners, HelpResource: Resources.DeploymentAdminResources.Names.Deployment_Listeners_Help, FieldType: FieldTypes.ChildList, ResourceType: typeof(DeploymentAdminResources))]
        public List<EntityHeader<ListenerConfiguration>> Listeners { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Deployment_Planner, WaterMark: Resources.DeploymentAdminResources.Names.Deployment_Planner_Select, HelpResource: Resources.DeploymentAdminResources.Names.Deployment_Planner_Help, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), IsRequired:true)]
        public EntityHeader<PlannerConfiguration> Planner { get; set; }


        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Common_Key, HelpResource: Resources.DeploymentAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.DeploymentAdminResources.Names.Common_Key_Validation, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public String Key { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Solution_DeviceConfigurations, HelpResource: Resources.DeploymentAdminResources.Names.Solution_DeviceConfigurations_Help, FieldType: FieldTypes.ChildItem, ResourceType: typeof(DeploymentAdminResources))]
        public List<EntityHeader<DeviceConfiguration>> DeviceConfigurations { get; set; }

        public string MonitoringEndpoint { get; set; }

        public SolutionSummary CreateSummary()
        {
            return new SolutionSummary()
            {
                Id = Id,
                Key = Key,
                Name = Name,
                IsPublic = IsPublic,
                Description = Description
            };
        }
    }


    [EntityDescription(DeploymentAdminDomain.DeploymentAdmin, DeploymentAdminResources.Names.Solution_Title, Resources.DeploymentAdminResources.Names.Solution_Help, Resources.DeploymentAdminResources.Names.Solution_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(DeploymentAdminResources))]
    public class SolutionSummary : LagoVista.Core.Models.SummaryData
    { 
    }
}