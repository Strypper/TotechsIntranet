using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntranetUWP.Helpers
{
    public class IntranetSignalRHelper
    {
        private readonly HubConnection _hubConnection;
        public IntranetSignalRHelper(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
        }

        public async Task Connect() => await _hubConnection.StartAsync();
    }
}
