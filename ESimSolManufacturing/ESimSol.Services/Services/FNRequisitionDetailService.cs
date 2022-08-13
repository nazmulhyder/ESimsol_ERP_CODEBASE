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
	public class FNRequisitionDetailService : MarshalByRefObject, IFNRequisitionDetailService
	{
		#region Private functions and declaration
		private FNRequisitionDetail MapObject(NullHandler oReader)
		{
			FNRequisitionDetail oFNRequisitionDetail = new FNRequisitionDetail();
			oFNRequisitionDetail.FNRDetailID = oReader.GetInt32("FNRDetailID");
			oFNRequisitionDetail.FNRID = oReader.GetInt32("FNRID");
			oFNRequisitionDetail.ProductID = oReader.GetInt32("ProductID");
			oFNRequisitionDetail.LotID = oReader.GetInt32("LotID");
            oFNRequisitionDetail.DestinationLotID = oReader.GetInt32("DestinationLotID");
			oFNRequisitionDetail.RequiredQty = oReader.GetDouble("RequiredQty");
			oFNRequisitionDetail.DisburseQty = oReader.GetDouble("DisburseQty");
			oFNRequisitionDetail.Remarks = oReader.GetString("Remarks");
			oFNRequisitionDetail.ProductName = oReader.GetString("ProductName");
			oFNRequisitionDetail.ProductCode = oReader.GetString("ProductCode");
            oFNRequisitionDetail.LotNo = oReader.GetString("LotNo");
            oFNRequisitionDetail.DestinationLotNo = oReader.GetString("DestinationLotNo");
            oFNRequisitionDetail.LotBalance = oReader.GetDouble("LotBalance");
            oFNRequisitionDetail.DestinationLotBalance = oReader.GetDouble("LotBalance");
            oFNRequisitionDetail.Rate = oReader.GetDouble("Rate");
            oFNRequisitionDetail.MUName = oReader.GetString("MUName");
            oFNRequisitionDetail.FNRNo = oReader.GetString("FNRNo");
            
            oFNRequisitionDetail.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oFNRequisitionDetail.RequestDate = oReader.GetDateTime("RequestDate");
			return oFNRequisitionDetail;
		}

		private FNRequisitionDetail CreateObject(NullHandler oReader)
		{
			FNRequisitionDetail oFNRequisitionDetail = new FNRequisitionDetail();
			oFNRequisitionDetail = MapObject(oReader);
			return oFNRequisitionDetail;
		}

		private List<FNRequisitionDetail> CreateObjects(IDataReader oReader)
		{
			List<FNRequisitionDetail> oFNRequisitionDetail = new List<FNRequisitionDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNRequisitionDetail oItem = CreateObject(oHandler);
				oFNRequisitionDetail.Add(oItem);
			}
			return oFNRequisitionDetail;
		}
		#endregion

		#region Interface implementation
        public FNRequisitionDetail Save(FNRequisitionDetail oFNRequisitionDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            FNRequisition oFNRequisition = new FNRequisition();
            oFNRequisition = oFNRequisitionDetail.FNRequisition;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (oFNRequisitionDetail.FNRID == 0)
                {
                    if (oFNRequisitionDetail.FNRequisition != null)
                    {
                        reader = FNRequisitionDA.InsertUpdate(tc, oFNRequisitionDetail.FNRequisition, EnumDBOperation.Insert, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFNRequisition = FNRequisitionService.CreateObject(oReader);
                        }
                        reader.Close();
                        oFNRequisitionDetail.FNRID = oFNRequisition.FNRID;
                    }
                    else { throw new Exception("No FN Production information found to save."); }
                }
                if (oFNRequisitionDetail.FNRDetailID <= 0)
                {
                    reader = FNRequisitionDetailDA.InsertUpdate(tc, oFNRequisitionDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNRequisitionDetailDA.InsertUpdate(tc, oFNRequisitionDetail, EnumDBOperation.Update, nUserID);
                }
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNRequisitionDetail = new FNRequisitionDetail();
                    oFNRequisitionDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNRequisitionDetail.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            oFNRequisitionDetail.FNRequisition = oFNRequisition;
            return oFNRequisitionDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNRequisitionDetail oFNRequisitionDetail = new FNRequisitionDetail();
                oFNRequisitionDetail.FNRDetailID = id;
                FNRequisitionDetailDA.Delete(tc, oFNRequisitionDetail, EnumDBOperation.Delete, nUserId);
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


			public FNRequisitionDetail Get(int id, Int64 nUserId)
			{
				FNRequisitionDetail oFNRequisitionDetail = new FNRequisitionDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FNRequisitionDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFNRequisitionDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FNRequisitionDetail", e);
					#endregion
				}
				return oFNRequisitionDetail;
			}

			public List<FNRequisitionDetail> Gets(int id, Int64 nUserID)
			{
				List<FNRequisitionDetail> oFNRequisitionDetails = new List<FNRequisitionDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNRequisitionDetailDA.Gets(id, tc);
					oFNRequisitionDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FNRequisitionDetail oFNRequisitionDetail = new FNRequisitionDetail();
					oFNRequisitionDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFNRequisitionDetails;
			}

			public List<FNRequisitionDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<FNRequisitionDetail> oFNRequisitionDetails = new List<FNRequisitionDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNRequisitionDetailDA.Gets(tc, sSQL);
					oFNRequisitionDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FNRequisitionDetail", e);
					#endregion
				}
				return oFNRequisitionDetails;
			}

		#endregion
	}

}
