using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ESimSolFinancial.Hubs
{
    public class ProgressHub : Hub
    {
        public static void SendMessage(string msg, double nCount,int nUserID)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            hubContext.Clients.All.broadcastMessage(string.Format(msg), nCount);
            //hubContext.Clients.Client()
        }

        public class CustomUserIdProvider : IUserIdProvider
        {
            public string GetUserId(IRequest request)
            {
                // your logic to fetch a user identifier goes here.

                // for example:
                return request.User.Identity.Name;
                //var userId = MyCustomUserClass.FindUserId();
                //return ((User)Session[SessionInfo.CurrentUser]).UserID;
                //return userId.ToString();
            }
        }
    }
}