using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
using DO;
using DS;


namespace DL
{
    public class DLObject:IDL
    {
        #region singelton
        static readonly DLObject instance = new DLObject();
        static DLObject() { }// static ctor to ensure instance init is done just before first usage
        DLObject() { } // default => private
        public static DLObject Instance { get => instance; }// The public Instance property to use
        #endregion

        #region Bus
        public IEnumerable<DO.Bus> GetAllBuses()
        {
            return from bus in DataSource.ListBuses
                   where bus.IsDeleted == false
                   select bus.Clone();
        }
        public IEnumerable<DO.Bus> GetAllBusesBy(Predicate<DO.Bus> predicate)
        {
            return from bus in DataSource.ListBuses
                   where predicate(bus)
                   select bus.Clone();
        }
        public DO.Bus GetBus(int licenseNum)
        {
            DO.Bus bus = DataSource.ListBuses.Find(b => b.LicenseNum == licenseNum && b.IsDeleted==false);

            if (bus != null)
                return bus.Clone();
            else
                throw new BadLicenseNumException(licenseNum,"The bus does not exist");
        }
        public void AddBus(DO.Bus bus)
        { 
            if (DataSource.ListBuses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum && b.IsDeleted == false) != null)
                throw new BadInputException("The bus is already exists");
            if (bus.FromDate > DateTime.Now)
                throw new BadInputException("The date of start operation is not valid");
            if(bus.TotalTrip<0)
                throw new BadInputException("The total trip is not valid");
            if(bus.FuelRemain<0 || bus.FuelRemain>1200)
                throw new BadInputException("The fuel remain is not valid");
            int lengthLicNum = LengthLicenseNum(bus.LicenseNum);
            if (!((lengthLicNum == 7 && bus.FromDate.Year < 2018) || (lengthLicNum == 8 && bus.FromDate.Year >= 2018)))
                throw new BadInputException("The license number and the date of start operation do not match");
            if (bus.DateLastTreat > DateTime.Now || bus.DateLastTreat < bus.FromDate)
                throw new BadInputException("The date of last treatment is not valid");
            if (bus.KmLastTreat < 0 || bus.KmLastTreat > bus.TotalTrip)
                throw new BadInputException("The kilometrage of last treatment is not valid");
            DataSource.ListBuses.Add(bus.Clone());
        }
        private int LengthLicenseNum(int licenseNum)
        {
            int counter = 0;
            while(licenseNum!=0)
            {
                licenseNum = licenseNum / 10;
                counter++;
            }
            return counter;
        }
       
        public void UpdateBus(DO.Bus bus)
        {
            DO.Bus busFind = DataSource.ListBuses.Find(b => b.LicenseNum == bus.LicenseNum && b.IsDeleted == false);
            if (busFind == null)
                throw new BadLicenseNumException(bus.LicenseNum, "The bus does not exist");
            DO.Bus newBus = bus.Clone();
            DataSource.ListBuses.Remove(busFind);
            DataSource.ListBuses.Add(newBus);

            //DO.Bus newBus = bus.Clone();//copy of the bus that the function got
            //busFind = newBus;//update
        }
        public void UpdateBus(int licenseNum, Action<DO.Bus> update)
        {
            DO.Bus busFind = DataSource.ListBuses.Find(b => b.LicenseNum == licenseNum && b.IsDeleted == false);
            if (busFind == null)
                throw new BadLicenseNumException(licenseNum, "The bus does not exist");
            update(busFind);
        }
        public void DeleteBus(int licenseNum)
        {
            DO.Bus bus = DataSource.ListBuses.Find(b => b.LicenseNum == licenseNum && b.IsDeleted == false);
            if(bus==null)
                throw new BadLicenseNumException(licenseNum, "The bus does not exist");
            bus.IsDeleted = true;//delete

        }
        #endregion

