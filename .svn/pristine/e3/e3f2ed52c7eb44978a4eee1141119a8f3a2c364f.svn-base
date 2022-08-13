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

    #region HRResponsibility

    public class HRResponsibility : BusinessObject
    {
        public HRResponsibility()
        {
            HRRID = 0;
            Code = "";
            Description = "";
            DescriptionInBangla = "";
        }

        #region Properties

        public int HRRID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DescriptionInBangla { get; set; }
        public string ErrorMessage { get; set; }


        #endregion

        #region derived property
        #endregion

        #region Functions

        public HRResponsibility Get(int id, long nUserID)
        {
            return HRResponsibility.Service.Get(id, nUserID);
        }
        public static List<HRResponsibility> Gets(long nUserID)
        {
            return HRResponsibility.Service.Gets(nUserID);
        }
        public HRResponsibility Save(long nUserID)
        {
            return HRResponsibility.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return HRResponsibility.Service.Delete(id, nUserID);
        }

        public static List<HRResponsibility> Gets(string sSQL, long nUserID)
        {
            return HRResponsibility.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IHRResponsibilityService Service
        {
            get { return (IHRResponsibilityService)Services.Factory.CreateService(typeof(IHRResponsibilityService)); }
        }

        #endregion
    }
    #endregion


    #region IHRResponsibilityService interface

    public interface IHRResponsibilityService
    {
        HRResponsibility Get(int id, Int64 nUserID);
        List<HRResponsibility> Gets(Int64 nUserID);
        List<HRResponsibility> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        HRResponsibility Save(HRResponsibility oHRResponsibility, Int64 nUserID);
    }
    #endregion

}
