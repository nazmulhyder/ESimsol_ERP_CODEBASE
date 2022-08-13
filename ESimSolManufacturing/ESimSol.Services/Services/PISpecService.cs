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
	public class PISpecService : MarshalByRefObject, IPISpecService
	{
		#region Private functions and declaration

		private PISpec MapObject(NullHandler oReader)
		{
			PISpec oPISpec = new PISpec();
			oPISpec.PISpecID = oReader.GetInt32("PISpecID");
			oPISpec.SpecHeadID = oReader.GetInt32("SpecHeadID");
			oPISpec.PIDetailID = oReader.GetInt32("PIDetailID");
			oPISpec.PIDescription = oReader.GetString("PIDescription");
            oPISpec.SpecName = oReader.GetString("SpecName");
			return oPISpec;
		}

		private PISpec CreateObject(NullHandler oReader)
		{
			PISpec oPISpec = new PISpec();
			oPISpec = MapObject(oReader);
			return oPISpec;
		}

		private List<PISpec> CreateObjects(IDataReader oReader)
		{
			List<PISpec> oPISpec = new List<PISpec>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PISpec oItem = CreateObject(oHandler);
				oPISpec.Add(oItem);
			}
			return oPISpec;
		}

		#endregion

		#region Interface implementation
        public PISpec IUD(PISpec oPISpec, short nDBOperation, int nUserID)
        {
            List<PISpec> _oPISpecs = new List<PISpec>();
            _oPISpecs = oPISpec.PISpecs;
            string sPISpecIDs = "";
            PISpec _oPISpec = new PISpec();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (PISpec oItem in _oPISpecs)
                {
                    oItem.PIDetailID = oPISpec.PIDetailID;
                    if (oItem.PISpecID <= 0)
                    {
                        reader = PISpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        reader = PISpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        _oPISpec = new PISpec();
                        _oPISpec = CreateObject(oReader);
                        sPISpecIDs = sPISpecIDs + oReader.GetString("PISpecID") + ",";
                    }
                    reader.Close();
                }
                if (sPISpecIDs.Length > 0)
                {
                    sPISpecIDs = sPISpecIDs.Remove(sPISpecIDs.Length - 1, 1);
                }

                PISpec oTempPISpec = new PISpec();
                oTempPISpec.PIDetailID = oPISpec.PIDetailID;
                PISpecDA.Delete(tc, oTempPISpec, (int)EnumDBOperation.Delete, nUserID, sPISpecIDs);
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                _oPISpec = new PISpec();
                _oPISpec.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return _oPISpec;
        }

			public PISpec Get(int id, Int64 nUserId)
			{
				PISpec oPISpec = new PISpec();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PISpecDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPISpec = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PISpec", e);
					#endregion
				}
				return oPISpec;
			}

			public List<PISpec> Gets(Int64 nUserID)
			{
				List<PISpec> oPISpecs = new List<PISpec>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PISpecDA.Gets(tc);
					oPISpecs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PISpec oPISpec = new PISpec();
					oPISpec.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPISpecs;
			}

			public List<PISpec> Gets (string sSQL, Int64 nUserID)
			{
				List<PISpec> oPISpecs = new List<PISpec>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PISpecDA.Gets(tc, sSQL);
					oPISpecs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PISpec", e);
					#endregion
				}
				return oPISpecs;
			}

		#endregion
	}

}
