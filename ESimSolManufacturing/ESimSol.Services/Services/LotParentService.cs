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
	public class LotParentService : MarshalByRefObject, ILotParentService
	{
		#region Private functions and declaration

		private LotParent MapObject(NullHandler oReader)
		{
			LotParent oLotParent = new LotParent();
            oLotParent.LotParentID = oReader.GetInt32("LotParentID");
            oLotParent.ProductID = oReader.GetInt32("ProductID");
            oLotParent.DUPGLDetailID = oReader.GetInt32("DUPGLDetailID");
            oLotParent.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oLotParent.ParentType = oReader.GetInt32("ParentType");
            oLotParent.LotID = oReader.GetInt32("LotID");
            oLotParent.ParentLotID = oReader.GetInt32("ParentLotID");
            oLotParent.Qty = oReader.GetDouble("Qty");
            oLotParent.Qty_Soft = oReader.GetDouble("Qty_Soft");
            oLotParent.Qty_Order = oReader.GetDouble("Qty_Order");
            oLotParent.Qty_Batch_Out = oReader.GetDouble("Qty_Batch_Out");
            oLotParent.BalanceLot = oReader.GetDouble("BalanceLot");
            oLotParent.Balance = oReader.GetDouble("Balance");
            oLotParent.UnitPrice = oReader.GetDouble("UnitPrice");
            oLotParent.UnitPriceBC = oReader.GetDouble("UnitPriceBC");
            oLotParent.LotNo = oReader.GetString("LotNo");
            oLotParent.ContractorName = oReader.GetString("ContractorName");
            oLotParent.ProductName = oReader.GetString("ProductName");
            oLotParent.ProductCode = oReader.GetString("ProductCode");
            oLotParent.ProductNameLot = oReader.GetString("ProductNameLot");
            oLotParent.MUName = oReader.GetString("MUName");
            oLotParent.OperationUnitName = oReader.GetString("OperationUnitName");
            oLotParent.DyeingOrderNo = oReader.GetString("DyeingOrderNo");
            oLotParent.LotParentDate = oReader.GetDateTime("DBServerDateTime");
            oLotParent.OrderDate = oReader.GetDateTime("OrderDate");
            oLotParent.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oLotParent.DyeingOrderID_Out = oReader.GetInt32("DyeingOrderID_Out");
            oLotParent.EntryDate = oReader.GetDateTime("DBServerDateTime");
			return oLotParent;
		}

		private LotParent CreateObject(NullHandler oReader)
		{
			LotParent oLotParent = new LotParent();
			oLotParent = MapObject(oReader);
			return oLotParent;
		}

		private List<LotParent> CreateObjects(IDataReader oReader)
		{
			List<LotParent> oLotParent = new List<LotParent>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				LotParent oItem = CreateObject(oHandler);
				oLotParent.Add(oItem);
			}
			return oLotParent;
		}

		#endregion

		#region Interface implementation
		public LotParent Get(int id, Int64 nUserId)
		{
			LotParent oLotParent = new LotParent();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = LotParentDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oLotParent = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get LotParent", e);
				#endregion
			}
			return oLotParent;
		}

		public List<LotParent> Gets(Int64 nUserID)
		{
			List<LotParent> oLotParents = new List<LotParent>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = LotParentDA.Gets(tc);
				oLotParents = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				LotParent oLotParent = new LotParent();
				oLotParent.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oLotParents;
		}

		public List<LotParent> Gets (string sSQL, Int64 nUserID)
		{
			List<LotParent> oLotParents = new List<LotParent>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = LotParentDA.Gets(tc, sSQL);
				oLotParents = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get LotParent", e);
				#endregion
			}
			return oLotParents;
		}
        public List<LotParent> GetsBy(int nLotParentID, Int64 nUserID)
        {
            List<LotParent> oLotParents = new List<LotParent>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LotParentDA.GetsBy(tc, nLotParentID);
                oLotParents = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LotParent", e);
                #endregion
            }
            return oLotParents;
        }

		#endregion

        #region Interface implementation (SP)
        public LotParent Save(LotParent oLotParent, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLotParent.LotParentID <= 0)
                {
                    reader = LotParentDA.InsertUpdate(tc, oLotParent, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LotParentDA.InsertUpdate(tc, oLotParent, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotParent = new LotParent();
                    oLotParent = CreateObject(oReader);
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
                    oLotParent = new LotParent();
                    oLotParent.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oLotParent;
        }
        public LotParent Lot_Transfer(LotParent oLotParent, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                
                reader = LotParentDA.InsertUpdate(tc, oLotParent, EnumDBOperation.Disburse, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotParent = new LotParent();
                    oLotParent = CreateObject(oReader);
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
                    oLotParent = new LotParent();
                    oLotParent.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oLotParent;
        }
        public LotParent Lot_Return(LotParent oLotParent, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = LotParentDA.InsertUpdate(tc, oLotParent, EnumDBOperation.Cancel, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotParent = new LotParent();
                    oLotParent = CreateObject(oReader);
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
                    oLotParent = new LotParent();
                    oLotParent.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oLotParent;
        }
        public LotParent Lot_Adjustment(LotParent oLotParent, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLotParent.LotParentID <= 0)
                {
                    reader = LotParentDA.Lot_Adjustment(tc, oLotParent, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LotParentDA.Lot_Adjustment(tc, oLotParent, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotParent = new LotParent();
                    oLotParent = CreateObject(oReader);
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
                    oLotParent = new LotParent();
                    oLotParent.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oLotParent;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LotParent oLotParent = new LotParent();
                oLotParent.LotParentID = id;
                DBTableReferenceDA.HasReference(tc, "LotParent", id);
                LotParentDA.Delete(tc, oLotParent, EnumDBOperation.Delete, nUserId);
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

        #endregion
	}
}
