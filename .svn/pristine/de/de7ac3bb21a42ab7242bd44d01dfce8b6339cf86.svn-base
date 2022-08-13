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
    #region FNBatchCard
    
    public class FNBatchCard : BusinessObject
    {
        public FNBatchCard()
        {
            FNBatchCardID = 0;
            FNTreatmentProcessID = 0;
            FNBatchID = 0;
            PlannedDate = DateTime.MinValue;
            Params = "";
            Description = "";
            FNTreatment = EnumFNTreatment.None;
            PrepareByName = "";
            QCStatus = EnumQCStatus.Yet_To_QC;
            FNBatchCards = new List<FNBatchCard>();
            StartQty = 0;
            EndQty = 0;
            Qty_Prod = 0;
            Qty_ReProd = 0;
            Qty_FNPBatch = 0;
            Qty_FNBatch = 0;
            StartWidth = "";
            EndWidth = "";
            SequenceNo = 0;
            SequenceNo_Pro = 0;
            FNBatchNo = "";
            IsProduction = false;
            ErrorMessage = "";
            oFNBatchCards = new List<FNBatchCard>();
            
        }

        #region Properties
        public int FNBatchCardID { get; set; }
        public int FNTreatmentProcessID { get; set; }
        public int FNBatchID { get; set; }
        public string FNBatchNo { get; set; }
        public DateTime PlannedDate { get; set; }
        public string Params { get; set; }
        public string FNProcess { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int FSCDID { get; set; }
        public EnumQCStatus QCStatus { get; set; }
        public string PrepareByName { get; set; }
        public List<FNBatchCard> FNBatchCards { get; set; }
        public string ErrorMessage { get; set; }
        public int SequenceNo { get; set; }
        public int SequenceNo_Pro { get; set; }
        public int PreparedBy { get; set; }
        public int PreparedByName { get; set; }
        public double StartQty { get; set; }
        public double EndQty { get; set; }

        public double Qty_FNBatch { get; set; }
        public double Qty_Prod { get; set; }
        public double Qty_ReProd { get; set; }
        public double Qty_FNPBatch { get; set; }
        public double Qty_YetTo { get { return (this.Qty_FNPBatch - this.Qty_Prod); } }
        public int FNPBatchID { get; set; }
        public string StartWidth { get; set; }
        public string EndWidth { get; set; }
        public bool IsProduction { get; set; }
        public EnumFNTreatment FNTreatment { get; set; }
        #endregion

        #region Derived Property
        
        public List<FNBatchCard> oFNBatchCards { get; set; }
        public string PlannedDateSt
        {
            get
            {
                if (this.PlannedDate ==  DateTime.MinValue || this.PlannedDate.ToString("dd MMM yyyy") == "01 Jan 1900") return "";
                else return PlannedDate.ToString("dd MMM yyyy");
            }
        }
        public string FNTreatmentSt { get { return FNTreatment.ToString(); } set { } }
        public string QCStatusStr { get { return QCStatus.ToString(); } }
        #endregion

        #region Functions

        public static List<FNBatchCard> Gets(long nUserID)
        {
            return FNBatchCard.Service.Gets(nUserID);
        }
        public static List<FNBatchCard> Gets(string sSQL, Int64 nUserID)
        {
            return FNBatchCard.Service.Gets(sSQL, nUserID);
        }
        public FNBatchCard Get(int nId, long nUserID)
        {
            return FNBatchCard.Service.Get(nId,nUserID);
        }
        public FNBatchCard Save(long nUserID)
        {
            return FNBatchCard.Service.Save(this, nUserID);
        }
        public List<FNBatchCard> SaveFNBatchCards(FNBatchCard oFNBatchCard, long nUserID)
        {
            return FNBatchCard.Service.SaveFNBatchCards(oFNBatchCard, nUserID);
        }
        public string Delete(long nUserID)
        {
            return FNBatchCard.Service.Delete(this, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IFNBatchCardService Service
        {
            get { return (IFNBatchCardService)Services.Factory.CreateService(typeof(IFNBatchCardService)); }
        }
        #endregion

    }
    #endregion

    #region IFNBatchCard interface
    
    public interface IFNBatchCardService
    {
        FNBatchCard Get(int id, long nUserID);
        List<FNBatchCard> Gets(long nUserID);
        List<FNBatchCard> Gets(string sSQL, Int64 nUserID);
        string Delete(FNBatchCard oFNBatchCard, long nUserID);
        FNBatchCard Save(FNBatchCard oFNBatchCard, long nUserID);
        List<FNBatchCard> SaveFNBatchCards(FNBatchCard oFNBatchCard, long nUserID);
        
    }
    #endregion
}