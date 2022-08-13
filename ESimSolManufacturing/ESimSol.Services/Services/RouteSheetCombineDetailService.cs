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
    public class RouteSheetCombineDetailService : MarshalByRefObject, IRouteSheetCombineDetailService
    {
        #region Private functions and declaration
        private RouteSheetCombineDetail MapObject(NullHandler oReader)
        {
            RouteSheetCombineDetail oRouteSheetCombineDetail = new RouteSheetCombineDetail();
            oRouteSheetCombineDetail.RouteSheetCombineDetailID = oReader.GetInt32("RouteSheetCombineDetailID");
            oRouteSheetCombineDetail.RouteSheetCombineID = oReader.GetInt32("RouteSheetCombineID");
            oRouteSheetCombineDetail.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetCombineDetail.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheetCombineDetail.RouteSheetDate = oReader.GetDateTime("RouteSheetDate");
            //oRouteSheetCombineDetail.MachineID = oReader.GetInt32("MachineID");
            oRouteSheetCombineDetail.ProductID_Raw = oReader.GetInt32("ProductID_Raw");
            oRouteSheetCombineDetail.LotID = oReader.GetInt32("LotID");
            oRouteSheetCombineDetail.Qty = oReader.GetDouble("Qty");
            oRouteSheetCombineDetail.QtyDye = oReader.GetDouble("QtyDye");
            oRouteSheetCombineDetail.RSState = (EnumRSState)oReader.GetInt16("RSState");
            //oRouteSheetCombineDetail.LocationID = oReader.GetInt32("LocationID");
            //oRouteSheetCombineDetail.PTUID = oReader.GetInt32("PTUID");
            oRouteSheetCombineDetail.DUPScheduleID = oReader.GetInt32("DUPScheduleID");
            oRouteSheetCombineDetail.Note = oReader.GetString("Note");
            oRouteSheetCombineDetail.TtlLiquire = oReader.GetDouble("TtlLiquire");
            oRouteSheetCombineDetail.TtlCotton = oReader.GetDouble("TtlCotton");
            //oRouteSheetCombineDetail.MachineName = oReader.GetString("MachineName");
            oRouteSheetCombineDetail.ProductCode = oReader.GetString("ProductCode");
            oRouteSheetCombineDetail.ProductName = oReader.GetString("ProductName");
            oRouteSheetCombineDetail.ProductName_Raw = oReader.GetString("ProductName_Raw");
            oRouteSheetCombineDetail.LotNo = oReader.GetString("LotNo");
            oRouteSheetCombineDetail.LocationID = oReader.GetInt32("LocationID");
            oRouteSheetCombineDetail.PTUID = oReader.GetInt32("PTUID");
            oRouteSheetCombineDetail.MachineID = oReader.GetInt32("MachineID");

            oRouteSheetCombineDetail.MachineName = oReader.GetString("MachineName");
            oRouteSheetCombineDetail.OrderNo = oReader.GetString("OrderNo");

            oRouteSheetCombineDetail.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oRouteSheetCombineDetail.ColorName = oReader.GetString("ColorName");
            oRouteSheetCombineDetail.Shade = oReader.GetInt16("Shade");
            //oRouteSheetCombineDetail.PantonNo = oReader.GetString("PantonNo");
            oRouteSheetCombineDetail.ColorNo = oReader.GetString("ColorNo");
            oRouteSheetCombineDetail.LabdipNo = oReader.GetString("LabdipNo");
            oRouteSheetCombineDetail.NoOfHanksCone = oReader.GetInt32("NoOfHanksCone");
          
           
            return oRouteSheetCombineDetail;

        }
        private RouteSheetCombineDetail CreateObject(NullHandler oReader)
        {
            RouteSheetCombineDetail oRouteSheetCombineDetail = MapObject(oReader);
            return oRouteSheetCombineDetail;
        }
        private List<RouteSheetCombineDetail> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetCombineDetail> oRouteSheetCombineDetails = new List<RouteSheetCombineDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetCombineDetail oItem = CreateObject(oHandler);
                oRouteSheetCombineDetails.Add(oItem);
            }
            return oRouteSheetCombineDetails;
        }

   
        #endregion

        #region Interface implementation
        public RouteSheetCombineDetailService() { }

        public RouteSheetCombineDetail Save(RouteSheetCombineDetail oRouteSheetCombineDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRouteSheetCombineDetail.RouteSheetCombineDetailID <= 0)
                {
                    reader = RouteSheetCombineDetailDA.InsertUpdate(tc, oRouteSheetCombineDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = RouteSheetCombineDetailDA.InsertUpdate(tc, oRouteSheetCombineDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetCombineDetail = new RouteSheetCombineDetail();
                    oRouteSheetCombineDetail = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oRouteSheetCombineDetail = new RouteSheetCombineDetail();
                oRouteSheetCombineDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oRouteSheetCombineDetail;
        }
        public RouteSheetCombineDetail Get(int nDOID, Int64 nUserId)
        {
            RouteSheetCombineDetail oRouteSheetCombineDetail = new RouteSheetCombineDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetCombineDetailDA.Get(nDOID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetCombineDetail = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get RouteSheetCombineDetail", e);
                oRouteSheetCombineDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetCombineDetail;
        }

        public List<RouteSheetCombineDetail> GetsBy(int nRouteSheetID, Int64 nUserID)
        {
            List<RouteSheetCombineDetail> oRouteSheetCombineDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetCombineDetailDA.GetsBy(tc, nRouteSheetID);
                oRouteSheetCombineDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetCombineDetail", e);
                #endregion
            }
            return oRouteSheetCombineDetail;
        }
        public List<RouteSheetCombineDetail> Gets(int nRSCID, Int64 nUserID)
        {
            List<RouteSheetCombineDetail> oRouteSheetCombineDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetCombineDetailDA.Gets(tc, nRSCID);
                oRouteSheetCombineDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetCombineDetail", e);
                #endregion
            }
            return oRouteSheetCombineDetail;
        }
    
        public string Delete(RouteSheetCombineDetail oRouteSheetCombineDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                RouteSheetCombineDetailDA.Delete(tc, oRouteSheetCombineDetail, EnumDBOperation.Delete, nUserId);
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
        public List<RouteSheetCombineDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<RouteSheetCombineDetail> oRouteSheetCombineDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetCombineDetailDA.Gets(sSQL, tc);
                oRouteSheetCombineDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetCombineDetail", e);
                #endregion
            }
            return oRouteSheetCombineDetail;
        }
   
        
        #endregion
       
    }
}
