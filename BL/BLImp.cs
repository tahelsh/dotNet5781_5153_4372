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
            try
            {
                dl.DeleteBus(licenseNum);
            }
            catch
            {
                throw new Exception();
            }
        }

        public IEnumerable<Bus> GetAllSBuses()
        {
            return from item in dl.GetAllBuses()
                   select busDoBoAdapter(item);
        }

        public Bus GetBus(int licenseNum)
        {
            DO.Bus busDO;
            try
            {
                busDO = dl.GetBus(licenseNum);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return busDoBoAdapter(busDO);
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusDetails(Bus bus)
        {
            DO.Bus busDO = new DO.Bus();
            bus.CopyPropertiesTo(busDO);
            try
            {
                dl.UpdateBus(busDO);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public void AddBus(BO.Bus bus)
        {
            DO.Bus busDO = new DO.Bus();
            bus.CopyPropertiesTo(busDO);
            try
            {
                dl.AddBus(busDO);
            }
            catch
            {
                throw new Exception();
            }

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

        public IEnumerable<BO.Line> GetAllLines()
        {
            return from item in dl.GetAllLines()
                   select lineDoBoAdapter(item);
        }

        public IEnumerable<BO.Line> GelAllLinesBy(Predicate<BO.Line> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineDetails(BO.Line line)
        {            
            DO.Line lineDO = new DO.Line();
            line.CopyPropertiesTo(lineDO);
            try
            {
                dl.UpdateLine(lineDO);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void DeleteLine(int lineId)
        {
            try
            {
                dl.DeleteLine(lineId);
            }
            catch
            {
                throw new Exception();
            }
            
        }
        #endregion


    }
}
