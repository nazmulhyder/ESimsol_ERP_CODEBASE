using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region IssueFigure
    
    public class IssueFigure : BusinessObject
    {
        public IssueFigure()
        {
            IssueFigureID = 0;
            ContractorID = 0;
            ChequeIssueTo = "";
            SecondLineIssueTo = "";
            DetailNote = "";
            IsActive = true;
            
            ErrorMessage = "";
        }

        #region Properties
        
        public int IssueFigureID { get; set; }
        
        public int ContractorID { get; set; }
        
        public string ChequeIssueTo { get; set; }
        
        public string SecondLineIssueTo { get; set; }
        
        public string DetailNote { get; set; }
        
        public bool IsActive { get; set; }
        
        
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Property

        public string ContractorName { get; set; }

        public string IsActiveInString { get { return this.IsActive ? "Active" : "In-active"; } }
        public int IsActiveInInt { get { return this.IsActive ? 1 : 0; } }
        #endregion

        #region Functions
        public static List<IssueFigure> Gets(int nCurrentUserID)
        {
            return IssueFigure.Service.Gets(nCurrentUserID);
        }
        public static List<IssueFigure> Gets(int nContractorID, int nCurrentUserID)
        {
            return IssueFigure.Service.Gets(nContractorID, nCurrentUserID);
        }
        public IssueFigure Get(int id, int nCurrentUserID)
        {
            return IssueFigure.Service.Get(id, nCurrentUserID);
        }
        public IssueFigure Save(int nCurrentUserID)
        {
            return IssueFigure.Service.Save(this, nCurrentUserID);
        }
        public string Delete(int id, int nCurrentUserID)
        {
            return IssueFigure.Service.Delete(id, nCurrentUserID);
        }
        public static List<IssueFigure> Gets(string sSQL, int nCurrentUserID)
        {
            return IssueFigure.Service.Gets(sSQL, nCurrentUserID);
        }
        public static List<IssueFigure> GetsByName(int nContractorID, string sName, int nCurrentUserID)
        {
            return IssueFigure.Service.GetsByName(nContractorID, sName, nCurrentUserID);
        }
        public static List<IssueFigure> Gets(int nContractorID, bool bIsActive, int nCurrentUserID)
        {
            return IssueFigure.Service.Gets(nContractorID, bIsActive, nCurrentUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IIssueFigureService Service
        {
            get { return (IIssueFigureService)Services.Factory.CreateService(typeof(IIssueFigureService)); }
        }
        #endregion
    }
    #endregion

    #region IIssueFigure interface
    
    public interface IIssueFigureService
    {
        
        List<IssueFigure> Gets(int nContractorID, bool bIsActive, int nCurrentUserID);
        
        List<IssueFigure> GetsByName(int nContractorID, string sName, int nCurrentUserID);
        
        IssueFigure Get(int id, int nCurrentUserID);
        
        List<IssueFigure> Gets(int nCurrentUserID);
        
        List<IssueFigure> Gets(int nContractorID, int nCurrentUserID);
        
        List<IssueFigure> Gets(string sSQL, int nCurrentUserID);
        
        string Delete(int id, int nCurrentUserID);
        
        IssueFigure Save(IssueFigure oIssueFigure, int nCurrentUserID);
    }
    #endregion
}