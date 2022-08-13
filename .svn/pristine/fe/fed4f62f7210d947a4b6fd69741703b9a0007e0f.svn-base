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
	public class LotSpecService : MarshalByRefObject, ILotSpecService
	{
		#region Private functions and declaration

		private LotSpec MapObject(NullHandler oReader)
		{
			LotSpec oLotSpec = new LotSpec();
			oLotSpec.LotSpecID = oReader.GetInt32("LotSpecID");
			oLotSpec.SpecHeadID = oReader.GetInt32("SpecHeadID");
			oLotSpec.LotID = oReader.GetInt32("LotID");
			oLotSpec.SpecDescription = oReader.GetString("SpecDescription");
            oLotSpec.SpecName = oReader.GetString("SpecName");
			return oLotSpec;
		}

		private LotSpec CreateObject(NullHandler oReader)
		{
			LotSpec oLotSpec = new LotSpec();
			oLotSpec = MapObject(oReader);
			return oLotSpec;
		}

		private List<LotSpec> CreateObjects(IDataReader oReader)
		{
			List<LotSpec> oLotSpec = new List<LotSpec>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				LotSpec oItem = CreateObject(oHandler);
				oLotSpec.Add(oItem);
			}
			return oLotSpec;
		}

		#endregion

		#region Interface implementation
        public LotSpec IUD(LotSpec oLotSpec, short nDBOperation, int nUserID)
        {
            List<LotSpec> _oLotOSs = new List<LotSpec>();
            LotSpec _oLotSpec = new LotSpec();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    foreach (LotSpec oItem in oLotSpec.oLotSpecs)
                    {
                        if (oItem.LotSpecID > 0)
                        {
                            reader = LotSpecDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserID);
                        }
                        else
                        {
                            reader = LotSpecDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            _oLotSpec = new LotSpec();
                            _oLotSpec = CreateObject(oReader);
                            _oLotOSs.Add(_oLotSpec);
                        }
                        reader.Close();
                    }
                    if (_oLotOSs.Count > 0)
                    {
                        string sSQL = "";
                        sSQL = "delete from LotSpec where LotID = " + _oLotOSs.FirstOrDefault().LotID + "AND LotSpecID NOT IN (" + string.Join(",", _oLotOSs.Select(x => x.LotSpecID).ToList()) + ")";
                        LotSpecDA.Delete(tc, sSQL);
                    }
                    _oLotSpec = new LotSpec();
                    _oLotSpec.oLotSpecs = _oLotOSs;

                }
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                _oLotSpec = new LotSpec();
                _oLotSpec.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return _oLotSpec;
        }

			public LotSpec Get(int id, Int64 nUserId)
			{
				LotSpec oLotSpec = new LotSpec();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = LotSpecDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oLotSpec = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get LotSpec", e);
					#endregion
				}
				return oLotSpec;
			}

			public List<LotSpec> Gets(Int64 nUserID)
			{
				List<LotSpec> oLotSpecs = new List<LotSpec>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LotSpecDA.Gets(tc);
					oLotSpecs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					LotSpec oLotSpec = new LotSpec();
					oLotSpec.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oLotSpecs;
			}

			public List<LotSpec> Gets (string sSQL, Int64 nUserID)
			{
				List<LotSpec> oLotSpecs = new List<LotSpec>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LotSpecDA.Gets(tc, sSQL);
					oLotSpecs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get LotSpec", e);
					#endregion
				}
				return oLotSpecs;
			}

		#endregion
	}

}
