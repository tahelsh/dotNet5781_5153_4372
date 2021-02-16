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

        #region singelton
        static readonly BLImp instance = new BLImp();
        static BLImp() { }// static ctor to ensure instance init is done just before first usage
        BLImp() { } // default => private
        public static BLImp Instance { get => instance; }// The public Instance property to use
        #endregion

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
                throw new BO.BadLicenseNumException(ex.Message,ex);
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
                throw new BO.BadLicenseNumException(ex.Message, ex);
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
                throw new BO.BadLicenseNumException(ex.Message, ex);
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
                if (busDO.FromDate > DateTime.Now)
                    throw new BO.BadInputException("The date of start operation is not valid");
                if (busDO.TotalTrip < 0)
                    throw new BO.BadInputException("The total trip is not valid");
                if (busDO.FuelRemain < 0 || bus.FuelRemain > 1200)
                    throw new BO.BadInputException("The fuel remain is not valid");
                int lengthLicNum = LengthLicenseNum(busDO.LicenseNum);
                if (!((lengthLicNum == 7 && bus.FromDate.Year < 2018) || (lengthLicNum == 8 && busDO.FromDate.Year >= 2018)))
                    throw new BO.BadInputException("The license number and the date of start operation do not match");
                if (busDO.DateLastTreat > DateTime.Now || busDO.DateLastTreat < busDO.FromDate)
                    throw new BO.BadInputException("The date of last treatment is not valid");
                if (busDO.KmLastTreat < 0 || busDO.KmLastTreat > busDO.TotalTrip)
                    throw new BO.BadInputException("The kilometrage of last treatment is not valid");
                dl.AddBus(busDO);
            }
            catch (DO.BadLicenseNumException ex)
            {
                throw new BO.BadLicenseNumException(ex.Message, ex);
            }
        }
        private int LengthLicenseNum(int licenseNum)
        {
            int counter = 0;
            while (licenseNum != 0)
            {
                licenseNum = licenseNum / 10;
                counter++;
            }
            return counter;
        }

        public void RefuelBus(BO.Bus busBO)
        {
            try
            {
                DO.Bus busDO = dl.GetBus(busBO.LicenseNum);
                if (busDO.FuelRemain == 1200)
                    throw new BO.BadInputException("The fuel remain of the bus ia already full");
                busDO.FuelRemain = 1200;
                dl.UpdateBus(busDO);

            }
            catch(DO.BadLicenseNumException ex)
            {
                throw new BO.BadLicenseNumException(ex.Message, ex);
            }
        }
        public void TreatBus(BO.Bus busBO)
        {
            try
            {
                DO.Bus busDO = dl.GetBus(busBO.LicenseNum);
                if (busDO.DateLastTreat.ToShortDateString() == DateTime.Now.ToShortDateString())
                    throw new BO.BadInputException("The bus is already treated today");
                busDO.DateLastTreat = DateTime.Now;
                busDO.KmLastTreat = busDO.TotalTrip;
                dl.UpdateBus(busDO);

            }
            catch (DO.BadLicenseNumException ex)
            {
                throw new BO.BadLicenseNumException(ex.Message, ex);
            }
        }
        #endregion

        #region Line
        BO.Line lineDoBoAdapter(DO.Line lineDO)
        {
            BO.Line lineBO = new BO.Line();
            int lineId = lineDO.LineId;
            lineDO.CopyPropertiesTo(lineBO);
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
            lineBO.DepTimes= (from lineTrip in dl.GetAllLineTripsBy(lineTrip => lineTrip.LineId == lineId && lineTrip.IsDeleted == false)//LineTrip
                              select lineTrip.StartAt).OrderBy(s=>s.TotalMinutes).ToList();
            return lineBO;

        }
        public Line GetLine(int lineId)
        {
            DO.Line lineDO;
            try
            {
                lineDO = dl.GetLine(lineId);
            }
            catch (DO.BadTripIdException ex)
            {
                throw new BO.BadLineIdException(ex.Message, ex);
            }
            return lineDoBoAdapter(lineDO);
        }
        public void AddNewLine(BO.Line lineBo)
        {
            DO.Line lineDo = new DO.Line();
            lineBo.CopyPropertiesTo(lineDo);
            lineDo.FirstStation = lineBo.Stations[0].StationCode;
            lineDo.LastStation = lineBo.Stations[lineBo.Stations.Count-1].StationCode;
            
            //בודק אם יש קו עם אותו מספר עם אוה תחנה סופית באותו איזור
            List<DO.Line> ltemp = dl.GetAllLinesBy(s => s.LineNum == lineDo.LineNum && s.LastStation==lineDo.LastStation && s.Area==lineDo.Area).ToList();
            if(ltemp.Count()!=0)
                throw new BO.BadInputException($"There is already a line with the number {lineDo.LineNum} in {lineDo.Area} with last station {lineDo.LastStation}");
            dl.AddLine(lineDo);//add line
            //עידכונים של תחנות עוקבות ותחנות קו
            lineDo.LineId = dl.GetAllLinesBy(s => s.LineNum == lineDo.LineNum && s.Area == lineDo.Area).FirstOrDefault().LineId;
            int sc1 = lineBo.Stations[0].StationCode;//stationCode of the first station
            int sc2 = lineBo.Stations[1].StationCode;//station Code of the last station
            lineDo.FirstStation = sc1;
            lineDo.LastStation = sc2;
            try
            {
                if (!dl.IsExistAdjacentStations(sc1, sc2))//add to adjcenct stations if its not exists
                {
                    DO.AdjacentStations adj = new DO.AdjacentStations() { StationCode1 = sc1, StationCode2 = sc2, Distance = lineBo.Stations[0].Distance, Time = lineBo.Stations[0].Time };
                    dl.AddAdjacentStations(adj);
                }
                DO.LineStation first = new DO.LineStation() { LineId = lineDo.LineId, StationCode = sc1, LineStationIndex = lineBo.Stations[0].LineStationIndex, IsDeleted = false, PrevStationCode=0, NextStationCode=sc2 };
                DO.LineStation last = new DO.LineStation() { LineId = lineDo.LineId, StationCode = sc2, LineStationIndex = lineBo.Stations[1].LineStationIndex, IsDeleted = false, PrevStationCode=sc1, NextStationCode=0 };
                dl.AddLineStation(first);//add first line station
                dl.AddLineStation(last);//add last line ststion
            }
            catch (DO.BadTripIdException ex)
            {
                throw new BO.BadLineIdException(ex.Message, ex);
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
            lineDO.FirstStation = line.Stations[0].StationCode;
            lineDO.LastStation = line.Stations[line.Stations.Count - 1].StationCode;
            try
            {
                dl.UpdateLine(lineDO);
            }
            catch (DO.BadTripIdException ex)
            {
                throw new BO.BadLineIdException(ex.Message, ex);
            }
        }

        public void DeleteLine(int lineId)
        {
            try
            {
                dl.DeleteLine(lineId);
                //delete fron the line station list
                List<DO.LineStation> listLineStations = dl.GetAllLineStationsBy(s => s.LineId == lineId).ToList();
                foreach (DO.LineStation s in listLineStations)
                {
                    dl.DeleteLineStation(s.LineId, s.StationCode);
                }
            }
            catch (DO.BadTripIdException ex)
            {
                throw new BO.BadLineIdException(ex.Message, ex);
            }

        }

        private int IsStationFound(BO.Line line, int stationCode)
        {
            foreach (BO.StationInLine stat in line.Stations)
            {
                if (stat.StationCode == stationCode)
                    return stat.LineStationIndex;
            }
            return -1;
        }
        private TimeSpan TimeTravel(BO.Line line, int stationCode1, int stationCode2)
        {
            int index1 = IsStationFound(line, stationCode1);
            int index2 = IsStationFound(line, stationCode2);
            TimeSpan sum = TimeSpan.Zero;
            for(int i=index1-1; i<index2-1; i++)
            {
                sum += line.Stations[i].Time;
            }
            return sum;
        }
        public List<string> FindRoute(int stationCode1, int stationCode2)
        {
            if(stationCode1==stationCode2)
                throw new BO.BadInputException("The source station and the destination station must be different");
            List<string> linesInRoute = new List<string>();//list of lines with line number and time travel
            List<BO.Line> linesInRouteTemp = new List<BO.Line>();//list of lines that pass on the two stations-temp list
            List<BO.Line> allLines = GetAllLines().ToList();//all lines
            foreach (BO.Line line in allLines)
            {

                int index1 = IsStationFound(line, stationCode1);
                int index2 = IsStationFound(line, stationCode2);
                if (index1 != -1 && index2 != -1 && index1 < index2)
                {
                    linesInRouteTemp.Add(line);
                }
            }
            if (linesInRouteTemp.Count == 0)
                throw new BO.BadInputException("There are no lines in this route");
            linesInRouteTemp = linesInRouteTemp.OrderBy(l => TimeTravel(l, stationCode1, stationCode2)).ToList();
            foreach (BO.Line line in linesInRouteTemp)
            {
                linesInRoute.Add(line.LineNum + "\t" + TimeTravel(line, stationCode1, stationCode2));
            }
            return linesInRoute;
        }
        #endregion

        #region Station
        public BO.Station StationDoBoAdapter(DO.Station stationDO)
        {
            BO.Station stationBO = new BO.Station();
            int stationCode = stationDO.Code;
            stationDO.CopyPropertiesTo(stationBO);
            stationBO.Lines = (from stat in dl.GetAllLineStationsBy(stat => stat.StationCode == stationCode && stat.IsDeleted == false)//Linestation
                               let line = dl.GetLine(stat.LineId)//line
                               select line.CopyToLineInStation(stat)).ToList();
            
            foreach (BO.LineInStation item in stationBO.Lines)//לאתחל תחנה סופית
            {
                var line = dl.GetLine(item.LineId);
                var station = dl.GetStation(line.LastStation);
                item.NameLastStation = station.Name;
                //item.area = (BO.Area)Enum.Parse(typeof(BO.Area), line.Area.ToString());
            }
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
                throw new BO.BadStationCodeException(ex.Message, ex);
            }

        }
        public void DeleteStation(int code)
        {
            try
            {
                DO.Station statDO = dl.GetStation(code);
                BO.Station statBO = StationDoBoAdapter(statDO);
                if (statBO.Lines.Count != 0)//if there are lines that stop in the station
                    throw new BO.BadStationCodeException(code,"station cant be deleted because other buses stop there");
                dl.DeleteStation(code);
                List<DO.AdjacentStations> listAdj = dl.GetAllAdjacentStations().ToList();
                foreach (DO.AdjacentStations s in listAdj)//delete from adjacent Station list
                {
                    if (s.StationCode1 == code || s.StationCode2 == code)
                        dl.DeleteAdjacentStations(s.StationCode1, s.StationCode2);
                }

            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException(ex.Message, ex);
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
            catch (BO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException(ex.Message, ex);
            }
        }
        #region Simulator

        public IEnumerable<LineTiming> GetLineTimingPerStation(BO.Station stationBO, TimeSpan currentTime)
        {
            //list of lines that pass in the station
            List<BO.Line> listLines = (from l in GetAllLines()
                                      where l.Stations.Find(s => s.StationCode == stationBO.Code) != null
                                      select l).ToList();
           
            List<LineTiming> times = new List<LineTiming>();//list of the lines that the function will return
            TimeSpan hour = new TimeSpan(1, 0, 0);//help to find the times that in the range of one hour from currentTime                           
            for (int i = 0; i < listLines.Count(); i++)//for all the lines that pass in the station
            {//calculate the times 
                TimeSpan tmp;//the current time
                int currentLineid = listLines[i].LineId;// line id of the current line
                List<DO.LineTrip> lineSchedual = dl.GetAllLineTripsBy(trip=>trip.LineId==currentLineid && trip.IsDeleted==false ).ToList();// times of the current Line
                TimeSpan timeTilStatin = travelTime(stationBO.Code, currentLineid);
                int numOfTimes = 0;
                List<int> timesOfCurrentLine = new List<int>();
                for (int j = 0; j < lineSchedual.Count && numOfTimes < 3 ; j++)//for all the times in line sSchedual
                {
                    //check if currentTime-LeavingTime-travelTime more than zero and in the range of hour
                    if (lineSchedual[j].StartAt + timeTilStatin <= currentTime + hour
                        && lineSchedual[j].StartAt + timeTilStatin >= currentTime)
                    //check if the bus already passed the statioin   
                    {
                        if (currentTime - lineSchedual[j].StartAt >= TimeSpan.Zero)//if the line already get out from the station
                        {
                            tmp = timeTilStatin - (currentTime - lineSchedual[j].StartAt);
                        }
                        else//if the line didnt get out from the station
                            tmp = timeTilStatin+ (lineSchedual[j].StartAt - currentTime);
                    
                        timesOfCurrentLine.Add(tmp.Minutes);
                        //timesString = timesString + tmp.Minutes+ ", ";                                                                                                  
                        numOfTimes++;
                    }
                }

                if (timesOfCurrentLine.Count != 0)
                {
                    string timesString = "";//the string of times
                    timesOfCurrentLine = timesOfCurrentLine.OrderBy(s => s).ToList();//order the times in ascending order
                    for (int k=0; k< timesOfCurrentLine.Count-1; k++)
                    {
                        timesString = timesString + timesOfCurrentLine[k] + ", ";
                    }
                    timesString= timesString + timesOfCurrentLine[timesOfCurrentLine.Count - 1] ;//add the last one without ","
                    times.Add(new LineTiming
                    {
                        LineId = currentLineid,
                        LineNum = listLines[i].LineNum,
                        DestinationStation = listLines[i].Stations[listLines[i].Stations.Count() - 1].Name,
                        Stringtimes = timesString,                                     
                    }) ;

                }
                numOfTimes = 0;
                //timesString = "";
            }
            times = times.OrderBy(lt => lt.LineNum).ToList();//order the list by the number of the lines in ascending order
            return times;

        }

        private TimeSpan travelTime(int stationCode, int lineID)
        {//func that return the time from first station in line to specific station
            TimeSpan sumTime = TimeSpan.Zero;
            BO.Line line = GetLine(lineID);
            foreach (var s in line.Stations)
            {
                if (s.StationCode != stationCode)
                    sumTime += s.Time;
                else
                {
                    //sumTime += s.TimeFromPrevStation;
                    break;
                }
            }
            return sumTime;
        }

        //public IEnumerable<LineTiming> GetLineTimingPerStation(BO.Station stationBO, TimeSpan currentTime)
        //{
        //    //list of lines that pass in the station
        //    List<BO.Line> listLines = (from l in GetAllLines()
        //                               where l.Stations.Find(s => s.StationCode == stationBO.Code) != null
        //                               select l).ToList();

        //    List<LineTiming> times = new List<LineTiming>();
        //    TimeSpan hour = new TimeSpan(1, 0, 0);//help to find the times that in the range of one hour from currentTime                           

        //    for (int i = 0; i < listLines.Count(); i++)//for all the lines that pass in the station
        //    {//calculate the times 
        //        TimeSpan tmp;//= TimeSpan.Zero;
        //        int currentLineid = listLines[i].LineId;// line id of the current line
        //        List<DO.LineTrip> lineSchedual = dl.GetAllLineTripsBy(trip => trip.LineId == currentLineid && trip.IsDeleted == false).ToList();// times of the current Line
        //        TimeSpan timeTilStatin = travelTime(stationBO.Code, currentLineid);
        //        for (int j = 0; j < lineSchedual.Count; j++)//for all the times in line sSchedual
        //        {//check if currentTime-LeavingTime-travelTime more than zero and in the range of hour

        //            if (lineSchedual[j].StartAt + timeTilStatin <= currentTime + hour
        //                && lineSchedual[j].StartAt + timeTilStatin >= currentTime)
        //            //check if the bus already passed the statioin   
        //            {
        //                if (currentTime - lineSchedual[j].StartAt >= TimeSpan.Zero)//if the line already get out from the station
        //                {
        //                    tmp = timeTilStatin - (currentTime - lineSchedual[j].StartAt);
        //                }
        //                else//if the line didnt get out from the station
        //                    tmp = timeTilStatin + (lineSchedual[j].StartAt - currentTime);
        //                //creat LineTiming and add to the list
        //                times.Add(new LineTiming
        //                {
        //                    LineId = currentLineid,
        //                    LineNum = listLines[i].LineNum,
        //                    DestinationCode = listLines[i].Stations[listLines[i].Stations.Count() - 1].StationCode,
        //                    SourceCode = listLines[i].Stations[0].StationCode,
        //                    EstimatedTime = TimeSpan.Parse(tmp.ToString().Substring(0, 8))
        //                });

        //            }
        //        }
        //    }
        //    times = times.OrderBy(lt => lt.EstimatedTime).ToList();
        //    return times;

        //}

        //private TimeSpan travelTime(int stationCode, int lineID)
        //{//func that return the time from first station in line to specific station
        //    TimeSpan sumTime = TimeSpan.Zero;
        //    BO.Line line = GetLine(lineID);
        //    foreach (var s in line.Stations)
        //    {
        //        if (s.StationCode != stationCode)
        //            sumTime += s.Time;
        //        else
        //        {
        //            //sumTime += s.TimeFromPrevStation;
        //            break;
        //        }
        //    }
        //    return sumTime;
        //}


        #endregion

        #endregion

        #region StationInLine
        public void UpdateTimeAndDistance(BO.StationInLine first, BO.StationInLine second)
        {
            try
            {
                DO.AdjacentStations adj = new DO.AdjacentStations() { StationCode1 = first.StationCode, StationCode2 = second.StationCode, Distance = first.Distance, Time = first.Time, IsDeleted = false };
                dl.UpdateAdjacentStations(adj);
            }
            catch (DO.BadAdjacentStationsException ex)
            {
                throw new BO.BadAdjacentStationsException( ex.Message, ex);
            }
            //{
            //    throw new Exception("Error, it cannot be update");
            //}
        }
        #endregion

        #region LineStation
        public bool IsExistLineStation(DO.LineStation s)
        {
            try
            {
                DO.LineStation linestation = dl.GetLineStation(s.LineId, s.StationCode);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public void AddLineStation(BO.LineStation s)
        {
            DO.LineStation sDO = (DO.LineStation)s.CopyPropertiesToNew(typeof(DO.LineStation));
            try
            {
                if (IsExistLineStation(sDO))
                    throw new BO.BadLineStationException(sDO.LineId, sDO.StationCode, "The station is already exist in the line");
                //עידכון של כל האינדקסים של התחנות הבאות בקו
                List<DO.LineStation> lSList = ((dl.GetAllLineStationsBy(stat => stat.LineId == sDO.LineId && stat.IsDeleted == false)).OrderBy(stat => stat.LineStationIndex)).ToList();
                int indexlast = lSList[lSList.Count - 1].LineStationIndex;
                if (sDO.LineStationIndex != indexlast+1)//if we didnt add a last station
                {
                    for (int i = sDO.LineStationIndex; i < indexlast+1; i++)
                    {
                        lSList[i - 1].LineStationIndex++;
                    }
                }
                //עידכון תחנה קודמת והבאה וגם את התחנה הראשונה והאחרונה של היישות קו
                DO.LineStation prev;
                DO.LineStation next;
                if (sDO.LineStationIndex > 1)//its not the first station, update the prev station
                {
                    prev = lSList[sDO.LineStationIndex - 2];
                    prev.NextStationCode = sDO.StationCode;
                    sDO.PrevStationCode = prev.StationCode;
                }
                else//if its the first station-we need to update the first station in the DO.Line
                {
                    DO.Line line = dl.GetLine(sDO.LineId);
                    line.FirstStation = sDO.StationCode;
                    dl.UpdateLine(line);
                }
                if (sDO.LineStationIndex != indexlast+1)//if its not the last station, update the next station
                {
                    next = lSList[sDO.LineStationIndex-1];
                    next.PrevStationCode = sDO.StationCode;
                    sDO.NextStationCode = next.StationCode;
                }
                else//if its the last station we need to update the last station in the DO.Line
                {
                    DO.Line line = dl.GetLine(sDO.LineId);
                    line.LastStation = sDO.StationCode;
                    dl.UpdateLine(line);
                }
                foreach (DO.LineStation item in lSList)//update the line station list after all the changes
                {
                    dl.UpdateLineStation(item);
                }

                dl.AddLineStation(sDO);

                //טיפול בתחנות עוקבות לאחר ההוספה
                List<DO.LineStation> lst = ((dl.GetAllLineStationsBy(stat => stat.LineId == sDO.LineId && stat.IsDeleted == false)).OrderBy(stat => stat.LineStationIndex)).ToList();
                if (s.LineStationIndex != 1)//if its the first station- it doesnt have prev
                {
                    prev = lst[s.LineStationIndex - 2];
                    if (!IsExistAdjacentStations(prev.StationCode, s.StationCode))
                    {
                        DO.AdjacentStations adjPrev = new DO.AdjacentStations() { StationCode1 = prev.StationCode, StationCode2 = s.StationCode };
                        dl.AddAdjacentStations(adjPrev);
                    }
                }
                if (s.LineStationIndex != lst[lst.Count - 1].LineStationIndex)//if its the last station- it doesnt have next
                {
                    next = lst[s.LineStationIndex];
                    if (!IsExistAdjacentStations(s.StationCode, next.StationCode))
                    {
                        DO.AdjacentStations adjNext = new DO.AdjacentStations() { StationCode1 = s.StationCode, StationCode2 = next.StationCode };
                        dl.AddAdjacentStations(adjNext);
                    }
                }

            }
            catch (DO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException( ex.Message, ex);
            }
            catch (DO.BadAdjacentStationsException ex)
            {
                throw new BO.BadAdjacentStationsException(ex.Message, ex);
            }
        }
        public void DeleteLineStation(int lineId, int stationCode)
        {
            try
            {  
                DO.LineStation statDel = dl.GetLineStation(lineId, stationCode);//the station that we want to delete
                BO.Line line = GetLine(lineId);
                if (line.Stations.Count <= 2)//אם יש 2 תחנות בקו אי אפשר למחוק
                    throw new BO.BadInputException("The Station cannot be deleted, there is only 2 stations in the line route");

                //AdjacentStation
                if (line.Stations[0].StationCode != stationCode && line.Stations[line.Stations.Count - 1].StationCode != stationCode)//if its not the first or the last station
                {
                    BO.StationInLine prev = line.Stations[statDel.LineStationIndex - 2];
                    BO.StationInLine next = line.Stations[statDel.LineStationIndex];
                    if (!dl.IsExistAdjacentStations(prev.StationCode, next.StationCode))
                    {
                        DO.AdjacentStations adj = new DO.AdjacentStations() { StationCode1 = prev.StationCode, StationCode2 = next.StationCode, IsDeleted = false };
                        dl.AddAdjacentStations(adj);
                    }
                }
                //delete the line station
                List<DO.LineStation> lSList = ((dl.GetAllLineStationsBy(stat => stat.LineId == statDel.LineId && stat.IsDeleted == false)).OrderBy(stat => stat.LineStationIndex)).ToList();
                DO.LineStation NextFind;
                if (statDel.LineStationIndex > 1)//if its not the first station
                {
                    DO.LineStation PrevFind = lSList[statDel.LineStationIndex - 2];
                    if(statDel.LineStationIndex!=lSList[lSList.Count-1].LineStationIndex)//if its not the last station
                    {
                        NextFind = lSList[statDel.LineStationIndex];
                        PrevFind.NextStationCode = NextFind.StationCode;
                        NextFind.PrevStationCode = PrevFind.StationCode;
                    }
                    else
                    {
                        PrevFind.NextStationCode = 0;
                    }
                }
                else//if its the first station
                {
                    if (statDel.LineStationIndex != lSList[lSList.Count - 1].LineStationIndex)
                    {
                        NextFind = lSList[statDel.LineStationIndex];
                        NextFind.PrevStationCode = 0;
                    }
                }
                //update index;
                if (statDel.LineStationIndex != lSList[lSList.Count - 1].LineStationIndex)//update all the indexes of all the next stations if its not the last station
                {
                    for(int i=statDel.LineStationIndex; i< lSList.Count; i++)
                    {
                        lSList[i].LineStationIndex--;
                    }
                }
                foreach(DO.LineStation item in lSList)
                {
                    dl.UpdateLineStation(item);
                }

                dl.DeleteLineStation(lineId, stationCode);
            }
            catch (DO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException(ex.Message, ex);
            }
            catch (BO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException(ex.Message, ex);
            }
            catch (DO.BadAdjacentStationsException ex)
            {
                throw new BO.BadAdjacentStationsException(ex.Message, ex);
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
            catch (DO.BadUserNameException ex)
            {
                throw new BO.BadUserNameException(ex.Message, ex);
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
                throw new BO.BadUserNameException(ex.Message, ex);
            }
            return userBO;
        }


        #endregion

        #region LineTrip
        public void DeleteDepTime(int lineId, TimeSpan dep)
        {
            try
            {
                dl.DeleteLineTrip(lineId, dep);
            }
            catch(DO.BadLineTripException ex)
            {
                throw new BO.BadLineTripException( ex.Message, ex);
            }
        }
        public void AddDepTime(int lineId, TimeSpan dep)
        {
            try
            {
                dl.AddLineTrip(new DO.LineTrip() { LineId = lineId,StartAt= dep, IsDeleted=false});
            }
            catch (DO.BadLineTripException ex)
            {
                throw new BO.BadLineTripException(ex.Message, ex);
            }
        }
        #endregion


    }
}
