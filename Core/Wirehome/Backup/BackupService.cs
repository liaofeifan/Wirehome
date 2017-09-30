﻿using System;
using Wirehome.Contracts.Api;
using Wirehome.Contracts.Backup;
using Wirehome.Contracts.Services;
using Newtonsoft.Json.Linq;

namespace Wirehome.Backup
{
    [ApiServiceClass(typeof(IBackupService))]
    public class BackupService : ServiceBase, IBackupService
    {
        public event EventHandler<BackupEventArgs> CreatingBackup;
        public event EventHandler<BackupEventArgs> RestoringBackup;

        [ApiMethod]
        public void CreateBackup(IApiCall apiCall)
        {
            var backup = new JObject
            {
                ["Type"] = "Wirehome.Backup",
                ["Timestamp"] = DateTime.Now.ToString("O"),
                ["Version"] = 1
            };

            var eventArgs = new BackupEventArgs(backup);
            CreatingBackup?.Invoke(this, eventArgs);

            apiCall.Result = backup;
        }

        [ApiMethod]
        public void RestoreBackup(IApiCall apiCall)
        {
            if (apiCall.Parameter.Type != JTokenType.Object)
            {
                throw new NotSupportedException();
            }

            var eventArgs = new BackupEventArgs(apiCall.Parameter);
            RestoringBackup?.Invoke(this, eventArgs);
        }
    }
}
