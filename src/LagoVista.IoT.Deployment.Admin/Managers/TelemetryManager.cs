﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.Core.Models;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Deployment.Admin.Services;
using LagoVista.Core.Managers;
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Models.UIMetaData;

namespace LagoVista.IoT.Deployment.Admin.Managers
{
    public class TelemetryManager : ManagerBase, ITelemetryManager
    {
        ITelemetryService _telemetryService;

        public TelemetryManager(IAdminLogger adminLogger, IAppConfig appConfig, ITelemetryService telemetryService, IDependencyManager dependencyManager, ISecurity security) : base(adminLogger, appConfig, dependencyManager, security)
        {
            _telemetryService = telemetryService;
        }

        public async Task<IEnumerable<TelemetryReportData>> GetForDeviceAsync(string deviceId, string recordType, ListRequest request, EntityHeader org, EntityHeader user)
        {
            await base.AuthorizeOrgAccessAsync(user, org, typeof(TelemetryReportData));
            return await _telemetryService.GetForDeviceAsync(deviceId, recordType, request);
        }

        public async Task<IEnumerable<TelemetryReportData>> GetForDeviceTypeAsync(string deviceTypeId, string recordType, ListRequest request, EntityHeader org, EntityHeader user)
        {
            await base.AuthorizeOrgAccessAsync(user, org, typeof(TelemetryReportData));
            return await _telemetryService.GetForDeviceTypeAsync(deviceTypeId, recordType, request);
        }

        public async Task<IEnumerable<TelemetryReportData>> GetForHostAsync(string hostId, string recordType, ListRequest request, EntityHeader org, EntityHeader user)
        {
            await  base.AuthorizeOrgAccessAsync(user, org, typeof(TelemetryReportData));
            return await  _telemetryService.GetForHostAsync(hostId, recordType, request);
        }

        public async Task<IEnumerable<TelemetryReportData>> GetForInstanceAsync(string instanceId, string recordType, ListRequest request, EntityHeader org, EntityHeader user)
        {
            await  base.AuthorizeOrgAccessAsync(user, org, typeof(TelemetryReportData));
            return await _telemetryService.GetForInstanceAsync(instanceId, recordType, request);
        }

        public async Task<IEnumerable<TelemetryReportData>> GetForPipelineModuleAsync(string pipelineModuleId, string recordType, ListRequest request, EntityHeader org, EntityHeader user)
        {
            await base.AuthorizeOrgAccessAsync(user, org, typeof(TelemetryReportData));
            return await _telemetryService.GetForPipelineModuleAsync(pipelineModuleId, recordType, request);
        }

        public async Task<IEnumerable<TelemetryReportData>> GetForPipelineQueueAsync(string pipelineModuleId, string recordType, ListRequest request, EntityHeader org, EntityHeader user)
        {
            await base.AuthorizeOrgAccessAsync(user, org, typeof(TelemetryReportData));
            return await _telemetryService.GetForPipelineQueueAsync(pipelineModuleId, recordType, request);
        }

        public async Task<string> GetItemDetailAsync(string itemId, string recordType, EntityHeader org, EntityHeader user)
        {
            await base.AuthorizeOrgAccessAsync(user, org, typeof(TelemetryReportData));
            return await _telemetryService.GetItemDetailsAsync(itemId, recordType);
        }
    }
}
