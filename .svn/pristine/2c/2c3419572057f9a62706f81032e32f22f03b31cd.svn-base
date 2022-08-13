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
	public class BUWiseProductCategoryService : MarshalByRefObject, IBUWiseProductCategoryService
	{
		#region Private functions and declaration

		private BUWiseProductCategory MapObject(NullHandler oReader)
		{
			BUWiseProductCategory oBUWiseProductCategory = new BUWiseProductCategory();
			oBUWiseProductCategory.BUWiseProductCategoryID = oReader.GetInt32("BUWiseProductCategoryID");
			oBUWiseProductCategory.BUID = oReader.GetInt32("BUID");
			oBUWiseProductCategory.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
			oBUWiseProductCategory.BUName = oReader.GetString("BUName");
			oBUWiseProductCategory.ProductCategoryName = oReader.GetString("ProductCategoryName");
			return oBUWiseProductCategory;
		}

		private BUWiseProductCategory CreateObject(NullHandler oReader)
		{
			BUWiseProductCategory oBUWiseProductCategory = new BUWiseProductCategory();
			oBUWiseProductCategory = MapObject(oReader);
			return oBUWiseProductCategory;
		}

		private List<BUWiseProductCategory> CreateObjects(IDataReader oReader)
		{
			List<BUWiseProductCategory> oBUWiseProductCategory = new List<BUWiseProductCategory>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				BUWiseProductCategory oItem = CreateObject(oHandler);
				oBUWiseProductCategory.Add(oItem);
			}
			return oBUWiseProductCategory;
		}

		#endregion

		#region Interface implementation
			public BUWiseProductCategory Save(BUWiseProductCategory oBUWiseProductCategory, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oBUWiseProductCategory.BUWiseProductCategoryID <= 0)
					{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BUWiseProductCategory", EnumRoleOperationType.Add);
						reader = BUWiseProductCategoryDA.InsertUpdate(tc, oBUWiseProductCategory, EnumDBOperation.Insert, nUserID);
					}
					else{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BUWiseProductCategory", EnumRoleOperationType.Edit);
						reader = BUWiseProductCategoryDA.InsertUpdate(tc, oBUWiseProductCategory, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oBUWiseProductCategory = new BUWiseProductCategory();
						oBUWiseProductCategory = CreateObject(oReader);
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
						oBUWiseProductCategory = new BUWiseProductCategory();
						oBUWiseProductCategory.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oBUWiseProductCategory;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					BUWiseProductCategory oBUWiseProductCategory = new BUWiseProductCategory();
					oBUWiseProductCategory.BUWiseProductCategoryID = id;
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "BUWiseProductCategory", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "BUWiseProductCategory", id);
					BUWiseProductCategoryDA.Delete(tc, oBUWiseProductCategory, EnumDBOperation.Delete, nUserId);
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

			public BUWiseProductCategory Get(int id, Int64 nUserId)
			{
				BUWiseProductCategory oBUWiseProductCategory = new BUWiseProductCategory();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = BUWiseProductCategoryDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oBUWiseProductCategory = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get BUWiseProductCategory", e);
					#endregion
				}
				return oBUWiseProductCategory;
			}

			public List<BUWiseProductCategory> Gets(int nID, Int64 nUserID)
			{
				List<BUWiseProductCategory> oBUWiseProductCategorys = new List<BUWiseProductCategory>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = BUWiseProductCategoryDA.Gets(tc, nID);
					oBUWiseProductCategorys = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					BUWiseProductCategory oBUWiseProductCategory = new BUWiseProductCategory();
					oBUWiseProductCategory.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oBUWiseProductCategorys;
			}

			public List<BUWiseProductCategory> Gets (string sSQL, Int64 nUserID)
			{
				List<BUWiseProductCategory> oBUWiseProductCategorys = new List<BUWiseProductCategory>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = BUWiseProductCategoryDA.Gets(tc, sSQL);
					oBUWiseProductCategorys = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get BUWiseProductCategory", e);
					#endregion
				}
				return oBUWiseProductCategorys;
			}

		#endregion
	}

}
