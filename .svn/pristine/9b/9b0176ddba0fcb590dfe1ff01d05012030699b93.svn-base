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
	public class FabricLotAssignService : MarshalByRefObject, IFabricLotAssignService
	{
		#region Private functions and declaration

		private FabricLotAssign MapObject(NullHandler oReader)
		{
			FabricLotAssign oFabricLotAssign = new FabricLotAssign();
			oFabricLotAssign.FabricLotAssignID = oReader.GetInt32("FabricLotAssignID");
            oFabricLotAssign.LotID = oReader.GetInt32("LotID");
            oFabricLotAssign.LotNo = oReader.GetString("LotNo");
            oFabricLotAssign.ProductCode = oReader.GetString("ProductCode");
            oFabricLotAssign.ProductName = oReader.GetString("ProductName");
            oFabricLotAssign.ProductNameLot = oReader.GetString("ProductNameLot");
			oFabricLotAssign.FEOSDID = oReader.GetInt32("FEOSDID");
            oFabricLotAssign.FEOSID = oReader.GetInt32("FEOSID");
            oFabricLotAssign.ProductID = oReader.GetInt32("ProductID");
            oFabricLotAssign.ParentLotID = oReader.GetInt32("ParentLotID");
            oFabricLotAssign.WarpWeftType = (EnumWarpWeft)oReader.GetInt32("WarpWeftType");
            oFabricLotAssign.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
			oFabricLotAssign.Qty = oReader.GetDouble("Qty");
            oFabricLotAssign.Balance = oReader.GetDouble("Balance");
            oFabricLotAssign.Qty_Order = oReader.GetDouble("Qty_Order");
            oFabricLotAssign.Qty_Req = oReader.GetDouble("Qty_Req");
            oFabricLotAssign.Qty_RS = oReader.GetDouble("Qty_RS");
            oFabricLotAssign.BalanceLot = oReader.GetDouble("BalanceLot");
            oFabricLotAssign.Qty_Assign = oReader.GetDouble("Qty_Assign");
            
            oFabricLotAssign.FabricLotAssignDate = oReader.GetDateTime("DBServerDateTime");
            oFabricLotAssign.OperationUnitName = oReader.GetString("OperationUnitName");
            oFabricLotAssign.LocationName = oReader.GetString("LocationName");
            
            oFabricLotAssign.ExeNo = oReader.GetString("ExeNo");
            oFabricLotAssign.BuyerName = oReader.GetString("BuyerName");
            oFabricLotAssign.CustomerName = oReader.GetString("CustomerName");
            oFabricLotAssign.ColorName = oReader.GetString("ColorName");
            oFabricLotAssign.MUName = oReader.GetString("MUName");
            oFabricLotAssign.MUSymbol = oReader.GetString("MUSymbol");
            oFabricLotAssign.FSCDetailID = oReader.GetInt32("FSCDetailID");
            oFabricLotAssign.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oFabricLotAssign.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            
			return oFabricLotAssign;
		}

		private FabricLotAssign CreateObject(NullHandler oReader)
		{
			FabricLotAssign oFabricLotAssign = new FabricLotAssign();
			oFabricLotAssign = MapObject(oReader);
			return oFabricLotAssign;
		}

		private List<FabricLotAssign> CreateObjects(IDataReader oReader)
		{
			List<FabricLotAssign> oFabricLotAssign = new List<FabricLotAssign>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FabricLotAssign oItem = CreateObject(oHandler);
				oFabricLotAssign.Add(oItem);
			}
			return oFabricLotAssign;
		}

		#endregion

		#region Interface implementation
		public FabricLotAssign Save(FabricLotAssign oFabricLotAssign, Int64 nUserID)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				IDataReader reader;
				if (oFabricLotAssign.FabricLotAssignID <= 0)
				{
					reader = FabricLotAssignDA.InsertUpdate(tc, oFabricLotAssign, EnumDBOperation.Insert, nUserID);
				}
				else{
					reader = FabricLotAssignDA.InsertUpdate(tc, oFabricLotAssign, EnumDBOperation.Update, nUserID);
				}
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
					oFabricLotAssign = new FabricLotAssign();
					oFabricLotAssign = CreateObject(oReader);
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
					oFabricLotAssign = new FabricLotAssign();
					oFabricLotAssign.ErrorMessage = e.Message.Split('!')[0];
				}
				#endregion
			}
			return oFabricLotAssign;
		}
        public FabricLotAssign Save(List<FabricLotAssign> oFabricLotAssigns, Int64 nUserID)
        {
            TransactionContext tc = null;
            FabricLotAssign _oFabricLotAssign = new FabricLotAssign();

            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FabricLotAssign oitem in oFabricLotAssigns) 
                {
                    IDataReader reader;
                    if (oitem.FabricLotAssignID <= 0)
                    {
                        reader = FabricLotAssignDA.InsertUpdate(tc, oitem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FabricLotAssignDA.InsertUpdate(tc, oitem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        FabricLotAssign oFabricLotAssign = new FabricLotAssign();
                        oFabricLotAssign = CreateObject(oReader);
                        _oFabricLotAssign.FabricLotAssigns.Add(oFabricLotAssign);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    _oFabricLotAssign = new FabricLotAssign();
                    _oFabricLotAssign.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return _oFabricLotAssign;
        }
		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				FabricLotAssign oFabricLotAssign = new FabricLotAssign();
				oFabricLotAssign.FabricLotAssignID = id;
				FabricLotAssignDA.Delete(tc, oFabricLotAssign, EnumDBOperation.Delete, nUserId);
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exceptionif (tc != null)
				tc.HandleError();
				return e.Message.Split('!')[0];
				#endregion
			}
			return "Data delete successfully";
		}
		public FabricLotAssign Get(int id, Int64 nUserId)
		{
			FabricLotAssign oFabricLotAssign = new FabricLotAssign();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = FabricLotAssignDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oFabricLotAssign = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get FabricLotAssign", e);
				#endregion
			}
			return oFabricLotAssign;
		}
		public List<FabricLotAssign> Gets(Int64 nUserID)
		{
			List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FabricLotAssignDA.Gets(tc);
				oFabricLotAssigns = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				FabricLotAssign oFabricLotAssign = new FabricLotAssign();
				oFabricLotAssign.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oFabricLotAssigns;
		}
		public List<FabricLotAssign> Gets (string sSQL, Int64 nUserID)
		{
			List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FabricLotAssignDA.Gets(tc, sSQL);
				oFabricLotAssigns = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get FabricLotAssign", e);
				#endregion
			}
			return oFabricLotAssigns;
		}
		#endregion
	}

}
