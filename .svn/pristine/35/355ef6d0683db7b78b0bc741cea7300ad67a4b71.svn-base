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
	public class FARegisterSummeryService : MarshalByRefObject, IFARegisterSummeryService
	{
		#region Private functions and declaration

		private FARegisterSummery MapObject(NullHandler oReader)
		{
			FARegisterSummery oFARegisterSummery = new FARegisterSummery();
			oFARegisterSummery.ProductID = oReader.GetInt32("ProductID");
			oFARegisterSummery.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oFARegisterSummery.ReportViewLayout = oReader.GetInt32("ReportViewLayout");
			oFARegisterSummery.ProductName = oReader.GetString("ProductName");
			oFARegisterSummery.CategoryName = oReader.GetString("CategoryName");
			oFARegisterSummery.DeprRate = oReader.GetDouble("DeprRate");
			oFARegisterSummery.SubGroupHeadID = oReader.GetInt32("SubGroupHeadID");
			oFARegisterSummery.AssetOpeningAmount = oReader.GetDouble("AssetOpeningAmount");
			oFARegisterSummery.AssetAdditionAmount = oReader.GetDouble("AssetAdditionAmount");
			oFARegisterSummery.TotalAssetAmount = oReader.GetDouble("TotalAssetAmount");
			oFARegisterSummery.DeprOpeningAmount = oReader.GetDouble("DeprOpeningAmount");
			oFARegisterSummery.DeprAdditionAmount = oReader.GetDouble("DeprAdditionAmount");
			oFARegisterSummery.TotalDeprAmount = oReader.GetDouble("TotalDeprAmount");
			oFARegisterSummery.EndingAssetAmount = oReader.GetDouble("EndingAssetAmount");
			return oFARegisterSummery;
		}

		private FARegisterSummery CreateObject(NullHandler oReader)
		{
			FARegisterSummery oFARegisterSummery = new FARegisterSummery();
			oFARegisterSummery = MapObject(oReader);
			return oFARegisterSummery;
		}

		private List<FARegisterSummery> CreateObjects(IDataReader oReader)
		{
			List<FARegisterSummery> oFARegisterSummery = new List<FARegisterSummery>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FARegisterSummery oItem = CreateObject(oHandler);
				oFARegisterSummery.Add(oItem);
			}
			return oFARegisterSummery;
		}

		#endregion

		#region Interface implementation


        public List<FARegisterSummery> Gets(string BUIDs, DateTime StartDate, DateTime EndDate, int ProductCategoryID, int ReportLayout, Int64 nUserID)
			{
				List<FARegisterSummery> oFARegisterSummerys = new List<FARegisterSummery>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FARegisterSummeryDA.Gets(BUIDs,StartDate, EndDate, ProductCategoryID, ReportLayout, tc);
					oFARegisterSummerys = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FARegisterSummery oFARegisterSummery = new FARegisterSummery();
					oFARegisterSummery.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFARegisterSummerys;
			}

			public List<FARegisterSummery> Gets (string sSQL, Int64 nUserID)
			{
				List<FARegisterSummery> oFARegisterSummerys = new List<FARegisterSummery>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FARegisterSummeryDA.Gets(tc, sSQL);
					oFARegisterSummerys = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FARegisterSummery", e);
					#endregion
				}
				return oFARegisterSummerys;
			}

		#endregion
	}

}
