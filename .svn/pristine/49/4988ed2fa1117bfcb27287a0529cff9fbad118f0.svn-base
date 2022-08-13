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
	public class NOASpecService : MarshalByRefObject, INOASpecService
	{
		#region Private functions and declaration

		private NOASpec MapObject(NullHandler oReader)
		{
            NOASpec oNOASpec = new NOASpec();
            oNOASpec.NOASpecLogID = oReader.GetInt32("NOASpecLogID");
            oNOASpec.NOADetailLogID = oReader.GetInt32("NOADetailLogID");
            oNOASpec.NOASpecID = oReader.GetInt32("NOASpecID");
			oNOASpec.SpecHeadID = oReader.GetInt32("SpecHeadID");
			oNOASpec.NOADetailID = oReader.GetInt32("NOADetailID");
			oNOASpec.NOADescription = oReader.GetString("NOADescription");
            oNOASpec.SpecName = oReader.GetString("SpecName");
			return oNOASpec;
		}

		private NOASpec CreateObject(NullHandler oReader)
		{
			NOASpec oNOASpec = new NOASpec();
			oNOASpec = MapObject(oReader);
			return oNOASpec;
		}

		private List<NOASpec> CreateObjects(IDataReader oReader)
		{
			List<NOASpec> oNOASpec = new List<NOASpec>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				NOASpec oItem = CreateObject(oHandler);
				oNOASpec.Add(oItem);
			}
			return oNOASpec;
		}

		#endregion

		#region Interface implementation
        public NOASpec IUD(NOASpec oNOASpec, short nDBOperation, int nUserID)
        {
            List<NOASpec> _oNOASpecs = new List<NOASpec>();
            _oNOASpecs = oNOASpec.NOASpecs;
            string sNOASpecIDs = "";
            NOASpec _oNOASpec = new NOASpec();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (NOASpec oItem in _oNOASpecs)
                {
                    oItem.NOADetailID = oNOASpec.NOADetailID;
                    if (oItem.NOASpecID <= 0)
                    {
                        reader = NOASpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        reader = NOASpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        _oNOASpec = new NOASpec();
                        _oNOASpec = CreateObject(oReader);
                        sNOASpecIDs = sNOASpecIDs + oReader.GetString("NOASpecID") + ",";
                    }
                    reader.Close();
                }
                if (sNOASpecIDs.Length > 0)
                {
                    sNOASpecIDs = sNOASpecIDs.Remove(sNOASpecIDs.Length - 1, 1);
                }

                NOASpec oTempNOASpec = new NOASpec();
                oTempNOASpec.NOADetailID = oNOASpec.NOADetailID;
                NOASpecDA.Delete(tc, oTempNOASpec, (int)EnumDBOperation.Delete, nUserID, sNOASpecIDs);
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                _oNOASpec = new NOASpec();
                _oNOASpec.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return _oNOASpec;
        }

			public NOASpec Get(int id, Int64 nUserId)
			{
				NOASpec oNOASpec = new NOASpec();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = NOASpecDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oNOASpec = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get NOASpec", e);
					#endregion
				}
				return oNOASpec;
			}

			public List<NOASpec> Gets(Int64 nUserID)
			{
				List<NOASpec> oNOASpecs = new List<NOASpec>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = NOASpecDA.Gets(tc);
					oNOASpecs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					NOASpec oNOASpec = new NOASpec();
					oNOASpec.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oNOASpecs;
			}

            public List<NOASpec> Gets(string sSQL, Int64 nUserID)
            {
                List<NOASpec> oNOASpecs = new List<NOASpec>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = NOASpecDA.Gets(tc, sSQL);
                    oNOASpecs = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null) ;
                    tc.HandleError();
                    ExceptionLog.Write(e);
                    throw new ServiceException("Failed to Get NOASpec", e);
                    #endregion
                }
                return oNOASpecs;
            }
            public List<NOASpec> GetsByLog(string sSQL, Int64 nUserID)
            {
                List<NOASpec> oNOASpecs = new List<NOASpec>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = NOASpecDA.GetsByLog(tc, sSQL);
                    oNOASpecs = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null) ;
                    tc.HandleError();
                    ExceptionLog.Write(e);
                    throw new ServiceException("Failed to Get NOASpec", e);
                    #endregion
                }
                return oNOASpecs;
            }

		#endregion
	}

}
