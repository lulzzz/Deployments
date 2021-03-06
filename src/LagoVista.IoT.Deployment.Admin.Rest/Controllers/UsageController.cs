﻿using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Deployment.Admin.Rest.Controllers
{
    [Authorize]
    public class UsageController : LagoVistaBaseController
    {
        IUsageMetricsManager _usageManager;
        public UsageController(IUsageMetricsManager usageManager, UserManager<AppUser> userManager, IAdminLogger logger) : base(userManager, logger)
        {
            _usageManager = usageManager;
        }

        /// <summary>
        /// Usage Metrics - Get For Host
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/usagemetrics/host/{id}")]
        public Task<ListResponse<UsageMetrics>> GetHostUsageAsync(String id)
        {
            return _usageManager.GetMetricsForHostAsync(id, GetListRequestFromHeader(), OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Usage Metrics - Get For Instance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/usagemetrics/instance/{id}")]
        public Task<ListResponse<UsageMetrics>> GetInstanceUsageAsync(String id)
        {
            return _usageManager.GetMetricsForInstanceAsync(id, GetListRequestFromHeader(), OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Usage Metrics - Get For PipelineModule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/usagemetrics/pipelinemodule/{id}")]
        public Task<ListResponse<UsageMetrics>> GetPiplelineModuleUsageAsync(String id)
        {
            return _usageManager.GetMetricsForPipelineModuleAsync(id, GetListRequestFromHeader(), OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Usage Metrics - Get For PipelineModule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/usagemetrics/dependency/{id}")]
        public Task<ListResponse<UsageMetrics>> GetDependencyUsageAsync(String id)
        {
            return _usageManager.GetMetricsForDependencyAsync(id, GetListRequestFromHeader(), OrgEntityHeader, UserEntityHeader);
        }

    }
}