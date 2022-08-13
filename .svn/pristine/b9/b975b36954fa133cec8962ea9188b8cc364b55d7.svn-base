using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region CostCalculation
    public class CostCalculation : BusinessObject
    {
        public CostCalculation()
        {
            CostCalculationID = 0;
            FileNo = "";
            DateOfIssue = DateTime.Now;
            DateOfExpire = DateTime.Now;
            VehicleModelID = 0;
            CurrencyID = 0;
            BasePrice = 0.00;
            CRate = 0.00;
            BasePriceBC = 0.00;
            DutyPercent = 0.00;
            DutyAmount = 0.00;
            CustomAndInsurenceFeePercent = 0.00;
            CustomAndInsurenceFeeAmount = 0.00;
            TransportCost = 0.00;
            LandedCost = 0.00;
            LandedCostBC = 0.00;
            MarginRate = 0.00;
            AdditionalCost = 0;
            AdditionalCostBC = 0;
            TotalLandedCost = 0;
            TotalLandedCostBC = 0;
            ExShowroomPrice = 0.00;
            ExShowroomPriceBC = 0.00;
            MarginAmount = 0.00;
            MarginAmountBC = 0.00;
            OfferPriceBC = 0.00;
            Remarks = "";
            CDPercent = 0.00;
            CDAmount = 0.00;
            RDPercent = 0.00;
            RDAmount = 0.00;
            SDPercent = 0.00;
            SDAmount = 0.00;
            VATPercent = 0.00;
            VATAmount = 0.00;
            AITPercent = 0.00;
            AITAmount = 0.00;
            ProfitForATVPercent = 0.00;
            ProfitForATVAmount = 0.00;
            TotalValueForATV = 0.00;
            ATVPercent = 0.00;
            ATVAmount = 0.00;
            TotalDutyAmount = 0.00;
            TotalDutyPercent = 0.00;
            ApprovedBy = 0;
            ApprovedByName = "-";
            ModelNo = "";
            CategoryName = "";
            CostCalculationList = new List<CostCalculation>();
        }

        #region Property
        public int CostCalculationID { get; set; }
        public string FileNo { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DateOfExpire { get; set; }
        public int VehicleModelID { get; set; }
        public int CurrencyID { get; set; }
        public double BasePrice { get; set; }
        public double CRate { get; set; }
        public double BasePriceBC { get; set; }
        public double DutyPercent { get; set; }
        public double DutyAmount { get; set; }
        public double CustomAndInsurenceFeePercent { get; set; }
        public double CustomAndInsurenceFeeAmount { get; set; }
        public double TransportCost { get; set; }
        public double LandedCost { get; set; }
        public double LandedCostBC { get; set; }
        public double MarginRate { get; set; }
        public double AdditionalCost { get; set; }
        public double AdditionalCostBC { get; set; }
        public double TotalLandedCost { get; set; }
        public double TotalLandedCostBC { get; set; }
        public double ExShowroomPrice { get; set; }
        public double ExShowroomPriceBC { get; set; }
        public double MarginAmount { get; set; }
        public double MarginAmountBC { get; set; }
        public double OfferPriceBC { get; set; }
        public string Remarks { get; set; }
        public double CDPercent { get; set; }
        public double CDAmount { get; set; }
        public double RDPercent { get; set; }
        public double RDAmount { get; set; }
        public double DPercent { get; set; }
        public double SDPercent { get; set; }
        public double SDAmount { get; set; }
        public double VATPercent { get; set; }
        public double VATAmount { get; set; }
        public double AITPercent { get; set; }
        public double AITAmount { get; set; }
        public double ProfitForATVPercent { get; set; }
        public double ProfitForATVAmount { get; set; }
        public double TotalValueForATV { get; set; }
        public double ATVPercent { get; set; }
        public double ATVAmount { get; set; }
        public double TotalDutyAmount { get; set; }
        public double TotalDutyPercent { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string ModelNo { get; set; }
        public string CategoryName { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string DateOfIssueInString
        {
            get
            {
                return this.DateOfIssue.ToString("dd MMM yyyy");
            }
        }

        public string DateOfExpireInString
        {
            get
            {
                return this.DateOfExpire.ToString("dd MMM yyyy");
            }
        }
        public List<CostCalculation> CostCalculationList { get; set; }
        #endregion

        #region Functions
        public List<CostCalculation> Gets(string sSQL, long nUserID)
        {
            return CostCalculation.Service.Gets(sSQL, nUserID);
        }
        public CostCalculation Get(int id, long nUserID)
        {
            return CostCalculation.Service.Get(id, nUserID);
        }
        public CostCalculation Save(long nUserID)
        {
            return CostCalculation.Service.Save(this, nUserID);
        }

        public CostCalculation Approve(long nUserID)
        {
            return CostCalculation.Service.Approve(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return CostCalculation.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICostCalculationService Service
        {
            get { return (ICostCalculationService)Services.Factory.CreateService(typeof(ICostCalculationService)); }
        }
        #endregion
    }
    #endregion

    #region ICostCalculation interface
    public interface ICostCalculationService
    {
        CostCalculation Get(int id, Int64 nUserID);
        List<CostCalculation> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        CostCalculation Save(CostCalculation oCostCalculation, Int64 nUserID);
        CostCalculation Approve(CostCalculation oCostCalculation, Int64 nUserID);

    }
    #endregion
}
