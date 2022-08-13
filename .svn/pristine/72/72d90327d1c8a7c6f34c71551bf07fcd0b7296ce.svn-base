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
	public class VehicleOrderService : MarshalByRefObject, IVehicleOrderService
	{
		#region Private functions and declaration

		private VehicleOrder MapObject(NullHandler oReader)
		{
			VehicleOrder oVehicleOrder = new VehicleOrder();
            oVehicleOrder.VehicleOrderID = oReader.GetInt32("VehicleOrderID");
            oVehicleOrder.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oVehicleOrder.VehicleOrderImageID = oReader.GetInt32("VehicleOrderImageID");
            oVehicleOrder.FileNo = oReader.GetString("FileNo");
            oVehicleOrder.ChassisNo = oReader.GetString("ChassisNo");
            oVehicleOrder.EngineNo = oReader.GetString("EngineNo");
            oVehicleOrder.ModelNo = oReader.GetString("ModelNo");
            oVehicleOrder.RefNo = oReader.GetString("RefNo");
            oVehicleOrder.ExteriorColorCode = oReader.GetString("ExteriorColorCode");
            oVehicleOrder.ExteriorColorName = oReader.GetString("ExteriorColorName");
            oVehicleOrder.Remarks = oReader.GetString("Remarks");
            oVehicleOrder.BUID = oReader.GetInt32("BUID");
            oVehicleOrder.ExteriorColorID = oReader.GetInt32("ExteriorColorID");
            oVehicleOrder.InteriorColorCode = oReader.GetString("InteriorColorCode");
            oVehicleOrder.InteriorColorID = oReader.GetInt32("InteriorColorID");
            oVehicleOrder.InteriorColorCode = oReader.GetString("InteriorColorCode");
            oVehicleOrder.InteriorColorName = oReader.GetString("InteriorColorName");

            oVehicleOrder.Upholstery = oReader.GetString("Upholstery");
            oVehicleOrder.UpholsteryCode = oReader.GetString("UpholsteryCode");
            oVehicleOrder.UpholsteryID = oReader.GetInt32("UpholsteryID");
            oVehicleOrder.Trim = oReader.GetString("Trim");
            oVehicleOrder.TrimCode = oReader.GetString("TrimCode");
            oVehicleOrder.TrimID = oReader.GetInt32("TrimID");
            oVehicleOrder.Wheels = oReader.GetString("Wheels");
            oVehicleOrder.WheelsCode = oReader.GetString("WheelsCode");
            oVehicleOrder.WheelsID = oReader.GetInt32("WheelsID");

            oVehicleOrder.IssueDate = oReader.GetDateTime("IssueDate");
            oVehicleOrder.FeatureSetupName = oReader.GetString("FeatureSetupName");
            oVehicleOrder.ChassisID = oReader.GetInt32("ChassisID");
            oVehicleOrder.EngineID = oReader.GetInt32("EngineID");
            oVehicleOrder.ETAValue = oReader.GetInt32("ETAValue");
            oVehicleOrder.ETAType = (EnumDisplayPart)oReader.GetInt32("ETAType");
            oVehicleOrder.CurrencyID = oReader.GetInt32("CurrencyID");
            oVehicleOrder.ETATypeInInt = oReader.GetInt32("ETAType");
            oVehicleOrder.OrderStatus = (EnumVOStatus)oReader.GetInt32("OrderStatus");
            oVehicleOrder.OrderStatusInInt = oReader.GetInt32("OrderStatus");
            oVehicleOrder.VATPercentage = oReader.GetDouble("VATPercentage");
            oVehicleOrder.OfferPrice = oReader.GetDouble("OfferPrice");
            oVehicleOrder.ExShowroomPriceBC = oReader.GetDouble("ExShowroomPriceBC");

            return oVehicleOrder;
		}

		private VehicleOrder CreateObject(NullHandler oReader)
		{
			VehicleOrder oVehicleOrder = new VehicleOrder();
			oVehicleOrder = MapObject(oReader);
			return oVehicleOrder;
		}

		private List<VehicleOrder> CreateObjects(IDataReader oReader)
		{
			List<VehicleOrder> oVehicleOrder = new List<VehicleOrder>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				VehicleOrder oItem = CreateObject(oHandler);
				oVehicleOrder.Add(oItem);
			}
			return oVehicleOrder;
		}

		#endregion

		#region Interface implementation
        public VehicleOrder Save(VehicleOrder oVehicleOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sVehicleOrderDetailIDs = "";
            List<VehicleOrderDetail> oVehicleOrderDetails = new List<VehicleOrderDetail>();
            VehicleOrderDetail oVehicleOrderDetail = new VehicleOrderDetail();
            oVehicleOrderDetails = oVehicleOrder.VehicleOrderDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleOrder.VehicleOrderID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VehicleOrder, EnumRoleOperationType.Add);
                    reader = VehicleOrderDA.InsertUpdate(tc, oVehicleOrder, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VehicleOrder, EnumRoleOperationType.Edit);
                    reader = VehicleOrderDA.InsertUpdate(tc, oVehicleOrder, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleOrder = new VehicleOrder();
                    oVehicleOrder = CreateObject(oReader);
                }
                reader.Close();
                #region Recycle Process Detail Detail Part
                foreach (VehicleOrderDetail oItem in oVehicleOrderDetails)
                {
                    IDataReader readerdetail;
                    oItem.VehicleOrderID = oVehicleOrder.VehicleOrderID;
                    if (oItem.VehicleOrderDetailID <= 0)
                    {
                        readerdetail = VehicleOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = VehicleOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sVehicleOrderDetailIDs = sVehicleOrderDetailIDs + oReaderDetail.GetString("VehicleOrderDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sVehicleOrderDetailIDs.Length > 0)
                {
                    sVehicleOrderDetailIDs = sVehicleOrderDetailIDs.Remove(sVehicleOrderDetailIDs.Length - 1, 1);
                }
                oVehicleOrderDetail = new VehicleOrderDetail();
                oVehicleOrderDetail.VehicleOrderID = oVehicleOrder.VehicleOrderID;
                VehicleOrderDetailDA.Delete(tc, oVehicleOrderDetail, EnumDBOperation.Delete, nUserID, sVehicleOrderDetailIDs);
                #endregion

                #region Get Recycle Process
                reader = VehicleOrderDA.Get(tc, oVehicleOrder.VehicleOrderID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleOrder = new VehicleOrder();
                    oVehicleOrder = CreateObject(oReader);
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
                    oVehicleOrder = new VehicleOrder();
                    oVehicleOrder.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oVehicleOrder;
        }

        public VehicleOrder Approve(VehicleOrder oVehicleOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VehicleOrder, EnumRoleOperationType.Approved);
                reader = VehicleOrderDA.InsertUpdate(tc, oVehicleOrder, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleOrder = new VehicleOrder();
                    oVehicleOrder = CreateObject(oReader);
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
                    oVehicleOrder = new VehicleOrder();
                    oVehicleOrder.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oVehicleOrder;
        }

		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				VehicleOrder oVehicleOrder = new VehicleOrder();
				oVehicleOrder.VehicleOrderID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.VehicleOrder, EnumRoleOperationType.Delete);
				DBTableReferenceDA.HasReference(tc, "VehicleOrder", id);
				VehicleOrderDA.Delete(tc, oVehicleOrder, EnumDBOperation.Delete, nUserId);
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

		public VehicleOrder Get(int id, Int64 nUserId)
		{
			VehicleOrder oVehicleOrder = new VehicleOrder();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = VehicleOrderDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oVehicleOrder = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get VehicleOrder", e);
				#endregion
			}
			return oVehicleOrder;
		}

        public List<VehicleOrder> BUWiseGets(int buid, Int64 nUserID)
		{
			List<VehicleOrder> oVehicleOrders = new List<VehicleOrder>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
                reader = VehicleOrderDA.BUWiseGets(buid, tc);
				oVehicleOrders = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				VehicleOrder oVehicleOrder = new VehicleOrder();
				oVehicleOrder.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oVehicleOrders;
		}

		public List<VehicleOrder> Gets (string sSQL, Int64 nUserID)
		{
			List<VehicleOrder> oVehicleOrders = new List<VehicleOrder>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = VehicleOrderDA.Gets(tc, sSQL);
				oVehicleOrders = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get VehicleOrder", e);
				#endregion
			}
			return oVehicleOrders;
		}

		#endregion
	}

}
