using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.ServiceModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    
    public class UserActionLogCount : BusinessObject
    {
        public UserActionLogCount()
        {
            LogIn = 0;
            LogOut = 0;
            WrongPass = 0;
        }
        
        public int LogIn { get; set; }
        
        public int LogOut { get; set; }
        
        public int WrongPass { get; set; }
    }
}