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
    #region FabricExecutionOrderSpecificationDetail

    public class FabricExecutionOrderSpecificationDetail : BusinessObject
    {
        #region  Constructor
        public FabricExecutionOrderSpecificationDetail()
        {
            FEOSDID = 0;
            FEOSID = 0;
            IsWarp = true;
            ProductID  = 0;
            LabdipDetailID = 0;
            ColorName = "";
            EndsCount = 0;
            Value = 0;
            PantonNo = "";
            ColorNo = "";
            Length = 0;
            Qty = 0;
            Allowance = 0;
            ErrorMessage = "";
            ValueMin = 0;
            AllowanceWarp = 0;
            SLNo = 0;
            BatchNo = "";
            LDNo = "";
            YarnType = "";
            CellRowSpans = new List<CellRowSpan>();
            CellRowSpansWeft = new List<CellRowSpan>();
            TwistedGroup = 0;
            BeamType = EnumBeamType.None;
            IsYarnExist = false;
            TwistedGroupInt = 0;
            SCNoFull="";
		    IsInHouse=true;
            PINo = "" ;
            FSCDID = 0;
        }
        #endregion

        #region Properties
        public int FEOSDID { get; set; }
        public int FEOSID { get; set; }
        public bool IsWarp { get; set; }
        public int ProductID { get; set; }
        public string ColorName { get; set; }
        public int EndsCount { get; set; }
        public double Value { get; set; }
        public double ValueMin { get; set; }
        public double Allowance { get; set; }//percentage
        public double AllowanceWarp { get; set; }//percentage
        public double Qty { get; set; }
        public double Length { get; set; }
        public double SLNo { get; set; }
        public string BatchNo { get; set; }
        public string LDNo { get; set; }/// entry manualy ,Please delete it after LD implement
        public int TwistedGroupInt { get; set; }
        public int TwistedGroup { get; set; }

        public int FSCDID { get; set; }
        public string SCNoFull { get; set; }
        public bool IsInHouse { get; set; }
        public string PINo { get; set; }
        public string ExeNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Properties
        public double TotalEnd { get; set; }
        public double TotalEndActual { get; set; }
        public FabricExecutionOrderSpecification FEOS { get; set; }
        public List<CellRowSpan> CellRowSpans { get; set; }
        public List<CellRowSpan> CellRowSpansWeft { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ColorNo { get; set; }
        public string PantonNo { get; set; }
        public int LabdipDetailID { get; set; }
        public string ProductShortName { get; set; }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }
        public string YarnType { get; set; }
        public EnumBeamType BeamType { get; set; }
        public bool IsYarnExist { get; set; }
        
        #endregion

        #region Functions
        public static FabricExecutionOrderSpecificationDetail Get(int nFEOSDID, long nUserID)
        {
            return FabricExecutionOrderSpecificationDetail.Service.Get(nFEOSDID, nUserID);
        }
        public static List<FabricExecutionOrderSpecificationDetail> Gets(int nFEOSID, long nUserID)
        {
            return FabricExecutionOrderSpecificationDetail.Service.Gets(nFEOSID, nUserID);
        }
        public static List<FabricExecutionOrderSpecificationDetail> Gets(string sSQL, long nUserID)
        {
            return FabricExecutionOrderSpecificationDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricExecutionOrderSpecificationDetail IUD(int nDBOperation, long nUserID)
        {
            return FabricExecutionOrderSpecificationDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public FabricExecutionOrderSpecificationDetail UpdateAllowance(Int64 nUserID)
        {
            return FabricExecutionOrderSpecificationDetail.Service.UpdateAllowance(this, nUserID);
        }
     
        public string DeleteAll(Int64 nUserID)
        {
            return FabricExecutionOrderSpecificationDetail.Service.DeleteAll(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricExecutionOrderSpecificationDetailService Service
        {
            get { return (IFabricExecutionOrderSpecificationDetailService)Services.Factory.CreateService(typeof(IFabricExecutionOrderSpecificationDetailService)); }
        }
        #endregion

    }
    #endregion

    #region IFabricExecutionOrderSpecificationDetail interface
    public interface IFabricExecutionOrderSpecificationDetailService
    {
        FabricExecutionOrderSpecificationDetail Get(int nFEOSDID, long nUserID);
        List<FabricExecutionOrderSpecificationDetail> Gets(int nFEOSID, long nUserID);
        List<FabricExecutionOrderSpecificationDetail> Gets(string sSQL, long nUserID);
        FabricExecutionOrderSpecificationDetail IUD(FabricExecutionOrderSpecificationDetail oFabricExecutionOrderSpecificationDetail, int nDBOperation, long nUserID);
        FabricExecutionOrderSpecificationDetail UpdateAllowance(FabricExecutionOrderSpecificationDetail oFESDetail, Int64 nUserID);
     
        string DeleteAll(FabricExecutionOrderSpecificationDetail oFabricExecutionOrderSpecificationDetail, Int64 nUserID);
    }
    #endregion
}