using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DevelopmentRecapHistory
    
    public class DevelopmentRecapHistory : BusinessObject
    {
        public DevelopmentRecapHistory()
        {
            DevelopmentRecapHistoryID = 0;
            DevelopmentRecapID = 0;
            CurrentStatus = EnumDevelopmentStatus.Initialize;
            PreviousStatus = EnumDevelopmentStatus.Initialize;
            OperationDate = DateTime.Now;
            OperationBy = 0;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int DevelopmentRecapHistoryID { get; set; }
         
        public int DevelopmentRecapID { get; set; }
         
        public EnumDevelopmentStatus CurrentStatus { get; set; }
         
        public EnumDevelopmentStatus PreviousStatus { get; set; }
         
        public DateTime OperationDate { get; set; }
         
        public string Note { get; set; }
         
        public string ErrorMessage { get; set; }
         
        public int OperationBy { get; set; }
         
        public string MarchandiserName { get; set; }
        #endregion

        #region Derived Property

        public string OperationDateInString
        {
            get
            {
                return OperationDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
        public static List<DevelopmentRecapHistory> Gets(long nUserID)
        {
            return DevelopmentRecapHistory.Service.Gets(nUserID);
        }

        public DevelopmentRecapHistory Get(int id, long nUserID)
        {
            
            return DevelopmentRecapHistory.Service.Get(id, nUserID);
        }

        public DevelopmentRecapHistory Save(long nUserID)
        {
            return DevelopmentRecapHistory.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return DevelopmentRecapHistory.Service.Delete(id, nUserID);
        }

        public static List<DevelopmentRecapHistory> GetsDevelopmentRecapHistotry(int nDevelopmentRecapID, int nCurrentStatus, long nUserID)
        {
            return DevelopmentRecapHistory.Service.GetsDevelopmentRecapHistotry(nDevelopmentRecapID,nCurrentStatus, nUserID);
        }
        #endregion

        #region ServiceFactory
    
        internal static IDevelopmentRecapHistoryService Service
        {
            get { return (IDevelopmentRecapHistoryService)Services.Factory.CreateService(typeof(IDevelopmentRecapHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region IDevelopmentRecapHistory interface
     
    public interface IDevelopmentRecapHistoryService
    {
         
        DevelopmentRecapHistory Get(int id, Int64 nUserID);
         
        List<DevelopmentRecapHistory> Gets(Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        DevelopmentRecapHistory Save(DevelopmentRecapHistory oDevelopmentRecapHistory, Int64 nUserID);
         
        List<DevelopmentRecapHistory> GetsDevelopmentRecapHistotry(int nDevelopmentRecapID, int nCurrentStatus, Int64 nUserID);
    }
    #endregion
}
