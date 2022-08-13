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
	public class VehicleModelService : MarshalByRefObject, IVehicleModelService
	{
		#region Private functions and declaration

		private VehicleModel MapObject(NullHandler oReader)
		{
			VehicleModel oVehicleModel = new VehicleModel();
            oVehicleModel.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oVehicleModel.ModelCategoryID = oReader.GetInt32("ModelCategoryID");
            oVehicleModel.ModelSessionID = oReader.GetInt32("ModelSessionID");
            oVehicleModel.FileNo = oReader.GetString("FileNo");
            oVehicleModel.ModelNo = oReader.GetString("ModelNo");
            oVehicleModel.ModelShortName = oReader.GetString("ModelShortName");
            oVehicleModel.DriveType = oReader.GetInt32("DriveType");
            oVehicleModel.CurrencyName = oReader.GetString("CurrencyName");
            oVehicleModel.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oVehicleModel.SeatingCapacity = oReader.GetString("SeatingCapacity");
            oVehicleModel.ModelSessionName = oReader.GetString("ModelSessionName");
            oVehicleModel.Remarks = oReader.GetString("Remarks");
            oVehicleModel.CurrencyID = oReader.GetInt32("CurrencyID");
            oVehicleModel.VehicleModelImageID = oReader.GetInt32("VehicleModelImageID");
            oVehicleModel.MaxPrice = oReader.GetDouble("MaxPrice");
            oVehicleModel.CategoryName = oReader.GetString("CategoryName");
            oVehicleModel.SeatingCapacity = oReader.GetString("SeatingCapacity");
            oVehicleModel.MinPrice = oReader.GetDouble("MinPrice");
            oVehicleModel.OfferPrice = oReader.GetDouble("OfferPrice");
            oVehicleModel.ExShowroomPriceBC = oReader.GetDouble("ExShowroomPriceBC");
            oVehicleModel.VATPercentage = oReader.GetDouble("VATPercentage");
            oVehicleModel.EngineType = oReader.GetString("EngineType");
            oVehicleModel.MaxPowerOutput = oReader.GetString("MaxPowerOutput");
            oVehicleModel.MaximumTorque = oReader.GetString("MaximumTorque");
            oVehicleModel.Transmission = oReader.GetString("Transmission");
            oVehicleModel.DisplacementCC = oReader.GetString("DisplacementCC");
            oVehicleModel.TopSpeed = oReader.GetString("TopSpeed");
            oVehicleModel.Acceleration = oReader.GetString("Acceleration");
            oVehicleModel.ModelCode = oReader.GetString("ModelCode");
			return oVehicleModel;
		}

		private VehicleModel CreateObject(NullHandler oReader)
		{
			VehicleModel oVehicleModel = new VehicleModel();
			oVehicleModel = MapObject(oReader);
			return oVehicleModel;
		}

		private List<VehicleModel> CreateObjects(IDataReader oReader)
		{
			List<VehicleModel> oVehicleModel = new List<VehicleModel>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				VehicleModel oItem = CreateObject(oHandler);
				oVehicleModel.Add(oItem);
			}
			return oVehicleModel;
		}

		#endregion

		#region Interface implementation
        public VehicleModel Save(VehicleModel oVehicleModel, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sModelFeatureIDs = "";
            List<ModelFeature> oModelFeatures = new List<ModelFeature>();
            ModelFeature oModelFeature = new ModelFeature();
            oModelFeatures = oVehicleModel.ModelFeatures;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleModel.VehicleModelID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VehicleModel, EnumRoleOperationType.Add);
                    reader = VehicleModelDA.InsertUpdate(tc, oVehicleModel, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VehicleModel, EnumRoleOperationType.Edit);
                    reader = VehicleModelDA.InsertUpdate(tc, oVehicleModel, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleModel = new VehicleModel();
                    oVehicleModel = CreateObject(oReader);
                }
                reader.Close();
                #region Recycle Process Detail Detail Part
                foreach (ModelFeature oItem in oModelFeatures)
                {
                    IDataReader readerdetail;
                    oItem.VehicleModelID = oVehicleModel.VehicleModelID;
                    if (oItem.ModelFeatureID <= 0)
                    {
                        readerdetail = ModelFeatureDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ModelFeatureDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sModelFeatureIDs = sModelFeatureIDs + oReaderDetail.GetString("ModelFeatureID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sModelFeatureIDs.Length > 0)
                {
                    sModelFeatureIDs = sModelFeatureIDs.Remove(sModelFeatureIDs.Length - 1, 1);
                }
                oModelFeature = new ModelFeature();
                oModelFeature.VehicleModelID = oVehicleModel.VehicleModelID;
                ModelFeatureDA.Delete(tc, oModelFeature, EnumDBOperation.Delete, nUserID, sModelFeatureIDs);
                #endregion

                #region Get Recycle Process
                reader = VehicleModelDA.Get(tc, oVehicleModel.VehicleModelID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleModel = new VehicleModel();
                    oVehicleModel = CreateObject(oReader);
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
                    oVehicleModel = new VehicleModel();
                    oVehicleModel.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oVehicleModel;
        }

        public VehicleModel Approve(VehicleModel oVehicleModel, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VehicleModel, EnumRoleOperationType.Approved);
                reader = VehicleModelDA.InsertUpdate(tc, oVehicleModel, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleModel = new VehicleModel();
                    oVehicleModel = CreateObject(oReader);
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
                    oVehicleModel = new VehicleModel();
                    oVehicleModel.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oVehicleModel;
        }

		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				VehicleModel oVehicleModel = new VehicleModel();
				oVehicleModel.VehicleModelID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.VehicleModel, EnumRoleOperationType.Delete);
				DBTableReferenceDA.HasReference(tc, "VehicleModel", id);
				VehicleModelDA.Delete(tc, oVehicleModel, EnumDBOperation.Delete, nUserId);
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

		public VehicleModel Get(int id, Int64 nUserId)
		{
			VehicleModel oVehicleModel = new VehicleModel();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = VehicleModelDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oVehicleModel = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get VehicleModel", e);
				#endregion
			}
			return oVehicleModel;
		}

        public List<VehicleModel> GetsByModelNo(string ModelNo, Int64 nUserID)
		{
			List<VehicleModel> oVehicleModels = new List<VehicleModel>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
                reader = VehicleModelDA.GetsByModelNo(ModelNo,tc);
				oVehicleModels = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				VehicleModel oVehicleModel = new VehicleModel();
				oVehicleModel.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oVehicleModels;
		}

		public List<VehicleModel> Gets (string sSQL, Int64 nUserID)
		{
			List<VehicleModel> oVehicleModels = new List<VehicleModel>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = VehicleModelDA.Gets(tc, sSQL);
				oVehicleModels = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get VehicleModel", e);
				#endregion
			}
			return oVehicleModels;
		}

		#endregion
	}

}
