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
	public class FARuleService : MarshalByRefObject, IFARuleService
	{
		#region Private functions and declaration

		private FARule MapObject(NullHandler oReader)
		{
			FARule oFARule = new FARule();
			oFARule.FARuleID = oReader.GetInt32("FARuleID");
            oFARule.FACodeFull = oReader.GetString("FACodeFull");
            oFARule.FAMethod = (EnumFAMethod)oReader.GetInt32("FAMethod");
            oFARule.FAMethodInt = oReader.GetInt32("FAMethod");
            oFARule.ProductID = oReader.GetInt32("ProductID");
            oFARule.DEPCalculateOn = (EnumDEPCalculateOn)oReader.GetInt32("DEPCalculateOn");
            oFARule.DEPCalculateOnInt = oReader.GetInt32("DEPCalculateOn");
            oFARule.DefaultSalvage = oReader.GetDouble("DefaultSalvage");
			oFARule.UseFullLife = oReader.GetInt32("UseFullLife");
            oFARule.DefaultDepRate = oReader.GetDouble("DefaultDepRate");
		
			oFARule.CurrencyID = oReader.GetInt32("CurrencyID");
			oFARule.MUnitID = oReader.GetInt32("MUnitID");
            oFARule.ProductName = oReader.GetString("ProductName");
            oFARule.ProductCode = oReader.GetString("ProductCode");
            oFARule.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oFARule.BUName = oReader.GetString("BUName");
            oFARule.MUName = oReader.GetString("MUName");
            oFARule.MUSymbol = oReader.GetString("MUSymbol");
            oFARule.CurrencyName = oReader.GetString("CurrencyName");
            oFARule.CurrencySymbol = oReader.GetString("CurrencySymbol");

            oFARule.DefaultCostPrice = oReader.GetDouble("DefaultCostPrice");
            oFARule.CostPriceEffectOn = (EnumFAEffectOn)oReader.GetInt32("CostPriceEffectOn");
            oFARule.DepEffectFormOn = (EnumFADeptEffectFrom)oReader.GetInt32("DepEffectFormOn");
            oFARule.Remarks = oReader.GetString("Remarks");
            oFARule.RegisterApplyOn = (EnumFARegisterOn)oReader.GetInt32("RegisterApplyOn");

			return oFARule;
		}

		private FARule CreateObject(NullHandler oReader)
		{
			FARule oFARule = new FARule();
			oFARule = MapObject(oReader);
			return oFARule;
		}

		private List<FARule> CreateObjects(IDataReader oReader)
		{
			List<FARule> oFARule = new List<FARule>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FARule oItem = CreateObject(oHandler);
				oFARule.Add(oItem);
			}
			return oFARule;
		}

		#endregion

		#region Interface implementation
			public FARule Save(FARule oFARule, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFARule.FARuleID <= 0)
					{
						reader = FARuleDA.InsertUpdate(tc, oFARule, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = FARuleDA.InsertUpdate(tc, oFARule, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFARule = new FARule();
						oFARule = CreateObject(oReader);
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
						oFARule = new FARule();
						oFARule.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFARule;
			}
            public FARule Remove_FACode(FARule oFARule, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader = null;
                    NullHandler oReader = null;
                    Int32 FARegisterID = 0;
                    if (oFARule.ProductID > 0)
                    {
                        reader = FARuleDA.GetFARegister(tc, oFARule);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            FARegisterID = oReader.GetInt32("FARegisterID");
                        }
                        reader.Close();
                        if (FARegisterID==0)
                        {
                            reader = FARuleDA.Remove_FACode(tc, oFARule);
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oFARule = new FARule();
                                oFARule = CreateObject(oReader);
                            }
                            reader.Close();
                        }
                        else
                        {  
                            oFARule.ErrorMessage = "Sorry, FA Register Already Exist.";
                        }
                    }
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                    {
                        tc.HandleError();
                        oFARule = new FARule();
                        oFARule.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFARule;
            }
            public FARule Remove_FARule(FARule oFARule, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader = null;
                    if (oFARule.ProductID > 0)
                    {
                        reader = FARuleDA.Remove_FARule(tc, oFARule);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFARule = new FARule();
                        oFARule = CreateObject(oReader);
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
                        oFARule = new FARule();
                        oFARule.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFARule;
            }

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FARule oFARule = new FARule();
					oFARule.FARuleID = id;
					DBTableReferenceDA.HasReference(tc, "FARule", id);
					FARuleDA.Delete(tc, oFARule, EnumDBOperation.Delete, nUserId);
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

			public FARule Get(int id, Int64 nUserId)
			{
				FARule oFARule = new FARule();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FARuleDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFARule = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FARule", e);
					#endregion
				}
				return oFARule;
			}
            public FARule GetByProduct(int pid, Int64 nUserId)
            {
                FARule oFARule = new FARule();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = FARuleDA.GetByProduct(tc, pid);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFARule = CreateObject(oReader);
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
                    throw new ServiceException("Failed to Get FARule", e);
                    #endregion
                }
                return oFARule;
            }

			public List<FARule> Gets(Int64 nUserID)
			{
				List<FARule> oFARules = new List<FARule>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FARuleDA.Gets(tc);
					oFARules = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FARule oFARule = new FARule();
					oFARule.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFARules;
			}

			public List<FARule> Gets (string sSQL, Int64 nUserID)
			{
				List<FARule> oFARules = new List<FARule>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FARuleDA.Gets(tc, sSQL);
					oFARules = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FARule", e);
					#endregion
				}
				return oFARules;
			}

		#endregion
	}

}
