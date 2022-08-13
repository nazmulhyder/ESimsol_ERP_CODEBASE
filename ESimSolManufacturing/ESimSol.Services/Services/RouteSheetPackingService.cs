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
    public class RouteSheetPackingService : MarshalByRefObject, IRouteSheetPackingService
    {
        #region Private functions and declaration
        private RouteSheetPacking MapObject(NullHandler oReader)
        {
            RouteSheetPacking oRouteSheetPacking = new RouteSheetPacking();
            oRouteSheetPacking.PackingID = oReader.GetInt32("PackingID");
            oRouteSheetPacking.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetPacking.Weight = oReader.GetDouble("Weight");
            oRouteSheetPacking.NoOfHankCone = oReader.GetInt32("NoOfHankCone");
            oRouteSheetPacking.BagNo = oReader.GetInt32("BagNo");
            oRouteSheetPacking.Note = oReader.GetString("Note");
            oRouteSheetPacking.Warp = (EnumWarpWeft)oReader.GetInt16("Warp");
            oRouteSheetPacking.Length = oReader.GetString("Length");
            oRouteSheetPacking.PackedByEmpID = oReader.GetInt32("PackedByEmpID");
            oRouteSheetPacking.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oRouteSheetPacking.YarnType = (EnumYarnType)oReader.GetInt16("YarnType");
            oRouteSheetPacking.Note = oReader.GetString("Note");
            oRouteSheetPacking.QCBYName = oReader.GetString("QCBYName");
            oRouteSheetPacking.QCDate = oReader.GetDateTime("QCDate");
            oRouteSheetPacking.QtyPack = oReader.GetDouble("QtyPack");
            oRouteSheetPacking.BagType = (EnumRSBagType)oReader.GetInt16("BagType");
            oRouteSheetPacking.BagWeight = oReader.GetDouble("BagWeight");
            oRouteSheetPacking.QtyGW = oReader.GetDouble("QtyGW");
            oRouteSheetPacking.LDPE = oReader.GetDouble("LDPE");
            oRouteSheetPacking.HDPE = oReader.GetDouble("HDPE");
            oRouteSheetPacking.CTN = oReader.GetDouble("CTN");
            oRouteSheetPacking.DUHardWindingID = oReader.GetInt32("DUHardWindingID");
            
            return oRouteSheetPacking;
        }

        private RouteSheetPacking CreateObject(NullHandler oReader)
        {
            RouteSheetPacking oRouteSheetPacking = MapObject(oReader);
            return oRouteSheetPacking;
        }

        private List<RouteSheetPacking> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetPacking> oRouteSheetPacking = new List<RouteSheetPacking>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetPacking oItem = CreateObject(oHandler);
                oRouteSheetPacking.Add(oItem);
            }
            return oRouteSheetPacking;
        }

        #endregion

        #region Interface implementation
        public RouteSheetPackingService() { }

        public RouteSheetPacking IUD(RouteSheetPacking oRouteSheetPacking, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = RouteSheetPackingDA.IUD(tc, oRouteSheetPacking, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetPacking = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oRouteSheetPacking = new RouteSheetPacking();
                    oRouteSheetPacking.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRouteSheetPacking.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oRouteSheetPacking;
        }
        public RouteSheetPacking Get(int nRouteSheetPackingID, Int64 nUserId)
        {
            RouteSheetPacking oRouteSheetPacking = new RouteSheetPacking();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetPackingDA.Get(nRouteSheetPackingID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetPacking = CreateObject(oReader);
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
                oRouteSheetPacking.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetPacking;
        }
        public List<RouteSheetPacking> Gets(string sSQL, Int64 nUserID)
        {
            List<RouteSheetPacking> oRouteSheetPacking = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetPackingDA.Gets(sSQL, tc);
                oRouteSheetPacking = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetPacking", e);
                #endregion
            }
            return oRouteSheetPacking;
        }

        public List<RouteSheetPacking> PackingMultiple(RouteSheetPacking oRouteSheetPacking, int nBag, Int64 nUserID)
        {
            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                //double nQty = RouteSheetDA.GetRouteSheetQty(tc, oRouteSheetPacking.RouteSheetID);
                //nQty = nQty + (nQty*10 / 100); // With Additional 10 precent

                //if ((oRouteSheetPacking.Weight*nBag) > nQty)
                //    throw new Exception("Maximum allowed quantity " + Global.MillionFormat(nQty) + " Kg");

                for (int i = 0; i < nBag; i++)
                {
                    IDataReader reader;
                    reader = RouteSheetPackingDA.IUD(tc, oRouteSheetPacking, nUserID, (int)EnumDBOperation.Insert);

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRouteSheetPackings.Add(CreateObject(oReader));
                    }
                    reader.Close();
                }

                //RouteSheetPackingDA.Update_RS_BagNo(oRouteSheetPacking.RouteSheetID, tc);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRouteSheetPackings = new List<RouteSheetPacking>();
                oRouteSheetPacking.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View   
                oRouteSheetPackings.Add(oRouteSheetPacking);
                #endregion
            }
            return oRouteSheetPackings;
        }
        
        #endregion
    }
}
