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
    public class PRSpecService : MarshalByRefObject, IPRSpecService
    {
        #region Private functions and declaration

        private PRSpec MapObject(NullHandler oReader)
        {
            PRSpec oPRSpec = new PRSpec();
            oPRSpec.PRSpecID = oReader.GetInt32("PRSpecID");
            oPRSpec.SpecHeadID = oReader.GetInt32("SpecHeadID");
            oPRSpec.PRDetailID = oReader.GetInt32("PRDetailID");
            oPRSpec.PRDescription = oReader.GetString("PRDescription");
            oPRSpec.SpecName = oReader.GetString("SpecName");
            oPRSpec.SL = oReader.GetInt32("SL");
            return oPRSpec;
        }

        private PRSpec CreateObject(NullHandler oReader)
        {
            PRSpec oPRSpec = new PRSpec();
            oPRSpec = MapObject(oReader);
            return oPRSpec;
        }

        private List<PRSpec> CreateObjects(IDataReader oReader)
        {
            List<PRSpec> oPRSpec = new List<PRSpec>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PRSpec oItem = CreateObject(oHandler);
                oPRSpec.Add(oItem);
            }
            return oPRSpec;
        }

        #endregion
        #region Interface implementation


        public PRSpec IUD(PRSpec oPRSpec, short nDBOperation, int nUserID)
        {
            List<PRSpec> _oPRSpecs = new List<PRSpec>();
            _oPRSpecs = oPRSpec.PRSpecs;
            string sPRSpecIDs = "";
            PRSpec _oPRSpec = new PRSpec();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                
                foreach (PRSpec oItem in _oPRSpecs)
                {
                    oItem.PRDetailID = oPRSpec.PRDetailID;
                    if (oItem.PRSpecID<=0)
                    {
                        reader = PRSpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        reader = PRSpecDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        _oPRSpec = new PRSpec();
                        _oPRSpec = CreateObject(oReader);
                        sPRSpecIDs = sPRSpecIDs + oReader.GetString("PRSpecID") + ",";
                    }
                    reader.Close();
                }
                if (sPRSpecIDs.Length > 0)
                 {
                            sPRSpecIDs = sPRSpecIDs.Remove(sPRSpecIDs.Length - 1, 1);
                 }

                PRSpec oTempPRSpec = new PRSpec();
                oTempPRSpec.PRDetailID = oPRSpec.PRDetailID;
                PRSpecDA.Delete(tc, oTempPRSpec, (int)EnumDBOperation.Delete, nUserID, sPRSpecIDs);
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                _oPRSpec = new PRSpec();
                _oPRSpec.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return _oPRSpec;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PRSpec oPRSpec = new PRSpec();
                oPRSpec.PRSpecID = id;
                DBTableReferenceDA.HasReference(tc, "PRSpec", id);
                PRSpecDA.Delete(tc, oPRSpec, (int)EnumDBOperation.Delete, (int)nUserId,"");
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

        public PRSpec Get(int id, Int64 nUserId)
        {
            PRSpec oPRSpec = new PRSpec();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PRSpecDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPRSpec = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PRSpec", e);
                #endregion
            }
            return oPRSpec;
        }

        public List<PRSpec> Gets(Int64 nUserID)
        {
            List<PRSpec> oPRSpecs = new List<PRSpec>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PRSpecDA.Gets(tc);
                oPRSpecs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                PRSpec oPRSpec = new PRSpec();
                oPRSpec.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPRSpecs;
        }

        public List<PRSpec> Gets(string sSQL, Int64 nUserID)
        {
            List<PRSpec> oPRSpecs = new List<PRSpec>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PRSpecDA.Gets(tc, sSQL);
                oPRSpecs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PRSpec", e);
                #endregion
            }
            return oPRSpecs;
        }

        public List<PRSpec> RefreshSequence(List<PRSpec> oPRSpecs, Int64 nUserID)
        {
            PRSpec oPRSpec = new PRSpec();
            List<PRSpec> _oPRSpecs = new List<PRSpec>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oPRSpecs.Count > 0)
                {
                    foreach (PRSpec oTempPRSpec in oPRSpecs)
                    {
                        if (oTempPRSpec.PRSpecID > 0 && oTempPRSpec.SL > 0)
                        {
                            PRSpecDA.UpdateSequence(tc, oTempPRSpec);
                        }
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oPRSpec = new PRSpec();
                    oPRSpec.ErrorMessage = e.Message;
                    _oPRSpecs = new List<PRSpec>();
                    _oPRSpecs.Add(oPRSpec);
                }
            }
            return _oPRSpecs;
        }
        #endregion
    }
}
