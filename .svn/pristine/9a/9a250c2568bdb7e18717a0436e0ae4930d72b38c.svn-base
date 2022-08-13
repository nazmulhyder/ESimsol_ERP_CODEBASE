using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class RouteSheetCancelService : MarshalByRefObject, IRouteSheetCancelService
    {
        #region Private functions and declaration
        private RouteSheetCancel MapObject(NullHandler oReader)
        {
            RouteSheetCancel oRouteSheetCancel = new RouteSheetCancel();
            oRouteSheetCancel.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetCancel.IsNewLot = oReader.GetBoolean("IsNewLot");
            oRouteSheetCancel.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oRouteSheetCancel.Remarks = oReader.GetString("Remarks");
            oRouteSheetCancel.ApproveBy = oReader.GetInt32("ApproveBy");
            oRouteSheetCancel.ApprovalRemarks = oReader.GetString("ApprovalRemarks");
            oRouteSheetCancel.RequestedByName = oReader.GetString("RequestedByName");
            oRouteSheetCancel.ApprovedByName = oReader.GetString("ApprovedByName");
            oRouteSheetCancel.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheetCancel.StoreName = oReader.GetString("StoreName");
            oRouteSheetCancel.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oRouteSheetCancel.ProductID = oReader.GetInt32("ProductID");
            oRouteSheetCancel.ProductName = oReader.GetString("ProductName");
            oRouteSheetCancel.ReDyeingStatus = (EnumReDyeingStatus)oReader.GetInt32("ReDyeingStatus");
            return oRouteSheetCancel;
        }

        private RouteSheetCancel CreateObject(NullHandler oReader)
        {
            RouteSheetCancel oRouteSheetCancel = MapObject(oReader);
            return oRouteSheetCancel;
        }

        private List<RouteSheetCancel> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetCancel> oRouteSheetCancel = new List<RouteSheetCancel>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetCancel oItem = CreateObject(oHandler);
                oRouteSheetCancel.Add(oItem);
            }
            return oRouteSheetCancel;
        }

        #endregion

        #region Interface implementation
        public RouteSheetCancelService() { }

        public RouteSheetCancel IUD(RouteSheetCancel oRouteSheetCancel, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = RouteSheetCancelDA.IUD(tc, oRouteSheetCancel, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetCancel = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oRouteSheetCancel = new RouteSheetCancel();
                    oRouteSheetCancel.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRouteSheetCancel = new RouteSheetCancel();
                oRouteSheetCancel.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oRouteSheetCancel;
        }
        public RouteSheetCancel Get(int nRouteSheetID, Int64 nUserId)
        {
            RouteSheetCancel oRouteSheetCancel = new RouteSheetCancel();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetCancelDA.Get(nRouteSheetID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetCancel = CreateObject(oReader);
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
                oRouteSheetCancel.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetCancel;
        }
        public List<RouteSheetCancel> Gets(string sSQL, Int64 nUserID)
        {
            List<RouteSheetCancel> oRouteSheetCancel = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetCancelDA.Gets(sSQL, tc);
                oRouteSheetCancel = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetCancel", e);
                #endregion
            }
            return oRouteSheetCancel;
        }

        #endregion
    }
}
