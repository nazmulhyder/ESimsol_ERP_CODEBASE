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
	public class MerchanidisingReportService : MarshalByRefObject, IMerchanidisingReportService
	{
		#region Private functions and declaration

		private MerchanidisingReport MapObject(NullHandler oReader)
		{
			MerchanidisingReport oMerchanidisingReport = new MerchanidisingReport();
			oMerchanidisingReport.BuyerID = oReader.GetInt32("BuyerID");
			oMerchanidisingReport.BuyerName = oReader.GetString("BuyerName");
			oMerchanidisingReport.NumberOfStyle = oReader.GetInt32("NumberOfStyle");
			oMerchanidisingReport.NumberOfOrderRecap = oReader.GetInt32("NumberOfOrderRecap");
			oMerchanidisingReport.NumberOfCostSheet = oReader.GetInt32("NumberOfCostSheet");
			oMerchanidisingReport.NumberOfTAP = oReader.GetInt32("NumberOfTAP");
			return oMerchanidisingReport;
		}

		private MerchanidisingReport CreateObject(NullHandler oReader)
		{
			MerchanidisingReport oMerchanidisingReport = new MerchanidisingReport();
			oMerchanidisingReport = MapObject(oReader);
			return oMerchanidisingReport;
		}

		private List<MerchanidisingReport> CreateObjects(IDataReader oReader)
		{
			List<MerchanidisingReport> oMerchanidisingReport = new List<MerchanidisingReport>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				MerchanidisingReport oItem = CreateObject(oHandler);
				oMerchanidisingReport.Add(oItem);
			}
			return oMerchanidisingReport;
		}

		#endregion

		#region Interface implementation

        public List<MerchanidisingReport> Gets(string sMainSQL, string sTSSQL, string sORSQL, string sCSSQL, string sTAPSQL, long nUserID)
			{
				List<MerchanidisingReport> oMerchanidisingReports = new List<MerchanidisingReport>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = MerchanidisingReportDA.Gets(tc, sMainSQL,sTSSQL, sORSQL, sCSSQL, sTAPSQL);
					oMerchanidisingReports = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get MerchanidisingReport", e);
					#endregion
				}
				return oMerchanidisingReports;
			}

		#endregion
	}

}
 