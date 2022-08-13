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
    public class ExportFundAllocationHead : BusinessObject
    {
        public ExportFundAllocationHead()
        {
            ExportFundAllocationHeadID = 0;
            Code = "";
            Name = "";
            Sequence = 0;
            Remarks = "";
            ErrorMessage = "";
        }
        #region Properties
        public int ExportFundAllocationHeadID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public ExportFundAllocationHead Save(long nUserID)
        {
            return ExportFundAllocationHead.Service.Save(this, nUserID);
        }
        public static List<ExportFundAllocationHead> Gets(string sSQL, long nUserID)
        {
            return ExportFundAllocationHead.Service.Gets(sSQL, nUserID);
        }
        public ExportFundAllocationHead Get(int id, long nUserID)
        {
            return ExportFundAllocationHead.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ExportFundAllocationHead.Service.Delete(id, nUserID);
        }
        public List<ExportFundAllocationHead> RefreshSequence(List<ExportFundAllocationHead> oExportFundAllocationHeads,long nUserID)
        {
            return ExportFundAllocationHead.Service.RefreshSequence(oExportFundAllocationHeads, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IExportFundAllocationHeadService Service
        {
            get { return (IExportFundAllocationHeadService)Services.Factory.CreateService(typeof(IExportFundAllocationHeadService)); }
        }
        #endregion
    }
    #region IExportFundAllocationHead interface

    public interface IExportFundAllocationHeadService
    {
        ExportFundAllocationHead Save(ExportFundAllocationHead oExportFundAllocationHead, Int64 nUserID);
        List<ExportFundAllocationHead> Gets(string sSQL, long nUserID);
        ExportFundAllocationHead Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        List<ExportFundAllocationHead> RefreshSequence(List<ExportFundAllocationHead> oExportFundAllocationHead, long nUserID);
    }
    #endregion
}
