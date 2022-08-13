using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
	public class PQSpecService : MarshalByRefObject, IPQSpecService
	{
		#region Private functions and declaration

		private PQSpec MapObject(NullHandler oReader)
		{
            PQSpec oPQSpec = new PQSpec();
            oPQSpec.PQSpecID = oReader.GetInt32("PQSpecID");
            oPQSpec.PQSpecLogID = oReader.GetInt32("PQSpecLogID");
            oPQSpec.PQDetailLogID = oReader.GetInt32("PQDetailLogID");
			oPQSpec.SpecHeadID = oReader.GetInt32("SpecHeadID");
			oPQSpec.PQDetailID = oReader.GetInt32("PQDetailID");
			oPQSpec.PQDescription = oReader.GetString("PQDescription");
            oPQSpec.SpecName = oReader.GetString("SpecName");
            oPQSpec.SupplierID = oReader.GetInt32("SupplierID");
            oPQSpec.ProductID = oReader.GetInt32("ProductID");
			return oPQSpec;
		}

		private PQSpec CreateObject(NullHandler oReader)
		{
			PQSpec oPQSpec = new PQSpec();
			oPQSpec = MapObject(oReader);
			return oPQSpec;
		}

		private List<PQSpec> CreateObjects(IDataReader oReader)
		{
			List<PQSpec> oPQSpec = new List<PQSpec>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PQSpec oItem = CreateObject(oHandler);
				oPQSpec.Add(oItem);
			}
			return oPQSpec;
		}

		#endregion

		#region Interface implementation


        public PQSpec IUD(PQSpec oPQSpec, short nDBOperation, int nUserID)
        {
            List<PQSpec> _oPQSpecs = new List<PQSpec>();
            _oPQSpecs = oPQSpec.PQSpecs;
            string sPQSpecIDs = "";
            PQSpec _oPQSpec = new PQSpec();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (PQSpec oItem in _oPQSpecs)
                {
                    oItem.PQDetailID = oPQSpec.PQDetailID;
                    if (oItem.PQSpecID <= 0)
                    {
                        reader = PQSpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        reader = PQSpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        _oPQSpec = new PQSpec();
                        _oPQSpec = CreateObject(oReader);
                        sPQSpecIDs = sPQSpecIDs + oReader.GetString("PQSpecID") + ",";
                    }
                    reader.Close();
                }
                if (sPQSpecIDs.Length > 0)
                {
                    sPQSpecIDs = sPQSpecIDs.Remove(sPQSpecIDs.Length - 1, 1);
                }

                PQSpec oTempPQSpec = new PQSpec();
                oTempPQSpec.PQDetailID = oPQSpec.PQDetailID;
                PQSpecDA.Delete(tc, oTempPQSpec, (int)EnumDBOperation.Delete, nUserID, sPQSpecIDs);
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                _oPQSpec = new PQSpec();
                _oPQSpec.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return _oPQSpec;
        }
		
			public PQSpec Get(int id, Int64 nUserId)
			{
				PQSpec oPQSpec = new PQSpec();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PQSpecDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPQSpec = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PQSpec", e);
					#endregion
				}
				return oPQSpec;
			}

			public List<PQSpec> Gets(Int64 nUserID)
			{
				List<PQSpec> oPQSpecs = new List<PQSpec>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PQSpecDA.Gets(tc);
					oPQSpecs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PQSpec oPQSpec = new PQSpec();
					oPQSpec.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPQSpecs;
			}

            public List<PQSpec> Gets(string sSQL, Int64 nUserID)
            {
                List<PQSpec> oPQSpecs = new List<PQSpec>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = PQSpecDA.Gets(tc, sSQL);
                    oPQSpecs = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null) ;
                    tc.HandleError();
                    ExceptionLog.Write(e);
                    throw new ServiceException("Failed to Get PQSpec", e);
                    #endregion
                }
                return oPQSpecs;
            }
            public List<PQSpec> GetsByLog(string sSQL, Int64 nUserID)
            {
                List<PQSpec> oPQSpecs = new List<PQSpec>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = PQSpecDA.GetsByLog(tc, sSQL);
                    oPQSpecs = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null) ;
                    tc.HandleError();
                    ExceptionLog.Write(e);
                    throw new ServiceException("Failed to Get PQSpec", e);
                    #endregion
                }
                return oPQSpecs;
            }

		#endregion
	}

}
