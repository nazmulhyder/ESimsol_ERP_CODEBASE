using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class CostCalculationService : MarshalByRefObject, ICostCalculationService
    {
        #region Private functions and declaration

        private CostCalculation MapObject(NullHandler oReader)
        {
            CostCalculation oCostCalculation = new CostCalculation();
            oCostCalculation.CostCalculationID = oReader.GetInt32("CostCalculationID");
            oCostCalculation.FileNo = oReader.GetString("FileNo");
            oCostCalculation.DateOfIssue = oReader.GetDateTime("DateOfIssue");
            oCostCalculation.DateOfExpire = oReader.GetDateTime("DateOfExpire");
            oCostCalculation.VehicleModelID = oReader.GetInt16("VehicleModelID");
            oCostCalculation.ModelNo = oReader.GetString("ModelNo");
            oCostCalculation.CurrencyID = oReader.GetInt16("CurrencyID");
            oCostCalculation.BasePrice = oReader.GetDouble("BasePrice");
            oCostCalculation.CRate = oReader.GetDouble("CRate");
            oCostCalculation.BasePriceBC = oReader.GetDouble("BasePriceBC");
            oCostCalculation.DutyPercent = oReader.GetDouble("DutyPercent");
            oCostCalculation.DutyAmount = oReader.GetDouble("DutyAmount");
            oCostCalculation.CustomAndInsurenceFeePercent = oReader.GetDouble("CustomAndInsurenceFeePercent");
            oCostCalculation.CustomAndInsurenceFeeAmount = oReader.GetDouble("CustomAndInsurenceFeeAmount");
            oCostCalculation.CustomAndInsurenceFeeAmount = oReader.GetDouble("CustomAndInsurenceFeeAmount");
            oCostCalculation.TransportCost = oReader.GetDouble("TransportCost");
            oCostCalculation.CurrencyID = oReader.GetInt32("CurrencyID");
            oCostCalculation.LandedCost = oReader.GetDouble("LandedCost");
            oCostCalculation.LandedCostBC = oReader.GetDouble("LandedCostBC");
            oCostCalculation.MarginRate = oReader.GetDouble("MarginRate");
            oCostCalculation.AdditionalCost = oReader.GetDouble("AdditionalCost");
            oCostCalculation.AdditionalCostBC = oReader.GetDouble("AdditionalCostBC");
            oCostCalculation.TotalLandedCost = oReader.GetDouble("TotalLandedCost");
            oCostCalculation.TotalLandedCostBC = oReader.GetDouble("TotalLandedCostBC");
            oCostCalculation.ExShowroomPrice = oReader.GetDouble("ExShowroomPrice");
            oCostCalculation.ExShowroomPriceBC = oReader.GetDouble("ExShowroomPriceBC");
            oCostCalculation.OfferPriceBC = oReader.GetDouble("OfferPriceBC");
            oCostCalculation.ExShowroomPrice = oReader.GetDouble("ExShowroomPrice");
            oCostCalculation.MarginAmount = oReader.GetDouble("MarginAmount");
            oCostCalculation.MarginAmountBC = oReader.GetDouble("MarginAmountBC");
            oCostCalculation.Remarks = oReader.GetString("Remarks");
            oCostCalculation.CDPercent = oReader.GetDouble("CDPercent");
            oCostCalculation.CDAmount = oReader.GetDouble("CDAmount");
            oCostCalculation.RDPercent = oReader.GetDouble("RDPercent");
            oCostCalculation.RDAmount = oReader.GetDouble("RDAmount");
            oCostCalculation.SDPercent = oReader.GetDouble("SDPercent");
            oCostCalculation.SDAmount = oReader.GetDouble("SDAmount");
            oCostCalculation.VATPercent = oReader.GetDouble("VATPercent");
            oCostCalculation.VATAmount = oReader.GetDouble("VATAmount");
            oCostCalculation.AITPercent = oReader.GetDouble("AITPercent");
            oCostCalculation.AITAmount = oReader.GetDouble("AITAmount");
            oCostCalculation.ProfitForATVPercent = oReader.GetDouble("ProfitForATVPercent");
            oCostCalculation.ProfitForATVAmount = oReader.GetDouble("ProfitForATVAmount");
            oCostCalculation.TotalValueForATV = oReader.GetDouble("TotalValueForATV");
            oCostCalculation.ATVAmount = oReader.GetDouble("ATVAmount");
            oCostCalculation.ATVPercent = oReader.GetDouble("ATVPercent");
            oCostCalculation.TotalDutyAmount = oReader.GetDouble("TotalDutyAmount");
            oCostCalculation.TotalDutyPercent = oReader.GetDouble("TotalDutyPercent");
            oCostCalculation.ApprovedBy = oReader.GetInt16("ApprovedBy");
            oCostCalculation.CategoryName = oReader.GetString("CategoryName"); 
            oCostCalculation.ApprovedByName = oReader.GetString("ApprovedByName");

            return oCostCalculation;
        }

        private CostCalculation CreateObject(NullHandler oReader)
        {
            CostCalculation oCostCalculation = new CostCalculation();
            oCostCalculation = MapObject(oReader);
            return oCostCalculation;
        }

        private List<CostCalculation> CreateObjects(IDataReader oReader)
        {
            List<CostCalculation> oCostCalculation = new List<CostCalculation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostCalculation oItem = CreateObject(oHandler);
                oCostCalculation.Add(oItem);
            }
            return oCostCalculation;
        }

        #endregion

        #region Interface implementation
        public CostCalculation Save(CostCalculation oCostCalculation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCostCalculation.CostCalculationID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CostCalculation, EnumRoleOperationType.Add);
                    reader = CostCalculationDA.InsertUpdate(tc, oCostCalculation, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CostCalculation, EnumRoleOperationType.Edit);
                    reader = CostCalculationDA.InsertUpdate(tc, oCostCalculation, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostCalculation = new CostCalculation();
                    oCostCalculation = CreateObject(oReader);
                }
                reader.Close();

                #region Get Recycle Process
                reader = CostCalculationDA.Get(tc, oCostCalculation.CostCalculationID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostCalculation = new CostCalculation();
                    oCostCalculation = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oCostCalculation = new CostCalculation();
                    oCostCalculation.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oCostCalculation;
        }

        public CostCalculation Approve(CostCalculation oCostCalculation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CostCalculation, EnumRoleOperationType.Approved);
                reader = CostCalculationDA.InsertUpdate(tc, oCostCalculation, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostCalculation = new CostCalculation();
                    oCostCalculation = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oCostCalculation = new CostCalculation();
                    oCostCalculation.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oCostCalculation;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CostCalculation oCostCalculation = new CostCalculation();
                oCostCalculation.CostCalculationID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CostCalculation, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "CostCalculation", id);
                CostCalculationDA.Delete(tc, oCostCalculation, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public CostCalculation Get(int id, Int64 nUserId)
        {
            CostCalculation oCostCalculation = new CostCalculation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = CostCalculationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostCalculation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCalculation", e);
                #endregion
            }
            return oCostCalculation;
        }

   
        public List<CostCalculation> Gets(string sSQL, Int64 nUserID)
        {
            List<CostCalculation> oCostCalculations = new List<CostCalculation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = CostCalculationDA.Gets(tc, sSQL);
                oCostCalculations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCalculation", e);
                #endregion
            }
            return oCostCalculations;
        }

        #endregion
    }

}
