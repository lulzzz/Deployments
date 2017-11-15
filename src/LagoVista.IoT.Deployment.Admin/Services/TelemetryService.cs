﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Logging.Models;

namespace LagoVista.IoT.Deployment.Admin.Services
{
    //TODO: This needs a bit more thought put into it, should be good enough to start with though.
    public class TelemetryService : ITelemetryService
    {
        ILogReader _logReader;

        public TelemetryService(ILogReader reader)
        {
            _logReader = reader;
        }

        
        private ListResponse<TelemetryReportData> ToTelemetryData(ListResponse<LogRecord> logRecords, string recordType)
        {
            var lr = new ListResponse<TelemetryReportData>
            {
                NextPartitionKey = logRecords.NextPartitionKey,
                NextRowKey = logRecords.NextRowKey
            };

            var trdList = new List<TelemetryReportData>();
            foreach (var logRecord in logRecords.Model)
            {
                trdList.Add(TelemetryReportData.FromLogRecord(logRecord, recordType));
            }

            return lr;
        }

        private async Task<ListResponse<TelemetryReportData>> GetRecordsAsync(string deviceId, string recordType, ListRequest request)
        {
            if (recordType == "error")
            {
                var logRecords = await _logReader.GetErrorsAsync(deviceId, request, ResourceType.Device);
                return ToTelemetryData(logRecords, recordType);
            }
            else
            {
                var logRecords = await _logReader.GetLogRecordsAsync(deviceId, request, ResourceType.Device);
                return ToTelemetryData(logRecords, recordType);
            }
        }

        public Task<ListResponse<TelemetryReportData>> GetForDeviceAsync(string deviceId, string recordType, ListRequest request)
        {
            return GetRecordsAsync(deviceId, recordType, request);
        }

        public Task<ListResponse<TelemetryReportData>> GetForDeviceTypeAsync(string deviceTypeId, string recordType, ListRequest request)
        {
            return GetRecordsAsync(deviceTypeId, recordType, request);
        }

        public Task<ListResponse<TelemetryReportData>> GetForHostAsync(string hostId, string recordType, ListRequest request)
        {
            return GetRecordsAsync(hostId, recordType, request);
        }

        public Task<ListResponse<TelemetryReportData>> GetForInstanceAsync(string instanceId, string recordType, ListRequest request)
        {
            return GetRecordsAsync(instanceId, recordType, request);
        }

        public Task<ListResponse<TelemetryReportData>> GetForPipelineModuleAsync(string pipelineModuleId, string recordType, ListRequest request)
        {
            return GetRecordsAsync(pipelineModuleId, recordType, request);
        }

        public Task<ListResponse<TelemetryReportData>> GetForPipelineQueueAsync(string pipelineModuleId, string recordType, ListRequest request)
        {
            return GetRecordsAsync(pipelineModuleId, recordType, request);
        }
    }
}
