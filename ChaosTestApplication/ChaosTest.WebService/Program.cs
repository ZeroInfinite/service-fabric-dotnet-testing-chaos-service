﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace ChaosTest.WebService
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using ChaosTest.Common;
    using Microsoft.ServiceFabric.Services.Runtime;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync(
                    StringResource.ChaosTestWebServiceType,
                    context =>
                        new WebService(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(WebService).Name);

                // "Restart code package" fault - generated by Chaos engine - uses Ctrl+C internally, 
                // and visual studio debugger breaks on Thread.Sleep if Ctrl+C happens;
                // So, if debugger break on the line below, please just hit Continue.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e);
                throw;
            }
        }
    }
}