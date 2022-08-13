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
    public class FAScheduleService : MarshalByRefObject, IFAScheduleService
    {
        #region FASchedule
        private FASchedule MapObject(NullHandler oReader)
		{
			FASchedule oFASchedule = new FASchedule();
			oFASchedule.FAScheduleID = oReader.GetInt32("FAScheduleID");
            oFASchedule.FARegisterID = oReader.GetInt32("FARegisterID");
            oFASchedule.StartDate = oReader.GetDateTime("StartDate");
            oFASchedule.EndDate = oReader.GetDateTime("EndDate");
            oFASchedule.MonthCount = oReader.GetInt32("MonthCount");
            oFASchedule.OpeningBookValue = oReader.GetDouble("OpeningBookValue");
            oFASchedule.DepreciationMethod = oReader.GetInt32("DepreciationMethod");
            oFASchedule.DepreciationRate = oReader.GetDouble("DepreciationRate");
            oFASchedule.DepreciationValue = oReader.GetDouble("DepreciationValue");
            oFASchedule.AdditionValue = oReader.GetDouble("AdditionValue");
            oFASchedule.AccumulatedDepValue = oReader.GetDouble("AccumulatedValue");
            oFASchedule.ClosingBookValue = oReader.GetDouble("ClosingBookValue");
			return oFASchedule;
		}
		private FASchedule CreateObject(NullHandler oReader)
		{
			FASchedule oFASchedule = new FASchedule();
			oFASchedule = MapObject(oReader);
			return oFASchedule;
		}
		private List<FASchedule> CreateObjects(IDataReader oReader)
		{
			List<FASchedule> oFASchedule = new List<FASchedule>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FASchedule oItem = CreateObject(oHandler);
				oFASchedule.Add(oItem);
			}
			return oFASchedule;
		}
        #endregion

        #region FAScheduleReport
        private FAScheduleReport MapReportObject(NullHandler oReader)
        {
            FAScheduleReport oFAScheduleReport = new FAScheduleReport();
            oFAScheduleReport.FARegisterID = oReader.GetInt32("FARegisterID");
            oFAScheduleReport.ProductID = oReader.GetInt32("ProductID");
            oFAScheduleReport.ProductName = oReader.GetString("ProductName");
            oFAScheduleReport.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oFAScheduleReport.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oFAScheduleReport.Amount_Cost = oReader.GetDouble("Amount_Cost");
            oFAScheduleReport.SalvageValue = oReader.GetDouble("SalvageValue");
            oFAScheduleReport.DEPPercentage = oReader.GetDouble("DEPPercentage");
            oFAScheduleReport.OpenningCost = oReader.GetDouble("OpenningCost");
            oFAScheduleReport.DepreciationperYear = oReader.GetDouble("DepreciationperYear");
            oFAScheduleReport.TotalAccumulatedCost = oReader.GetDouble("TotalAccumulatedCost");
            oFAScheduleReport.ClosingCost = oReader.GetDouble("ClosingCost");
            oFAScheduleReport.UsefulLifetime = oReader.GetDouble("UsefulLifetime");
            oFAScheduleReport.PurchaseDate = oReader.GetDateTime("PurchaseDate");
            return oFAScheduleReport;
        }
        private FAScheduleReport CreateReportObject(NullHandler oReader)
        {
            FAScheduleReport oFAScheduleReport = new FAScheduleReport();
            oFAScheduleReport = MapReportObject(oReader);
            return oFAScheduleReport;
        }
        private List<FAScheduleReport> CreateReportObjects(IDataReader oReader)
        {
            List<FAScheduleReport> oFAScheduleReports = new List<FAScheduleReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FAScheduleReport oItem = CreateReportObject(oHandler);
                oFAScheduleReports.Add(oItem);
            }
            return oFAScheduleReports;
        }
        #endregion

        #region Interface implementation
        public List<FASchedule> Gets(int nFARID, Int64 nUserID)
        {
            List<FASchedule> oFASchedules = new List<FASchedule>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                //IDataReader reader = null;
                //reader = FAScheduleDA.Gets(tc, nFARID, nUserID);

                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oFASchedules = CreateObjects(reader);
                //}
                //reader.Close();
                //tc.End();

                IDataReader reader = null;
                reader = FAScheduleDA.Gets(tc, nFARID, nUserID);
                oFASchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FASchedule oFASchedule = new FASchedule();
                oFASchedule.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFASchedules;
        }
        //public List<FASchedule> GetsLogScheduleBy(int nFARLogID, Int64 nUserID)
        //{
        //    List<FASchedule> oFASchedules = new List<FASchedule>();
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin();
        //        IDataReader reader = null;
        //        reader = FAScheduleDA.GetsLogScheduleBy(tc, nFARLogID, nUserID);

        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oFASchedules = CreateObjects(reader);
        //        }
        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        FASchedule oFASchedule = new FASchedule();
        //        oFASchedule.ErrorMessage = e.Message.Split('!')[0];
        //        #endregion
        //    }
        //    return oFASchedules;
        //}
        public List<FASchedule> GetsLogScheduleBy(int nFARLogID, Int64 nUserID)
        {
            List<FASchedule> oFASchedules = new List<FASchedule>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FAScheduleDA.GetsLogScheduleBy(tc, nFARLogID, nUserID);
                oFASchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FASchedule oFASchedule = new FASchedule();
                oFASchedule.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFASchedules;
        }
		public List<FASchedule> Gets(Int64 nUserID)
		{
			List<FASchedule> oFASchedules = new List<FASchedule>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FAScheduleDA.Gets(tc);
				oFASchedules = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				FASchedule oFASchedule = new FASchedule();
				oFASchedule.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oFASchedules;
		}
		public List<FASchedule> Gets (string sSQL, Int64 nUserID)
		{
			List<FASchedule> oFASchedules = new List<FASchedule>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FAScheduleDA.Gets(tc, sSQL);
				oFASchedules = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get FASchedule", e);
				#endregion
			}
			return oFASchedules;
		}
        public List<FASchedule> SaveFASchedules(int nFARID, Int64 nUserID)
        {
            List<FASchedule> oFASchedules = new List<FASchedule>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FAScheduleDA.SaveFASchedules(tc, nFARID, nUserID);
                oFASchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FASchedule oFASchedule = new FASchedule();
                oFASchedule.ErrorMessage = e.Message.Split('!')[0];
                oFASchedules.Add(oFASchedule);
                #endregion
            }
            return oFASchedules;
        }
		#endregion
    }
}
