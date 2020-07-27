﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;

namespace MagicOnion.Server
{
    public class CompositeMagicOnionLogger : IMagicOnionLogger
    {
        private readonly IMagicOnionLogger[] inner;

        public CompositeMagicOnionLogger(params IMagicOnionLogger[] inner)
        {
            this.inner = inner;
        }

        public void BeginBuildServiceDefinition()
        {
            foreach (var x in inner)
            {
                x.BeginBuildServiceDefinition();
            }
        }

        public void EndBuildServiceDefinition(double elapsed)
        {
            foreach (var x in inner)
            {
                x.EndBuildServiceDefinition(elapsed);
            }
        }

        public void BeginInvokeMethod(ServiceContext context, byte[] request, Type type)
        {
            foreach (var x in inner)
            {
                x.BeginInvokeMethod(context, request, type);
            }
        }

        public void EndInvokeMethod(ServiceContext context, byte[] response, Type type, double elapsed, bool isErrorOrInterrupted)
        {
            foreach (var x in inner)
            {
                x.EndInvokeMethod(context, response, type, elapsed, isErrorOrInterrupted);
            }
        }

        public void WriteToStream(ServiceContext context, byte[] writeData, Type type)
        {
            foreach (var x in inner)
            {
                x.WriteToStream(context, writeData, type);
            }
        }

        public void ReadFromStream(ServiceContext context, byte[] readData, Type type, bool complete)
        {
            foreach (var x in inner)
            {
                x.ReadFromStream(context, readData, type, complete);
            }
        }

        public void BeginInvokeHubMethod(StreamingHubContext context, ReadOnlyMemory<byte> request, Type type)
        {
            foreach (var x in inner)
            {
                x.BeginInvokeHubMethod(context, request, type);
            }
        }

        public void EndInvokeHubMethod(StreamingHubContext context, int responseSize, Type type, double elapsed, bool isErrorOrInterrupted)
        {
            foreach (var x in inner)
            {
                x.EndInvokeHubMethod(context, responseSize, type, elapsed, isErrorOrInterrupted);
            }
        }

        public void InvokeHubBroadcast(string groupName, int responseSize, int broadcastGroupCount)
        {
            foreach (var x in inner)
            {
                x.InvokeHubBroadcast(groupName, responseSize, broadcastGroupCount);
            }
        }
    }
}
