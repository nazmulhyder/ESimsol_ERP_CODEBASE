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
	public class FACodeService : MarshalByRefObject, IFACodeService
	{
		#region Private functions and declaration

		private FACode MapObject(NullHandler oReader)
		{
			FACode oFACode = new FACode();
			oFACode.FACodeID = oReader.GetInt32("FACodeID");
			oFACode.ProductID = oReader.GetInt32("ProductID");
            oFACode.CodingPartType = (EnumFACodingPartType)oReader.GetInt32("CodingPartType");
            oFACode.CodingPartTypeInt = oReader.GetInt32("CodingPartType");
            oFACode.CodingPartValue = oReader.GetString("CodingPartValue");
			//oFACode.ValueLength = oReader.GetInt32("ValueLength");
			oFACode.Sequence = oReader.GetInt32("Sequence");
			oFACode.Remarks = oReader.GetString("Remarks");
			return oFACode;
		}

		private FACode CreateObject(NullHandler oReader)
		{
			FACode oFACode = new FACode();
			oFACode = MapObject(oReader);
			return oFACode;
		}

		private List<FACode> CreateObjects(IDataReader oReader)
		{
			List<FACode> oFACode = new List<FACode>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FACode oItem = CreateObject(oHandler);
				oFACode.Add(oItem);
			}
			return oFACode;
		}

		#endregion

		#region Interface implementation
			public FACode Save(FACode oFACode, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFACode.FACodeID <= 0)
					{
						reader = FACodeDA.InsertUpdate(tc, oFACode, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = FACodeDA.InsertUpdate(tc, oFACode, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFACode = new FACode();
						oFACode = CreateObject(oReader);
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
						oFACode = new FACode();
						oFACode.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFACode;
			}
            public FACode Update(FACode oFACode, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    #region Delivery Order Detail Part
                    foreach (FACode oItem in oFACode.FACodes)
                    {
                        if (oItem.FACodeID > 0)
                        {
                            IDataReader readerdetail;
                            readerdetail = FACodeDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            readerdetail.Close();
                        }
                    }
                    #endregion
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                    {
                        tc.HandleError();
                        oFACode = new FACode();
                        oFACode.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFACode;
            }

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FACode oFACode = new FACode();
					oFACode.FACodeID = id;
					DBTableReferenceDA.HasReference(tc, "FACode", id);
					FACodeDA.Delete(tc, oFACode, EnumDBOperation.Delete, nUserId);
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

            public FACode Get(int id, Int64 nUserId)
            {
                FACode oFACode = new FACode();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = FACodeDA.Get(tc, id);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFACode = CreateObject(oReader);
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
                    throw new ServiceException("Failed to Get FACode", e);
                    #endregion
                }
                return oFACode;
            }

            public List<FACode> GetsByProduct(int id, Int64 nUserId)
            {
                List<FACode> oFACodes = new List<FACode>();
                FACode oFACode = new FACode();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = FACodeDA.GetsByProduct(tc, id);
                    oFACodes = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    ExceptionLog.Write(e);
                    throw new ServiceException("Failed to Get FACode", e);
                    #endregion
                }
                return oFACodes;
            }

			public List<FACode> Gets(Int64 nUserID)
			{
				List<FACode> oFACodes = new List<FACode>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FACodeDA.Gets(tc);
					oFACodes = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FACode oFACode = new FACode();
					oFACode.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFACodes;
			}

			public List<FACode> Gets (string sSQL, Int64 nUserID)
			{
				List<FACode> oFACodes = new List<FACode>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FACodeDA.Gets(tc, sSQL);
					oFACodes = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FACode", e);
					#endregion
				}
				return oFACodes;
			}

		#endregion
	}

}
