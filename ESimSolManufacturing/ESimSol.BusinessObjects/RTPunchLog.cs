using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region Bank

    public class RTPunchLog : BusinessObject
    {
        public RTPunchLog()
        {
            RTId = 0;
            C_Date = "";
            C_Time = "";
            C_Unique = "";
            ErrorMessage = "";
            RTPs = new List<RTPunchLog>();

        }

        #region Properties

        public int RTId { get; set; }

        public string C_Date { get; set; }

        public string C_Time { get; set; }

        public string C_Unique { get; set; }

        public string ErrorMessage { get; set; }

        public List<RTPunchLog> RTPs { get; set; }
        #endregion
    }
    #endregion

}