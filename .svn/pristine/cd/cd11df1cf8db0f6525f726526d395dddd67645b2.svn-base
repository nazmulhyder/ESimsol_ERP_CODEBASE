using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region EmployeeProductionSheet
    [DataContract]
    public class EmployeeProduction : BusinessObject
    {
        public EmployeeProduction()
        {
            EPSID = 0;
            EPSNO = "";
            EmployeeID = 0;
            OrderRecapDetailID = 0;
            ProductionProcess = EnumProductionProcess.None;
            //GarmentPart = EnumGarmentPart.None;
            GPID = 0;
            MachineNo = "";
            TSPID = 0;
            IssueQty = 0;
            IssueBy = 0;
            IssueDate = DateTime.Now;
            RcvQty = 0;
            YarnRcvQty = 0;
            ApproveBy = 0;
            ApproveByDate = DateTime.Now;
            ApproveByName = "";
            EPSLotNo = "";
            IsActive = true;
            ReferenceEPSID = 0;
            SLNO = "";
            DepartmentID = 0;
            ErrorMessage = "";

            //Derive
            OrderRecapDetails = new List<OrderRecapDetail>();
            TechnicalSheetProductions = new List<TechnicalSheetProduction>();
            EmployeeProductions = new List<EmployeeProduction>();
            EmployeeProductionReceiveDetails = new List<EmployeeProductionReceiveDetail>();
            EmployeeOfficial = new EmployeeOfficial();
            Company = new Company();
            EmployeeName = "";
            Code = "";
            EmpOfficial = "";
            ColorName = "";
            SizeCategoryName = "";
            StyleNo = "";
            LotNo = "";
            ReferenceLotNo = "";
            ReferenceMachineNo = "";
            ReferenceEPSNo = "";
            ApproveByName = "";
            OrderRecapNo = "";
            OrderRecapID = 0;
            GPName = "";
        }

        #region Properties
        public int EPSID { get; set; }
        public string EPSNO { get; set; }
        public int EmployeeID { get; set; }
        public int OrderRecapDetailID { get; set; }
        public EnumProductionProcess ProductionProcess { get; set; }
        //
        //public EnumGarmentPart GarmentPart { get; set; }
        
        public int GPID { get; set; }
        public string MachineNo { get; set; }
        public int TSPID { get; set; }
        public double IssueQty { get; set; }
        public int IssueBy { get; set; }
        public DateTime IssueDate { get; set; }
        public double RcvQty { get; set; }
        public double YarnRcvQty { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public string EPSLotNo { get; set; }
        public bool IsActive { get; set; }
        public int ReferenceEPSID { get; set; }
        public string SLNO { get; set; }
        public int DepartmentID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public List<OrderRecapDetail> OrderRecapDetails { get; set; }
        
        public List<TechnicalSheetProduction> TechnicalSheetProductions { get; set; }

        public List<TechnicalSheetBodyPart> TechnicalSheetBodyParts { get; set; }
        
        public List<SizeCategory> SizeCategorys { get; set; }

        public List<EmployeeProduction> EmployeeProductions { get; set; }
        
        public List<EmployeeProductionReceiveDetail> EmployeeProductionReceiveDetails { get; set; }
        
        public EmployeeOfficial EmployeeOfficial { get; set; }

        public Company Company { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeNameCode
        {
            get
            {
                return EmployeeName + "[" + Code + "]";
            }
        }

        
        public string Code { get; set; }
        
        public string EmpOfficial { get; set; }

        
        public string ColorName { get; set; }
        
        public string SizeCategoryName { get; set; }
        
        public string StyleNo { get; set; }
        
        public string LotNo { get; set; }
        
        public string ReferenceLotNo { get; set; }
        
        public string ReferenceMachineNo { get; set; }
        
        public string ReferenceEPSNo { get; set; }


        public int ProductionProcessInt { get; set; }
        public string ProductionProcessInString
        {
            get
            {
                return ProductionProcess.ToString();
            }
        }

        //public int GarmentPartInt { get; set; }
        //public string GarmentPartInString
        //{
        //    get
        //    {
        //        return GarmentPart.ToString();
        //    }
        //}
        
        public string GPName { get; set; }
        public string IssueDateInString
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }

        public double EmpWiseBalanceQty
        {
            get
            {
                return (IssueQty - RcvQty);
            }
        }

        
        public string ApproveByName { get; set; }

        
        public string OrderRecapNo { get; set; }
        
        public int OrderRecapID { get; set; }


        
        public DateTime RcvByDate { get; set; }

        public string RcvByDateInString
        {
            get
            {
                if (RcvQty > 0)

                    return RcvByDate.ToString("dd MMM yyyy");

                else

                    return "--";

            }
        }

        public string NewLotNo
        {
            get
            {
                if (EPSLotNo == "")

                    return LotNo;

                else

                    return EPSLotNo;

            }
        }

        public string Color_Size_BodyPart
        {
            get
            {
                return ColorName + "[" + SizeCategoryName + "] - " + GPName;
            }
        }

        public string ActivityStatus { get { if (IsActive)return "Active"; else return "Inactive"; } }

        
        public List<GarmentPart> GarmentParts { get; set; }

        #endregion

        #region Functions
        public static EmployeeProduction Get(int id, long nUserID)
        {
            return EmployeeProduction.Service.Get(id, nUserID);
        }

        public static EmployeeProduction Get(string sSQL, long nUserID)
        {
            return EmployeeProduction.Service.Get(sSQL, nUserID);
        }

        public static List<EmployeeProduction> Gets(long nUserID)
        {
            return EmployeeProduction.Service.Gets(nUserID);
        }

        public static List<EmployeeProduction> Gets(string sSQL, long nUserID)
        {
            return EmployeeProduction.Service.Gets(sSQL, nUserID);
        }

        public EmployeeProduction IUD(int nDBOperation, long nUserID)
        {
            return EmployeeProduction.Service.IUD(this, nDBOperation, nUserID);
        }

        public EmployeeProduction TransferEmployeeProduction(long nUserID)
        {
            return EmployeeProduction.Service.TransferEmployeeProduction(this, nUserID);
        }

        public static EmployeeProduction Activity(int nId, bool bActive, long nUserID)
        {
            return EmployeeProduction.Service.Activity(nId, bActive, nUserID);
        }

        public EmployeeProduction AdvanceEdit(long nUserID)
        {
            return EmployeeProduction.Service.AdvanceEdit(this, nUserID);
        }

        public string GetBalance(string Ssql, long nUserID)
        {
            return EmployeeProduction.Service.GetBalance(Ssql, nUserID);
        }



        #endregion

        #region ServiceFactory
        internal static IEmployeeProductionService Service
        {
            get { return (IEmployeeProductionService)Services.Factory.CreateService(typeof(IEmployeeProductionService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeProduction interface
    
    public interface IEmployeeProductionService
    {
        EmployeeProduction Get(int id, Int64 nUserID);

        EmployeeProduction Get(string sSQL, Int64 nUserID);

        List<EmployeeProduction> Gets(Int64 nUserID);
        
        List<EmployeeProduction> Gets(string sSQL, Int64 nUserID);

        EmployeeProduction IUD(EmployeeProduction oEmployeeProductionSheet, int nDBOperation, Int64 nUserID);
        
        EmployeeProduction TransferEmployeeProduction(EmployeeProduction oEmployeeProductionSheet, Int64 nUserID);
        
        EmployeeProduction Activity(int nId, bool bActive, Int64 nUserID);

        EmployeeProduction AdvanceEdit(EmployeeProduction oEmployeeProduction, Int64 nUserID);

        string GetBalance(string sSql, Int64 nUserID);

    }
    #endregion
}
