using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region ExportPITandCClause
    public class ExportPITandCClause : BusinessObject
    {
        public ExportPITandCClause()
        {
            ExportPITandCClauseID = 0;
            ExportPIID = 0;
            TermsAndCondition = "";
            PITandCClauseLogID = 0;
            ExportTnCCaptionID = 0;
            DocFor = EnumDocFor.Common;
            ErrorMessage = "";

            ExportPITandCClauseLogID = 0;
            ExportPILogID = 0;
        }

        #region Properties        
        public int ExportPITandCClauseID { get; set; }
        public int ExportPIID { get; set; }
        public string CaptionName { get; set; }  
        public string TermsAndCondition { get; set; }        
        public int PITandCClauseLogID { get; set; }
        public int ExportTnCCaptionID { get; set; }        
        public string ErrorMessage { get; set; }
        public int ExportPITandCClauseLogID { get; set; }
        public int ExportPILogID { get; set; }
        public EnumDocFor DocFor { get; set; }
        public int DocForInInt { get; set; }
 
        #endregion
        #region 
        public string DocForInString
        {
            get
            {
                return this.DocFor.ToString();

            }
        }
        #endregion

        #region Functions
        public static List<ExportPITandCClause> Gets(int nPIID, Int64 nUserID)
        {
            return ExportPITandCClause.Service.Gets(nPIID, nUserID);
        }
        public static List<ExportPITandCClause> GetsPILog(int id, Int64 nUserID) // id is PI Log ID
        {
            return ExportPITandCClause.Service.GetsPILog(id, nUserID);            
        }
        public static List<ExportPITandCClause> Gets(string sSQL, Int64 nUserID)
        {
            return ExportPITandCClause.Service.Gets(sSQL, nUserID);            
        }
        public ExportPITandCClause Get(int id, Int64 nUserID)
        {
            return ExportPITandCClause.Service.Get(id, nUserID);            
        }
        public ExportPITandCClause Save(Int64 nUserID)
        {
            return ExportPITandCClause.Service.Save(this, nUserID);            
        }
        public string PITandCClauseSave(List<ExportPITandCClause> oExportPITandCClauses, Int64 nUserID)
        {
            return ExportPITandCClause.Service.PITandCClauseSave(oExportPITandCClauses, nUserID);            
        }
        public string Delete( Int64 nUserID)
        {
            return ExportPITandCClause.Service.Delete(this, nUserID);            
        }
        public string DeleteALL(Int64 nUserID)
        {
            return ExportPITandCClause.Service.DeleteALL(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportPITandCClauseService Service
        {
            get { return (IExportPITandCClauseService)Services.Factory.CreateService(typeof(IExportPITandCClauseService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPITandCClause interface
    public interface IExportPITandCClauseService
    {        
        ExportPITandCClause Get(int id, Int64 nUserID);        
        List<ExportPITandCClause> Gets(int nPIID, Int64 nUserID);        
        List<ExportPITandCClause> GetsPILog(int id, Int64 nUserID);        
        List<ExportPITandCClause> Gets(string sSQL, Int64 nUserID);
        ExportPITandCClause Save(ExportPITandCClause oExportPITandCClause, Int64 nUserID);        
        string PITandCClauseSave(List<ExportPITandCClause> oExportPITandCClauses, Int64 nUserID);        
        string Delete(ExportPITandCClause ooExportPITandCClause, Int64 nUserID);
        string DeleteALL(ExportPITandCClause ooExportPITandCClause, Int64 nUserID);
    }
    #endregion
}
