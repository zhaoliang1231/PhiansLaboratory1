using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians
{
    public class SignalR_User_id : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            if (request.GetHttpContext().Request.Cookies["SignalRID"] != null)
            {
                return request.GetHttpContext().Request.Cookies["SignalRID"].Value.Trim();
            }
            return "admins";
            // return Guid.NewGuid().ToString();
        }
    }
}