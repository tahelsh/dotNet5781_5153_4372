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
            catch(DO.BadLicenseNumException ex)
            {
                throw new BO.BadLicenseNumException(ex.licenseNum, ex.Message);
            }
        }

        public IEnumerable<BO.Bus> GetAllBuses()
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
            catch (DO.BadLicenseNumException ex)
            {
                throw new BO.BadLicenseNumException(ex.licenseNum, ex.Message);
            }
            return busDoBoAdapter(busDO);
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusDetails(BO.Bus bus)
        {
            DO.Bus busDO = new DO.Bus();
            bus.CopyPropertiesTo(busDO);
            try
            {
                dl.UpdateBus(busDO);
            }
            catch (DO.BadLicenseNumException ex)
            {
                throw new BO.BadLicenseNumException(ex.licenseNum, ex.Message);
            }
            catch (DO.BadInputException ex)
            {
                throw new BO.BadInputException(ex.Message);
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
            catch(DO.BadLicenseNumException ex)
            {
                throw new BO.BadLicenseNumException(ex.licenseNum, ex.Message);
            }
            catch (DO.BadInputException ex)
            {
                throw new BO.BadInputException(ex.Message);
            }
        }
        #endregion

        #region Line
        BO.Line lineDoBoAdapter(DO.Line lineDO)
        {
            BO.Line lineBO = new BO.Line();
            int lineId = lineDO.LineId;
            lineDO.CopyPropertiesTo(lineBO);
            //lineBO.Stations = from stat in dl.GetAllLineStationsBy(stat => stat.LineId == lineId)//Linestation
            //                                         let station = dl.GetStation(stat.StationCode)//station
            //                                         select (BO.StationInLine)station.CopyPropertiesToNew(typeof(BO.StationInLine));
            IEnumerable<BO.StationInLine> stations = from stat in dl.GetAllLineStationsBy(stat => stat.LineId == lineId && stat.IsDeleted==false)//Linestation
                                                     let station = dl.GetStation(stat.StationCode)//station
                                                     select station.CopyToStationInLine(stat);
            //select (BO.StationInLine)station.CopyPropertiesToNew(typeof(BO.StationInLine));
            lineBO.Stations = stations.OrderBy(s => s.LineStationIndex);
            return lineBO;
        }
        public Line GetLine(int lineId)
        {
            DO.Line lineDO;
            try
            {
                lineDO = dl.GetLine(lineId);
            }
            catch(DO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException(ex.ID, ex.Message);
            }
            return lineDoBoAdapter(lineDO);
        }
        public void AddNewLine(BO.Line lineBo)
        {
            DO.Line lineDo = new DO.Line();
            lineBo.CopyPropertiesTo(lineDo);
            lineDo.FirstStation = lineBo.Stations.ToList()[0].StationCode;
            lineDo.FirstStation = lineBo.Stations.ToList()[0].StationCode;
            try
            {
                dl.AddLine(lineDo);
            }
            catch(BO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException(ex.ID, ex.Message);
            }

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
            catch (DO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException(ex.ID, ex.Message);
            }
        }

        public void DeleteLine(int lineId)
        {
            try
            {
                dl.DeleteLine(lineId);
            }
            catch(DO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException(ex.ID, ex.Message);
            }
            
        }
        #endregion

        #region Station
        public BO.Station stationDoBoAdapter(DO.Station stationDO)
        {
            BO.Station stationBO = new BO.Station();
            int stationCode = stationDO.Code;
            stationDO.CopyPropertiesTo(stationBO);
            //lineBO.Stations = from stat in dl.GetAllLineStationsBy(stat => stat.LineId == lineId)//Linestation
            //                                         let station = dl.GetStation(stat.StationCode)//station
            //                                         select (BO.StationInLine)station.CopyPropertiesToNew(typeof(BO.StationInLine));
           stationBO.Lines = from stat  in dl.GetAllLineStationsBy(stat => stat.StationCode == stationCode)//Linestation
                                                     let line = dl.GetLine(stat.LineId)//station
                                                     select line.CopyToLineInStation(stat);
            //select (BO.StationInLine)station.CopyPropertiesToNew(typeof(BO.StationInLine));
            //stationBO. = stations.OrderBy(s => s.LineStationIndex);
            return stationBO;
        }
        public IEnumerable<BO.Station> GetAllStations()
        {
            return from item in dl.GetAllStations()
                   select stationDoBoAdapter(item);
        }
        #endregion

        #region StationInLine
        //public void AddStationInLine(BO.StationInLine s)
        //{
        //    DO.LineStation sDO = (DO.LineStation)s.CopyPropertiesToNew(typeof(DO.LineStation));

        //}
        #endregion

        #region LineStation
        public void AddLineStation(BO.LineStation s)
        {
            DO.LineStation sDO = (DO.LineStation)s.CopyPropertiesToNew(typeof(DO.LineStation));
            try
            {
                dl.AddLineStation(sDO);
            }
            catch(Exception)
            {
                throw new Exception();
            }
        }
        public void DeleteLineStation(int lineId, int stationCode)
        {
            try
            {
                dl.DeleteLineStation(lineId, stationCode);
            }
            catch(Exception)
            {

            }
        }
        #endregion

        #region AdjacentStations
        public bool IsExistAdjacentStations(int stationCode1, int stationCode2)
        {
            if (dl.IsExistAdjacentStations(stationCode1, stationCode2))
                return true;
            return false;
        }
        public void AddAdjacentStations(BO.AdjacentStation adjBO)
        {
            try
            {
                DO.AdjacentStations adjDO = new DO.AdjacentStations();
                adjBO.CopyPropertiesTo(adjDO);
                dl.AddAdjacentStations(adjDO);
            }
            catch (Exception)
            {

            }
        
        }

        #endregion


    }
}
