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
	public class ProductSpecHeadService : MarshalByRefObject, IProductSpecHeadService
	{
		#region Private functions and declaration

		private ProductSpecHead MapObject(NullHandler oReader)
		{
			ProductSpecHead oProductSpecHead = new ProductSpecHead();
			oProductSpecHead.ProductSpecHeadID = oReader.GetInt32("ProductSpecHeadID");
			oProductSpecHead.ProductID = oReader.GetInt32("ProductID");
			oProductSpecHead.SpecHeadID = oReader.GetInt32("SpecHeadID");
			oProductSpecHead.Sequence = oReader.GetInt32("Sequence");
            oProductSpecHead.ProductCode = oReader.GetString("ProductCode");
            oProductSpecHead.ProductName = oReader.GetString("ProductName");
            oProductSpecHead.SpecName = oReader.GetString("SpecName");
			return oProductSpecHead;
		}

		private ProductSpecHead CreateObject(NullHandler oReader)
		{
			ProductSpecHead oProductSpecHead = new ProductSpecHead();
			oProductSpecHead = MapObject(oReader);
			return oProductSpecHead;
		}

		private List<ProductSpecHead> CreateObjects(IDataReader oReader)
		{
			List<ProductSpecHead> oProductSpecHead = new List<ProductSpecHead>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ProductSpecHead oItem = CreateObject(oHandler);
				oProductSpecHead.Add(oItem);
			}
			return oProductSpecHead;
		}

		#endregion

		#region Interface implementation
			public ProductSpecHead Save(ProductSpecHead oProductSpecHead, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oProductSpecHead.ProductSpecHeadID <= 0)
					{
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "ProductSpecHead", EnumRoleOperationType.Add);
                       
						reader = ProductSpecHeadDA.InsertUpdate(tc, oProductSpecHead, EnumDBOperation.Insert, nUserID);
					}
					else{
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "ProductSpecHead", EnumRoleOperationType.Edit);
						reader = ProductSpecHeadDA.InsertUpdate(tc, oProductSpecHead, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oProductSpecHead = new ProductSpecHead();
						oProductSpecHead = CreateObject(oReader);
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
						oProductSpecHead = new ProductSpecHead();
						oProductSpecHead.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oProductSpecHead;
			}

			public string Delete(int id, int productID, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					ProductSpecHead oProductSpecHead = new ProductSpecHead();
					oProductSpecHead.ProductSpecHeadID = id;
                    oProductSpecHead.ProductID = productID;
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "ProductSpecHead", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "ProductSpecHead", id);
					ProductSpecHeadDA.Delete(tc, oProductSpecHead, EnumDBOperation.Delete, nUserId);
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

			public ProductSpecHead Get(int id, Int64 nUserId)
			{
				ProductSpecHead oProductSpecHead = new ProductSpecHead();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ProductSpecHeadDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oProductSpecHead = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ProductSpecHead", e);
					#endregion
				}
				return oProductSpecHead;
			}

			public List<ProductSpecHead> Gets(Int64 nUserID)
			{
				List<ProductSpecHead> oProductSpecHeads = new List<ProductSpecHead>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ProductSpecHeadDA.Gets(tc);
					oProductSpecHeads = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					ProductSpecHead oProductSpecHead = new ProductSpecHead();
					oProductSpecHead.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oProductSpecHeads;
			}

			public List<ProductSpecHead> Gets (string sSQL, Int64 nUserID)
			{
				List<ProductSpecHead> oProductSpecHeads = new List<ProductSpecHead>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ProductSpecHeadDA.Gets(tc, sSQL);
					oProductSpecHeads = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ProductSpecHead", e);
					#endregion
				}
				return oProductSpecHeads;
			}

            public ProductSpecHead UpDown(ProductSpecHead oProductSpecHead, Int64 nUserID)
            {
                List<ProductSpecHead> oProductSpecHeads = new List<ProductSpecHead>();
                TransactionContext tc = null;
                IDataReader reader;
                try
                {
                    tc = TransactionContext.Begin(true);
                    reader = ProductSpecHeadDA.UpDown(tc, oProductSpecHead, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProductSpecHead = new ProductSpecHead();
                        oProductSpecHead = CreateObject(oReader);
                        //oProductSpecHeads = CreateObjects(reader);
                    }
                    reader.Close();

                    tc.End();
                }
                catch (Exception ex)
                {
                    #region Handle Exception
                    tc.HandleError();
                    oProductSpecHead = new ProductSpecHead();
                    oProductSpecHead.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                    #endregion

                }
                return oProductSpecHead;
                //return oProductSpecHeads;
            }

		#endregion
	}

}
