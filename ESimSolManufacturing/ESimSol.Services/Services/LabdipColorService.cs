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
    public class LabdipColorService :MarshalByRefObject ,ILabdipColorService
    {
        #region Private functions and declaration
        private LabdipColor MapObject(NullHandler oReader)
        {
            LabdipColor oLapdipColor = new LabdipColor();
            oLapdipColor.LabdipColorID = oReader.GetInt32("LabdipColorID");
            oLapdipColor.Name = oReader.GetString("Name");
            oLapdipColor.Note = oReader.GetString("Note");
            oLapdipColor.Code = oReader.GetString("Code");
            oLapdipColor.CodeNo = oReader.GetString("CodeNo");
           
            return oLapdipColor;
        }

        private LabdipColor CreateObject(NullHandler oReader)
        {
            LabdipColor oLapdipColors = MapObject(oReader);
            return oLapdipColors;
        }

        private List<LabdipColor> CreateObjects(IDataReader oReader)
        {
            List<LabdipColor> oLapdipColors = new List<LabdipColor>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabdipColor oItem = CreateObject(oHandler);
                oLapdipColors.Add(oItem);
            }
            return oLapdipColors;
        }

        #endregion

        #region Interface implementation
        public LabdipColorService() { }

        public LabdipColor IUD(LabdipColor oLapdipColor, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
              
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update )
                {
                    reader = LabdipColorDA.IUD(tc, oLapdipColor, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oLapdipColor = new LabdipColor();
                        oLapdipColor = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = LabdipColorDA.IUD(tc, oLapdipColor, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oLapdipColor.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLapdipColor = new LabdipColor();
                oLapdipColor.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oLapdipColor;
        }

        public LabdipColor Get(int nSUPCRID, Int64 nUserId)
        {
            LabdipColor oLapdipColor = new LabdipColor();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabdipColorDA.Get(tc, nSUPCRID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLapdipColor = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oLapdipColor = new LabdipColor();
                oLapdipColor.ErrorMessage = ex.Message;
                #endregion
            }

            return oLapdipColor;
        }

        public List<LabdipColor> Gets(string sSQL, Int64 nUserID)
        {
            List<LabdipColor> oLapdipColors = new List<LabdipColor>();
            LabdipColor oLapdipColor = new LabdipColor();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LabdipColorDA.Gets(tc, sSQL);
                oLapdipColors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oLapdipColor.ErrorMessage = ex.Message;
                oLapdipColors.Add(oLapdipColor);
                #endregion
            }

            return oLapdipColors;
        }

        #endregion
    }
}
