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
    public class DispoProductionCommentViewerService : MarshalByRefObject, IDispoProductionCommentViewerService
    {
        #region Private functions and declaration

        private DispoProductionCommentViewer MapObject(NullHandler oReader)
        {
            DispoProductionCommentViewer oDispoProductionCommentViewer = new DispoProductionCommentViewer();
            oDispoProductionCommentViewer.DispoProductionCommentViewerID = oReader.GetInt32("DispoProductionCommentViewerID");
            oDispoProductionCommentViewer.DispoProductionCommentID = oReader.GetInt32("DispoProductionCommentID");
            oDispoProductionCommentViewer.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oDispoProductionCommentViewer.DBUserID = oReader.GetInt32("DBUserID");
            oDispoProductionCommentViewer.UserName = oReader.GetString("UserName");

            return oDispoProductionCommentViewer;
        }

        private DispoProductionCommentViewer CreateObject(NullHandler oReader)
        {
            DispoProductionCommentViewer oDispoProductionCommentViewer = new DispoProductionCommentViewer();
            oDispoProductionCommentViewer = MapObject(oReader);
            return oDispoProductionCommentViewer;
        }

        private List<DispoProductionCommentViewer> CreateObjects(IDataReader oReader)
        {
            List<DispoProductionCommentViewer> oDispoProductionCommentViewer = new List<DispoProductionCommentViewer>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DispoProductionCommentViewer oItem = CreateObject(oHandler);
                oDispoProductionCommentViewer.Add(oItem);
            }
            return oDispoProductionCommentViewer;
        }

        #endregion

        #region Interface implementation
        public List<DispoProductionCommentViewer> Save(List<DispoProductionCommentViewer> oDispoProductionCommentViewers, Int64 nUserID)
        {
            DispoProductionCommentViewer oDispoProductionCommentViewer = new DispoProductionCommentViewer();
            List<DispoProductionCommentViewer> oDPCVs = new List<DispoProductionCommentViewer>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Dispo Production Comment Viewer
                foreach (DispoProductionCommentViewer oItem in oDispoProductionCommentViewers)
                {
                    IDataReader reader;
                    if (oItem.DispoProductionCommentViewerID <= 0)
                    {
                        reader = DispoProductionCommentViewerDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = DispoProductionCommentViewerDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDispoProductionCommentViewer = new DispoProductionCommentViewer();
                        oDispoProductionCommentViewer = CreateObject(oReader);
                        oDPCVs.Add(oDispoProductionCommentViewer);
                    }
                    reader.Close();
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
                    oDispoProductionCommentViewer = new DispoProductionCommentViewer();
                    oDispoProductionCommentViewer.ErrorMessage = e.Message.Split('!')[0];
                    oDPCVs = new List<DispoProductionCommentViewer>();
                    oDPCVs.Add(oDispoProductionCommentViewer);
                }
                #endregion
            }
            return oDPCVs;
        }

        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        DispoProductionCommentViewer oDispoProductionCommentViewer = new DispoProductionCommentViewer();
        //        oDispoProductionCommentViewer.DispoProductionCommentViewerID = id;
        //        DispoProductionCommentViewerDA.Delete(tc, oDispoProductionCommentViewer, EnumDBOperation.Delete, nUserId);
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exceptionif (tc != null)
        //        tc.HandleError();
        //        return e.Message.Split('!')[0];
        //        #endregion
        //    }
        //    return Global.DeleteMessage;
        //}

        public DispoProductionCommentViewer Get(int id, Int64 nUserId)
        {
            DispoProductionCommentViewer oDispoProductionCommentViewer = new DispoProductionCommentViewer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DispoProductionCommentViewerDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDispoProductionCommentViewer = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DispoProductionCommentViewer", e);
                #endregion
            }
            return oDispoProductionCommentViewer;
        }

        public List<DispoProductionCommentViewer> Gets(Int64 nUserID)
        {
            List<DispoProductionCommentViewer> oDispoProductionCommentViewers = new List<DispoProductionCommentViewer>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DispoProductionCommentViewerDA.Gets(tc);
                oDispoProductionCommentViewers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                DispoProductionCommentViewer oDispoProductionCommentViewer = new DispoProductionCommentViewer();
                oDispoProductionCommentViewer.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oDispoProductionCommentViewers;
        }

        public List<DispoProductionCommentViewer> Gets(string sSQL, Int64 nUserID)
        {
            List<DispoProductionCommentViewer> oDispoProductionCommentViewers = new List<DispoProductionCommentViewer>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DispoProductionCommentViewerDA.Gets(tc, sSQL);
                oDispoProductionCommentViewers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DispoProductionCommentViewer", e);
                #endregion
            }
            return oDispoProductionCommentViewers;
        }


        #endregion
    }

}
