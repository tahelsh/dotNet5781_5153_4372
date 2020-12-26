using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
//using DO;
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

        }
        public IEnumerable<DO.Bus> GetAllBusesBy(Predicate<DO.Bus> predicate)
        {

        }
        public DO.Bus GetBus(int licenseNum)
        {
            DO.Bus bus = DataSource.ListBuses.Find(b => b.LicenseNum == licenseNum);

            if (bus != null)
                return bus.Clone();
            else
                throw new Exception();
        }
        public void AddBus(DO.Bus bus)
        {
            if (DataSource.ListBuses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum) != null)
                throw new Exception();
            DataSource.ListBuses.Add(bus.Clone());
        }
        public void UpdateBus(DO.Bus bus)
        {
            DO.Bus busFind = DataSource.ListBuses.Find(b => b.LicenseNum == bus.LicenseNum);
            if (busFind == null)
                throw new Exception();
            DO.Bus newBus = bus.Clone();//copy of the bus that the function got
            busFind = newBus;//update
        }
        public void UpdateBus(int licenseNum, Action<DO.Bus> update)
        {

        }
        public void DeleteBus(int licenseNum)
        {

        }
        #endregion

        #region AdjacentStations
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStations()
        {

        }
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate)
        {

        }
        public DO.AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2)
        {
            DO.AdjacentStations adjStations = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1&& adj.StationCode2 == stationCode2));

            if (adjStations != null)
                return adjStations.Clone();
            else
                throw new Exception();
        }
        public void AddAdjacentStations(DO.AdjacentStations adjStations)
        {
            if (DataSource.ListAdjacentStations.FirstOrDefault(adj => (adj.StationCode1 == adjStations.StationCode1&&adj.StationCode2==adjStations.StationCode2)) != null)//if those adjacent stations already exist in the list
                throw new Exception();
            DataSource.ListAdjacentStations.Add(adjStations.Clone());
        }
        public void UpdateAdjacentStations(DO.AdjacentStations adjStations)
        {
            DO.AdjacentStations adjFind = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == adjStations.StationCode1 && adj.StationCode2 == adjStations.StationCode2));
            if (adjFind == null)
                throw new Exception();
            DO.AdjacentStations newAdj = adjStations.Clone();//copy of the bus that the function got
            adjFind = newAdj;//update
        }
        public void UpdateAdjacentStations(int stationCode1, int stationCode2, Action<DO.AdjacentStations> update)
        {

        }
        public void DeleteAdjacentStations(int stationCode1, int stationCode2)
        {

        }

        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {

        }
        public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)
        {

        }
        public DO.Line GetLine(int lineId)
        {
            DO.Line line = DataSource.ListLines.Find(l => l.LineId == lineId);

            if (line != null)
                return line.Clone();
            else
                throw new Exception();
        }
        public void AddLine(DO.Line line)
        {
            if (DataSource.ListLines.FirstOrDefault(l => l.LineId == line.LineId) != null)
                throw new Exception();
            DataSource.ListLines.Add(line.Clone());
        }
        public void UpdateLine(DO.Line line)
        {
            DO.Line lineFind = DataSource.ListLines.Find(l => l.LineId == line.LineId);
            if (lineFind == null)
                throw new Exception();
            DO.Line newLine = line.Clone();//copy of the bus that the function got
            lineFind = newLine;//update
        }
        public void UpdateLine(int lineId, Action<DO.Line> update)
        {

        }
        public void DeleteLine(int lineId)
        {

        }

        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetAllLineStations()
        {

        }
        public IEnumerable<DO.LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate)
        {

        }
        public DO.LineStation GetLineStation(int lineId, int stationCode)
        {
            DO.LineStation lineStation = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineId && lStat.StationCode == stationCode));

            if (lineStation != null)
                return lineStation.Clone();
            else
                throw new Exception();
        }
        public void AddLineStation(DO.LineStation lineStation)
        {
            if (DataSource.ListLineStations.FirstOrDefault(lStat => (lStat.LineId == lineStation.LineId && lStat.StationCode == lineStation.StationCode)) != null)//if this line station already exists in the list
                throw new Exception();
            DataSource.ListLineStations.Add(lineStation.Clone());
        }
        public void UpdateLineStation(DO.LineStation lineStation)
        {
            DO.LineStation lStatFind = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.StationCode == lineStation.StationCode));
            if (lStatFind == null)
                throw new Exception();
            DO.LineStation newAdj = lineStation.Clone();//copy of the bus that the function got
            lStatFind = newAdj;//update
        }
        public void UpdateLineStation(int lineId, int stationCode, Action<DO.LineStation> update)
        {

        }
        public void DeleteLineStation(int lineId, int stationCode)
        {

        }



        #endregion

        #region LineTrip
        public IEnumerable<DO.LineTrip> GetAllLineTrips()
        {

        }
        public IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate)
        {

        }
         public DO.LineTrip GetLineTrip(int lineTripId)
        {
            DO.LineTrip lineTrip = DataSource.ListLineTrips.Find(l => l.LineTripId == lineTripId);

            if (lineTrip != null)
                return lineTrip.Clone();
            else
                throw new Exception();
        }
        public void AddLineTrip(DO.LineTrip lineTrip)
        {
            if (DataSource.ListLineTrips.FirstOrDefault(l => l.LineTripId == lineTrip.LineTripId) != null)
                throw new Exception();
            DataSource.ListLineTrips.Add(lineTrip.Clone());
        }
        public void UpdateLineTrip(DO.LineTrip lineTrip)
        {
            DO.LineTrip lTripFind = DataSource.ListLineTrips.Find(l => l.LineTripId == lineTrip.LineTripId);
            if (lTripFind == null)
                throw new Exception();
            DO.LineTrip newLTrip = lineTrip.Clone();//copy of the bus that the function got
            lTripFind = newLTrip;//update
        }
        public void UpdateLineTrip(int lineTripId, Action<DO.LineTrip> update)
        {

        }
        public void DeleteLineTrip(int lineTripId)
        {

        }

        #endregion

        #region User
        public IEnumerable<DO.User> GetAllUsers()
        {
            
        }
        public IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate)
        {

        }
        public DO.User GetUser(string userName)
        {
            DO.User user = DataSource.ListUsers.Find(b => b.UserName == userName);

            if (user != null)
                return user.Clone();
            else
                throw new Exception();
        }
        public void AddUser(DO.User user)
        {
            if (DataSource.ListUsers.FirstOrDefault(u => u.UserName == user.UserName) != null)
                throw new Exception();
            DataSource.ListUsers.Add(user.Clone());
        }
        public void UpdateUser(DO.User user)
        {
            DO.User userFind = DataSource.ListUsers.Find(u => u.UserName == user.UserName);
            if (userFind == null)
                throw new Exception();
            DO.User newUser = user.Clone();//copy of the bus that the function got
            userFind = newUser;//update
        }
        public void UpdateUser(string userName, Action<DO.User> update)
        {

        }
        public void DeleteUser(string userName)
        {

        }

        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()
        {

        }
        public IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate)
        {

        }
        public DO.Station GetStation(int code)
        {
            DO.Station station = DataSource.ListStations.Find(s => s.Code == code);

            if (station != null)
                return station.Clone();
            else
                throw new Exception();
        }
        public void AddStation(DO.Station station)
        {
            if (DataSource.ListStations.FirstOrDefault(s => s.Code == station.Code) != null)
                throw new Exception();
            DataSource.ListStations.Add(station.Clone());
        }
        public void UpdateStation(DO.Station station)
        {
            DO.Station statFind = DataSource.ListStations.Find(s => s.Code == station.Code);
            if (statFind == null)
                throw new Exception();
            DO.Station newStation = station.Clone();//copy of the bus that the function got
            statFind = newStation;//update
        }
        public void UpdateStation(int code, Action<DO.Station> update)
        {

        }
        public void DeleteStation(int code)
        {

        }

        #endregion

        #region Trip
        public IEnumerable<DO.Trip> GetAllBuses()
        {

        }
        public IEnumerable<DO.Trip> GetAllBusesBy(Predicate<DO.Trip> predicate)
        {

        }
        public DO.Trip GetTrip(int tripId)
        {
            DO.Trip trip = DataSource.ListTrips.Find(t => t.TripId == tripId);

            if (trip != null)
                return trip.Clone();
            else
                throw new Exception();
        }
        public void AddTrip(DO.Trip trip)
        {
            if (DataSource.ListTrips.FirstOrDefault(t => t.TripId == trip.TripId) != null)
                throw new Exception();
            DataSource.ListTrips.Add(trip.Clone());
        }
        public void UpdateTrip(DO.Trip trip)
        {
            DO.Trip tripFind = DataSource.ListTrips.Find(t => t.TripId == trip.TripId);
            if (tripFind == null)
                throw new Exception();
            DO.Trip newTrip = trip.Clone();//copy of the bus that the function got
            tripFind = newTrip;//update
        }
        public void UpdateTrip(int tripId, Action<DO.Trip> update)
        {

        }
        public void DeleteTrip(int tripId)
        {

        }

        #endregion

    }
}
