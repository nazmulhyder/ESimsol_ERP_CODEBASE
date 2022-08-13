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
	public class SpecHeadService : MarshalByRefObject, ISpecHeadService
	{
		#region Private functions and declaration

		private SpecHead MapObject(NullHandler oReader)
		{
			SpecHead oSpecHead = new SpecHead();
			oSpecHead.SpecHeadID = oReader.GetInt32("SpecHeadID");
			oSpecHead.SpecName = oReader.GetString("SpecName");
			oSpecHead.IsActive = oReader.GetBoolean("IsActive");
			return oSpecHead;
		}

		private SpecHead CreateObject(NullHandler oReader)
		{
			SpecHead oSpecHead = new SpecHead();
			oSpecHead = MapObject(oReader);
			return oSpecHead;
		}

		private List<SpecHead> CreateObjects(IDataReader oReader)
		{
			List<SpecHead> oSpecHead = new List<SpecHead>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				SpecHead oItem = CreateObject(oHandler);
				oSpecHead.Add(oItem);
			}
			return oSpecHead;
		}

		#endregion

		#region Interface implementation
		    public SpecHead IUD(SpecHead oSpecHead, int nDBOperation, Int64 nUserID)
            {
                TransactionContext tc = null;
                IDataReader reader;
                try
                {
                    tc = TransactionContext.Begin(true);

                    if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval || nDBOperation == (int)EnumDBOperation.Revise)
                    {
                        reader = SpecHeadDA.IUD(tc, oSpecHead, nDBOperation, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oSpecHead = new SpecHead();
                            oSpecHead = CreateObject(oReader);
                        }
                        reader.Close();

                    }
                    else if (nDBOperation == (int)EnumDBOperation.Delete)
                    {
                        reader = SpecHeadDA.IUD(tc, oSpecHead, nDBOperation, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        reader.Close();
                        oSpecHead.ErrorMessage = Global.DeleteMessage;
                    }
                    else
                    {
                        throw new Exception("Invalid Operation In Service");
                    }

                    tc.End();
                }
                catch (Exception ex)
                {
                    #region Handle Exception
                    tc.HandleError();
                    oSpecHead = new SpecHead();
                    oSpecHead.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                    #endregion
                }
                return oSpecHead;
            }

            public SpecHead Get(int nSpecHeadID, Int64 nUserId)
            {
                SpecHead oSpecHead = new SpecHead();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();

                    IDataReader reader = SpecHeadDA.Get(tc, nSpecHeadID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSpecHead = CreateObject(oReader);
                    }
                    reader.Close();
                    tc.End();
                }
                catch (Exception ex)
                {
                    #region Handle Exception
                    tc.HandleError();
                    oSpecHead = new SpecHead();
                    oSpecHead.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                    #endregion
                }

                return oSpecHead;
            }

            public List<SpecHead> Gets(string sSQL, Int64 nUserID)
            {
                List<SpecHead> oSpecHeads = new List<SpecHead>();
                SpecHead oSpecHead = new SpecHead();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();

                    IDataReader reader = null;
                    reader = SpecHeadDA.Gets(tc, sSQL);
                    oSpecHeads = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception ex)
                {
                    #region Handle Exception
                    tc.HandleError();
                    oSpecHead.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                    oSpecHeads.Add(oSpecHead);
                    #endregion
                }

                return oSpecHeads;
            }

		#endregion
	}

}
