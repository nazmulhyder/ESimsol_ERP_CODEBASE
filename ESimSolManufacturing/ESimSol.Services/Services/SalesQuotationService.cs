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
	public class SalesQuotationService : MarshalByRefObject, ISalesQuotationService
	{
		#region Private functions and declaration

		private SalesQuotation MapObject(NullHandler oReader)
		{
			SalesQuotation oSalesQuotation = new SalesQuotation();
			oSalesQuotation.SalesQuotationID = oReader.GetInt32("SalesQuotationID");
            oSalesQuotation.BuyerID = oReader.GetInt32("BuyerID");
            oSalesQuotation.FileNo = oReader.GetString("FileNo");
            oSalesQuotation.KommNo = oReader.GetString("KommNo");
            oSalesQuotation.ChassisNo = oReader.GetString("ChassisNo");
            oSalesQuotation.EngineNo = oReader.GetString("EngineNo");
            oSalesQuotation.ModelNo = oReader.GetString("ModelNo");
            oSalesQuotation.ModelCode = oReader.GetString("ModelCode");
            oSalesQuotation.MarketingPerson = oReader.GetInt32("MarketingPerson");
            oSalesQuotation.RefNo = oReader.GetString("RefNo");
            oSalesQuotation.UnitPrice = oReader.GetDouble("UnitPrice");
            oSalesQuotation.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oSalesQuotation.DiscountPercent = oReader.GetDouble("DiscountPercent");
            oSalesQuotation.ExteriorColorCode = oReader.GetString("ExteriorColorCode");
            oSalesQuotation.ExteriorColorName = oReader.GetString("ExteriorColorName");
            oSalesQuotation.Remarks = oReader.GetString("Remarks");
            oSalesQuotation.BUID = oReader.GetInt32("BUID");
            oSalesQuotation.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesQuotation.InteriorColorCode = oReader.GetString("InteriorColorCode");
            oSalesQuotation.InteriorColorCode = oReader.GetString("InteriorColorCode");
            oSalesQuotation.InteriorColorName = oReader.GetString("InteriorColorName");
            oSalesQuotation.QuotationDate = oReader.GetDateTime("QuotationDate");
            oSalesQuotation.FeatureSetupName = oReader.GetString("FeatureSetupName");
            oSalesQuotation.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oSalesQuotation.QuotationType = (EnumQuotationType)oReader.GetInt32("QuotationType");
            oSalesQuotation.QuotationTypeInInt = oReader.GetInt32("QuotationType");
            oSalesQuotation.KommFileID = oReader.GetInt32("KommFileID");
            oSalesQuotation.ExteriorColorID = oReader.GetInt32("ExteriorColorID");
            oSalesQuotation.InteriorColorID = oReader.GetInt32("InteriorColorID");
            oSalesQuotation.VehicleChassisID = oReader.GetInt32("VehicleChassisID");
            oSalesQuotation.VehicleEngineID = oReader.GetInt32("VehicleEngineID");
            oSalesQuotation.Upholstery = oReader.GetString("Upholstery");
            oSalesQuotation.UpholsteryCode = oReader.GetString("UpholsteryCode");
            oSalesQuotation.UpholsteryID = oReader.GetInt32("UpholsteryID");
            oSalesQuotation.Trim = oReader.GetString("Trim");
            oSalesQuotation.TrimCode = oReader.GetString("TrimCode");
            oSalesQuotation.TrimID = oReader.GetInt32("TrimID");
            oSalesQuotation.Wheels = oReader.GetString("Wheels");
            oSalesQuotation.WheelsCode = oReader.GetString("WheelsCode");
            oSalesQuotation.WheelsID = oReader.GetInt32("WheelsID");
            oSalesQuotation.OfferPrice = oReader.GetDouble("OfferPrice");
            oSalesQuotation.NewOfferPrice = oReader.GetDouble("NewOfferPrice");
            oSalesQuotation.DiscountPrice = oReader.GetDouble("DiscountPrice");
            oSalesQuotation.OptionTotal = oReader.GetDouble("OptionTotal");
            oSalesQuotation.UnitPrice = oReader.GetDouble("UnitPrice");
            oSalesQuotation.VatAmount = oReader.GetDouble("VatAmount");
            oSalesQuotation.TDSAmount = oReader.GetDouble("TDSAmount");
            oSalesQuotation.RegistrationFee = oReader.GetDouble("RegistrationFee");
            oSalesQuotation.OTRAmount = oReader.GetDouble("OTRAmount");
            oSalesQuotation.Warranty = oReader.GetString("Warranty");
            oSalesQuotation.Acceptance = oReader.GetString("Acceptance");
            oSalesQuotation.DeliveryDate = oReader.GetString("DeliveryDate");
            oSalesQuotation.AdvancePayment = oReader.GetDouble("AdvancePayment");
            oSalesQuotation.PaymentTerm = oReader.GetString("PaymentTerm");
            oSalesQuotation.ValidityOfOffer = oReader.GetString("ValidityOfOffer");
            oSalesQuotation.AfterSalesService = oReader.GetString("AfterSalesService");
            oSalesQuotation.OfferValidity = oReader.GetString("OfferValidity");
            oSalesQuotation.OrderSpecifications = oReader.GetString("OrderSpecifications");
            oSalesQuotation.VehicleInspection = oReader.GetString("VehicleInspection");
            oSalesQuotation.CancelOrChangeOrder = oReader.GetString("CancelOrChangeOrder");
            oSalesQuotation.PaymentMode = oReader.GetString("PaymentMode");
            oSalesQuotation.DeliveryDescription = oReader.GetString("DeliveryDescription");
            oSalesQuotation.PriceFluctuationClause = oReader.GetString("PriceFluctuationClause");
            oSalesQuotation.CustomsClearance = oReader.GetString("CustomsClearance");
            oSalesQuotation.Insurance = oReader.GetString("Insurance");
            oSalesQuotation.ForceMajeure = oReader.GetString("ForceMajeure");
            oSalesQuotation.FuelQuality = oReader.GetString("FuelQuality");
            oSalesQuotation.SpecialInstruction = oReader.GetString("SpecialInstruction");
            oSalesQuotation.WarrantyTerms = oReader.GetString("WarrantyTerms");
            oSalesQuotation.BuyerName = oReader.GetString("BuyerName");
            oSalesQuotation.BuyerShortName = oReader.GetString("BuyerShortName");
            oSalesQuotation.BuyerAddress = oReader.GetString("BuyerAddress");
            oSalesQuotation.CurrencyName = oReader.GetString("CurrencyName");
            oSalesQuotation.Symbol = oReader.GetString("Symbol");
            oSalesQuotation.MarketingAccountName = oReader.GetString("MarketingAccountName");
            oSalesQuotation.VehicleSpecification = oReader.GetString("VehicleSpecification");
            oSalesQuotation.UserID = oReader.GetInt32("UserID");
            oSalesQuotation.SalesQuotationImageID = oReader.GetInt32("SalesQuotationImageID");
            oSalesQuotation.VehicleOrderImageID = oReader.GetInt32("VehicleOrderImageID");
            oSalesQuotation.VehicleOrderID = oReader.GetInt32("VehicleOrderID");
            oSalesQuotation.SeatingCapacity = oReader.GetString("SeatingCapacity");
            oSalesQuotation.ModelSessionName = oReader.GetString("ModelSessionName");
            oSalesQuotation.ManufacturerName = oReader.GetString("ManufacturerName");
            oSalesQuotation.MaxPowerOutput = oReader.GetString("MaxPowerOutput");
            oSalesQuotation.MaximumTorque = oReader.GetString("MaximumTorque");
            oSalesQuotation.EngineType = oReader.GetString("EngineType");
            oSalesQuotation.CountryOfOrigin = oReader.GetString("CountryOfOrigin");
            oSalesQuotation.Transmission = oReader.GetString("Transmission");
            oSalesQuotation.YearOfManufacture = oReader.GetString("YearOfManufacture");
            oSalesQuotation.YearOfModel = oReader.GetString("YearOfModel");
            oSalesQuotation.SalesStatus = oReader.GetInt32("SalesStatus");
            oSalesQuotation.PartyWiseBankID = oReader.GetInt32("PartyWiseBankID");
            oSalesQuotation.Capacity = oReader.GetString("Capacity");
            oSalesQuotation.Complementary = oReader.GetString("Complementary");
            oSalesQuotation.SalesStatusRemarks = oReader.GetString("SalesStatusRemarks");
            oSalesQuotation.VATPercentage = oReader.GetDouble("VATPercentage");
            oSalesQuotation.ExShowroomPriceBC = oReader.GetDouble("ExShowroomPriceBC");
            
            oSalesQuotation.ModelShortName = oReader.GetString("ModelShortName");
            oSalesQuotation.DisplacementCC = oReader.GetString("DisplacementCC");
            oSalesQuotation.Acceleration = oReader.GetString("Acceleration");
            oSalesQuotation.TopSpeed = oReader.GetString("TopSpeed");
            oSalesQuotation.DriveType = (EnumDriveType)oReader.GetInt32("DriveType");

            oSalesQuotation.SalesQuotationLogID = oReader.GetInt32("SalesQuotationLogID");
            oSalesQuotation.VersionNo = oReader.GetInt32("VersionNo");
            oSalesQuotation.ApproveBy = oReader.GetInt32("ApproveBy");
            oSalesQuotation.ApproveByName = oReader.GetString("ApproveByName");
            oSalesQuotation.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oSalesQuotation.PrintOTRAmount = oReader.GetDouble("PrintOTRAmount");
            oSalesQuotation.ETAValue = oReader.GetInt32("ETAValue");
            oSalesQuotation.ETAType = (EnumDisplayPart)oReader.GetInt32("ETAType");
            oSalesQuotation.IssueDate = oReader.GetDateTime("IssueDate");

			return oSalesQuotation;
		}

		private SalesQuotation CreateObject(NullHandler oReader)
		{
			SalesQuotation oSalesQuotation = new SalesQuotation();
			oSalesQuotation = MapObject(oReader);
			return oSalesQuotation;
		}

		private List<SalesQuotation> CreateObjects(IDataReader oReader)
		{
			List<SalesQuotation> oSalesQuotation = new List<SalesQuotation>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				SalesQuotation oItem = CreateObject(oHandler);
				oSalesQuotation.Add(oItem);
			}
			return oSalesQuotation;
		}

		#endregion

		#region Interface implementation
        public SalesQuotation Save(SalesQuotation oSalesQuotation, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sSalesQuotationDetailIDs = "";
            List<SalesQuotationDetail> oSalesQuotationDetails = new List<SalesQuotationDetail>();
            SalesQuotationDetail oSalesQuotationDetail = new SalesQuotationDetail();
            oSalesQuotationDetails = oSalesQuotation.SalesQuotationDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSalesQuotation.SalesQuotationID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SalesQuotation, EnumRoleOperationType.Add);
                    reader = SalesQuotationDA.InsertUpdate(tc, oSalesQuotation, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SalesQuotation, EnumRoleOperationType.Edit);
                    reader = SalesQuotationDA.InsertUpdate(tc, oSalesQuotation, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation = CreateObject(oReader);
                }
                reader.Close();
                #region SalesQuotationDetail Part
                foreach (SalesQuotationDetail oItem in oSalesQuotationDetails)
                {
                    IDataReader readerdetail;
                    oItem.SalesQuotationID = oSalesQuotation.SalesQuotationID;
                    if (oItem.SalesQuotationDetailID <= 0)
                    {
                        readerdetail = SalesQuotationDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = SalesQuotationDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sSalesQuotationDetailIDs = sSalesQuotationDetailIDs + oReaderDetail.GetString("SalesQuotationDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sSalesQuotationDetailIDs.Length > 0)
                {
                    sSalesQuotationDetailIDs = sSalesQuotationDetailIDs.Remove(sSalesQuotationDetailIDs.Length - 1, 1);
                }
                oSalesQuotationDetail = new SalesQuotationDetail();
                if (oSalesQuotation.SalesQuotationID > 0)
                {
                    oSalesQuotationDetail.SalesQuotationID = oSalesQuotation.SalesQuotationID;
                    SalesQuotationDetailDA.Delete(tc, oSalesQuotationDetail, EnumDBOperation.Delete, nUserID, sSalesQuotationDetailIDs);
                }
                #endregion

                #region Get SalesQuotation
                reader = SalesQuotationDA.Get(tc, oSalesQuotation.SalesQuotationID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation = CreateObject(oReader);
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
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSalesQuotation;
        }

        public SalesQuotation UpdateStatus(SalesQuotation oSalesQuotation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                
                reader = SalesQuotationDA.UpdateStatus(tc, oSalesQuotation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update Sales Status. Because of " + e.Message, e);
                #endregion
            }
            return oSalesQuotation;
        }
        public SalesQuotation Approve(SalesQuotation oSalesQuotation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SalesQuotation, EnumRoleOperationType.Approved);
                reader = SalesQuotationDA.InsertUpdate(tc, oSalesQuotation, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation = CreateObject(oReader);
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
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSalesQuotation;
        }
        public SalesQuotation UndoApprove(SalesQuotation oSalesQuotation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SalesQuotation, EnumRoleOperationType.Approved);
                reader = SalesQuotationDA.InsertUpdate(tc, oSalesQuotation, EnumDBOperation.UnApproval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation = CreateObject(oReader);
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
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSalesQuotation;
        }
        public SalesQuotation Revise(SalesQuotation oSalesQuotation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SalesQuotation, EnumRoleOperationType.Approved);
                reader = SalesQuotationDA.InsertUpdate(tc, oSalesQuotation, EnumDBOperation.Revise, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation = CreateObject(oReader);
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
                    oSalesQuotation = new SalesQuotation();
                    oSalesQuotation.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSalesQuotation;
        }
		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				SalesQuotation oSalesQuotation = new SalesQuotation();
				oSalesQuotation.SalesQuotationID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SalesQuotation, EnumRoleOperationType.Delete);
				DBTableReferenceDA.HasReference(tc, "SalesQuotation", id);
				SalesQuotationDA.Delete(tc, oSalesQuotation, EnumDBOperation.Delete, nUserId);
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
        public string UpdateBQ(SalesQuotation oSalesQuotation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SalesQuotation, EnumRoleOperationType.Edit);
                DBTableReferenceDA.HasReference(tc, "SalesQuotation", oSalesQuotation.SalesQuotationID);
                SalesQuotationDA.UpdateBQ(tc, oSalesQuotation);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Updated";
        }
        public SalesQuotation Get(int id, Int64 nUserId)
        {
            SalesQuotation oSalesQuotation = new SalesQuotation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SalesQuotationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalesQuotation", e);
                #endregion
            }
            return oSalesQuotation;
        }
        public SalesQuotation GetLog(int id, Int64 nUserId)
        {
            SalesQuotation oSalesQuotation = new SalesQuotation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SalesQuotationDA.GetLog(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalesQuotation", e);
                #endregion
            }
            return oSalesQuotation;
        }

        public List<SalesQuotation> BUWiseGets(int buid, Int64 nUserID)
		{
			List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
                reader = SalesQuotationDA.BUWiseGets(buid, tc);
				oSalesQuotations = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				SalesQuotation oSalesQuotation = new SalesQuotation();
				oSalesQuotation.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oSalesQuotations;
		}

		public List<SalesQuotation> Gets (string sSQL, Int64 nUserID)
		{
			List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = SalesQuotationDA.Gets(tc, sSQL);
				oSalesQuotations = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get SalesQuotation", e);
				#endregion
			}
			return oSalesQuotations;
		}

		#endregion
	}

}
