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
	public class CapacityAllocationService : MarshalByRefObject, ICapacityAllocationService
	{
		#region Private functions and declaration

		private CapacityAllocation MapObject(NullHandler oReader)
		{
			CapacityAllocation oCapacityAllocation = new CapacityAllocation();
			oCapacityAllocation.CapacityAllocationID = oReader.GetInt32("CapacityAllocationID");
			oCapacityAllocation.Code = oReader.GetString("Code");
			oCapacityAllocation.BuyerID = oReader.GetInt32("BuyerID");
			oCapacityAllocation.Quantity = oReader.GetDouble("Quantity");
			oCapacityAllocation.MUnitID = oReader.GetInt32("MUnitID");
			oCapacityAllocation.Remarks = oReader.GetString("Remarks");
			oCapacityAllocation.BuyerName = oReader.GetString("BuyerName");
			oCapacityAllocation.MUName = oReader.GetString("MUName");
            //Property for Report
            oCapacityAllocation.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oCapacityAllocation.OrderQty = oReader.GetDouble("OrderQty");
            oCapacityAllocation.OrderValue = oReader.GetDouble("OrderValue");
			return oCapacityAllocation;
		}

		private CapacityAllocation CreateObject(NullHandler oReader)
		{
			CapacityAllocation oCapacityAllocation = new CapacityAllocation();
			oCapacityAllocation = MapObject(oReader);
			return oCapacityAllocation;
		}

		private List<CapacityAllocation> CreateObjects(IDataReader oReader)
		{
			List<CapacityAllocation> oCapacityAllocation = new List<CapacityAllocation>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CapacityAllocation oItem = CreateObject(oHandler);
				oCapacityAllocation.Add(oItem);
			}
			return oCapacityAllocation;
		}

		#endregion

		#region Interface implementation
			public CapacityAllocation Save(CapacityAllocation oCapacityAllocation, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oCapacityAllocation.CapacityAllocationID <= 0)
					{
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "CapacityAllocation", EnumRoleOperationType.Add);
						reader = CapacityAllocationDA.InsertUpdate(tc, oCapacityAllocation, EnumDBOperation.Insert, nUserID);
					}
					else{
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "CapacityAllocation", EnumRoleOperationType.Edit);
						reader = CapacityAllocationDA.InsertUpdate(tc, oCapacityAllocation, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oCapacityAllocation = new CapacityAllocation();
						oCapacityAllocation = CreateObject(oReader);
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
						oCapacityAllocation = new CapacityAllocation();
						oCapacityAllocation.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oCapacityAllocation;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					CapacityAllocation oCapacityAllocation = new CapacityAllocation();
					oCapacityAllocation.CapacityAllocationID = id;
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "CapacityAllocation", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "CapacityAllocation", id);
					CapacityAllocationDA.Delete(tc, oCapacityAllocation, EnumDBOperation.Delete, nUserId);
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

			public CapacityAllocation Get(int id, Int64 nUserId)
			{
				CapacityAllocation oCapacityAllocation = new CapacityAllocation();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = CapacityAllocationDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oCapacityAllocation = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get CapacityAllocation", e);
					#endregion
				}
				return oCapacityAllocation;
			}

			public List<CapacityAllocation> Gets(Int64 nUserID)
			{
				List<CapacityAllocation> oCapacityAllocations = new List<CapacityAllocation>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CapacityAllocationDA.Gets(tc);
					oCapacityAllocations = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					CapacityAllocation oCapacityAllocation = new CapacityAllocation();
					oCapacityAllocation.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oCapacityAllocations;
			}

            public List<CapacityAllocation> GetsBookingStatus(DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
            {
                List<CapacityAllocation> oCapacityAllocations = new List<CapacityAllocation>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = CapacityAllocationDA.GetsBookingStatus(dStartDate, dEndDate, tc);
                    oCapacityAllocations = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    CapacityAllocation oCapacityAllocation = new CapacityAllocation();
                    oCapacityAllocation.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oCapacityAllocations;
            }
			public List<CapacityAllocation> Gets (string sSQL, Int64 nUserID)
			{
				List<CapacityAllocation> oCapacityAllocations = new List<CapacityAllocation>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CapacityAllocationDA.Gets(tc, sSQL);
					oCapacityAllocations = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CapacityAllocation", e);
					#endregion
				}
				return oCapacityAllocations;
			}

		#endregion
	}

}
