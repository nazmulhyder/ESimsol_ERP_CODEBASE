using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class CharitySlab : BusinessObject
    {
        public CharitySlab()
        {
            CharitySlabID = 0;
            SalaryHeadID = 0;
            StartSalaryRange = 0;
            EndSalaryRange = 0;
            CharityAmount = 0;
            SalaryHeadName = "";
            UserName = "";
            ErrorMessage = "";
        }
        #region Properties

        public int CharitySlabID { get; set; }
        public int SalaryHeadID { get; set; }
        public double StartSalaryRange { get; set; }
        public double EndSalaryRange { get; set; }
        public double CharityAmount { get; set; }
        public string SalaryHeadName { get; set; }
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        public CharitySlab Save(long nUserID)
        {
            return CharitySlab.Service.Save(this, nUserID);
        }
        public static List<CharitySlab> Gets(string sSQL, long nUserID)
        {
            return CharitySlab.Service.Gets(sSQL, nUserID);
        }
        public CharitySlab Get(int id, long nUserID)
        {
            return CharitySlab.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return CharitySlab.Service.Delete(id, nUserID);
        }

    
        #region ServiceFactory
        internal static ICharitySlabService Service
        {
            get { return (ICharitySlabService)Services.Factory.CreateService(typeof(ICharitySlabService)); }
        }
        #endregion
    }
    #region ICharitySlabService interface

    public interface ICharitySlabService
    {
        CharitySlab Save(CharitySlab oCharitySlab, Int64 nUserID);
        List<CharitySlab> Gets(string sSQL, long nUserID);
        CharitySlab Get(int id, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
