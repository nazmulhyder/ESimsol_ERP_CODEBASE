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
	public class POSpecService : MarshalByRefObject, IPOSpecService
	{
		#region Private functions and declaration

		private POSpec MapObject(NullHandler oReader)
		{
			POSpec oPOSpec = new POSpec();
			oPOSpec.POSpecID = oReader.GetInt32("POSpecID");
			oPOSpec.SpecHeadID = oReader.GetInt32("SpecHeadID");
			oPOSpec.PODetailID = oReader.GetInt32("PODetailID");
			oPOSpec.PODescription = oReader.GetString("PODescription");
            oPOSpec.SpecName = oReader.GetString("SpecName");
			return oPOSpec;
		}

		private POSpec CreateObject(NullHandler oReader)
		{
			POSpec oPOSpec = new POSpec();
			oPOSpec = MapObject(oReader);
			return oPOSpec;
		}

		private List<POSpec> CreateObjects(IDataReader oReader)
		{
			List<POSpec> oPOSpec = new List<POSpec>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				POSpec oItem = CreateObject(oHandler);
				oPOSpec.Add(oItem);
			}
			return oPOSpec;
		}

		#endregion

		#region Interface implementation
        public POSpec IUD(POSpec oPOSpec, short nDBOperation, int nUserID)
        {
            List<POSpec> _oPOSpecs = new List<POSpec>();
            _oPOSpecs = oPOSpec.POSpecs;
            string sPOSpecIDs = "";
            POSpec _oPOSpec = new POSpec();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (POSpec oItem in _oPOSpecs)
                {
                    oItem.PODetailID = oPOSpec.PODetailID;
                    if (oItem.POSpecID <= 0)
                    {
                        reader = POSpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        reader = POSpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        _oPOSpec = new POSpec();
                        _oPOSpec = CreateObject(oReader);
                        sPOSpecIDs = sPOSpecIDs + oReader.GetString("POSpecID") + ",";
                    }
                    reader.Close();
                }
                if (sPOSpecIDs.Length > 0)
                {
                    sPOSpecIDs = sPOSpecIDs.Remove(sPOSpecIDs.Length - 1, 1);
                }

                POSpec oTempPOSpec = new POSpec();
                oTempPOSpec.PODetailID = oPOSpec.PODetailID;
                POSpecDA.Delete(tc, oTempPOSpec, (int)EnumDBOperation.Delete, nUserID, sPOSpecIDs);
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                _oPOSpec = new POSpec();
                _oPOSpec.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return _oPOSpec;
        }
			public POSpec Get(int id, Int64 nUserId)
			{
				POSpec oPOSpec = new POSpec();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = POSpecDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPOSpec = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get POSpec", e);
					#endregion
				}
				return oPOSpec;
			}

			public List<POSpec> Gets(Int64 nUserID)
			{
				List<POSpec> oPOSpecs = new List<POSpec>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = POSpecDA.Gets(tc);
					oPOSpecs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					POSpec oPOSpec = new POSpec();
					oPOSpec.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPOSpecs;
			}

			public List<POSpec> Gets (string sSQL, Int64 nUserID)
			{
				List<POSpec> oPOSpecs = new List<POSpec>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = POSpecDA.Gets(tc, sSQL);
					oPOSpecs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get POSpec", e);
					#endregion
				}
				return oPOSpecs;
			}

		#endregion
	}

}
