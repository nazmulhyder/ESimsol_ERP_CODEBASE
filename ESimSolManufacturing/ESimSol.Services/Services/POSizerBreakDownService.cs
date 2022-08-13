using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class POSizerBreakDownService : MarshalByRefObject, IPOSizerBreakDownService
    {
        #region Private functions and declaration
        private POSizerBreakDown MapObject(NullHandler oReader)
        {
            POSizerBreakDown oPOSizerBreakDown = new POSizerBreakDown();
            oPOSizerBreakDown.POSizerBreakDownID = oReader.GetInt32("POSizerBreakDownID");
            oPOSizerBreakDown.ProductionOrderID = oReader.GetInt32("ProductionOrderID");
            oPOSizerBreakDown.POSizerBreakDownLogID = oReader.GetInt32("POSizerBreakDownLogID");
            oPOSizerBreakDown.ProductionOrderLogID = oReader.GetInt32("ProductionOrderLogID");
            oPOSizerBreakDown.ProductID = oReader.GetInt32("ProductID");
            oPOSizerBreakDown.ColorID = oReader.GetInt32("ColorID");
            oPOSizerBreakDown.Model = oReader.GetString("Model");
            oPOSizerBreakDown.StyleNo = oReader.GetString("StyleNo");
            oPOSizerBreakDown.PantonNo = oReader.GetString("PantonNo");
            oPOSizerBreakDown.SizeID = oReader.GetInt32("SizeID");
            oPOSizerBreakDown.Quantity = oReader.GetDouble("Quantity");
            oPOSizerBreakDown.ColorName = oReader.GetString("ColorName");
            oPOSizerBreakDown.SizeName = oReader.GetString("SizeName");
            oPOSizerBreakDown.ProductCode = oReader.GetString("ProductCode");
            oPOSizerBreakDown.ProductName = oReader.GetString("ProductName");
            oPOSizerBreakDown.Remarks = oReader.GetString("Remarks");
            return oPOSizerBreakDown;
        }

        private POSizerBreakDown CreateObject(NullHandler oReader)
        {
            POSizerBreakDown oPOSizerBreakDown = new POSizerBreakDown();
            oPOSizerBreakDown = MapObject(oReader);
            return oPOSizerBreakDown;
        }

        private List<POSizerBreakDown> CreateObjects(IDataReader oReader)
        {
            List<POSizerBreakDown> oPOSizerBreakDown = new List<POSizerBreakDown>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                POSizerBreakDown oItem = CreateObject(oHandler);
                oPOSizerBreakDown.Add(oItem);
            }
            return oPOSizerBreakDown;
        }

        #endregion

        #region Interface implementation
        public POSizerBreakDownService() { }

        public POSizerBreakDown Save(POSizerBreakDown oPOSizerBreakDown, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPOSizerBreakDown.POSizerBreakDownID <= 0)
                {
                    reader = POSizerBreakDownDA.InsertUpdate(tc, oPOSizerBreakDown, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = POSizerBreakDownDA.InsertUpdate(tc, oPOSizerBreakDown, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPOSizerBreakDown = new POSizerBreakDown();
                    oPOSizerBreakDown = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save POSizerBreakDown. Because of " + e.Message, e);
                #endregion
            }
            return oPOSizerBreakDown;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                POSizerBreakDown oPOSizerBreakDown = new POSizerBreakDown();
                oPOSizerBreakDown.POSizerBreakDownID = id;
                POSizerBreakDownDA.Delete(tc, oPOSizerBreakDown, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete POSizerBreakDown. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public POSizerBreakDown Get(int id, int nUserId)
        {
            POSizerBreakDown oAccountHead = new POSizerBreakDown();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = POSizerBreakDownDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get POSizerBreakDown", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<POSizerBreakDown> Gets(int nGRNID, int nUserID)
        {
            List<POSizerBreakDown> oPOSizerBreakDown = new List<POSizerBreakDown>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POSizerBreakDownDA.Gets(tc, nGRNID);
                oPOSizerBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get POSizerBreakDown", e);
                #endregion
            }

            return oPOSizerBreakDown;
        }

        //GetsByLog
        public List<POSizerBreakDown> GetsByLog(int nPILogID, int nUserID)
        {
            List<POSizerBreakDown> oPOSizerBreakDown = new List<POSizerBreakDown>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POSizerBreakDownDA.GetsByLog(tc, nPILogID);
                oPOSizerBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get POSizerBreakDown", e);
                #endregion
            }

            return oPOSizerBreakDown;
        }
        public List<POSizerBreakDown> Gets(int nUserID)
        {
            List<POSizerBreakDown> oPOSizerBreakDown = new List<POSizerBreakDown>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POSizerBreakDownDA.Gets(tc);
                oPOSizerBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get POSizerBreakDown", e);
                #endregion
            }

            return oPOSizerBreakDown;
        }
        public List<POSizerBreakDown> Gets(string sSQL,int nUserID)
        {
            List<POSizerBreakDown> oPOSizerBreakDown = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                sSQL = "Select * from POSizerBreakDown where POSizerBreakDownID in (1,2,80,272,347,370,60,45)";
                    }
                reader = POSizerBreakDownDA.Gets(tc, sSQL);
                oPOSizerBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get POSizerBreakDown", e);
                #endregion
            }

            return oPOSizerBreakDown;
        }

    
        #endregion
    }   
}