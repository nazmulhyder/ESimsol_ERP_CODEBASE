using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;

namespace ESimSol.BusinessObjects
{
    #region SalarySchemeDetail

    public class SalarySchemeDetail : BusinessObject
    {
        public SalarySchemeDetail()
        {
            SalarySchemeDetailID = 0;
            SalarySchemeID = 0;
            SalaryHeadID = 0;            
            Condition = EnumAllowanceCondition.None;
            Period = EnumPeriod.None;
            Times = 0;
            DeferredDay = 0;
            ActivationAfter = EnumRecruitmentEvent.None;
            ErrorMessage = "";
            SalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
            IsCalculated = false;
            SalaryType = EnumSalaryType.Both;
        }

        #region Properties


        public int SalarySchemeDetailID { get; set; }

        public int SalarySchemeID { get; set; }

        public int SalaryHeadID { get; set; }
        public EnumAllowanceCondition Condition { get; set; }

        public EnumPeriod Period { get; set; }

        public int Times { get; set; }

        public int DeferredDay { get; set; }

        public EnumRecruitmentEvent ActivationAfter { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public List<SalarySchemeDetailCalculation> SalarySchemeDetailCalculations { get; set; }
        public string SalaryHeadName { get; set; }

        public EnumSalaryHeadType SalaryHeadType { get; set; }
        public int SalaryHeadTypeInt { get; set; }
        public EnumSalaryType SalaryType { get; set; }
        public int SalaryTypeInt { get; set; }

        public string SalaryTypeInString
        {
            get
            {
                return SalaryType.ToString();
            }
        }

        public string Calculation { get; set; }

        public int Amount { get; set; }
        public int CompAmount { get; set; }
        public int ConditionInt { get; set; }
        public string ConditionInString
        {
            get
            {
                return Condition.ToString();
            }
        }

        public string SalaryHeadTypeInString
        {
            get
            {
                return SalaryHeadType.ToString();
            }
        }


        public int PeriodInt { get; set; }
        public string PeriodInString
        {
            get
            {
                return Times + " Times" + " / " + Period;
            }
        }


        public int ActivationAfterInt { get; set; }
        public string ActivationAfterInString
        {
            get
            {
                return DeferredDay + " Days off " + ActivationAfter;
            }
        }

        public double MinAmount { get; set; } // Used in Salary Scheme Grade for Min Value Calculate
        public bool IsCalculated { get; set; } // Used in Salary Scheme Grade for Min Value Calculate
        public static List<SalarySchemeDetail> GetNewSalarySchemeDetail(List<SalarySchemeDetail> oDetails, List<SalarySchemeDetailCalculation> oDetailCalculations)
        {
            oDetails.OrderBy(a => a.SalarySchemeDetailID);
            oDetailCalculations.OrderBy(a => a.SalarySchemeDetailID);
            foreach (SalarySchemeDetail oDetailItem in oDetails)
            {
                List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = GetsSalarySchemeDetailCalculation(oDetailCalculations, oDetailItem.SalarySchemeDetailID);
                oDetailItem.Calculation = GetEquation(oSalarySchemeDetailCalculations);
            }
            return oDetails;
        }        

        private static List<SalarySchemeDetailCalculation> GetsSalarySchemeDetailCalculation(List<SalarySchemeDetailCalculation>oTempSchemeDetailCalculations, int nSalarySchemeDetailID)
        {
            List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
            foreach (SalarySchemeDetailCalculation oItem in oTempSchemeDetailCalculations)
            {
                if (oItem.SalarySchemeDetailID == nSalarySchemeDetailID)
                {
                    oSalarySchemeDetailCalculations.Add(oItem);
                }
            }
            return oSalarySchemeDetailCalculations;
        }

        public static string GetEquation(List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations)
        {
            string sDetailCalculation = "";
            foreach (SalarySchemeDetailCalculation oDetailCalculationItem in oSalarySchemeDetailCalculations)
            {
                if (oDetailCalculationItem.ValueOperator == EnumValueOperator.Value)
                {
                    if (oDetailCalculationItem.CalculationOn == EnumSalaryCalculationOn.Gross)
                    {
                        sDetailCalculation += "Gross";
                    }
                    else if (oDetailCalculationItem.CalculationOn == EnumSalaryCalculationOn.SalaryItem)
                    {
                        sDetailCalculation += oDetailCalculationItem.SalaryHeadName;

                    }
                    else if (oDetailCalculationItem.CalculationOn == EnumSalaryCalculationOn.Fixed)
                    {
                        sDetailCalculation += oDetailCalculationItem.FixedValue;

                    }
                }
                else if (oDetailCalculationItem.ValueOperator == EnumValueOperator.Operator)
                {

                    if (oDetailCalculationItem.Operator == EnumOperator.BracketStart)
                    {
                        sDetailCalculation += "(";
                    }
                    else if (oDetailCalculationItem.Operator == EnumOperator.BracketEnd)
                    {
                        sDetailCalculation += ")";
                    }
                    else if (oDetailCalculationItem.Operator == EnumOperator.Addition)
                    {
                        sDetailCalculation += "+";
                    }
                    else if (oDetailCalculationItem.Operator == EnumOperator.Subtruction)
                    {
                        sDetailCalculation += "-";
                    }
                    else if (oDetailCalculationItem.Operator == EnumOperator.Multiplication)
                    {
                        sDetailCalculation += "*";
                    }
                    else if (oDetailCalculationItem.Operator == EnumOperator.Division)
                    {
                        sDetailCalculation += "/";
                    }
                    else if (oDetailCalculationItem.Operator == EnumOperator.Percent)
                    {
                        sDetailCalculation += oDetailCalculationItem.PercentVelue + " % of ";
                    }
                }
            }
            return sDetailCalculation;
        }


        #endregion

        #region Functions
        public static SalarySchemeDetail Get(int id, long nUserID)
        {
            return SalarySchemeDetail.Service.Get(id, nUserID);
        }

        public static List<SalarySchemeDetail> Gets(int nSID, long nUserID)
        {
            return SalarySchemeDetail.Service.Gets(nSID, nUserID);
        }

        public static List<SalarySchemeDetail> Gets(string sSQL, long nUserID)
        {
            return SalarySchemeDetail.Service.Gets(sSQL, nUserID);
        }
        public SalarySchemeDetail IUD(long nUserID)
        {
            return SalarySchemeDetail.Service.IUD(this, nUserID);
        }
        public SalarySchemeDetail IUDGross(long nUserID)
        {
            return SalarySchemeDetail.Service.IUDGross(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return SalarySchemeDetail.Service.Delete(this, nUserID);
        }


        #endregion


        #region ServiceFactory
        internal static ISalarySchemeDetailService Service
        {
            get { return (ISalarySchemeDetailService)Services.Factory.CreateService(typeof(ISalarySchemeDetailService)); }
        }

        #endregion
    }
    #endregion

    #region ISalarySchemeDetail interface

    public interface ISalarySchemeDetailService
    {

        SalarySchemeDetail Get(int id, Int64 nUserID);


        List<SalarySchemeDetail> Gets(int nSID, Int64 nUserID);//nSID=SalarySchemeID


        List<SalarySchemeDetail> Gets(string sSQL, Int64 nUserID);

        SalarySchemeDetail IUD(SalarySchemeDetail oSalarySchemeDetail, Int64 nUserID);
        SalarySchemeDetail IUDGross(SalarySchemeDetail oSalarySchemeDetail, Int64 nUserID);

        string Delete(SalarySchemeDetail oSalarySchemeDetail, Int64 nUserID);


    }
    #endregion
}
