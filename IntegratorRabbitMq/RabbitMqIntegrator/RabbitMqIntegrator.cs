﻿using IntegratorRabbitMq.Interfaces.RabbitMqIntegrator;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratorRabbitMq.RabbitMqIntegrator
{
    public class RabbitMqIntegrator : IRabbitMqIntegrator
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _model;
        private readonly IModel _channel;
        
        public RabbitMqIntegrator(string connectionStringRabbit)
        {
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(connectionStringRabbit)
            };
            _model = _connectionFactory.CreateConnection();
            _channel = _model.CreateModel();
        }

        public void CreateQueueDeclare(string queueName)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            
        }    
        public void ConfigureQueue(string queueName, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            _channel.QueueDeclare(queue: queueName, durable: durable, exclusive: exclusive, autoDelete: autoDelete, arguments: arguments);
            
        }
        public void PublishQueue<T>(T message, string queueName)
        {
            var stringMesage = JsonConvert.SerializeObject(message);
            var byteMessage = Encoding.UTF8.GetBytes(stringMesage);
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: byteMessage);
        }
    }
}