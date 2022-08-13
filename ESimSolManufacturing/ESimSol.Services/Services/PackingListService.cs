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
	public class PackingListService : MarshalByRefObject, IPackingListService
	{
		#region Private functions and declaration

		private PackingList MapObject(NullHandler oReader)
		{
			 PackingList oPackingList = new PackingList();
			oPackingList.PackingListID = oReader.GetInt32("PackingListID");
			oPackingList.CIDID = oReader.GetInt32("CIDID");
			oPackingList.UnitInPack = oReader.GetDouble("UnitInPack");
			oPackingList.PackInCarton = oReader.GetDouble("PackInCarton");
			oPackingList.QtyInCarton = oReader.GetDouble("QtyInCarton");
			oPackingList.CartonQty = oReader.GetDouble("CartonQty");
			oPackingList.CartonNo = oReader.GetString("CartonNo");
			oPackingList.GrossWeight = oReader.GetDouble("GrossWeight");
			oPackingList.NetWeight = oReader.GetDouble("NetWeight");
			oPackingList.CTNMeasurement = oReader.GetString("CTNMeasurement");
			oPackingList.TotalVolume = oReader.GetDouble("TotalVolume");
			oPackingList.Note = oReader.GetString("Note");
			oPackingList.TotalGrossWeight = oReader.GetDouble("TotalGrossWeight");
			oPackingList.TotalNetWeight = oReader.GetDouble("TotalNetWeight");
			oPackingList.StyleNo = oReader.GetString("StyleNo");
			oPackingList.RecapNo = oReader.GetString("RecapNo");
			oPackingList.InvoiceNo = oReader.GetString("InvoiceNo");
			oPackingList.BuyerName = oReader.GetString("BuyerName");
			oPackingList.FactoryName = oReader.GetString("FactoryName");
			oPackingList.ProductName = oReader.GetString("ProductName");
			oPackingList.Fabrication = oReader.GetString("Fabrication");
			oPackingList.TotalQty = oReader.GetDouble("TotalQty");
            oPackingList.StyleWithRecapNo = oReader.GetString("StyleWithRecapNo");
            oPackingList.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oPackingList.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oPackingList.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oPackingList.ProductionFactoryID = oReader.GetInt32("ProductionFactoryID");
            
			return oPackingList;
		}

		private PackingList CreateObject(NullHandler oReader)
		{
			PackingList oPackingList = new PackingList();
			oPackingList = MapObject(oReader);
			return oPackingList;
		}

		private List<PackingList> CreateObjects(IDataReader oReader)
		{
			List<PackingList> oPackingList = new List<PackingList>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PackingList oItem = CreateObject(oHandler);
				oPackingList.Add(oItem);
			}
			return oPackingList;
		}

		#endregion

		#region Interface implementation
			public PackingList Save(PackingList oPackingList, Int64 nUserID)
			{
				TransactionContext tc = null;
                List<PackingListDetail> oPackingListDetails = new List<PackingListDetail>();
                oPackingListDetails = oPackingList.PackingListDetails;
                string sPackingListDetailIDs = "";
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oPackingList.PackingListID <= 0)
					{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PackingList, EnumRoleOperationType.Add);
						reader = PackingListDA.InsertUpdate(tc, oPackingList, EnumDBOperation.Insert, nUserID);
					}
					else{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PackingList, EnumRoleOperationType.Edit);
						reader = PackingListDA.InsertUpdate(tc, oPackingList, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oPackingList = new PackingList();
						oPackingList = CreateObject(oReader);
					}
					reader.Close();
                    #region Buyer Yarn Detail Part
                    if (oPackingListDetails != null)
                    {
                        foreach (PackingListDetail oItem in oPackingListDetails)
                        {
                            IDataReader readerdetail;
                            oItem.PackingListID = oPackingList.PackingListID;
                            if (oItem.PackingListDetailID <= 0)
                            {
                                readerdetail = PackingListDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = PackingListDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sPackingListDetailIDs = sPackingListDetailIDs + oReaderDetail.GetString("PackingListDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sPackingListDetailIDs.Length > 0)
                        {
                            sPackingListDetailIDs = sPackingListDetailIDs.Remove(sPackingListDetailIDs.Length - 1, 1);
                        }
                        PackingListDetail oPackingListDetail = new PackingListDetail();
                        oPackingListDetail.PackingListID = oPackingList.PackingListID;
                        PackingListDetailDA.Delete(tc, oPackingListDetail, EnumDBOperation.Delete, nUserID, sPackingListDetailIDs);
                    }

                    #endregion
                    reader = PackingListDA.Get(tc, oPackingList.PackingListID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPackingList = new PackingList();
                        oPackingList = CreateObject(oReader);
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
						oPackingList = new PackingList();
						oPackingList.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oPackingList;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					PackingList oPackingList = new PackingList();
					oPackingList.PackingListID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.PackingList, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "PackingList", id);
					PackingListDA.Delete(tc, oPackingList, EnumDBOperation.Delete, nUserId);
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

			public PackingList Get(int id, Int64 nUserId)
			{
				PackingList oPackingList = new PackingList();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PackingListDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPackingList = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PackingList", e);
					#endregion
				}
				return oPackingList;
			}

			public List<PackingList> Gets(Int64 nUserID)
			{
				List<PackingList> oPackingLists = new List<PackingList>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PackingListDA.Gets(tc);
					oPackingLists = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PackingList oPackingList = new PackingList();
					oPackingList.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPackingLists;
			}

			public List<PackingList> Gets (string sSQL, Int64 nUserID)
			{
				List<PackingList> oPackingLists = new List<PackingList>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PackingListDA.Gets(tc, sSQL);
					oPackingLists = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PackingList", e);
					#endregion
				}
				return oPackingLists;
			}

		#endregion
	}

}
