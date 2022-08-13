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
	public class FARegisterService : MarshalByRefObject, IFARegisterService
	{
		#region Private functions and declaration

		private FARegister MapObject(NullHandler oReader)
		{
			FARegister oFARegister = new FARegister();
			oFARegister.FARegisterID = oReader.GetInt32("FARegisterID");
			oFARegister.ProductID = oReader.GetInt32("ProductID");
			oFARegister.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oFARegister.FAMethodInt = oReader.GetInt32("FAMethod");
            oFARegister.FAMethod = (EnumFAMethod)oReader.GetInt32("FAMethod");
            oFARegister.DEPCalculateOn = (EnumDEPCalculateOn)oReader.GetInt32("DEPCalculateOn");
            oFARegister.DEPCalculateOnInt = oReader.GetInt32("DEPCalculateOn");
            oFARegister.ActualSalvage = oReader.GetDouble("ActualSalvage");
            oFARegister.FARegisterLogID = oReader.GetInt32("FARegisterLogID");
            oFARegister.BUID = oReader.GetInt32("BUID");  
	        oFARegister.LocationID = oReader.GetInt32("LocationID");
            oFARegister.LocationName = oReader.GetString("LocationName");
            oFARegister.VersionNo = oReader.GetInt32("VersionNo");

            
            oFARegister.UseFullLife = oReader.GetInt32("UseFullLife");
            oFARegister.ActualDepRate = oReader.GetDouble("ActualDepRate");
            oFARegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oFARegister.MUnitID = oReader.GetInt32("MUnitID");
            oFARegister.Qty = oReader.GetInt32("Qty");
            oFARegister.ActualCostPrice = oReader.GetDouble("ActualCostPrice");
            oFARegister.CurrencyName = oReader.GetString("CurrencyName");
            oFARegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oFARegister.MUName = oReader.GetString("MUName");
            oFARegister.MUSymbol = oReader.GetString("MUSymbol");
       
			oFARegister.FACodeFull = oReader.GetString("FACodeFull");
			oFARegister.BrandName = oReader.GetString("BrandName");
			oFARegister.Model = oReader.GetString("Model");
			oFARegister.Note = oReader.GetString("Note");

			oFARegister.PurchaseDate = oReader.GetDateTime("PurchaseDate");
            oFARegister.DepStartDate = oReader.GetDateTime("DepStartDate");
            oFARegister.WarrantyDate = oReader.GetDateTime("WarrantyDate");

            oFARegister.ScheduleStartDate = oReader.GetDateTime("ScheduleStartDate");
            oFARegister.ScheduleEndDate = oReader.GetDateTime("ScheduleEndDate");
            oFARegister.WarrantyDate_Exp = oReader.GetDateTime("WarrantyDate_Exp");
			
			oFARegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oFARegister.Qty = oReader.GetDouble("Qty");
			oFARegister.MUnitID = oReader.GetInt32("MUnitID");
            oFARegister.ProductCode = oReader.GetString("ProductCode");
            oFARegister.ProductName = oReader.GetString("ProductName");
            oFARegister.ProductCategoryName = oReader.GetString("ProductCategoryName");

            oFARegister.BrandName = oReader.GetString("BrandName");
            oFARegister.Manufacturer = oReader.GetString("Manufacturer");
            oFARegister.ManufacturerYear = oReader.GetString("ManufacturerYear");
            oFARegister.ProductSLNo = oReader.GetString("ProductSLNo");
            oFARegister.CountryOfOrigin = oReader.GetString("CountryOfOrigin");

            oFARegister.BasicFunction = oReader.GetString("BasicFunction");
            oFARegister.WarrantyPeriod = oReader.GetInt32("WarrantyPeriod");
            oFARegister.BasicFunction = oReader.GetString("BasicFunction");
            oFARegister.PowerConumption = oReader.GetString("PowerConumption");
            oFARegister.Capacity = oReader.GetString("Capacity");

            oFARegister.TechnicalSpec = oReader.GetString("TechnicalSpec");
            oFARegister.PerformanceSpec = oReader.GetString("PerformanceSpec");
            oFARegister.PortOfShipment = oReader.GetString("PortOfShipment");
            oFARegister.LCNoWithDate = oReader.GetString("LCNoWithDate");
            oFARegister.HSCode = oReader.GetString("HSCode");
            oFARegister.AssestNote = oReader.GetString("AssestNote");
            oFARegister.BUName = oReader.GetString("BUName");

            oFARegister.SupplierName = oReader.GetString("SupplierName");
            oFARegister.SupplierAddress = oReader.GetString("SupplierAddress");
            oFARegister.SupplierEmail = oReader.GetString("SupplierEmail");
            oFARegister.SupplierPhone = oReader.GetString("SupplierPhone");
            oFARegister.SupplierFax = oReader.GetString("SupplierFax");
            oFARegister.SupplierCPName = oReader.GetString("SupplierCPName");
            oFARegister.SupplierCPPhone = oReader.GetString("SupplierCPPhone");
            oFARegister.SupplierCPEmail = oReader.GetString("SupplierCPEmail");
            oFARegister.SupplierNote = oReader.GetString("SupplierNote");

            oFARegister.AgentName = oReader.GetString("AgentName");
            oFARegister.AgentAddress = oReader.GetString("AgentAddress");
            oFARegister.AgentEmail = oReader.GetString("AgentEmail");
            oFARegister.AgentPhone = oReader.GetString("AgentPhone");
            oFARegister.AgentFax = oReader.GetString("AgentFax");
            oFARegister.AgentCPName = oReader.GetString("AgentCPName");

            oFARegister.AgentCPPhone = oReader.GetString("AgentCPPhone");
            oFARegister.AgentCPEmail = oReader.GetString("AgentCPEmail");
            oFARegister.AgentWorkStation = oReader.GetString("AgentWorkStation");
            oFARegister.AgentNote = oReader.GetString("AgentNote");
            oFARegister.FAStatus = (EnumFAStatus)oReader.GetInt32("FAStatus");
			return oFARegister;
		}

		private FARegister CreateObject(NullHandler oReader)
		{
			FARegister oFARegister = new FARegister();
			oFARegister = MapObject(oReader);
			return oFARegister;
		}

		private List<FARegister> CreateObjects(IDataReader oReader)
		{
			List<FARegister> oFARegister = new List<FARegister>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FARegister oItem = CreateObject(oHandler);
				oFARegister.Add(oItem);
			}
			return oFARegister;
		}

		#endregion

		#region Interface implementation
			public FARegister Save(FARegister oFARegister, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFARegister.FARegisterID <= 0)
					{
						reader = FARegisterDA.InsertUpdate(tc, oFARegister, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = FARegisterDA.InsertUpdate(tc, oFARegister, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFARegister = new FARegister();
						oFARegister = CreateObject(oReader);
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
						oFARegister = new FARegister();
						oFARegister.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFARegister;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FARegister oFARegister = new FARegister();
					oFARegister.FARegisterID = id;
					DBTableReferenceDA.HasReference(tc, "FARegister", id);
					FARegisterDA.Delete(tc, oFARegister, EnumDBOperation.Delete, nUserId);
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
            public string GetFACode(FARegister oFARegister, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
                    tc = TransactionContext.Begin();
                    IDataReader reader = FARegisterDA.GetFACode(tc, oFARegister);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFARegister = CreateObject(oReader);
                    }
                    reader.Close();
                    tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('!')[0];
					#endregion
				}
                return oFARegister.FACodeFull;
			}

        

			public FARegister Get(int id, Int64 nUserId)
			{
				FARegister oFARegister = new FARegister();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FARegisterDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFARegister = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FARegister", e);
					#endregion
				}
				return oFARegister;
			}
            public FARegister GetLogByLogID(int id, Int64 nUserId)
            {
                FARegister oFARegister = new FARegister();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = FARegisterDA.GetLogByLogID(tc, id);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFARegister = CreateObject(oReader);
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
                    throw new ServiceException("Failed to Get FARegister", e);
                    #endregion
                }
                return oFARegister;
            }

			public List<FARegister> Gets(Int64 nUserID)
			{
				List<FARegister> oFARegisters = new List<FARegister>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FARegisterDA.Gets(tc);
					oFARegisters = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FARegister oFARegister = new FARegister();
					oFARegister.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFARegisters;
			}

			public List<FARegister> Gets (string sSQL, Int64 nUserID)
			{
				List<FARegister> oFARegisters = new List<FARegister>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FARegisterDA.Gets(tc, sSQL);
					oFARegisters = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FARegister", e);
					#endregion
				}
				return oFARegisters;
			}

            public List<FARegister> FA_Process(FARegister oFARegister,Int64 nUserID)
            {
                List<FARegister> oFARegisters = new List<FARegister>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = FARegisterDA.FA_Process(tc, oFARegister, nUserID);
                    oFARegisters = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                        oFARegister = new FARegister();
                    oFARegister.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oFARegisters;
            }

            public FARegister FARProcess(FARegister oFARegister, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader=null;
                    if (oFARegister.FARegisterID > 0)
                    {
                        reader = FARegisterDA.FARProcess(tc, oFARegister, EnumDBOperation.Insert, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFARegister = new FARegister();
                        oFARegister = CreateObject(oReader);
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
                        oFARegister = new FARegister();
                        oFARegister.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFARegister;
            }
            public FARegister RequestForRevise(FARegister oFARegister, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = FARegisterDA.InsertUpdate(tc, oFARegister, EnumDBOperation.Request, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFARegister = new FARegister();
                        oFARegister = CreateObject(oReader);
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
                        oFARegister = new FARegister();
                        oFARegister.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFARegister;
            }
            public FARegister Revise(FARegister oFARegister, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = FARegisterDA.InsertUpdate(tc, oFARegister, EnumDBOperation.Revise, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFARegister = new FARegister();
                        oFARegister = CreateObject(oReader);
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
                        oFARegister = new FARegister();
                        oFARegister.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFARegister;
            }
            public FARegister RequestForApprove(FARegister oFARegister, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = FARegisterDA.InsertUpdate(tc, oFARegister, EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFARegister = new FARegister();
                        oFARegister = CreateObject(oReader);
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
                        oFARegister = new FARegister();
                        oFARegister.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFARegister;
            }
            public FARegister SaveLog(FARegister oFARegister, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    if (oFARegister.FARegisterID <= 0)
                    {
                        reader = FARegisterDA.InsertUpdate(tc, oFARegister, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FARegisterDA.InsertUpdate(tc, oFARegister, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFARegister = new FARegister();
                        oFARegister = CreateObject(oReader);
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
                        oFARegister = new FARegister();
                        oFARegister.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFARegister;
            }
            public List<FARegister> FAGRNProcess(List<GRNDetail> oGRNDetails, Int64 nUserID)
            {
                List<FARegister> oFARegisters = new List<FARegister>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    foreach(GRNDetail oItem in oGRNDetails)
                    {
                        List<FARegister> oTempFARegisters = new List<FARegister>();
                        IDataReader reader = null;
                        reader = FARegisterDA.FAGRNProcess(tc, oItem, nUserID);
                        oTempFARegisters = CreateObjects(reader);
                        oFARegisters.AddRange(oTempFARegisters);
                        reader.Close();
                    }
                    foreach (FARegister oFARItem in oFARegisters)
                    {
                        IDataReader readerFAS = null;
                        readerFAS = FAScheduleDA.SaveFASchedules(tc, oFARItem.FARegisterID, nUserID);
                        readerFAS.Close();
                    }
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                    {
                        tc.HandleError();
                        FARegister oFARegister = new FARegister();
                        oFARegister.ErrorMessage = e.Message.Split('!')[0];
                        oFARegisters.Add(oFARegister);
                    }
                    #endregion
                }
                return oFARegisters;
            }

		#endregion
	}

}
