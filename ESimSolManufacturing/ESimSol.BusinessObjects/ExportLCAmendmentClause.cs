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
    #region ExportLCAmendmentClause
    
    public class ExportLCAmendmentClause : BusinessObject
    {
        #region  Constructor
        public ExportLCAmendmentClause()
        {
            ExportLCAmendmentClauseID=0;
            ExportLCAmendRequestID=0;
            Clause = "";
        }
        #endregion

        #region Properties
        public int ExportLCAmendmentClauseID { get; set; }
        public int ExportLCAmendRequestID { get; set; }
        public string Clause { get; set; }

        #endregion

        #region Functions

        public static List<ExportLCAmendmentClause> Gets(int nExportLCAmendRequestID,long nUserID)
        {
            return ExportLCAmendmentClause.Service.Gets(nExportLCAmendRequestID,nUserID);
        }
        #endregion
        #region ServiceFactory

        internal static IExportLCAmendmentClauseService Service
        {
            get { return (IExportLCAmendmentClauseService)Services.Factory.CreateService(typeof(IExportLCAmendmentClauseService)); }
        }
        #endregion
    }
    #endregion

    #region IExportLCAmendmentClause interface
    
    public interface IExportLCAmendmentClauseService
    {
        List<ExportLCAmendmentClause> Gets(int nExportLCAmendRequestID, Int64 nUserId);
       
    }
    #endregion

}
