using System;
using System.Collections.Generic;
using System.Linq;
using DLAPI;
using BLAPI;
using System.Threading;
using BO;

namespace BL
{
    class BLImp : IBL
    {
        IDL dl = DLFactory.GetDL();
        
        #region Bus
        BO.Bus busDoBoAdapter(DO.Bus busDO)
        {
            BO.Bus busBO = new BO.Bus();
           
            busDO.CopyPropertiesTo(busBO);
         
            return busBO;
        }
        public void DeleteBus(int licenseNum)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetAllSBuses()
        {
            throw new NotImplementedException();
        }

        public Bus GetBus(int licenseNum)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusDetails(Bus bus)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Line
        BO.Line lineDoBoAdapter(DO.Line lineDO)
        {
            BO.Line lineBO = new BO.Line();
            int lineId = lineDO.LineId;
            lineDO.CopyPropertiesTo(lineBO);
            lineBO.Stations = from stat in dl.GetAllLineStationsBy(stat => stat.LineId == lineId)
                                      let station = dl.GetStation(stat.StationCode)
                                      //select station.CopyToStudentCourse(stat);
                                      select (BO.StationInLine)station.CopyPropertiesToNew(typeof(BO.StationInLine));

            return lineBO;
        }
        public Line GetLine(int lineId)
        {
            DO.Line lineDO;
            try
            {
                lineDO = dl.GetLine(lineId);
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
            //catch (DO.BadPersonIdException ex)
            //{
            //    throw new BO.BadStudentIdException("Person id does not exist or he is not a student", ex);
            //}
            return lineDoBoAdapter(lineDO);
        }

        public IEnumerable<Line> GetAllSLines()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Line> LinesBy(Predicate<Line> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineDetails(Line line)
        {
            throw new NotImplementedException();
        }

        public void DeleteLine(int LineId)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
