using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApp_Infostrade.Hubs
{
    public class SignalrHub : Hub
    {
        public Task SendMessage(string iotMessage)
        {
            return Clients.All.SendAsync("iotMessage", iotMessage);
        }
    }
}
