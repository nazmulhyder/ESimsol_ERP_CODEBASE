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
    #region FabricSuggestionHistory

    public class FabricSuggestionHistory : BusinessObject
    {
        public FabricSuggestionHistory()
        {
            FabricSuggestionID = 0;
            FabricID = 0;
            ProductID = 0;
            Construction = string.Empty;
            ColorInfo = string.Empty;
            FabricWidth = string.Empty;
            FinishType  = 0;
            Remarks = string.Empty;
            ProcessType  = 0;
            FabricWeave  = 0;
            FabricDesignID  = 0;
            WeftColor = string.Empty;
            FabricNo = string.Empty;
            ProductCode = string.Empty;
            ProductName = string.Empty;
            FinishTypeName = string.Empty;
            ProcessTypeName = string.Empty;
            FabricWeaveName = string.Empty;
            ActualConstruction = string.Empty;
            CreatedByName = string.Empty;
            ModifiedByName = string.Empty;
            ErrorMessage = string.Empty;
            DBServerDateTime = DateTime.Now;
        }

        #region Properties
        public int FabricSuggestionID  {get; set;}
        public int FabricID  {get; set;}
        public int ProductID  {get; set;}
        public string Construction  {get; set;}
        public string ColorInfo {get; set;}
        public string FabricWidth  {get; set;}
        public int FinishType  {get; set;}
        public string Remarks {get; set;}
        public int ProcessType  {get; set;}
        public int FabricWeave  {get; set;}
        public int FabricDesignID { get; set; }
        public string WeftColor {get; set;}
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string FabricNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string FinishTypeName { get; set; }
        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public string ActualConstruction { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime DBServerDateTime { get; set; }

        public string ModifiedTimeStr
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy hh:mm");
            }
        }

        #endregion

        #region Functions
        public static List<FabricSuggestionHistory> Gets(string sSQL, Int64 nUserID)
        {
            return FabricSuggestionHistory.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricSuggestionHistoryService Service
        {
            get { return (IFabricSuggestionHistoryService)Services.Factory.CreateService(typeof(IFabricSuggestionHistoryService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricSuggestionHistory interface

    public interface IFabricSuggestionHistoryService
    {
        List<FabricSuggestionHistory> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}