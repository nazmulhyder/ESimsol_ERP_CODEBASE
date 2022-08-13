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
    #region ELProcessEditHistory
    [DataContract]
    public class ELProcessEditHistory :BusinessObject
    {
        public ELProcessEditHistory()
        {
            ELPEHID = 0;
            ELPID = 0;
            PreviousPresentBalance = 0;
            CurrentpresentBalance = 0;
            LastProcessDate = DateTime.Now;
            Description = "";
            ErrorMessage = "";
            EmployeeID = 0;
            UserName = "";
        }


        #region Properties
        public int ELPEHID { get; set; }
        public int ELPID { get; set; }
        public int PreviousPresentBalance { get; set; }
        public int CurrentpresentBalance { get; set; }
        public DateTime LastProcessDate { get; set; }
        public string Description { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Derived Property
        public string UserName { get; set; }
        public string LastProcessDateInString
        {
            get
            {
                return LastProcessDate.ToString("dd MMM yyyy");
            }
        }
        public int EmployeeID { get; set; }
        #endregion

        #region Functions
        public ELProcessEditHistory IUD(int nDBOperation, long nUserID)
        {
            return ELProcessEditHistory.Service.IUD(this, nDBOperation, nUserID);
        }
        public List<ELProcessEditHistory> Gets(string sSql, long nUserID)
        {
            return ELProcessEditHistory.Service.Gets(sSql, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IELProcessEditHistoryService Service
        {
            get { return (IELProcessEditHistoryService)Services.Factory.CreateService(typeof(IELProcessEditHistoryService)); }
        }
        #endregion
    }
    #endregion

    #region IELProcessEditHistory interface

    public interface IELProcessEditHistoryService
    {
        ELProcessEditHistory IUD(ELProcessEditHistory oELProcessEditHistory, int nDBOperation, Int64 nUserID);
        List<ELProcessEditHistory> Gets(string sSql, Int64 nUserID);

    }
    #endregion
}
