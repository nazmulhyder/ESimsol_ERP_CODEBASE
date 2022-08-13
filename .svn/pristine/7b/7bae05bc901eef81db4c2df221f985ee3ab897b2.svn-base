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
	public class DocPrintEngineService : MarshalByRefObject, IDocPrintEngineService
	{
		#region Private functions and declaration

		private DocPrintEngine MapObject(NullHandler oReader)
		{
			DocPrintEngine oDocPrintEngine = new DocPrintEngine();
            oDocPrintEngine.DocPrintEngineID = oReader.GetInt32("DocPrintEngineID");
            oDocPrintEngine.ModuleID = oReader.GetInt32("ModuleID");
            oDocPrintEngine.ModuleType = (EnumModuleName)oReader.GetInt32("ModuleID");
            oDocPrintEngine.LetterType = (EnumDocumentPrintType)oReader.GetInt32("LetterType");
			oDocPrintEngine.PageSize = oReader.GetString("PageSize");
			oDocPrintEngine.Margin = oReader.GetString("Margin");
            oDocPrintEngine.FontName = oReader.GetString("FontName");
            oDocPrintEngine.LetterName = oReader.GetString("LetterName");
            oDocPrintEngine.Activity = oReader.GetBoolean("Activity");
            oDocPrintEngine.BUID = oReader.GetInt32("BUID");
            oDocPrintEngine.BusinessUnitName = oReader.GetString("BusinessUnitName");
			return oDocPrintEngine;
		}

		private DocPrintEngine CreateObject(NullHandler oReader)
		{
			DocPrintEngine oDocPrintEngine = new DocPrintEngine();
			oDocPrintEngine = MapObject(oReader);
			return oDocPrintEngine;
		}

		private List<DocPrintEngine> CreateObjects(IDataReader oReader)
		{
			List<DocPrintEngine> oDocPrintEngine = new List<DocPrintEngine>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				DocPrintEngine oItem = CreateObject(oHandler);
				oDocPrintEngine.Add(oItem);
			}
			return oDocPrintEngine;
		}

		#endregion

		#region Interface implementation
			public DocPrintEngine Save(DocPrintEngine oDocPrintEngine, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oDocPrintEngine.DocPrintEngineID <= 0)
					{
						reader = DocPrintEngineDA.InsertUpdate(tc, oDocPrintEngine, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = DocPrintEngineDA.InsertUpdate(tc, oDocPrintEngine, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oDocPrintEngine = new DocPrintEngine();
						oDocPrintEngine = CreateObject(oReader);
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
						oDocPrintEngine = new DocPrintEngine();
						oDocPrintEngine.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oDocPrintEngine;
			}

            public DocPrintEngine Copy(DocPrintEngine oDocPrintEngine, Int64 nUserID)
            {
                DocPrintEngineDetail oDocPrintEngineDetail = new DocPrintEngineDetail();
                DocPrintEngine oUG = new DocPrintEngine();
                oUG = oDocPrintEngine;

                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);

                    #region DocPrintEngine
                    IDataReader reader;
                    if (oDocPrintEngine.DocPrintEngineID <= 0)
                    {
                        reader = DocPrintEngineDA.InsertUpdate(tc, oDocPrintEngine, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = DocPrintEngineDA.InsertUpdate(tc, oDocPrintEngine, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDocPrintEngine = new DocPrintEngine();
                        oDocPrintEngine = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion

                    #region DocPrintEngineDetail

                    if (oDocPrintEngine.DocPrintEngineID > 0)
                    {
                        //string sDocPrintEngineDetailIDs = "";
                        if (oUG.DocPrintEngineDetails.Count > 0)
                        {
                            IDataReader readerdetail;
                            foreach (DocPrintEngineDetail oDRD in oUG.DocPrintEngineDetails)
                            {
                                oDRD.DocPrintEngineID = oDocPrintEngine.DocPrintEngineID;
                                if (oDRD.DocPrintEngineDetailID <= 0)
                                {
                                    readerdetail = DocPrintEngineDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID);
                                }
                                else
                                {
                                    readerdetail = DocPrintEngineDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID);
                                }
                                
                                readerdetail.Close();
                            }
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
                        oDocPrintEngine = new DocPrintEngine();
                        oDocPrintEngine.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oDocPrintEngine;
            }

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					DocPrintEngine oDocPrintEngine = new DocPrintEngine();
					oDocPrintEngine.DocPrintEngineID = id;
					DBTableReferenceDA.HasReference(tc, "DocPrintEngine", id);
					DocPrintEngineDA.Delete(tc, oDocPrintEngine, EnumDBOperation.Delete, nUserId);
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

			public DocPrintEngine Get(int id, Int64 nUserId)
			{
				DocPrintEngine oDocPrintEngine = new DocPrintEngine();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = DocPrintEngineDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oDocPrintEngine = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get DocPrintEngine", e);
					#endregion
				}
				return oDocPrintEngine;
			}
            public DocPrintEngine GetActiveByType(int type, Int64 nUserId)
            {
                DocPrintEngine oDocPrintEngine = new DocPrintEngine();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = DocPrintEngineDA.GetActiveByType(tc, type);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDocPrintEngine = CreateObject(oReader);
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
                    throw new ServiceException("Failed to Get DocPrintEngine", e);
                    #endregion
                }
                return oDocPrintEngine;
            }

            public DocPrintEngine GetActiveByTypenModule(int type, int moduleID, Int64 nUserId)
            {
                DocPrintEngine oDocPrintEngine = new DocPrintEngine();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = DocPrintEngineDA.GetActiveByTypenModule(tc, type, moduleID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDocPrintEngine = CreateObject(oReader);
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
                    throw new ServiceException("Failed to Get DocPrintEngine", e);
                    #endregion
                }
                return oDocPrintEngine;
            }
			public List<DocPrintEngine> Gets(Int64 nUserID)
			{
				List<DocPrintEngine> oDocPrintEngines = new List<DocPrintEngine>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = DocPrintEngineDA.Gets(tc);
					oDocPrintEngines = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					DocPrintEngine oDocPrintEngine = new DocPrintEngine();
					oDocPrintEngine.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oDocPrintEngines;
			}

			public List<DocPrintEngine> Gets (string sSQL, Int64 nUserID)
			{
				List<DocPrintEngine> oDocPrintEngines = new List<DocPrintEngine>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = DocPrintEngineDA.Gets(tc, sSQL);
					oDocPrintEngines = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get DocPrintEngine", e);
					#endregion
				}
				return oDocPrintEngines;
			}
            public DocPrintEngine Update(int id, Int64 nUserId)
            {
                DocPrintEngine oDocPrintEngine = new DocPrintEngine();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = DocPrintEngineDA.Update(tc, id, nUserId);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDocPrintEngine = CreateObject(oReader);
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
                    throw new ServiceException("Failed to Get DocPrintEngine", e);
                    #endregion
                }
                return oDocPrintEngine;
            }

		#endregion
	}

}
