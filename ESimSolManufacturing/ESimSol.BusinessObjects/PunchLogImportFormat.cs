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
    #region PunchLogImportFormat

    public class PunchLogImportFormat : BusinessObject
    {
        public PunchLogImportFormat()
        {
            PLIFID = 0;
            PunchFormat = EnumPunchFormat.DD_MM_YY;
            ErrorMessage = "";

        }

        #region Properties

        public int PLIFID { get; set; }

        public EnumPunchFormat PunchFormat { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string PunchFormatInString
        {
            get
            {
                return PunchFormat.ToString();
            }
        }

        #endregion

        #region Functions
        public static List<PunchLogImportFormat> Gets(long nUserID)
        {
            return PunchLogImportFormat.Service.Gets(nUserID);
        }
        public PunchLogImportFormat IUD(long nUserID)
        {
            return PunchLogImportFormat.Service.IUD(this,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPunchLogImportFormatService Service
        {
            get { return (IPunchLogImportFormatService)Services.Factory.CreateService(typeof(IPunchLogImportFormatService)); }
        }

        #endregion
    }
    #endregion

    #region IPunchLogImportFormat interface

    public interface IPunchLogImportFormatService
    {
        List<PunchLogImportFormat> Gets(Int64 nUserID);
        PunchLogImportFormat IUD(PunchLogImportFormat oPunchLogImportFormat, Int64 nUserID);

    }
    #endregion
}
