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
	public class FNRequisitionService : MarshalByRefObject, IFNRequisitionService
	{
		#region Private functions and declaration
		public static FNRequisition MapObject(NullHandler oReader)
		{
			FNRequisition oFNRequisition = new FNRequisition();
			oFNRequisition.FNRID = oReader.GetInt32("FNRID");
			oFNRequisition.TreatmentID = (EnumFNTreatment) oReader.GetInt32("TreatmentID");
			oFNRequisition.ShiftID = (EnumShift)oReader.GetInt32("ShiftID");
            oFNRequisition.FNRNo = oReader.GetString("FNRNo");
			oFNRequisition.FNExODetailID = oReader.GetInt32("FNExODetailID");
			oFNRequisition.Shade = (EnumShade) oReader.GetInt32("Shade");
			oFNRequisition.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oFNRequisition.WorkingUnitReceiveID = oReader.GetInt32("WorkingUnitReceiveID");
            oFNRequisition.MachineID = oReader.GetInt32("MachineID");
            oFNRequisition.MachineName = oReader.GetString("MachineName");
            oFNRequisition.MachineCode = oReader.GetString("MachineCode");
			oFNRequisition.Remarks = oReader.GetString("Remarks");
            oFNRequisition.Construction = oReader.GetString("Construction");
			oFNRequisition.ApproveBy = oReader.GetInt32("ApproveBy");
			oFNRequisition.ApproveDate = oReader.GetDateTime("ApproveDate");
			oFNRequisition.DisburseBy = oReader.GetInt32("DisburseBy");
			oFNRequisition.DisburseDate = oReader.GetDateTime("DisburseDate");
			oFNRequisition.ReceiveBy = oReader.GetInt32("ReceiveBy");
			oFNRequisition.ReceiveDate = oReader.GetDateTime("ReceiveDate");
			oFNRequisition.WorkingUnitName = oReader.GetString("WorkingUnitName");
			oFNRequisition.ApproveByName = oReader.GetString("ApproveByName");
			oFNRequisition.DisburseByName = oReader.GetString("DisburseByName");
			oFNRequisition.ReceiveByName = oReader.GetString("ReceiveByName");
			oFNRequisition.IssueByName = oReader.GetString("IssueByName");
            oFNRequisition.FNExONo = oReader.GetString("FNExONo");
			oFNRequisition.BuyerName = oReader.GetString("BuyerName");
            oFNRequisition.ColorName =oReader.GetString("ColorName");
            oFNRequisition.LDNo= oReader.GetString("LDNo");
            oFNRequisition.ExeNo = oReader.GetString("ExeNoFull");            
			oFNRequisition.Qty = oReader.GetDouble("Qty");
            oFNRequisition.RequestDate = oReader.GetDateTime("RequestDate");
            oFNRequisition.IsRequisitionOpen = oReader.GetBoolean("IsRequisitionOpen");
			return oFNRequisition;
		}
        public static FNRequisition CreateObject(NullHandler oReader)
		{
			FNRequisition oFNRequisition = new FNRequisition();
			oFNRequisition = MapObject(oReader);
			return oFNRequisition;
		}
		private List<FNRequisition> CreateObjects(IDataReader oReader)
		{
			List<FNRequisition> oFNRequisition = new List<FNRequisition>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNRequisition oItem = CreateObject(oHandler);
				oFNRequisition.Add(oItem);
			}
			return oFNRequisition;
		}

		#endregion

		#region Interface implementation
			public FNRequisition Save(FNRequisition oFNRequisition, Int64 nUserID)
			{
				TransactionContext tc = null;
                FNRequisition objFNRequisition = new FNRequisition();
                objFNRequisition = oFNRequisition;
                FNRequisitionDetail oFNRequisitionDetail = new FNRequisitionDetail();
                List<FNRequisitionDetail> oFNRequisitionDetails = new List<FNRequisitionDetail>();
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFNRequisition.FNRID <= 0)
					{
						
						reader = FNRequisitionDA.InsertUpdate(tc, oFNRequisition, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = FNRequisitionDA.InsertUpdate(tc, oFNRequisition, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFNRequisition = new FNRequisition();
						oFNRequisition = CreateObject(oReader);
					}
					reader.Close();
                    #region DURequisition Part

                    foreach (FNRequisitionDetail oItem in objFNRequisition.FNRequisitionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.FNRID = oFNRequisition.FNRID;
                        if (oItem.FNRDetailID <= 0)
                        {
                            readerdetail = FNRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = FNRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            
                        }
                        readerdetail.Close();
                    }
                    #endregion
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					{
						tc.HandleError();
						oFNRequisition = new FNRequisition();
						oFNRequisition.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
                return oFNRequisition;
			}
            public FNRequisition Disburse(FNRequisition oFNRequisition, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = FNRequisitionDA.InsertUpdate(tc, oFNRequisition, EnumDBOperation.Disburse, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNRequisition = new FNRequisition();
                        oFNRequisition = CreateObject(oReader);
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
                        oFNRequisition = new FNRequisition();
                        oFNRequisition.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFNRequisition;
            }
            public FNRequisition Approval(FNRequisition oFNRequisition, bool bIsApprove, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    FNRequisitionDA.Approval(tc, oFNRequisition.FNRID, bIsApprove, nUserID);
                    IDataReader reader;
                    reader = FNRequisitionDA.Get(tc, oFNRequisition.FNRID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNRequisition = new FNRequisition();
                        oFNRequisition = CreateObject(oReader);
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
                        oFNRequisition = new FNRequisition();
                        oFNRequisition.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFNRequisition;
            }
            public FNRequisition Received(FNRequisition oFNRequisition, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    //FNRequisitionDA.Received(tc, oFNRequisition.FNRID, nUserID);
                    
                    IDataReader reader;
                    //reader = FNRequisitionDA.Get(tc, oFNRequisition.FNRID);
                    reader = FNRequisitionDA.InsertUpdate(tc, oFNRequisition, EnumDBOperation.Receive, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNRequisition = new FNRequisition();
                        oFNRequisition = CreateObject(oReader);
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
                        oFNRequisition = new FNRequisition();
                        oFNRequisition.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFNRequisition;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FNRequisition oFNRequisition = new FNRequisition();
					oFNRequisition.FNRID = id;
					DBTableReferenceDA.HasReference(tc, "FNRequisition", id);
					FNRequisitionDA.Delete(tc, oFNRequisition, EnumDBOperation.Delete, nUserId);
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
			public FNRequisition Get(int id, Int64 nUserId)
			{
				FNRequisition oFNRequisition = new FNRequisition();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FNRequisitionDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFNRequisition = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FNRequisition", e);
					#endregion
				}
				return oFNRequisition;
			}
			public List<FNRequisition> Gets(Int64 nUserID)
			{
				List<FNRequisition> oFNRequisitions = new List<FNRequisition>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNRequisitionDA.Gets(tc);
					oFNRequisitions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FNRequisition oFNRequisition = new FNRequisition();
					oFNRequisition.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFNRequisitions;
			}
			public List<FNRequisition> Gets (string sSQL, Int64 nUserID)
			{
				List<FNRequisition> oFNRequisitions = new List<FNRequisition>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNRequisitionDA.Gets(tc, sSQL);
					oFNRequisitions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FNRequisition", e);
					#endregion
				}
				return oFNRequisitions;
			}

		#endregion
	}

}
