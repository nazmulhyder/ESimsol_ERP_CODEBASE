using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ExportSCDetailDOService : MarshalByRefObject, IExportSCDetailDOService
    {
        #region Private functions and declaration
        private ExportSCDetailDO MapObject(NullHandler oReader)
        {
            ExportSCDetailDO oExportSCDetailDO = new ExportSCDetailDO();
            oExportSCDetailDO.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            oExportSCDetailDO.ExportSCID = oReader.GetInt32("ExportSCID");
            oExportSCDetailDO.ProductID = oReader.GetInt32("ProductID");
            oExportSCDetailDO.Qty = oReader.GetDouble("Qty");
            oExportSCDetailDO.MUnitID = oReader.GetInt32("MUnitID");
            oExportSCDetailDO.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportSCDetailDO.ProductCode = oReader.GetString("ProductCode");
            oExportSCDetailDO.ProductName = oReader.GetString("ProductName");
            oExportSCDetailDO.MUName = oReader.GetString("MUName");
            oExportSCDetailDO.POQty = oReader.GetDouble("POQty");
            oExportSCDetailDO.DOQty = oReader.GetDouble("DOQty");
            oExportSCDetailDO.ColorInfo = oReader.GetString("ColorInfo");
            oExportSCDetailDO.BuyerRef = oReader.GetString("BuyerRef");
            oExportSCDetailDO.StyleNo = oReader.GetString("StyleNo");
            oExportSCDetailDO.ProductionType = oReader.GetInt32("ProductionType");
            
         
            return oExportSCDetailDO;
        }
        private ExportSCDetailDO CreateObject(NullHandler oReader)
        {
            ExportSCDetailDO oExportSCDetailDO = new ExportSCDetailDO();
            oExportSCDetailDO = MapObject(oReader);
            return oExportSCDetailDO;
        }      
        private List<ExportSCDetailDO> CreateObjects(IDataReader oReader)
        {
            List<ExportSCDetailDO> oExportSCDetailDOs = new List<ExportSCDetailDO>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportSCDetailDO oItem = CreateObject(oHandler);
                oExportSCDetailDOs.Add(oItem);
            }
            return oExportSCDetailDOs;
        }
        #endregion

        #region Interface implementation
        public ExportSCDetailDOService() { }
     
        public ExportSCDetailDO Get(int id, Int64 nUserId)
        {
            ExportSCDetailDO oExportSCDetailDO = new ExportSCDetailDO();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDetailDODA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSCDetailDO = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportSCDetailDO", e);
                #endregion
            }

            return oExportSCDetailDO;
        }

        public List<ExportSCDetailDO> Gets(int nExportLCID, Int64 nUserId)
        {
            List<ExportSCDetailDO> oExportSCDetailDO = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDetailDODA.Gets(tc , nExportLCID);
                oExportSCDetailDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetail", e);
                #endregion
            }

            return oExportSCDetailDO;
        }

        public List<ExportSCDetailDO> GetsByESCID(int nExportSCID, Int64 nUserId)
        {
            List<ExportSCDetailDO> oExportSCDetailDO = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDetailDODA.GetsByESCID(tc, nExportSCID);
                oExportSCDetailDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetail", e);
                #endregion
            }

            return oExportSCDetailDO;
        }

        public List<ExportSCDetailDO> Gets(Int64 nUserId)
        {
            List<ExportSCDetailDO> oExportSCDetailDO = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportSCDetailDODA.Gets(tc);
                oExportSCDetailDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetail", e);
                #endregion
            }

            return oExportSCDetailDO;
        }


        public List<ExportSCDetailDO> GetsByPI(int nExportPIID,Int64 nUserId)
        {
            List<ExportSCDetailDO> oExportSCDetailDO = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDetailDODA.GetsByPI(tc, nExportPIID);
                oExportSCDetailDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportSCDetailDO", e);
                #endregion
            }

            return oExportSCDetailDO;
        }

        public List<ExportSCDetailDO> Gets(string sSQL, Int64 nUserId)
        {
            List<ExportSCDetailDO> oExportSCDetailDO = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDetailDODA.Gets(tc, sSQL);
                oExportSCDetailDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportSCDetailDO", e);
                #endregion
            }

            return oExportSCDetailDO;
        }
      
        #endregion
    }
}
