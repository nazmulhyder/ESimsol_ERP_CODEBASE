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
	public class FNRequisitionReportService : MarshalByRefObject, IFNRequisitionReportService
	{
		#region Private functions and declaration

		private FNRequisitionReport MapObject(NullHandler oReader)
		{
			FNRequisitionReport oFNRequisitionReport = new FNRequisitionReport();
			oFNRequisitionReport.FNRID  = oReader.GetInt32("FNRID");
			oFNRequisitionReport.FNRNo = oReader.GetString("FNRNo");
			oFNRequisitionReport.FNRDetailID = oReader.GetInt32("FNRDetailID");
			oFNRequisitionReport.DisburseQty  = oReader.GetDouble("DisburseQty");
			oFNRequisitionReport.RequestDate  = oReader.GetDateTime("RequestDate");
			oFNRequisitionReport.RequestByID  = oReader.GetInt32("RequestByID");
			oFNRequisitionReport.RequestByName  = oReader.GetString("RequestByName");
			oFNRequisitionReport.TreatmentID  = oReader.GetInt32("TreatmentID");
			oFNRequisitionReport.TreatmentName   = oReader.GetString("TreatmentName ");
			oFNRequisitionReport.ProductID  = oReader.GetInt32("ProductID");
			oFNRequisitionReport.ProductName   = oReader.GetString("ProductName");
			oFNRequisitionReport.LotID  = oReader.GetInt32("LotID");
			oFNRequisitionReport.LotNo  = oReader.GetString("LotNo");
			oFNRequisitionReport.FSCDID  = oReader.GetInt32("FSCDID");
			oFNRequisitionReport.DispoNo  = oReader.GetString("DispoNo");
			oFNRequisitionReport.Construction  = oReader.GetString("Construction");
			oFNRequisitionReport.ColorName  = oReader.GetString("ColorName");
			oFNRequisitionReport.BuyerID  = oReader.GetInt32("BuyerID");
			oFNRequisitionReport.BuyerName  = oReader.GetString("BuyerName");
			oFNRequisitionReport.MachineID  = oReader.GetInt32("MachineID");
			oFNRequisitionReport.MachineName  = oReader.GetString("MachineName");
			oFNRequisitionReport.WorkingUnitID  = oReader.GetInt32("WorkingUnitID");
			oFNRequisitionReport.WUName  = oReader.GetString("WUName");
			oFNRequisitionReport.Qty_Order  = oReader.GetDouble("Qty_Order");
			oFNRequisitionReport.Qty_Requisition  = oReader.GetDouble("Qty_Requisition");
			oFNRequisitionReport.Qty_Consume  = oReader.GetDouble("Qty_Consume");
			oFNRequisitionReport.MUnitID = oReader.GetInt32("MUnitID");
            oFNRequisitionReport.MUName = oReader.GetString("MUName");
			return oFNRequisitionReport;
		}

		private FNRequisitionReport CreateObject(NullHandler oReader)
		{
			FNRequisitionReport oFNRequisitionReport = new FNRequisitionReport();
			oFNRequisitionReport = MapObject(oReader);
			return oFNRequisitionReport;
		}

		private List<FNRequisitionReport> CreateObjects(IDataReader oReader)
		{
			List<FNRequisitionReport> oFNRequisitionReport = new List<FNRequisitionReport>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNRequisitionReport oItem = CreateObject(oHandler);
				oFNRequisitionReport.Add(oItem);
			}
			return oFNRequisitionReport;
		}

		#endregion

		#region Interface implementation
		public List<FNRequisitionReport> Gets (string sSQL, Int64 nUserID)
			{
				List<FNRequisitionReport> oFNRequisitionReports = new List<FNRequisitionReport>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNRequisitionReportDA.Gets(tc, sSQL);
					oFNRequisitionReports = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FNRequisitionReport", e);
					#endregion
				}
				return oFNRequisitionReports;
			}

		#endregion
	}

}
