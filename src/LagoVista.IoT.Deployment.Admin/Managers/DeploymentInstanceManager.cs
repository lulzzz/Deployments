﻿//#define WEBSERVERSIDECHECK

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.Core.Managers;
using LagoVista.Core;
using LagoVista.IoT.Deployment.Admin.Repos;
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Deployment.Admin.Services;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.DeviceManagement.Core.Managers;
using LagoVista.UserAdmin.Managers;

namespace LagoVista.IoT.Deployment.Admin.Managers
{
    public class DeploymentInstanceManager : DeploymentInstanceManagerCore, IDeploymentInstanceManager
    {

        public const string DeploymentAction_Deploy = "Instance.Deploy";
        public const string DeploymentAction_Start = "Instance.Start";
        public const string DeploymentAction_Pause = "Instance.Pause";
        public const string DeploymentAction_Stop = "Instance.Stop";
        public const string DeploymentAction_Remove = "Instance.Remove";
        public const string DeploymentAction_Monitor = "Instance.Monitor";

        IDeploymentInstanceRepo _instanceRepo;
        ISolutionManager _solutionManager;
        IDeploymentHostManager _hostManager;
        IDeploymentConnectorService _connector;
        IDeviceRepositoryManager _deviceRepoManager;
        IDeploymentActivityQueueManager _deploymentActivityQueueManager;


        public DeploymentInstanceManager(IDeviceRepositoryManager deviceRepoManager, IDeploymentConnectorService connector, IDeploymentHostManager hostManager,
                    IDeploymentActivityQueueManager deploymentActivityQueueManager, IDeploymentInstanceRepo instanceRepo, ISolutionManager deploymentConfigurationManager,
                     IAdminLogger logger, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) : base(hostManager, instanceRepo, logger, appConfig, depmanager, security)
        {
            _hostManager = hostManager;
            _instanceRepo = instanceRepo;
            _solutionManager = deploymentConfigurationManager;
            _deploymentActivityQueueManager = deploymentActivityQueueManager;
            _connector = connector;
            _deviceRepoManager = deviceRepoManager;
        }

        public async Task<InvokeResult> CreateAsync(String id, EntityHeader org, EntityHeader user)
        {
            var instance = await _instanceRepo.GetInstanceAsync(id);
            await AuthorizeAsync(instance, AuthorizeResult.AuthorizeActions.Perform, user, org, "deployInstance");


            if (instance.IsDeployed)
            {
                throw LagoVista.IoT.Logging.Exceptions.InvalidOperationException.FromErrorCode(Resources.DeploymentErrorCodes.AlreadyDeployed);
            }

            await _deploymentActivityQueueManager.Enqueue(new DeploymentActivity(DeploymentActivityResourceTypes.Instance, id, DeploymentActivityTaskTypes.Create)
            {
                RequestedByUserId = user.Id,
                RequestedByUserName = user.Text,
                RequestedByOrganizationId = org.Id,
                RequestedByOrganizationName = org.Text,
            });

            return InvokeResult.Success;
        }

        public async Task<InvokeResult> RestartHostAsync(String id, EntityHeader org, EntityHeader user)
        {
            var instance = await _instanceRepo.GetInstanceAsync(id);
            await AuthorizeAsync(instance, AuthorizeResult.AuthorizeActions.Perform, user, org, "resetartHost");
           
            await _deploymentActivityQueueManager.Enqueue(new DeploymentActivity(DeploymentActivityResourceTypes.Instance, id, DeploymentActivityTaskTypes.Reset)
            {
                RequestedByUserId = user.Id,
                RequestedByUserName = user.Text,
                RequestedByOrganizationId = org.Id,
                RequestedByOrganizationName = org.Text,
            });

            return InvokeResult.Success;
        }

        public async Task<InvokeResult> ReloadSolutionAsync(String id, EntityHeader org, EntityHeader user)
        {
            var instance = await _instanceRepo.GetInstanceAsync(id);
            await AuthorizeAsync(instance, AuthorizeResult.AuthorizeActions.Perform, user, org, "reloadSolution");


            if (!instance.IsDeployed)
            {
                throw LagoVista.IoT.Logging.Exceptions.InvalidOperationException.FromErrorCode(Resources.DeploymentErrorCodes.NotDeployed);
            }

            await _deploymentActivityQueueManager.Enqueue(new DeploymentActivity(DeploymentActivityResourceTypes.Instance, id, DeploymentActivityTaskTypes.Update)
            {
                RequestedByUserId = user.Id,
                RequestedByUserName = user.Text,
                RequestedByOrganizationId = org.Id,
                RequestedByOrganizationName = org.Text,
            });

            return InvokeResult.Success;
        }

        public async Task<InvokeResult> RemoveAsync(String id, EntityHeader org, EntityHeader user)
        {
            var instance = await _instanceRepo.GetInstanceAsync(id);
            await AuthorizeAsync(instance, AuthorizeResult.AuthorizeActions.Perform, user, org, "destroyInstance");

            await _deploymentActivityQueueManager.Enqueue(new DeploymentActivity(DeploymentActivityResourceTypes.Instance, id, DeploymentActivityTaskTypes.Remove)
            {
                RequestedByUserId = user.Id,
                RequestedByUserName = user.Text,
                RequestedByOrganizationId = org.Id,
                RequestedByOrganizationName = org.Text,
            });

            return InvokeResult.Success;
        }
        

        public async Task<InvokeResult<string>> GetRemoteMonitoringURIAsync(string channel, string id, string verbosity, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(user, org, $"wsrequest.{channel}", id);

            var host = await _hostManager.GetNotificationsHostAsync(org, user);
            return await _connector.GetRemoteMonitoringUriAsync(host, channel, id, verbosity, org, user);
        }
  

        public async Task<InvokeResult<InstanceRuntimeDetails>> GetInstanceDetailsAsync(string instanceId, EntityHeader org, EntityHeader user)
        {
            var instance = await GetInstanceAsync(instanceId, org, user)
;            var host = await _hostManager.GetDeploymentHostAsync(instance.Host.Id, org, user);
            await AuthorizeAsync(user, org, "instanceRuntimeDetails", instanceId);
            return await _connector.GetInstanceDetailsAsync(host, instanceId, org, user);
        }



        public async Task<DependentObjectCheckResult> CheckInUseAsync(string id, EntityHeader org, EntityHeader user)
        {
            var instance = await _instanceRepo.GetInstanceAsync(id);
            await AuthorizeAsync(instance, AuthorizeResult.AuthorizeActions.Read, user, org);
            return await CheckForDepenenciesAsync(instance);
        }

        

        public async Task<IEnumerable<DeploymentInstanceSummary>> GetInstanceForOrgAsync(string orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(DeploymentInstance));
            return await _instanceRepo.GetInstanceForOrgAsync(orgId);
        }

        public async Task<DeploymentInstance> LoadFullInstanceAsync(string id, EntityHeader org, EntityHeader user)
        {
            var instance = await _instanceRepo.GetInstanceAsync(id);
            await AuthorizeAsync(instance, AuthorizeResult.AuthorizeActions.Read, user, org);

            instance.DeviceRepository.Value = await _deviceRepoManager.GetDeviceRepositoryAsync(instance.DeviceRepository.Id, org, user);

            instance.Solution.Value = await _solutionManager.LoadFullSolutionAsync(instance.Solution.Id, org, user);
            instance.Solution.Id = instance.Solution.Value.Id;
            instance.Solution.Text = instance.Solution.Value.Name;

            return instance;
        }

        
    }
}
