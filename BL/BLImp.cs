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
            catch (DO.BadLicenseNumException ex)
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
            catch (DO.BadLicenseNumException ex)
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
            List<BO.StationInLine> stations = (from stat in dl.GetAllLineStationsBy(stat => stat.LineId == lineId && stat.IsDeleted == false)//Linestation
                                               let station = dl.GetStation(stat.StationCode)//station
                                               select station.CopyToStationInLine(stat)).ToList();
            stations = (stations.OrderBy(s => s.LineStationIndex)).ToList();
            foreach (StationInLine s in stations)
            {
                if (s.LineStationIndex != stations[stations.Count - 1].LineStationIndex)
                {
                    int sc1 = s.StationCode;//station code 1
                    int sc2 = stations[s.LineStationIndex].StationCode;//station code 2
                    DO.AdjacentStations adjStat = dl.GetAdjacentStations(sc1, sc2);
                    s.Distance = adjStat.Distance;
                    s.Time = adjStat.Time;
                }
            }
            lineBO.Stations = stations;
            return lineBO;


        }
        public Line GetLine(int lineId)
        {
            DO.Line lineDO;
            try
            {
                lineDO = dl.GetLine(lineId);
            }
            catch (DO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException(ex.ID, ex.Message);
            }
            return lineDoBoAdapter(lineDO);
        }
        public void AddNewLine(BO.Line lineBo)
        {
            DO.Line lineDo = new DO.Line();
            lineBo.CopyPropertiesTo(lineDo);
            lineDo.LineId = DO.Config.LineId++;
            int sc1 = lineBo.Stations[0].StationCode;//stationCode of the first station
            int sc2 = lineBo.Stations[1].StationCode;//station Code of the last station
            lineDo.FirstStation = sc1;
            lineDo.LastStation = sc2;
            try
            {
                if (!dl.IsExistAdjacentStations(sc1, sc2))
                {
                    DO.AdjacentStations adj = new DO.AdjacentStations() { StationCode1 = sc1, StationCode2 = sc2, Distance = lineBo.Stations[0].Distance, Time = lineBo.Stations[0].Time };
                    dl.AddAdjacentStations(adj);
                }

                dl.AddLine(lineDo);
                DO.LineStation first = new DO.LineStation() { LineId = lineDo.LineId, StationCode = sc1, LineStationIndex = lineBo.Stations[0].LineStationIndex, IsDeleted = false };
                DO.LineStation last = new DO.LineStation() { LineId = lineDo.LineId, StationCode = sc2, LineStationIndex = lineBo.Stations[1].LineStationIndex, IsDeleted = false };
                dl.AddLineStation(first);
                dl.AddLineStation(last);

            }
            catch (BO.BadLineIdException ex)
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
            catch (DO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException(ex.ID, ex.Message);
            }

        }
        #endregion

        #region Station
        public BO.Station StationDoBoAdapter(DO.Station stationDO)
        {
            BO.Station stationBO = new BO.Station();
            int stationCode = stationDO.Code;
            stationDO.CopyPropertiesTo(stationBO);
            //lineBO.Stations = from stat in dl.GetAllLineStationsBy(stat => stat.LineId == lineId)//Linestation
            //                                         let station = dl.GetStation(stat.StationCode)//station
            //                                         select (BO.StationInLine)station.CopyPropertiesToNew(typeof(BO.StationInLine));
            stationBO.Lines = (from stat in dl.GetAllLineStationsBy(stat => stat.StationCode == stationCode)//Linestation
                               let line = dl.GetLine(stat.LineId)//line
                               select line.CopyToLineInStation(stat)).ToList();
            //select (BO.StationInLine)station.CopyPropertiesToNew(typeof(BO.StationInLine));
            //stationBO. = stations.OrderBy(s => s.LineStationIndex);
            return stationBO;
        }
        public IEnumerable<BO.Station> GetAllStations()
        {
            return from item in dl.GetAllStations()
                   select StationDoBoAdapter(item);
        }
        public void AddStation(BO.Station stat)
        {
            DO.Station statDO = new DO.Station();
            stat.CopyPropertiesTo(statDO);
            statDO.IsDeleted = false;
            try
            {
                dl.AddStation(statDO);
            }
            catch (DO.BadStationCodeException ex) 
            {
                throw new BO.BadStationCodeException(ex.stationCode, ex.Message);
            }
           
        }
        public void DeleteStation(int code)
        {
            try
            {
                DO.Station statDO = dl.GetStation(code);
                BO.Station statBO = StationDoBoAdapter(statDO);
                if (statBO.Lines.Count != 0)
                    throw new BO.BadStationCodeException(code, "station cant be deleted because other buses stop there");
                dl.DeleteStation(code);

            }
            catch(DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException(ex.stationCode, ex.Message);
            }
        }
        public void UpdateStation(BO.Station statBO)
        {
            try
            {
                DO.Station statDO = new DO.Station();
                statBO.CopyPropertiesTo(statDO);
                statDO.IsDeleted = false;
                dl.UpdateStation(statDO);
            }
            catch(BO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException(ex.stationCode, ex.Message);
            }
        }

        #endregion

        #region StationInLine
        public void UpdateTimeAndDistance(BO.StationInLine first, BO.StationInLine second)
        {
            try
            {
                DO.AdjacentStations adj = new DO.AdjacentStations() { StationCode1 = first.StationCode, StationCode2 = second.StationCode, Distance = first.Distance, Time = first.Time, IsDeleted = false };
                dl.UpdateAdjacentStations(adj);
            }
            catch (Exception ex)
            {
                throw new Exception("Error, it cannot be update");
            }
        }
        #endregion

        #region LineStation
        public void AddLineStation(BO.LineStation s)
        {
            DO.LineStation sDO = (DO.LineStation)s.CopyPropertiesToNew(typeof(DO.LineStation));
            //sDO.IsDeleted = false;
            try
            {
                dl.AddLineStation(sDO);
                List<DO.LineStation> lst = ((dl.GetAllLineStationsBy(stat => stat.LineId == sDO.LineId && stat.IsDeleted == false)).OrderBy(stat => stat.LineStationIndex)).ToList();
                //lst.Order
                
                //DO.LineStation prev = lst[s.LineStationIndex - 2];
                //DO.LineStation next = lst[s.LineStationIndex + 1];
                if (s.LineStationIndex != 1)//if its the first station- it doesnt have prev
                {
                    DO.LineStation prev = lst[s.LineStationIndex - 2];
                    if (!IsExistAdjacentStations(prev.StationCode, s.StationCode))
                    {
                        DO.AdjacentStations adjPrev = new DO.AdjacentStations() { StationCode1 = prev.StationCode, StationCode2 = s.StationCode };
                        dl.AddAdjacentStations(adjPrev);
                    }
                }
                if (s.LineStationIndex != lst[lst.Count - 1].LineStationIndex)//if its the last station- it doesnt have next
                {
                    DO.LineStation next = lst[s.LineStationIndex];
                    if (!IsExistAdjacentStations(s.StationCode, next.StationCode))
                    {
                        DO.AdjacentStations adjNext = new DO.AdjacentStations() { StationCode1 = s.StationCode, StationCode2 = next.StationCode };
                        dl.AddAdjacentStations(adjNext);
                    }
                }

            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        public void DeleteLineStation(int lineId, int stationCode)
        {
            try
            {    //AdjacentStation
                DO.LineStation stat = dl.GetLineStation(lineId, stationCode);
                if (stat == null)
                    throw new Exception("The station does not exist in this line");
                BO.Line line = GetLine(lineId);
                if (line == null)
                    throw new Exception("The line does not exist");
                if(line.Stations[0].StationCode!=stationCode && line.Stations[line.Stations.Count-1].StationCode!=stationCode)//if its not the first or the last station
                {
                    BO.StationInLine prev = line.Stations[stat.LineStationIndex - 2];
                    BO.StationInLine next = line.Stations[stat.LineStationIndex];
                    if(!dl.IsExistAdjacentStations(prev.StationCode, next.StationCode))
                    {
                        DO.AdjacentStations adj = new DO.AdjacentStations() { StationCode1 = prev.StationCode, StationCode2 = next.StationCode, IsDeleted = false };
                        dl.AddAdjacentStations(adj);
                    }
                }
                //delete the line station
                dl.DeleteLineStation(lineId, stationCode);
            }
            catch (Exception)
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
        //public void AddAdjacentStations(BO.AdjacentStation adjBO)
        //{
        //    try
        //    {
        //        DO.AdjacentStations adjDO = new DO.AdjacentStations();
        //        adjBO.CopyPropertiesTo(adjDO);
        //        dl.AddAdjacentStations(adjDO);
        //    }
        //    catch (Exception)
        //    {

        //    }

        //}

        #endregion

        #region User
        public void AddUser(BO.User userBO)
        {
            try
            {
                DO.User userDO = new DO.User();
                userBO.CopyPropertiesTo(userDO);
                userDO.IsDeleted = false;
                dl.AddUser(userDO);
            }
            catch(DO.BadUserNameException ex)
            {
                throw new BO.BadUserNameException(ex.userName, ex.Message);
            }
        }
        public BO.User SignIn(string username, int passcode)
        {
            BO.User userBO;
            try
            {
                DO.User userDO = dl.GetUser(username);
                if (passcode != userDO.Passcode)
                    throw new BO.BadUserNameException(username, "The passcode is wrong");
                userBO = new BO.User();
                userDO.CopyPropertiesTo(userBO);
            }
            catch (DO.BadUserNameException ex)
            {
                throw new BO.BadUserNameException(ex.userName, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR");
            }
            return userBO;
        }


        #endregion


    }
}
