﻿using LagoVista.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Deployment.Admin.Models
{
    public class DeploymentConnectionSettings : IConnectionSettings
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public string Baud { get; set; }
        public string IPAddressV4 { get; set; }
        public string IPAddressV6 { get; set; }
        public string AccessKey { get; set; }
        public string UserName { get; set; }
        public string AccountId { get; set; }
        public string DeviceId { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string ResourceName { get; set; }
        public bool IsSSL { get; set; }
        public Func<bool> ValidationAction { get; set; }
        public Func<string> GetValidationErrors { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
