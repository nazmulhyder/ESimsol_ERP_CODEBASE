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
    #region AdjustmentRequisitionSlipDetail
    
    public class AdjustmentRequisitionSlipDetail : BusinessObject
    {
        #region  Constructor
        public AdjustmentRequisitionSlipDetail()
        {
            AdjustmentRequisitionSlipDetailID = 0;
            AdjustmentRequisitionSlipID = 0;
            ProductID = 0;
            Qty = 0;
            LotID = 0;        
            ErrorMessage = "";

        }
        #endregion

        #region Properties
        
        public int AdjustmentRequisitionSlipDetailID { get; set; }
        public int AdjustmentRequisitionSlipID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double CurrentBalance { get; set; }
        public int LotID { get; set; }        
        public string Note { get; set; }
        public String ErrorMessage { get; set; }
        public string ColorName { get; set; }
        #endregion

        #region derived
        public string ProductName { get; set; }
        public string LotNo { get; set; }
        public string WorkingUnitID { get; set; }
        public string LocationName { get; set; }
        public string LocationShortName { get; set; }
        public string OperationUnitName { get; set; }
        public string OUShortName { get; set; }
        public string ProductCode { get; set; }

        public string StoreShortName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.LocationShortName))
                {
                    if (!string.IsNullOrEmpty(this.OUShortName))
                        return this.LocationShortName + "[" + this.OUShortName + "]";
                    else
                        return this.LocationShortName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.OUShortName))
                        return "[" + this.OUShortName + "]";
                    else
                        return "";
                }
            }
        }
        
        #endregion

        #region Functions
        public AdjustmentRequisitionSlipDetail Get(int nAdjustmentRequisitionSlipDetailID, long nUserID)
        {
            return AdjustmentRequisitionSlipDetail.Service.Get(nAdjustmentRequisitionSlipDetailID, nUserID);
        }
        public static List<AdjustmentRequisitionSlipDetail> Gets(int nAdjustmentRequisitionSlipID, long nUserID)
        {
            return AdjustmentRequisitionSlipDetail.Service.Gets(nAdjustmentRequisitionSlipID, nUserID);
        }
        public AdjustmentRequisitionSlipDetail Save(long nUserID)
        {
            return AdjustmentRequisitionSlipDetail.Service.Save(this, nUserID);
        }
        public static List<AdjustmentRequisitionSlipDetail> Gets(string sSQL, long nUserID)
        {
            return AdjustmentRequisitionSlipDetail.Service.Gets(sSQL, nUserID);
        }   

        public string Delete(long nUserID)
        {
            return AdjustmentRequisitionSlipDetail.Service.Delete(this, nUserID);
        }
      

        #endregion

        #region ServiceFactory
        internal static IAdjustmentRequisitionSlipDetailService Service
        {
            get { return (IAdjustmentRequisitionSlipDetailService)Services.Factory.CreateService(typeof(IAdjustmentRequisitionSlipDetailService)); }
        }
        #endregion
    }
    #endregion



    #region IAdjustmentRequisitionSlipDetail interface
    
    public interface IAdjustmentRequisitionSlipDetailService
    {

        AdjustmentRequisitionSlipDetail Get(int id, long nUserID);
        List<AdjustmentRequisitionSlipDetail> Gets(string sSQL, long nUserID);    
        List<AdjustmentRequisitionSlipDetail> Gets(int nAdjustmentRequisitionSlipID, long nUserID);
        string Delete(AdjustmentRequisitionSlipDetail oAdjustmentRequisitionSlipDetail, long nUserID);
        AdjustmentRequisitionSlipDetail Save(AdjustmentRequisitionSlipDetail oAdjustmentRequisitionSlipDetail, long nUserID);
      

    }
    #endregion
}
