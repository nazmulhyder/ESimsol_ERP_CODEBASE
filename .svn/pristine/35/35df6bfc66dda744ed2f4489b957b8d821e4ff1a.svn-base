using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class RouteSheetGraceService : MarshalByRefObject, IRouteSheetGraceService
    {
        #region Private functions and declaration
        private RouteSheetGrace MapObject(NullHandler oReader)
        {
            RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
            oRouteSheetGrace.RouteSheetGraceID = oReader.GetInt32("RouteSheetGraceID");
            oRouteSheetGrace.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetGrace.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oRouteSheetGrace.QtyGrace = oReader.GetDouble("QtyGrace");
            oRouteSheetGrace.GraceCount= oReader.GetInt32("GraceCount");
            oRouteSheetGrace.Note = oReader.GetString("Note");
            oRouteSheetGrace.ApprovedByID = oReader.GetInt32("ApprovedByID");
            oRouteSheetGrace.ApproveDate = oReader.GetDateTime("ApproveDate");
            oRouteSheetGrace.NoteApp = oReader.GetString("NoteApp");
            oRouteSheetGrace.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oRouteSheetGrace.LastUpdateDateTime = oReader.GetDateTime("LastUpdateDateTime");

            oRouteSheetGrace.PrepareByName = oReader.GetString("PrepareByName");
            oRouteSheetGrace.ApprovedByName = oReader.GetString("ApprovedByName");
            oRouteSheetGrace.ProductName = oReader.GetString("ProductName");
            oRouteSheetGrace.LabdipNo = oReader.GetString("LabdipNo");
            oRouteSheetGrace.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oRouteSheetGrace.ColorName = oReader.GetString("ColorName");
            oRouteSheetGrace.ColorNo = oReader.GetString("ColorNo");
            oRouteSheetGrace.PantonNo = oReader.GetString("PantonNo");
            oRouteSheetGrace.ApproveLotNo = oReader.GetString("ApproveLotNo");
            oRouteSheetGrace.OrderNo = oReader.GetString("OrderNo");
            oRouteSheetGrace.StyleNo = oReader.GetString("StyleNo");
            oRouteSheetGrace.ContractorName = oReader.GetString("ContractorName");
            oRouteSheetGrace.DeliveryToName = oReader.GetString("DeliveryToName");
            oRouteSheetGrace.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oRouteSheetGrace.NoCode = oReader.GetString("NoCode");
            oRouteSheetGrace.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheetGrace.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oRouteSheetGrace.ContractorID = oReader.GetInt32("ContractorID");
            oRouteSheetGrace.OrderType = oReader.GetInt32("OrderType");
            oRouteSheetGrace.RSState = oReader.GetInt32("RSState");
            oRouteSheetGrace.OrderDate = oReader.GetDateTime("OrderDate");
            oRouteSheetGrace.OrderQty = oReader.GetDouble("OrderQty");
            oRouteSheetGrace.Qty_Pro = oReader.GetDouble("Qty_Pro");
            oRouteSheetGrace.RecycleQty = oReader.GetDouble("RecycleQty");
            oRouteSheetGrace.WastageQty = oReader.GetDouble("WastageQty");
            oRouteSheetGrace.RouteSheetDate = oReader.GetDateTime("RouteSheetDate");
            oRouteSheetGrace.QtyRS = oReader.GetDouble("QtyRS");

            return oRouteSheetGrace;
        }
        private RouteSheetGrace CreateObject(NullHandler oReader)
        {
            RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
            oRouteSheetGrace = MapObject(oReader);
            return oRouteSheetGrace;
        }
        private List<RouteSheetGrace> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetGrace> oRouteSheetGrace = new List<RouteSheetGrace>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetGrace oItem = CreateObject(oHandler);
                oRouteSheetGrace.Add(oItem);
            }
            return oRouteSheetGrace;
        }

        #endregion

        #region Interface implementation
        public RouteSheetGraceService() { }
        public RouteSheetGrace Save(RouteSheetGrace oRouteSheetGrace, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRouteSheetGrace.RouteSheetGraceID <= 0)
                {
                    reader = RouteSheetGraceDA.InsertUpdate(tc, oRouteSheetGrace, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = RouteSheetGraceDA.InsertUpdate(tc, oRouteSheetGrace, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetGrace = new RouteSheetGrace();
                    oRouteSheetGrace = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save RouteSheetGrace. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oRouteSheetGrace;
        }
        public RouteSheetGrace Approve(RouteSheetGrace oRouteSheetGrace, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRouteSheetGrace.RouteSheetGraceID <= 0)
                {
                    throw new Exception("Invalid Dyeing Grace!");
                    //reader = RouteSheetGraceDA.InsertUpdate(tc, oRouteSheetGrace, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = RouteSheetGraceDA.InsertUpdate(tc, oRouteSheetGrace, EnumDBOperation.Approval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetGrace = new RouteSheetGrace();
                    oRouteSheetGrace = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save RouteSheetGrace. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oRouteSheetGrace;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
                oRouteSheetGrace.RouteSheetGraceID = id;
                RouteSheetGraceDA.Delete(tc, oRouteSheetGrace, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public RouteSheetGrace Get(int id, Int64 nUserId)
        {
            RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = RouteSheetGraceDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetGrace = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RouteSheetGrace", e);
                #endregion
            }
            return oRouteSheetGrace;
        }
        public RouteSheetGrace GetByRS(int nRSID, Int64 nUserId)
        {
            RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = RouteSheetGraceDA.GetByRS(tc, nRSID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetGrace = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RouteSheetGrace", e);
                #endregion
            }
            return oRouteSheetGrace;
        }
        public List<RouteSheetGrace> Gets(Int64 nUserID)
        {
            List<RouteSheetGrace> oRouteSheetGraces = new List<RouteSheetGrace>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetGraceDA.Gets(tc);
                oRouteSheetGraces = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetGrace", e);
                #endregion
            }
            return oRouteSheetGraces;
        }
        public List<RouteSheetGrace> Gets(string sSQL,Int64 nUserID)
        {
            List<RouteSheetGrace> oRouteSheetGraces = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetGraceDA.Gets(tc,sSQL);
                oRouteSheetGraces = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetGrace", e);
                #endregion
            }
            return oRouteSheetGraces;
        }
        #endregion
    }   
}

