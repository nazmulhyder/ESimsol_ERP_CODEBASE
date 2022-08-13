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
	public class GRNDetailBreakDownService : MarshalByRefObject, IGRNDetailBreakDownService
	{
		#region Private functions and declaration

		private GRNDetailBreakDown MapObject(NullHandler oReader)
		{
			GRNDetailBreakDown oGRNDetailBreakDown = new GRNDetailBreakDown();
			oGRNDetailBreakDown.GRNDetailBreakDownID = oReader.GetInt32("GRNDetailBreakDownID");
			oGRNDetailBreakDown.GRNDetailID = oReader.GetInt32("GRNDetailID");
			oGRNDetailBreakDown.PurchaseInvoiceDetailID = oReader.GetInt32("PurchaseInvoiceDetailID");
			oGRNDetailBreakDown.RefType = oReader.GetInt32("RefType");
			oGRNDetailBreakDown.GRNID = oReader.GetInt32("GRNID");
			oGRNDetailBreakDown.ReceivedQty = oReader.GetDouble("ReceivedQty");
			oGRNDetailBreakDown.Amount = oReader.GetDouble("Amount");
			oGRNDetailBreakDown.ImportInvoiceDetailID = oReader.GetInt32("ImportInvoiceDetailID");
			oGRNDetailBreakDown.LandingCostValue = oReader.GetDouble("LandingCostValue");
			oGRNDetailBreakDown.CostHeadID = oReader.GetInt32("CostHeadID");
			oGRNDetailBreakDown.ProductName = oReader.GetString("ProductName");
			oGRNDetailBreakDown.MUSymbol = oReader.GetString("MUSymbol");
			oGRNDetailBreakDown.InvoiceNo = oReader.GetString("InvoiceNo");
            oGRNDetailBreakDown.LCNo = oReader.GetString("LCNo");
			oGRNDetailBreakDown.LCID = oReader.GetInt32("LCID");
			oGRNDetailBreakDown.InvoiceID = oReader.GetInt32("InvoiceID");
			oGRNDetailBreakDown.InvoiceDetailID = oReader.GetInt32("InvoiceDetailID");
			oGRNDetailBreakDown.CostHeadCode = oReader.GetString("CostHeadCode");
			oGRNDetailBreakDown.CostHeadName = oReader.GetString("CostHeadName");
			oGRNDetailBreakDown.ConvertionRate = oReader.GetDouble("ConvertionRate");
			oGRNDetailBreakDown.CurrencySymbol = oReader.GetString("CurrencySymbol");
			return oGRNDetailBreakDown;
		}

		private GRNDetailBreakDown CreateObject(NullHandler oReader)
		{
			GRNDetailBreakDown oGRNDetailBreakDown = new GRNDetailBreakDown();
			oGRNDetailBreakDown = MapObject(oReader);
			return oGRNDetailBreakDown;
		}

		private List<GRNDetailBreakDown> CreateObjects(IDataReader oReader)
		{
			List<GRNDetailBreakDown> oGRNDetailBreakDown = new List<GRNDetailBreakDown>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				GRNDetailBreakDown oItem = CreateObject(oHandler);
				oGRNDetailBreakDown.Add(oItem);
			}
			return oGRNDetailBreakDown;
		}

		#endregion

		#region Interface implementation
		
			public List<GRNDetailBreakDown> Gets (string sSQL, Int64 nUserID)
			{
				List<GRNDetailBreakDown> oGRNDetailBreakDowns = new List<GRNDetailBreakDown>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = GRNDetailBreakDownDA.Gets(tc, sSQL);
					oGRNDetailBreakDowns = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get GRNDetailBreakDown", e);
					#endregion
				}
				return oGRNDetailBreakDowns;
			}

		#endregion
	}

}
