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
	public class KommFileService : MarshalByRefObject, IKommFileService
	{
		#region Private functions and declaration

		private KommFile MapObject(NullHandler oReader)
		{
			KommFile oKommFile = new KommFile();
            oKommFile.KommFileID = oReader.GetInt32("KommFileID");
            oKommFile.VehicleOrderID = oReader.GetInt32("VehicleOrderID");
            oKommFile.KommFileImageID = oReader.GetInt32("KommFileImageID");
            oKommFile.FileNo = oReader.GetString("FileNo");
            oKommFile.KommNo = oReader.GetString("KommNo");
            oKommFile.ChassisNo = oReader.GetString("ChassisNo");
            oKommFile.EngineNo = oReader.GetString("EngineNo");
            oKommFile.ModelNo = oReader.GetString("ModelNo");
            oKommFile.YearOfManufacture = oReader.GetString("YearOfManufacture");
            oKommFile.MaximumTorque = oReader.GetString("MaximumTorque");
            oKommFile.MaxPowerOutput = oReader.GetString("MaxPowerOutput");
            oKommFile.Transmission = oReader.GetString("Transmission");
            oKommFile.CountryOfOrigin = oReader.GetString("CountryOfOrigin");
            oKommFile.EngineType = oReader.GetString("EngineType");
            oKommFile.ModelNo = oReader.GetString("ModelNo");
            oKommFile.VehicleOrderNo = oReader.GetString("VehicleOrderNo");
            oKommFile.RefNo = oReader.GetString("RefNo");
            oKommFile.UnitPrice = oReader.GetDouble("UnitPrice");
            oKommFile.VatInPercent = oReader.GetDouble("VatInPercent");
            oKommFile.VATPercentage = oReader.GetDouble("VATPercentage");
            oKommFile.ExShowroomPriceBC = oReader.GetDouble("ExShowroomPriceBC");
            oKommFile.RegistrationFeePercent = oReader.GetDouble("RegistrationFeePercent");
            oKommFile.ExteriorColorCode = oReader.GetString("ExteriorColorCode");
            oKommFile.ExteriorColorName = oReader.GetString("ExteriorColorName");
            oKommFile.Remarks = oReader.GetString("Remarks");
            oKommFile.BUID = oReader.GetInt32("BUID");
            oKommFile.CurrencyID = oReader.GetInt32("CurrencyID");
            oKommFile.CurrencyName = oReader.GetString("CurrencyName");
            oKommFile.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oKommFile.InteriorColorCode = oReader.GetString("InteriorColorCode");
            oKommFile.InteriorColorName = oReader.GetString("InteriorColorName");
            oKommFile.IssueDate = oReader.GetDateTime("IssueDate");
            oKommFile.FeatureSetupName = oReader.GetString("FeatureSetupName");
            oKommFile.ETAValue = oReader.GetInt32("ETAValue");
            oKommFile.ETAType = (EnumDisplayPart)oReader.GetInt32("ETAType");
            oKommFile.ETATypeInInt = oReader.GetInt32("ETAType");
            oKommFile.OrderStatus = (EnumVOStatus)oReader.GetInt32("OrderStatus");
            oKommFile.OrderStatusInInt = oReader.GetInt32("OrderStatus");
            oKommFile.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oKommFile.InteriorColorID = oReader.GetInt32("InteriorColorID");
            oKommFile.ExteriorColorID = oReader.GetInt32("ExteriorColorID");
            oKommFile.EngineID = oReader.GetInt32("EngineID");
            oKommFile.ChassisID = oReader.GetInt32("ChassisID");
            oKommFile.Upholstery = oReader.GetString("Upholstery");
            oKommFile.UpholsteryCode = oReader.GetString("UpholsteryCode");
            oKommFile.UpholsteryID = oReader.GetInt32("UpholsteryID");
            oKommFile.Trim = oReader.GetString("Trim");
            oKommFile.TrimCode = oReader.GetString("TrimCode");
            oKommFile.TrimID = oReader.GetInt32("TrimID");
            oKommFile.Wheels = oReader.GetString("Wheels");
            oKommFile.WheelsCode = oReader.GetString("WheelsCode");
            oKommFile.WheelsID = oReader.GetInt32("WheelsID");
            oKommFile.SalesStatus = oReader.GetInt32("SalesStatus");
            oKommFile.SalesQuotationCount = oReader.GetInt32("SalesQuotationCount");
            oKommFile.KommFileStatus = (EnumKommFileStatus)oReader.GetInt32("KommFileStatus");
            
			return oKommFile;
		}

		private KommFile CreateObject(NullHandler oReader)
		{
			KommFile oKommFile = new KommFile();
			oKommFile = MapObject(oReader);
			return oKommFile;
		}

		private List<KommFile> CreateObjects(IDataReader oReader)
		{
			List<KommFile> oKommFile = new List<KommFile>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KommFile oItem = CreateObject(oHandler);
				oKommFile.Add(oItem);
			}
			return oKommFile;
		}

		#endregion

		#region Interface implementation
        public KommFile Save(KommFile oKommFile, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sKommFileDetailIDs = "";
            List<KommFileDetail> oKommFileDetails = new List<KommFileDetail>();
            KommFileDetail oKommFileDetail = new KommFileDetail();
            oKommFileDetails = oKommFile.KommFileDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKommFile.KommFileID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KommFile, EnumRoleOperationType.Add);
                    reader = KommFileDA.InsertUpdate(tc, oKommFile, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KommFile, EnumRoleOperationType.Edit);
                    reader = KommFileDA.InsertUpdate(tc, oKommFile, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFile = new KommFile();
                    oKommFile = CreateObject(oReader);
                }
                reader.Close();
                #region Recycle Process Detail Detail Part
                foreach (KommFileDetail oItem in oKommFileDetails)
                {
                    IDataReader readerdetail;
                    oItem.KommFileID = oKommFile.KommFileID;
                    if (oItem.KommFileDetailID <= 0)
                    {
                        readerdetail = KommFileDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = KommFileDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sKommFileDetailIDs = sKommFileDetailIDs + oReaderDetail.GetString("KommFileDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sKommFileDetailIDs.Length > 0)
                {
                    sKommFileDetailIDs = sKommFileDetailIDs.Remove(sKommFileDetailIDs.Length - 1, 1);
                }
                oKommFileDetail = new KommFileDetail();
                oKommFileDetail.KommFileID = oKommFile.KommFileID;
                if (oKommFile.KommFileID>0)
                    KommFileDetailDA.Delete(tc, oKommFileDetail, EnumDBOperation.Delete, nUserID, sKommFileDetailIDs);
                #endregion

                #region Get Recycle Process
                reader = KommFileDA.Get(tc, oKommFile.KommFileID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFile = new KommFile();
                    oKommFile = CreateObject(oReader);
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
                    oKommFile = new KommFile();
                    oKommFile.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKommFile;
        }

        public KommFile Approve(KommFile oKommFile, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KommFile, EnumRoleOperationType.Approved);
                reader = KommFileDA.InsertUpdate(tc, oKommFile, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFile = new KommFile();
                    oKommFile = CreateObject(oReader);
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
                    oKommFile = new KommFile();
                    oKommFile.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKommFile;
        }
        public KommFile UpdateStatus(KommFile oKommFile, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KommFileDA.UpdateStatus(tc, oKommFile);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFile = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oKommFile = new KommFile();
                oKommFile.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oKommFile;
        }
		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				KommFile oKommFile = new KommFile();
				oKommFile.KommFileID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.KommFile, EnumRoleOperationType.Delete);
				DBTableReferenceDA.HasReference(tc, "KommFile", id);
				KommFileDA.Delete(tc, oKommFile, EnumDBOperation.Delete, nUserId);
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

		public KommFile Get(int id, Int64 nUserId)
		{
			KommFile oKommFile = new KommFile();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = KommFileDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oKommFile = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get KommFile", e);
				#endregion
			}
			return oKommFile;
		}

        public List<KommFile> BUWiseGets(int buid, Int64 nUserID)
		{
			List<KommFile> oKommFiles = new List<KommFile>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
                reader = KommFileDA.BUWiseGets(buid, tc);
				oKommFiles = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				KommFile oKommFile = new KommFile();
				oKommFile.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oKommFiles;
		}

		public List<KommFile> Gets (string sSQL, Int64 nUserID)
		{
			List<KommFile> oKommFiles = new List<KommFile>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = KommFileDA.Gets(tc, sSQL);
				oKommFiles = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get KommFile", e);
				#endregion
			}
			return oKommFiles;
		}

		#endregion
	}

}
