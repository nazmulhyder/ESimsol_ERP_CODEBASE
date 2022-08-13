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
	public class BTMAService : MarshalByRefObject, IBTMAService
	{
		#region Private functions and declaration

		private BTMA MapObject(NullHandler oReader)
		{
			BTMA oBTMA = new BTMA();
			oBTMA.BTMAID = oReader.GetInt32("BTMAID");
            oBTMA.ExportLCID = oReader.GetInt32("ExportLCID");			
            oBTMA.ExportLCNo = oReader.GetString("ExportLCNo");
            oBTMA.LCExpireDate = oReader.GetDateTime("LCExpireDate");
            oBTMA.LCDate = oReader.GetDateTime("LCDate");
			oBTMA.ExportBillID = oReader.GetInt32("ExportBillID");
            oBTMA.ExportBillNo = oReader.GetString("ExportBillNo");
            oBTMA.BankName = oReader.GetString("BankName");
			oBTMA.BranchName = oReader.GetString("BranchName");
            oBTMA.Amount = oReader.GetDouble("Amount");
            oBTMA.Amount_Bill = oReader.GetDouble("Amount_Bill");
            oBTMA.CertificateNo = oReader.GetDouble("CertificateNo");
            oBTMA.Amount_ImportLC = oReader.GetDouble("Amount_ImportLC");
			oBTMA.SupplierID = oReader.GetInt32("SupplierID");
            oBTMA.SupplierName = oReader.GetString("SupplierName");
            oBTMA.SupplierAddress = oReader.GetString("SupplierAddress");
            oBTMA.MasterLCNos = oReader.GetString("MasterLCNos");
			oBTMA.MasterLCDates = oReader.GetString("MasterLCDates");
			oBTMA.InvoiceDate = oReader.GetDateTime("InvoiceDate");
			oBTMA.ImportLCID = oReader.GetInt32("ImportLCID");
			oBTMA.ImportLCNo = oReader.GetString("ImportLCNo");
			oBTMA.ImportLCDate = oReader.GetDateTime("ImportLCDate");
			oBTMA.GarmentsQty = oReader.GetString("GarmentsQty");
			oBTMA.DeliveryDate = oReader.GetDateTime("DeliveryDate");
			oBTMA.DeliveryChallanNo = oReader.GetString("DeliveryChallanNo");
			oBTMA.MushakNo = oReader.GetString("MushakNo");
			oBTMA.MushakDate = oReader.GetDateTime("MushakDate");
			oBTMA.GatePassNo = oReader.GetString("GatePassNo");
			oBTMA.GatePassDate = oReader.GetDateTime("GatePassDate");
            oBTMA.PrintBy = oReader.GetInt32("PrintBy");
            oBTMA.BUID = oReader.GetInt32("BUID");
            oBTMA.BUName = oReader.GetString("BUName");
            oBTMA.PrintByName = oReader.GetString("PrintByName");
            oBTMA.PrintDate = oReader.GetDateTime("PrintDate");
            oBTMA.BUType = oReader.GetInt32("BUType");
            oBTMA.PartyAddress = oReader.GetString("PartyAddress");
            oBTMA.BUName = oReader.GetString("BUName");
            oBTMA.BUAddress = oReader.GetString("BUAddress");
            oBTMA.BUShortName = oReader.GetString("BUShortName");
            oBTMA.PartyName = oReader.GetString("PartyName");
            oBTMA.PartyAddress = oReader.GetString("PartyAddress");
            oBTMA.CurrencyID = oReader.GetInt32("CurrencyID");
            oBTMA.CurrencyName = oReader.GetString("CurrencyName");
            oBTMA.Currency = oReader.GetString("Currency");
            oBTMA.BUtypes = (EnumBusinessUnitType)oReader.GetInt32("BUType");
			
			return oBTMA;
		}
		private BTMA CreateObject(NullHandler oReader)
		{
			BTMA oBTMA = new BTMA();
			oBTMA = MapObject(oReader);
			return oBTMA;
		}
        private BTMA MapObject_MaxCNo(NullHandler oReader)
        {
            BTMA oBTMA = new BTMA();
            oBTMA.CertificateNo = oReader.GetInt32("CertificateNo");
            return oBTMA;
        }
        private BTMA CreateObject_MaxCNo(NullHandler oReader)
        {
            BTMA oBTMA = new BTMA();
            oBTMA = MapObject_MaxCNo(oReader);
            return oBTMA;
        }
		private List<BTMA> CreateObjects(IDataReader oReader)
		{
			List<BTMA> oBTMA = new List<BTMA>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				BTMA oItem = CreateObject(oHandler);
				oBTMA.Add(oItem);
			}
			return oBTMA;
		}

		#endregion

		#region Interface implementation
		public BTMA Save(BTMA oBTMA, Int64 nUserID)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				IDataReader reader;
				if (oBTMA.BTMAID <= 0)
				{
					reader = BTMADA.InsertUpdate(tc, oBTMA, EnumDBOperation.Insert, nUserID);
				}
				else{
					reader = BTMADA.InsertUpdate(tc, oBTMA, EnumDBOperation.Update, nUserID);
				}
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
					oBTMA = new BTMA();
					oBTMA = CreateObject(oReader);
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
					oBTMA = new BTMA();
					oBTMA.ErrorMessage = e.Message.Split('!')[0];
				}
				#endregion
			}
			return oBTMA;
		}

		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				BTMA oBTMA = new BTMA();
				oBTMA.BTMAID = id;
				DBTableReferenceDA.HasReference(tc, "BTMA", id);
				BTMADA.Delete(tc, oBTMA, EnumDBOperation.Delete, nUserId);
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

		public BTMA Get(int id, Int64 nUserId)
		{
			BTMA oBTMA = new BTMA();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = BTMADA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oBTMA = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get BTMA", e);
				#endregion
			}
			return oBTMA;
		}

		public List<BTMA> Gets(Int64 nUserID)
		{
			List<BTMA> oBTMAs = new List<BTMA>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = BTMADA.Gets(tc);
				oBTMAs = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				BTMA oBTMA = new BTMA();
				oBTMA.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oBTMAs;
		}

		public List<BTMA> Gets (string sSQL, Int64 nUserID)
		{
			List<BTMA> oBTMAs = new List<BTMA>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = BTMADA.Gets(tc, sSQL);
				oBTMAs = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get BTMA", e);
				#endregion
			}
			return oBTMAs;
		}
        public BTMA GetMaxCNo(Int64 nUserID)
        {
            BTMA oBTMA = new BTMA();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BTMADA.Get_MaxCNo(tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBTMA = CreateObject_MaxCNo(oReader);
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
                throw new ServiceException("Failed to Get BTMA", e);
                #endregion
            }
            return oBTMA;
        }
        public BTMA Update_PrintBy(BTMA oBTMA, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = BTMADA.Update_PrintBy(tc, oBTMA, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBTMA = new BTMA();
                    oBTMA = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update ExportIncentive. Because of " + e.Message, e);
                #endregion
            }
            return oBTMA;
        }
        public BTMA SaveBTMA(BTMA oBTMA, Int64 nUserID)
        {
            List<BTMADetail> oBTMADetails = new List<BTMADetail>();
            oBTMADetails = oBTMA.BTMADetails;
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBTMA.BTMAID <= 0)
                {
                    reader = BTMADA.InsertUpdate(tc, oBTMA, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BTMADA.InsertUpdate(tc, oBTMA, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBTMA = new BTMA();
                    oBTMA = CreateObject(oReader);
                }
                reader.Close();
                #region  BTMADetail
                if (oBTMADetails != null)
                {
                    sRefChildIDs = "";
                    foreach (BTMADetail oItem in oBTMADetails)
                    {
                        IDataReader readerdetail;
                        readerdetail = null;
                        oItem.BTMAID = oBTMA.BTMAID;
                        if (oItem.BTMADetailID <= 0)
                        {
                            readerdetail = BTMADetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = BTMADetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sRefChildIDs = sRefChildIDs + oReaderDetail.GetString("BTMADetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sRefChildIDs.Length > 0)
                    {
                        sRefChildIDs = sRefChildIDs.Remove(sRefChildIDs.Length - 1, 1);
                    }
                    BTMADetail oBTMADetail = new BTMADetail();
                    oBTMADetail.BTMAID = oBTMA.BTMAID;
                    BTMADetailDA.Delete(tc, oBTMADetail, EnumDBOperation.Delete, nUserID, sRefChildIDs);
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
                    oBTMA = new BTMA();
                    oBTMA.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oBTMA;
        }
		#endregion
	}

}