        #region AdjacentStations
        public bool IsExistAdjacentStations(int stationCode1, int stationCode2)
        {
            DO.AdjacentStations adjStations = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1 && adj.StationCode2 == stationCode2 && adj.IsDeleted == false));
            if (adjStations != null)
                return true;
            return false;
        }
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStations()
        {
            return from adj in DataSource.ListAdjacentStations
                   select adj.Clone();
        }
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate)
        {
            return from adj in DataSource.ListAdjacentStations
                   where predicate(adj)
                   select adj.Clone();
        }
        public DO.AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2)
        {
            DO.AdjacentStations adjStations = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1 && adj.StationCode2 == stationCode2 && adj.IsDeleted==false));

            if (adjStations == null)
                throw new DO.BadAdjacentStationsException(stationCode1,stationCode2, "The adjacent stations does not exist");
            return adjStations.Clone();
   
        }
        public void AddAdjacentStations(DO.AdjacentStations adjStations)
        {
            if (DataSource.ListAdjacentStations.FirstOrDefault(adj => (adj.StationCode1 == adjStations.StationCode1 && adj.StationCode2==adjStations.StationCode2 && adj.IsDeleted == false)) != null)//if those adjacent stations already exist in the list
                throw new BadAdjacentStationsException(adjStations.StationCode1, adjStations.StationCode2,"The adjacent stations are already exist");
            DataSource.ListAdjacentStations.Add(adjStations.Clone());
        }
        public void UpdateAdjacentStations(DO.AdjacentStations adjStations)
        {
            DO.AdjacentStations adjFind = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == adjStations.StationCode1 && adj.StationCode2 == adjStations.StationCode2 && adj.IsDeleted == false || adj.StationCode1 == adjStations.StationCode2 && adj.StationCode2 == adjStations.StationCode1 && adj.IsDeleted == false));
            if (adjFind == null)
                throw new BadAdjacentStationsException(adjStations.StationCode1, adjStations.StationCode2, "The adjacent stations does not exist");
            DO.AdjacentStations newAdj = adjStations.Clone();//copy of the bus that the function got
            DataSource.ListAdjacentStations.Remove(adjFind);
            DataSource.ListAdjacentStations.Add(newAdj);
        }
        public void UpdateAdjacentStations(int stationCode1, int stationCode2, Action<DO.AdjacentStations> update)
        {
            DO.AdjacentStations adjFind = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1 && adj.StationCode2 == stationCode2  && adj.IsDeleted == false));
            if (adjFind == null)
                throw new BadAdjacentStationsException(stationCode1, stationCode2, "The adjacent stations does not exist");
            update(adjFind);
        }
        public void DeleteAdjacentStations(int stationCode1, int stationCode2)
        {
            DO.AdjacentStations adjFind = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1 && adj.StationCode2 == stationCode2 && adj.IsDeleted == false ));
            if (adjFind == null)
                throw new BadAdjacentStationsException(stationCode1, stationCode2, "The adjacent stations does not exist");
            adjFind.IsDeleted = true;
        }

        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {
            return from line in DataSource.ListLines
                   where line.IsDeleted == false
                   select line.Clone();

        }
        public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)
        {
            return from line in DataSource.ListLines
                   where predicate(line)
                   select line.Clone();

        }
        public DO.Line GetLine(int lineId)
        {
            DO.Line line = DataSource.ListLines.Find(l => l.LineId == lineId && l.IsDeleted == false);

            if (line != null)
                return line.Clone();
            else
                throw new BadLineIdException(lineId, "The Line ID does not exist");
        }
        public void AddLine(DO.Line line)
        {
            //line.LineId = Config.LineId++;
            if (DataSource.ListLines.FirstOrDefault(l => l.LineId == line.LineId && l.IsDeleted == false) != null)
                throw new BadLineIdException(line.LineId, "The Line ID is already exist exist");
            DataSource.ListLines.Add(line.Clone());
        }
        public void UpdateLine(DO.Line line)
        {
            DO.Line lineFind = DataSource.ListLines.Find(l => l.LineId == line.LineId && l.IsDeleted == false);
            if (lineFind == null)
                throw new BadLineIdException(line.LineId, "The Line ID does not exist");
            DO.Line newLine = line.Clone();//copy of the bus that the function got
            DataSource.ListLines.Remove(lineFind);
            DataSource.ListLines.Add(newLine);
        }
        public void UpdateLine(int lineId, Action<DO.Line> update)
        {
            DO.Line lineFind = DataSource.ListLines.Find(l => l.LineId == lineId && l.IsDeleted == false);
            if (lineFind == null)
                throw new BadLineIdException(lineId, "The Line ID does not exist");
            update(lineFind);
        }
        public void DeleteLine(int lineId)
        {
            DO.Line lineFind = DataSource.ListLines.Find(l => l.LineId == lineId && l.IsDeleted == false);
            if (lineFind == null)
                throw new BadLineIdException(lineId, "The Line ID does not exist");
            lineFind.IsDeleted = true;
            foreach(DO.LineStation s in DataSource.ListLineStations)//delete fron the line station list
            {
                if (s.LineId == lineId && s.IsDeleted == false)
                    s.IsDeleted = true;
            }
            //foreach(DO.LineTrip l in DataSource.ListLineTrips)//delete fron line trip list
            //{
            //    if(l.LineId==lineId && l.IsDeleted == false)
            //        l.IsDeleted = true;
            //}
        }

        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetAllLineStations()
        {
            return from lStat in DataSource.ListLineStations
                   select lStat.Clone();
        }
        public IEnumerable<DO.LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate)
        {
            return from lStat in DataSource.ListLineStations
                   where predicate(lStat)
                   select lStat.Clone();
        }
        public DO.LineStation GetLineStation(int lineId, int stationCode)
        {
            DO.LineStation lineStation = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineId && lStat.StationCode == stationCode));

            if (lineStation != null)
                return lineStation.Clone();
            else
                throw new DO.BadLineStationException(lineId, stationCode,"The station line does not exist");
        }
        public void AddLineStation(DO.LineStation lineStation)
        {
           if (DataSource.ListLineStations.FirstOrDefault(lStat => (lStat.LineId == lineStation.LineId && lStat.StationCode == lineStation.StationCode && lStat.IsDeleted==false)) != null)//if this line station already exists in the list
                throw new DO.BadLineStationException(lineStation.LineId, lineStation.StationCode, "The station line does not exist");
            //update the line station index of all the next station
            DO.LineStation next= DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.LineStationIndex== lineStation.LineStationIndex && lStat.IsDeleted == false));
            DO.LineStation temp;
            int i;
            while (next != null)
            {
                
                //temp = next;
                i = next.LineStationIndex + 1;
                temp = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.LineStationIndex == i && lStat.IsDeleted == false));
                ++next.LineStationIndex;
                next = temp;
            }
          
            //update prev and next
            DO.LineStation prev = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.LineStationIndex == lineStation.LineStationIndex-1 && lStat.IsDeleted == false));
            next = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.LineStationIndex == lineStation.LineStationIndex+1 && lStat.IsDeleted == false));
            if(prev!=null)
            {
                prev.NextStationCode = lineStation.StationCode;
                lineStation.PrevStationCode = prev.StationCode;
            }
            if(next!=null)
            {
                lineStation.NextStationCode = next.StationCode;
                next.PrevStationCode = lineStation.StationCode;
            }
            DataSource.ListLineStations.Add(lineStation.Clone());
        }
        public void UpdateLineStation(DO.LineStation lineStation)
        {
            DO.LineStation lStatFind = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.StationCode == lineStation.StationCode && lStat.IsDeleted == false));
            if (lStatFind == null)
                throw new DO.BadLineStationException(lineStation.LineId, lineStation.StationCode, "The station line does not exist");
            DO.LineStation newAdj = lineStation.Clone();//copy of the bus that the function got
            lStatFind = newAdj;//update
        }
        public void UpdateLineStation(int lineId, int stationCode, Action<DO.LineStation> update)
        {
            DO.LineStation lStatFind = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineId && lStat.StationCode == stationCode && lStat.IsDeleted == false));
            if (lStatFind == null)
                throw new DO.BadLineStationException(lineId, stationCode, "The station line does not exist");
            update(lStatFind);
        }
        public void DeleteLineStation(int lineId, int stationCode)
        {
            DO.LineStation lStatFind = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineId && lStat.StationCode == stationCode && lStat.IsDeleted==false));
            if (lStatFind == null)
                throw new DO.BadLineStationException(lineId, stationCode, "The station line does not exist");
            lStatFind.IsDeleted = true;
            DO.LineStation NextFind;
            if (lStatFind.LineStationIndex>1)
            {
                DO.LineStation PrevFind= DataSource.ListLineStations.Find(prev => (prev.LineId == lineId && prev.LineStationIndex == lStatFind.LineStationIndex - 1 && prev.IsDeleted == false));
                NextFind = DataSource.ListLineStations.Find(next => (next.LineId == lineId  && next.LineStationIndex == lStatFind.LineStationIndex+1 && next.IsDeleted == false));
                if (NextFind != null)//if its not the last station
                {
                    PrevFind.NextStationCode = NextFind.StationCode;
                    NextFind.PrevStationCode = PrevFind.StationCode;
                }
            }
            else
            {
                NextFind = DataSource.ListLineStations.Find(next => (next.LineId == lineId && next.LineStationIndex == lStatFind.LineStationIndex + 1 && next.IsDeleted == false));
                if (NextFind != null)
                {
                    NextFind.PrevStationCode = 0;
                }
            }
            int index;
            while (NextFind != null)
            {
                index = NextFind.LineStationIndex;
                NextFind.LineStationIndex = NextFind.LineStationIndex - 1;
                NextFind = DataSource.ListLineStations.Find(next => (next.LineId == lineId && next.LineStationIndex == index + 1 && next.IsDeleted == false));
            }
        }



        #endregion

        #region LineTrip
        public IEnumerable<DO.LineTrip> GetAllLineTrips()
        {
            return from lTrip in DataSource.ListLineTrips
                   select lTrip.Clone();
        }
        public IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate)
        {
            return from lTrip in DataSource.ListLineTrips
                   where predicate(lTrip)
                   select lTrip.Clone();
        }
         public DO.LineTrip GetLineTrip(int lineTripId)
        {
            DO.LineTrip lineTrip = DataSource.ListLineTrips.Find(l => l.LineTripId == lineTripId && l.IsDeleted == false);

            if (lineTrip != null)
                return lineTrip.Clone();
            else
                throw new Exception();
        }
        public void AddLineTrip(DO.LineTrip lineTrip)
        {
            if (DataSource.ListLineTrips.FirstOrDefault(l => l.LineTripId == lineTrip.LineTripId && l.IsDeleted == false) != null)
                throw new Exception();
            DataSource.ListLineTrips.Add(lineTrip.Clone());
        }
        public void UpdateLineTrip(DO.LineTrip lineTrip)
        {
            DO.LineTrip lTripFind = DataSource.ListLineTrips.Find(l => l.LineTripId == lineTrip.LineTripId && l.IsDeleted == false);
            if (lTripFind == null)
                throw new Exception();
            DO.LineTrip newLTrip = lineTrip.Clone();//copy of the bus that the function got
            lTripFind = newLTrip;//update
        }
        public void UpdateLineTrip(int lineTripId, Action<DO.LineTrip> update)
        {
            DO.LineTrip lTripFind = DataSource.ListLineTrips.Find(l => l.LineTripId == lineTripId && l.IsDeleted == false);
            if (lTripFind == null)
                throw new Exception();
            update(lTripFind);
        }
        public void DeleteLineTrip(int lineTripId)
        {

            DO.LineTrip lineTrip = DataSource.ListLineTrips.Find(l => l.LineTripId == lineTripId && l.IsDeleted == false);
            if (lineTrip == null)
                throw new Exception();
            lineTrip.IsDeleted = true;
        }

        #endregion

        #region User
        public IEnumerable<DO.User> GetAllUsers()
        {
            return from user in DataSource.ListUsers
                   select user.Clone();
        }
        public IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate)
        {
            return from user in DataSource.ListUsers
                   where predicate(user)
                   select user.Clone();
        }
        public DO.User GetUser(string userName)
        {
            DO.User user = DataSource.ListUsers.FirstOrDefault(u => u.UserName == userName && u.IsDeleted == false);
            if (user == null)
                throw new DO.BadUserNameException(userName, "This user name does not exist");
            return user.Clone();
        }
        public void AddUser(DO.User user)
        {
            if (DataSource.ListUsers.FirstOrDefault(u => u.UserName == user.UserName && u.IsDeleted == false) != null)
                throw new BadUserNameException(user.UserName,"This user name is already exist");
            DataSource.ListUsers.Add(user.Clone());
        }
        public void UpdateUser(DO.User user)
        {
            DO.User userFind = DataSource.ListUsers.Find(u => u.UserName == user.UserName && u.IsDeleted == false);
            if (userFind == null)
                throw new Exception();
            DO.User newUser = user.Clone();//copy of the bus that the function got
            userFind = newUser;//update
        }
        public void UpdateUser(string userName, Action<DO.User> update)
        {
            DO.User userFind = DataSource.ListUsers.Find(u => u.UserName == userName && u.IsDeleted == false);
            if (userFind == null)
                throw new Exception();
            update(userFind);
        }
        public void DeleteUser(string userName)
        {
            DO.User user = DataSource.ListUsers.Find(u => u.UserName == userName && u.IsDeleted == false);

            if (user == null)
                throw new Exception();
            user.IsDeleted = true;
        }

        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()
        {
            return from stat in DataSource.ListStations
                   select stat.Clone();
        }
        public IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate)
        {
            return from stat in DataSource.ListStations
                   where predicate(stat)
                   select stat.Clone();
        }
        public DO.Station GetStation(int code)
        {
            DO.Station station = DataSource.ListStations.Find(s => s.Code == code && s.IsDeleted == false);

            if (station != null)
                return station.Clone();
            else
                throw new BadStationCodeException(code,"The station does not exist");
        }
        public void AddStation(DO.Station station)
        {
            if (DataSource.ListStations.FirstOrDefault(s => s.Code == station.Code && s.IsDeleted == false) != null)
                throw new BadStationCodeException(station.Code, "A station with this code is already exist");
            DataSource.ListStations.Add(station.Clone());
        }
        public void UpdateStation(DO.Station station)
        {
            DO.Station statFind = DataSource.ListStations.FirstOrDefault(s => s.Code == station.Code && s.IsDeleted == false);
            if (statFind == null)
                throw new BadStationCodeException(statFind.Code, "The station does not exist");
            //DO.Station newStation = station.Clone();//copy of the bus that the function got
            //statFind = newStation;//update
            DO.Station newStat = station.Clone();
            DataSource.ListStations.Remove(statFind);
            DataSource.ListStations.Add(newStat);
        }
        public void UpdateStation(int code, Action<DO.Station> update)
        {
            DO.Station statFind = DataSource.ListStations.FirstOrDefault(s => s.Code == code && s.IsDeleted == false);
            if (statFind == null)
                throw new BadStationCodeException(code, "The station does not exist");
            update(statFind);
        }
        public void DeleteStation(int code)
        {
            DO.Station statFind = DataSource.ListStations.FirstOrDefault(s => s.Code == code && s.IsDeleted == false);
            if (statFind == null)
                throw new BadStationCodeException(code, "The station does not exist");
            statFind.IsDeleted = true;
            //foreach (DO.LineStation s in DataSource.ListLineStations)//delete fron the line station list
            //{
            //    if (s.StationCode == code && s.IsDeleted == false)
            //        s.IsDeleted = true;
            //}
            foreach(DO.AdjacentStations s in DataSource.ListAdjacentStations)//delete from adjacent Station list
            {
                if ((s.StationCode1 == code || s.StationCode2 == code) && s.IsDeleted == false)
                    s.IsDeleted = true;
            }

        }

        #endregion

        #region Trip
        public IEnumerable<DO.Trip> GetAllTrips()
        {
            return from trip in DataSource.ListTrips
                   select trip.Clone();
        }
        public IEnumerable<DO.Trip> GetAllTripsBy(Predicate<DO.Trip> predicate)
        {
            return from trip in DataSource.ListTrips
                   where predicate(trip)
                   select trip.Clone();

        }
        public DO.Trip GetTrip(int tripId)
        {
            DO.Trip trip = DataSource.ListTrips.Find(t => t.TripId == tripId && t.IsDeleted == false);
            if (trip != null)
                return trip.Clone();
            else
                throw new Exception();
        }
        public void AddTrip(DO.Trip trip)
        {
            if (DataSource.ListTrips.FirstOrDefault(t => t.TripId == trip.TripId && t.IsDeleted == false) != null)
                throw new Exception();
            DataSource.ListTrips.Add(trip.Clone());
        }
        public void UpdateTrip(DO.Trip trip)
        {
            DO.Trip tripFind = DataSource.ListTrips.Find(t => t.TripId == trip.TripId && t.IsDeleted == false);
            if (tripFind == null)
                throw new Exception();
            DO.Trip newTrip = trip.Clone();//copy of the bus that the function got
            tripFind = newTrip;//update
        }
        public void UpdateTrip(int tripId, Action<DO.Trip> update)
        {
            DO.Trip tripFind = DataSource.ListTrips.Find(t => t.TripId == tripId && t.IsDeleted == false);
            if (tripFind == null)
                throw new Exception();
            update(tripFind);
        }
        public void DeleteTrip(int tripId)
        {
            DO.Trip trip = DataSource.ListTrips.Find(t => t.TripId == tripId && t.IsDeleted == false);
            if (trip == null)
                throw new Exception();
            trip.IsDeleted = true;
        }

        #endregion

    }
}
