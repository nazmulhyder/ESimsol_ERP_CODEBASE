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
    #region SalarySchemeDetailCalculation

    public class SalarySchemeDetailCalculation : BusinessObject
    {
        public SalarySchemeDetailCalculation()
        {
            SSDCID = 0;
            SalarySchemeDetailID = 0;
            ValueOperator = EnumValueOperator.None;
            CalculationOn = EnumSalaryCalculationOn.None;
            FixedValue = 0;
            Operator = EnumOperator.None;
            SalaryHeadID = 0;
            PercentVelue = 0.0;
            ErrorMessage = "";

            //FOR GROSS CALCULATION
            GSCID = 0;
            SalarySchemeID = 0;
        }

        #region Properties

        public int GSCID { get; set; }
        public int SalarySchemeID { get; set; }
        public int SSDCID { get; set; }
        public int SalarySchemeDetailID { get; set; }
        public EnumValueOperator ValueOperator { get; set; }
        public EnumSalaryCalculationOn CalculationOn { get; set; }
        public double FixedValue { get; set; }
        public EnumOperator Operator { get; set; }
        public int SalaryHeadID { get; set; }
        public double PercentVelue { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Derived Property
        public string SalaryHeadName { get; set; }
        public string Calculation { get; set; }
        public int ValueOperatorInt { get; set; }
        public string ValueOperatorInString
        {
            get
            {
                return ValueOperator.ToString();
            }
        }

        public int CalculationOnInt { get; set; }
        public string CalculationOnInString
        {
            get
            {
                return CalculationOn.ToString();
            }
        }

        public int OperatorInt { get; set; }
        public string OperatorInString
        {
            get
            {
                if (this.Operator == EnumOperator.BracketStart) { return "("; }
                else if (this.Operator == EnumOperator.BracketEnd) return ")";
                else if (this.Operator == EnumOperator.Addition) return "+";
                else if (this.Operator == EnumOperator.Subtruction) return "-";
                else if (this.Operator == EnumOperator.Multiplication) return "*";
                else if (this.Operator == EnumOperator.Division) return "/";
                else if (this.Operator == EnumOperator.Percent) return "%";
                else return "";

            }
        }

        #endregion

        #region Functions
        public static SalarySchemeDetailCalculation Get(int id, long nUserID)
        {
            return SalarySchemeDetailCalculation.Service.Get(id, nUserID);
        }
        public static List<SalarySchemeDetailCalculation> Gets(long nUserID)
        {
            return SalarySchemeDetailCalculation.Service.Gets(nUserID);
        }
        public static List<SalarySchemeDetailCalculation> Gets(string sSQL, long nUserID)
        {
            return SalarySchemeDetailCalculation.Service.Gets(sSQL, nUserID);
        }
        public static List<SalarySchemeDetailCalculation> GetsBySalarySchemeDetail(int nSalarySchemeDetailID, long nUserID)
        {
            return SalarySchemeDetailCalculation.Service.GetsBySalarySchemeDetail(nSalarySchemeDetailID, nUserID);
        }
        public static List<SalarySchemeDetailCalculation> GetsBySalarySchemeGross(int nSalarySchemeDetailID, long nUserID)
        {
            return SalarySchemeDetailCalculation.Service.GetsBySalarySchemeGross(nSalarySchemeDetailID, nUserID);
        }
        public SalarySchemeDetailCalculation IUD(int nDBOperation, long nUserID)
        {
            return SalarySchemeDetailCalculation.Service.IUD(this, nDBOperation, nUserID);
        }

        public SalarySchemeDetailCalculation IUDGross(int nDBOperation, long nUserID)
        {
            return SalarySchemeDetailCalculation.Service.IUDGross(this, nDBOperation, nUserID);
        }



        #endregion

        #region ServiceFactory
        internal static ISalarySchemeDetailCalculationService Service
        {
            get { return (ISalarySchemeDetailCalculationService)Services.Factory.CreateService(typeof(ISalarySchemeDetailCalculationService)); }
        }

        #endregion
    }
    #endregion

    #region ISalarySchemeDetailCalculation interface

    public interface ISalarySchemeDetailCalculationService
    {
        SalarySchemeDetailCalculation Get(int id, Int64 nUserID);
        List<SalarySchemeDetailCalculation> Gets(Int64 nUserID);
        List<SalarySchemeDetailCalculation> Gets(string sSQL, Int64 nUserID);
        List<SalarySchemeDetailCalculation> GetsBySalarySchemeDetail(int nSalarySchemeDetailID, Int64 nUserID);
        List<SalarySchemeDetailCalculation> GetsBySalarySchemeGross(int nSalarySchemeDetailID, Int64 nUserID);
        SalarySchemeDetailCalculation IUD(SalarySchemeDetailCalculation oSalarySchemeDetailCalculation, int nDBOperation, Int64 nUserID);
        SalarySchemeDetailCalculation IUDGross(SalarySchemeDetailCalculation oSalarySchemeDetailCalculation, int nDBOperation, Int64 nUserID);


    }
    #endregion
}
