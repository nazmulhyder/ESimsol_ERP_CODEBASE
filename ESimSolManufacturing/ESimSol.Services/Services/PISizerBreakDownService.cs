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
    public class PISizerBreakDownService : MarshalByRefObject, IPISizerBreakDownService
    {
        #region Private functions and declaration
        private PISizerBreakDown MapObject(NullHandler oReader)
        {
            PISizerBreakDown oPISizerBreakDown = new PISizerBreakDown();
            oPISizerBreakDown.PISizerBreakDownID = oReader.GetInt32("PISizerBreakDownID");
            oPISizerBreakDown.PISizerBreakDownLogID = oReader.GetInt32("PISizerBreakDownLogID");
            oPISizerBreakDown.ExportPILogID = oReader.GetInt32("ExportPILogID");
            oPISizerBreakDown.ExportPIID = oReader.GetInt32("ExportPIID");
            oPISizerBreakDown.ProductID = oReader.GetInt32("ProductID");
            oPISizerBreakDown.ColorID = oReader.GetInt32("ColorID");
            oPISizerBreakDown.Model = oReader.GetString("Model");
            oPISizerBreakDown.SizeID = oReader.GetInt32("SizeID");
            oPISizerBreakDown.StyleNo = oReader.GetString("StyleNo");
            oPISizerBreakDown.PantonNo = oReader.GetString("PantonNo");
            oPISizerBreakDown.Remarks = oReader.GetString("Remarks");
            oPISizerBreakDown.Quantity = oReader.GetDouble("Quantity");
            oPISizerBreakDown.ColorName = oReader.GetString("ColorName");
            oPISizerBreakDown.SizeName = oReader.GetString("SizeName");
            oPISizerBreakDown.ProductCode = oReader.GetString("ProductCode");
            oPISizerBreakDown.ProductName = oReader.GetString("ProductName");
            return oPISizerBreakDown;
        }

        private PISizerBreakDown CreateObject(NullHandler oReader)
        {
            PISizerBreakDown oPISizerBreakDown = new PISizerBreakDown();
            oPISizerBreakDown = MapObject(oReader);
            return oPISizerBreakDown;
        }

        private List<PISizerBreakDown> CreateObjects(IDataReader oReader)
        {
            List<PISizerBreakDown> oPISizerBreakDown = new List<PISizerBreakDown>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PISizerBreakDown oItem = CreateObject(oHandler);
                oPISizerBreakDown.Add(oItem);
            }
            return oPISizerBreakDown;
        }

        #endregion

        #region Interface implementation
        public PISizerBreakDownService() { }

        public PISizerBreakDown Save(PISizerBreakDown oPISizerBreakDown, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPISizerBreakDown.PISizerBreakDownID <= 0)
                {
                    reader = PISizerBreakDownDA.InsertUpdate(tc, oPISizerBreakDown, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = PISizerBreakDownDA.InsertUpdate(tc, oPISizerBreakDown, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPISizerBreakDown = new PISizerBreakDown();
                    oPISizerBreakDown = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save PISizerBreakDown. Because of " + e.Message, e);
                #endregion
            }
            return oPISizerBreakDown;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PISizerBreakDown oPISizerBreakDown = new PISizerBreakDown();
                oPISizerBreakDown.PISizerBreakDownID = id;
                PISizerBreakDownDA.Delete(tc, oPISizerBreakDown, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete PISizerBreakDown. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public PISizerBreakDown Get(int id, int nUserId)
        {
            PISizerBreakDown oAccountHead = new PISizerBreakDown();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PISizerBreakDownDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get PISizerBreakDown", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PISizerBreakDown> Gets(int nGRNID, int nUserID)
        {
            List<PISizerBreakDown> oPISizerBreakDown = new List<PISizerBreakDown>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PISizerBreakDownDA.Gets(tc, nGRNID);
                oPISizerBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PISizerBreakDown", e);
                #endregion
            }

            return oPISizerBreakDown;
        }

        //
        public List<PISizerBreakDown> GetsByLog(int nPILogID, int nUserID)
        {
            List<PISizerBreakDown> oPISizerBreakDown = new List<PISizerBreakDown>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PISizerBreakDownDA.GetsByLog(tc, nPILogID);
                oPISizerBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PISizerBreakDown", e);
                #endregion
            }

            return oPISizerBreakDown;
        }
        public List<PISizerBreakDown> Gets(int nUserID)
        {
            List<PISizerBreakDown> oPISizerBreakDown = new List<PISizerBreakDown>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PISizerBreakDownDA.Gets(tc);
                oPISizerBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PISizerBreakDown", e);
                #endregion
            }

            return oPISizerBreakDown;
        }
        public List<PISizerBreakDown> Gets(string sSQL,int nUserID)
        {
            List<PISizerBreakDown> oPISizerBreakDown = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                sSQL = "Select * from PISizerBreakDown where PISizerBreakDownID in (1,2,80,272,347,370,60,45)";
                    }
                reader = PISizerBreakDownDA.Gets(tc, sSQL);
                oPISizerBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PISizerBreakDown", e);
                #endregion
            }

            return oPISizerBreakDown;
        }

    
        #endregion
    }   
}