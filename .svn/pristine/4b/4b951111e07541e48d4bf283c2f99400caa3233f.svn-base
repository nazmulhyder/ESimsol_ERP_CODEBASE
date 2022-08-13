using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ImportInvChallan
    public class ImportInvChallan : BusinessObject
    {
        #region  Constructor
        public ImportInvChallan()
        {
            ImportInvChallanID = 0;
            ChallanNo = "";
            ChallanDate = DateTime.Now;
            PackCountBy = EnumPackCountBy.Bales;
            ImportInvChallanDetails = new List<ImportInvChallanDetail>();
            VehicleInfo = "";
            CotractNo = "";
            Note = "";
            PackCountByInInt = 0;
            IsGRN = false;
            ImportInvoiceNo = "";
            ApproveBy = 0;
            ReceiveBy = 0;
            PrepareByName = "";
            ApproveByName = "";
            ReceiveByName = "";
        }

        #endregion

        #region Properties
        public int ImportInvChallanID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public string DriverName { get; set; }
        public string VehicleInfo { get; set; }
        public int ApproveBy { get; set; }
        public int ReceiveBy { get; set; }
        public string CotractNo { get; set; }
        public bool IsGRN { get; set; }
        public EnumPackCountBy PackCountBy { get; set; }
        public string Note { get; set; }
        public int PackCountByInInt { get; set; }
        public string ErrorMessage { get; set; }
        public string ImportInvoiceNo { get; set; }
        #region Derive property
        public string PrepareByName { get; set; }
        public string ApproveByName { get; set; }
        public string ReceiveByName { get; set; }
        public List<ImportInvChallanDetail> ImportInvChallanDetails { get; set; }

        #endregion
        public string PackCountByInString
        {
            get
            {
                return this.PackCountBy.ToString();
            }

        }
        public string ChallanDateSt
        {
            get
            {
                if (this.ChallanDate == DateTime.MinValue)
             {
                 return "-";
             }
             else
             {
                 return ChallanDate.ToString("dd MMM yyyy");
             }
            }
        }
        public string IsGRNSt
        {
            get
            {
                if (this.IsGRN)
                {
                    return "GRN Created";
                }
                else
                {
                    return "Waiting For GRN";
                }
            }
        }
        

       
        #endregion

        #region Function New Version
        public ImportInvChallan Save(int nUserID)
        {
            return ImportInvChallan.Service.Save(this, nUserID);
        }
        public ImportInvChallan Approved(int nUserID)
        {
            return ImportInvChallan.Service.Approved(this, nUserID);
        }
        public ImportInvChallan Received(int nUserID)
        {
            return ImportInvChallan.Service.Received(this, nUserID);
        }
    
        public string Delete(int nUserID)
        {
            return ImportInvChallan.Service.Delete(this, nUserID);
        }

        public ImportInvChallan Get(int nImportInvChallanID, int nUserID)
        {
            return ImportInvChallan.Service.Get(nImportInvChallanID, nUserID);
        }
        public ImportInvChallan GetByInvoice(int nInvoiceID, int nUserID)
        {
            return ImportInvChallan.Service.GetByInvoice(nInvoiceID, nUserID);
        }

        public static List<ImportInvChallan> Gets(int nUserID)
        {
            return ImportInvChallan.Service.Gets(nUserID);
        }
        public static List<ImportInvChallan> Gets(string sSQL, int nUserID)
        {
            return ImportInvChallan.Service.Gets(sSQL, nUserID);
        }

        #endregion


        #region ServiceFactory
        internal static IImportInvChallanService Service
        {
            get { return (IImportInvChallanService)Services.Factory.CreateService(typeof(IImportInvChallanService)); }
        }
        #endregion

    }
    #endregion

    #region IImportInvChallan interface
    public interface IImportInvChallanService
    {
        List<ImportInvChallan> Gets(Int64 nUserID);
        string Delete(ImportInvChallan oImportLC, Int64 nUserID);
        ImportInvChallan Save(ImportInvChallan oImportInvChallan, Int64 nUserID);
        ImportInvChallan Approved(ImportInvChallan oImportInvChallan, Int64 nUserID);
        ImportInvChallan Received(ImportInvChallan oImportInvChallan, Int64 nUserID);
        ImportInvChallan Get(int nImportInvChallanID, Int64 nUserID);
        ImportInvChallan GetByInvoice(int nImportInvoiceID, Int64 nUserID);
        List<ImportInvChallan> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
