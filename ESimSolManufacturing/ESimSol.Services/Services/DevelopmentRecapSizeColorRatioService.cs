using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{


    public class DevelopmentRecapSizeColorRatioService : MarshalByRefObject, IDevelopmentRecapSizeColorRatioService
    {
        #region Private functions and declaration
        private DevelopmentRecapSizeColorRatio MapObject(NullHandler oReader)
        {
            DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();


            oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oReader.GetInt32("DevelopmentRecapSizeColorRatioID");
            oDevelopmentRecapSizeColorRatio.DevelopmentRecapDetailID = oReader.GetInt32("DevelopmentRecapDetailID");
            oDevelopmentRecapSizeColorRatio.ColorID = oReader.GetInt32("ColorID");
            oDevelopmentRecapSizeColorRatio.SizeID = oReader.GetInt32("SizeID");
            oDevelopmentRecapSizeColorRatio.Qty = oReader.GetInt32("Qty");
            oDevelopmentRecapSizeColorRatio.ColorName = oReader.GetString("ColorName");
            oDevelopmentRecapSizeColorRatio.SizeName = oReader.GetString("SizeName");

            return oDevelopmentRecapSizeColorRatio;
        }

        private DevelopmentRecapSizeColorRatio CreateObject(NullHandler oReader)
        {
            DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();
            oDevelopmentRecapSizeColorRatio = MapObject(oReader);
            return oDevelopmentRecapSizeColorRatio;
        }

        private List<DevelopmentRecapSizeColorRatio> CreateObjects(IDataReader oReader)
        {
            List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatio = new List<DevelopmentRecapSizeColorRatio>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DevelopmentRecapSizeColorRatio oItem = CreateObject(oHandler);
                oDevelopmentRecapSizeColorRatio.Add(oItem);
            }
            return oDevelopmentRecapSizeColorRatio;
        }

        #endregion

        #region Interface implementation
        public DevelopmentRecapSizeColorRatioService() { }

        public DevelopmentRecapSizeColorRatio Save(DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<DevelopmentRecapSizeColorRatio> _oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
            oDevelopmentRecapSizeColorRatio.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DevelopmentRecapSizeColorRatioDA.InsertUpdate(tc, oDevelopmentRecapSizeColorRatio, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();
                    oDevelopmentRecapSizeColorRatio = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDevelopmentRecapSizeColorRatio.ErrorMessage = e.Message;
                #endregion
            }
            return oDevelopmentRecapSizeColorRatio;
        }


        public DevelopmentRecapSizeColorRatio Get(int DevelopmentRecapSizeColorRatioID, Int64 nUserId)
        {
            DevelopmentRecapSizeColorRatio oAccountHead = new DevelopmentRecapSizeColorRatio();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DevelopmentRecapSizeColorRatioDA.Get(tc, DevelopmentRecapSizeColorRatioID);
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
                throw new ServiceException("Failed to Get DevelopmentRecapSizeColorRatio", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DevelopmentRecapSizeColorRatio> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatio = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapSizeColorRatioDA.Gets(LabDipOrderID, tc);
                oDevelopmentRecapSizeColorRatio = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecapSizeColorRatio", e);
                #endregion
            }

            return oDevelopmentRecapSizeColorRatio;
        }

        public List<DevelopmentRecapSizeColorRatio> Gets(string sSQL, Int64 nUserID)
        {
            List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatio = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapSizeColorRatioDA.Gets(tc, sSQL);
                oDevelopmentRecapSizeColorRatio = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecapSizeColorRatio", e);
                #endregion
            }

            return oDevelopmentRecapSizeColorRatio;
        }
        #endregion
    }
    

}
