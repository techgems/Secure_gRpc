﻿using Duplex;
using Microsoft.Extensions.Logging;
using Secure_gRpc.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Secure_gRpc
{
   
    public class ServerGrpcSubscribers
    {
        private readonly ILogger _logger;
        public HashSet<SubscribersModel> Subscribers = new HashSet<SubscribersModel>();

        public ServerGrpcSubscribers(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ServerGrpcSubscribers>();
        }

        public async Task BroadcastMessageAsync(MyMessage message)
        {
            foreach (var subscriber in Subscribers)
            {
                await SendMessageToSubscriber(subscriber, message);
            }
        }

        private async Task SendMessageToSubscriber(SubscribersModel subscriber, MyMessage message)
        {
            try
            {
                _logger.LogInformation($"Broadcasting: {message.Name} - {message.Message}");
                await subscriber.Subscriber.WriteAsync(message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Could not send");
            }
        }
    }
}
