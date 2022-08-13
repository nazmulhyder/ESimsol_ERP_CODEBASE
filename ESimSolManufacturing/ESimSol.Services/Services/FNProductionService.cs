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
using System.Linq;


namespace ESimSol.Services.Services
{
	public class FNProductionService : MarshalByRefObject, IFNProductionService
	{
		#region Private functions and declaration

        public static FNProduction MapObject(NullHandler oReader)
		{
			FNProduction oFNProduction = new FNProduction();
			oFNProduction.FNProductionID = oReader.GetInt32("FNProductionID");
			oFNProduction.FNMachineID = oReader.GetInt32("FNMachineID");
			oFNProduction.FNTPID = oReader.GetInt32("FNTPID");
			oFNProduction.IssueDate = oReader.GetDateTime("IssueDate");
			oFNProduction.StartDateTime = oReader.GetDateTime("StartDateTime");
			oFNProduction.EndDateTime = oReader.GetDateTime("EndDateTime");
			oFNProduction.FNMachineName = oReader.GetString("FNMachineName");
            oFNProduction.NoOfBatchInOrder = oReader.GetString("NoOfBatchInOrder");
			oFNProduction.FNTreatment = (EnumFNTreatment) oReader.GetInt32("FNTreatment");
            oFNProduction.FNProcess = oReader.GetString("FNProcess");
            oFNProduction.IsDelatable =oReader.GetBoolean("IsDelatable");
            oFNProduction.FNDyeingType = (EnumFNDyeingType)oReader.GetInt32("FNDyeingType");
            
			return oFNProduction;
		}

        public static FNProduction CreateObject(NullHandler oReader)
		{
			FNProduction oFNProduction = new FNProduction();
			oFNProduction = MapObject(oReader);
			return oFNProduction;
		}

        public static List<FNProduction> CreateObjects(IDataReader oReader)
		{
			List<FNProduction> oFNProduction = new List<FNProduction>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNProduction oItem = CreateObject(oHandler);
				oFNProduction.Add(oItem);
			}
			return oFNProduction;
		}

		#endregion

		#region Interface implementation
        public FNProduction Save(FNProduction oFNProduction, Int64 nUserID)
			{
                List<FNProductionBatch> oFNProductionBatchs = new List<FNProductionBatch>();
                oFNProductionBatchs = oFNProduction.FNProductionBatchs;
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    if (oFNProduction.FNProductionID <= 0)
                    {
                        reader = FNProductionDA.InsertUpdate(tc, oFNProduction, (int)EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FNProductionDA.InsertUpdate(tc, oFNProduction, (int)EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNProduction = new FNProduction();
                        oFNProduction = CreateObject(oReader);
                    }
                    reader.Close();

                    #region FN Production Batch
                    foreach (FNProductionBatch oFNPB in oFNProductionBatchs) 
                    {
                        oFNPB.FNProductionID = oFNProduction.FNProductionID;
                        if (oFNPB.FNPBatchID <= 0)
                        {
                            reader = FNProductionBatchDA.InsertUpdate(tc, oFNPB, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = FNProductionBatchDA.InsertUpdate(tc, oFNPB, EnumDBOperation.Update, nUserID);
                        }
                        //oReader = new NullHandler(reader);
                        //if (reader.Read())
                        //{
                        //    oFNProductionBatch = new FNProductionBatch();
                        //    oFNProductionBatch = CreateObject(oReader);
                        //}
                        reader.Close();
                    }
                    #endregion


                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();

                    oFNProduction = new FNProduction();
                    oFNProduction.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oFNProduction;
			}

        public FNProduction Run(FNProduction oFNProduction, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNProductionDA.Run(tc, oFNProduction);
                IDataReader reader = FNProductionDA.Get(tc, oFNProduction.FNProductionID);               
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNProduction = new FNProduction();
                    oFNProduction = CreateObject(oReader);

                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFNProduction = new FNProduction();
                oFNProduction.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFNProduction;
        }
        public FNProduction RunOut(FNProduction oFNProduction, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNProductionDA.RunOut(tc, oFNProduction, nUserID);
                IDataReader reader = FNProductionDA.Get(tc, oFNProduction.FNProductionID);      
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNProduction = new FNProduction();
                    oFNProduction = CreateObject(oReader);

                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFNProduction = new FNProduction();
                oFNProduction.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFNProduction;
        }
	    public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FNProduction oFNProduction = new FNProduction();
					oFNProduction.FNProductionID = id;
					FNProductionDA.Delete(tc, oFNProduction, EnumDBOperation.Delete, nUserId);
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('!')[0];
					#endregion
				}
				return Global.DeleteMessage;
			}
		public FNProduction Get(int id, Int64 nUserId)
			{
				FNProduction oFNProduction = new FNProduction();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FNProductionDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFNProduction = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FNProduction", e);
					#endregion
				}
				return oFNProduction;
			}
		public List<FNProduction> Gets(Int64 nUserID)
			{
				List<FNProduction> oFNProductions = new List<FNProduction>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNProductionDA.Gets(tc);
					oFNProductions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FNProduction oFNProduction = new FNProduction();
					oFNProduction.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFNProductions;
			}
		public List<FNProduction> Gets (string sSQL, Int64 nUserID)
			{
				List<FNProduction> oFNProductions = new List<FNProduction>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNProductionDA.Gets(tc, sSQL);
					oFNProductions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FNProduction", e);
					#endregion
				}
				return oFNProductions;
			}

		#endregion
	}

}
