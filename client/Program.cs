﻿using System;
using System.Threading.Tasks;
using NLog;

namespace SmartBulkCopy
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            int result = 0;
            var logger = LogManager.GetCurrentClassLogger();

            var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            logger.Info($"Smart Bulk Copy v. {v}");

            try
            {
                SmartBulkCopyConfiguration bulkCopyConfig;
                if (args.Length > 0)
                    bulkCopyConfig = SmartBulkCopyConfiguration.LoadFromConfigFile(args[0], logger);
                else 
                    bulkCopyConfig = SmartBulkCopyConfiguration.LoadFromConfigFile(logger);
                
                var sbc = new SmartBulkCopy(bulkCopyConfig, logger);
                result = await sbc.Copy();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception.");
                result = 1;
            }
            finally
            {
                LogManager.Shutdown();
            }

            return result;
        }
    }
}
