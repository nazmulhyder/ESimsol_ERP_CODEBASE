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
    #region FabricPatternDetail

    public class FabricPatternDetail : BusinessObject
    {
        #region  Constructor
        public FabricPatternDetail()
        {
            FPDID=0;
            FPID=0;
            IsWarp=false;
            ProductID=0;
            ColorName = "";
            EndsCount = 0;
            Value = 0;
            ErrorMessage = "";
            Params = "";
            RepeatNo = 0;
            FabricPatternDetails = new List<FabricPatternDetail>();
            Count = 0;
            SetNo = 0;
            GroupNo = 0;
            FPDetailIds = "";
            PantonNo = "";
            ColorNo = "";
            LabDipDetailID = 0;
            TwistedGroup = 0;
            SLNo = 0;
            CellRowSpans = new List<CellRowSpan>();
            CellRowSpansWeft = new List<CellRowSpan>();
        }
        #endregion

        #region Properties
        public int FPDID { get; set; }
        public int FPID { get; set; }
        public bool IsWarp { get; set; }
        public int ProductID { get; set; }
        public string ColorName { get; set; }
        public int EndsCount { get; set; }
        public double Value { get; set; }
        public int SetNo { get; set; }
        public int GroupNo { get; set; }
        public string ErrorMessage { get; set; }
        public string FPDetailIds { get; set; }
        public int LabDipDetailID { get; set; }
        public int TwistedGroup { get; set; }
        public int TwistedGroupInt { get; set; }
        public int SLNo { get; set; }
        #endregion

        #region Derive Properties
        public string Params { get; set; }
        public int RepeatNo { get; set; }
        public int Count { get; set; }
        public FabricPattern FP { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductShortName { get; set; }
        public List<FabricPatternDetail> FabricPatternDetails { get; set; }
        public List<CellRowSpan> CellRowSpans { get; set; }
        public List<CellRowSpan> CellRowSpansWeft { get; set; }
        public string ColorNo { get; set; }
        public string PantonNo { get; set; }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName+"["+this.ProductCode+"]";
            }
        }
        #endregion

        #region Functions
        public static FabricPatternDetail Get(int nFPDID, long nUserID)
        {
            return FabricPatternDetail.Service.Get(nFPDID, nUserID);
        }
        public static List<FabricPatternDetail> Gets(int nFPID, long nUserID)
        {
            return FabricPatternDetail.Service.Gets(nFPID, nUserID);
        }
        public static List<FabricPatternDetail> Gets(string sSQL, long nUserID)
        {
            return FabricPatternDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricPatternDetail IUD(int nDBOperation, long nUserID)
        {
            return FabricPatternDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public FabricPatternDetail SavePatternRepeat(long nUserID)
        {
            return FabricPatternDetail.Service.SavePatternRepeat(this, nUserID);
        }
        public FabricPatternDetail CopyFromWarp(long nUserID)
        {
            return FabricPatternDetail.Service.CopyFromWarp(this, nUserID);
        }
        public static List<FabricPatternDetail> MakeTwistedGroup(string sLabDipDetailID, int nLabDipID, int nTwistedGroup,  int nDBOperation, int nUserID)
        {
            return FabricPatternDetail.Service.MakeTwistedGroup(sLabDipDetailID, nLabDipID, nTwistedGroup, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricPatternDetailService Service
        {
            get { return (IFabricPatternDetailService)Services.Factory.CreateService(typeof(IFabricPatternDetailService)); }
        }
        #endregion


    }
    #endregion


    #region IFabricPatternDetail interface
    public interface IFabricPatternDetailService
    {
        FabricPatternDetail Get(int nFPDID, long nUserID);
        List<FabricPatternDetail> Gets(int nFPID, long nUserID);
        List<FabricPatternDetail> Gets(string sSQL, long nUserID);
        FabricPatternDetail IUD(FabricPatternDetail oFabricPatternDetail, int nDBOperation, long nUserID); 
        FabricPatternDetail SavePatternRepeat(FabricPatternDetail oFabricPatternDetail, long nUserID);
        FabricPatternDetail CopyFromWarp(FabricPatternDetail oFabricPatternDetail, long nUserID);
        List<FabricPatternDetail> MakeTwistedGroup(string sLabDipDetailID, int nLabDipID, int nTwistedGroup,  int nDBOperation, int nUserID);
    }
    #endregion
}