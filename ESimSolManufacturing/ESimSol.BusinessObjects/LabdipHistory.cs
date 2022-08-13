using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region LabdipHistory
    
    public class LabdipHistory : BusinessObject
    {

        #region  Constructor
        public LabdipHistory()
        {
            LabdipHistoryID = 0;
            LabDipID = 0;
            Currentstatus = EnumLabdipOrderStatus.None;
            Previousstatus = EnumLabdipOrderStatus.None;
            Note="";
            Days = "";
            DateTime = DateTime.Now;
        }
        #endregion

        #region Properties
       
        public int LabdipHistoryID { get; set; }
        public int LabDipID { get; set; }
        public EnumLabdipOrderStatus Currentstatus { get; set; }
        public EnumLabdipOrderStatus Previousstatus { get; set; }
        public string Note { get; set; }
        public string Days { get; set; }
        public DateTime DateTime { get; set; }
        
        #endregion
        #region Derived Properties
        public string UserName { get; set; }
        public string DateTimeSt { get { return DateTime.ToString("dd MMM yyyy hh:mm tt"); } }
        public string StatusSt { get { return EnumObject.jGet(this.Currentstatus);} }
         #endregion

        #region Functions
        public LabdipHistory Get(int nId, Int64 nUserID)
        {
            return LabdipHistory.Service.Get(nId, nUserID);
        }
        public LabdipHistory Getby(int nId, int nOrderStatus, Int64 nUserID)
        {
            return LabdipHistory.Service.Getby(nId, nOrderStatus, nUserID);
        }
        public static List<LabdipHistory> Gets(int nLabdipID, int nUserID)
        {
            return LabdipHistory.Service.Gets( nLabdipID, nUserID);
        }
        
        #region  Collection Functions


       
        #endregion
        #endregion

        #region ServiceFactory

        internal static ILabdipHistoryService Service
        {
            get { return (ILabdipHistoryService)Services.Factory.CreateService(typeof(ILabdipHistoryService)); }
        }

        #endregion
    }
    #endregion

    

    #region ILabdipHistory interface
    public interface ILabdipHistoryService
    {
        LabdipHistory Get(int id, Int64 nUserID);
        LabdipHistory Getby(int nLabdipID, int nOrderStatus, Int64 nUserID);
        List<LabdipHistory> Gets(int nLabdipID, Int64 nUserID);
     
        
    }
    #endregion
}
