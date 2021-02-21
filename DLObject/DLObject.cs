﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
using DO;
using DS;

//Dal
namespace DL
{
    public class DLObject : IDL
    {
        #region singelton
        static readonly DLObject instance = new DLObject();
        static DLObject() { }// static ctor to ensure instance init is done just before first usage
        DLObject() { } // default => private
        public static DLObject Instance { get => instance; }// The public Instance property to use
        #endregion

        #region Bus
        public IEnumerable<DO.Bus> GetAllBuses()//returns a list of all the buses
        {
            return from bus in DataSource.ListBuses
                   where bus.IsDeleted == false
                   select bus.Clone();
        }
        public IEnumerable<DO.Bus> GetAllBusesBy(Predicate<DO.Bus> predicate)//returns a filtered list of the buses
        {
            return from bus in DataSource.ListBuses
                   where predicate(bus)
                   select bus.Clone();
        }
        public DO.Bus GetBus(int licenseNum)//returns a bus by its license number
        {
            DO.Bus bus = DataSource.ListBuses.Find(b => b.LicenseNum == licenseNum && b.IsDeleted == false);

            if (bus != null)
                return bus.Clone();
            else
                throw new BadLicenseNumException(licenseNum, "The bus does not exist");
        }
        public void AddBus(DO.Bus bus)
        {
            if (DataSource.ListBuses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum && b.IsDeleted == false) != null)
                throw new BadLicenseNumException(bus.LicenseNum, "The bus is already exists");
            DataSource.ListBuses.Add(bus.Clone());
        }


        public void UpdateBus(DO.Bus bus)
        {
            DO.Bus busFind = DataSource.ListBuses.Find(b => b.LicenseNum == bus.LicenseNum && b.IsDeleted == false);
            if (busFind == null)
                throw new BadLicenseNumException(bus.LicenseNum, "The bus does not exist");
            DO.Bus newBus = bus.Clone();
            DataSource.ListBuses.Remove(busFind);
            DataSource.ListBuses.Add(newBus);
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
            if (bus == null)
                throw new BadLicenseNumException(licenseNum, "The bus does not exist");
            bus.IsDeleted = true;//delete

        }
        #endregion

        #region AdjacentStations
        public bool IsExistAdjacentStations(int stationCode1, int stationCode2)//checks if 2 stations are adjacent
        {
            DO.AdjacentStations adjStations = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1 && adj.StationCode2 == stationCode2 && adj.IsDeleted == false));
            if (adjStations != null)
                return true;
            return false;
        }
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStations()//returns a list of all the adjacent stations
        {
            return from adj in DataSource.ListAdjacentStations
                   where adj.IsDeleted == false
                   select adj.Clone();
        }
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate)//returns a filtered list of the adjacent stations
        {
            return from adj in DataSource.ListAdjacentStations
                   where predicate(adj)
                   select adj.Clone();
        }
        public DO.AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2)//returns the object of the adjacent stations
        {
            DO.AdjacentStations adjStations = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1 && adj.StationCode2 == stationCode2 && adj.IsDeleted == false));

            if (adjStations == null)
                throw new DO.BadAdjacentStationsException(stationCode1, stationCode2, "The adjacent stations does not exist");
            return adjStations.Clone();

        }
        public void AddAdjacentStations(DO.AdjacentStations adjStations)
        {
            if (DataSource.ListAdjacentStations.FirstOrDefault(adj => (adj.StationCode1 == adjStations.StationCode1 && adj.StationCode2 == adjStations.StationCode2 && adj.IsDeleted == false)) != null)//if those adjacent stations already exist in the list
                throw new BadAdjacentStationsException(adjStations.StationCode1, adjStations.StationCode2, "The adjacent stations are already exist");
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
            DO.AdjacentStations adjFind = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1 && adj.StationCode2 == stationCode2 && adj.IsDeleted == false));
            if (adjFind == null)
                throw new BadAdjacentStationsException(stationCode1, stationCode2, "The adjacent stations does not exist");
            update(adjFind);
        }
        public void DeleteAdjacentStations(int stationCode1, int stationCode2)
        {
            DO.AdjacentStations adjFind = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1 && adj.StationCode2 == stationCode2 && adj.IsDeleted == false));
            if (adjFind == null)
                throw new BadAdjacentStationsException(stationCode1, stationCode2, "The adjacent stations does not exist");
            adjFind.IsDeleted = true;
        }

        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()//return a list of all the lines
        {
            return from line in DataSource.ListLines
                   where line.IsDeleted == false
                   select line.Clone();

        }
        public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)//returns a filtered list of the lines
        {
            return from line in DataSource.ListLines
                   where predicate(line)
                   select line.Clone();

        }
        public DO.Line GetLine(int lineId)//returns a line by its id
        {
            DO.Line line = DataSource.ListLines.Find(l => l.LineId == lineId && l.IsDeleted == false);

            if (line != null)
                return line.Clone();
            else
                throw new BadLineIdException(lineId, "The line does not exist");
        }
        public void AddLine(DO.Line line)
        {
            line.LineId = Config.LineId++;
            if (DataSource.ListLines.FirstOrDefault(l => l.LineId == line.LineId && l.IsDeleted == false) != null)
                throw new BadLineIdException(line.LineId, $"The line {line.LineNum} is already exist");
            DataSource.ListLines.Add(line.Clone());
        }
        public void UpdateLine(DO.Line line)
        {
            DO.Line lineFind = DataSource.ListLines.Find(l => l.LineId == line.LineId && l.IsDeleted == false);
            if (lineFind == null)
                throw new BadLineIdException(line.LineId, $"The line {line.LineNum} does not exist");
            DO.Line newLine = line.Clone();//copy of the bus that the function got
            DataSource.ListLines.Remove(lineFind);
            DataSource.ListLines.Add(newLine);
        }
        public void UpdateLine(int lineId, Action<DO.Line> update)
        {
            DO.Line lineFind = DataSource.ListLines.Find(l => l.LineId == lineId && l.IsDeleted == false);
            if (lineFind == null)
                throw new BadLineIdException(lineId, "The line does not exist");
            update(lineFind);
        }
        public void DeleteLine(int lineId)
        {
            DO.Line lineFind = DataSource.ListLines.Find(l => l.LineId == lineId && l.IsDeleted == false);
            if (lineFind == null)
                throw new BadLineIdException(lineId, "The line does not exist");
            lineFind.IsDeleted = true;
        }

        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetAllLineStations()//returns a list of all the line stations
        {
            return from lStat in DataSource.ListLineStations
                   where lStat.IsDeleted == false
                   select lStat.Clone();
        }
        public IEnumerable<DO.LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate)//returns a filtered list of the line stations
        {
            return from lStat in DataSource.ListLineStations
                   where predicate(lStat)
                   select lStat.Clone();
        }
        public DO.LineStation GetLineStation(int lineId, int stationCode)//returns a line station by its id and station code
        {
            DO.LineStation lineStation = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineId && lStat.StationCode == stationCode && lStat.IsDeleted == false));

            if (lineStation != null)
                return lineStation.Clone();
            else
                throw new DO.BadLineStationException(lineId, stationCode, "The station line does not exist");
        }
        public void AddLineStation(DO.LineStation lineStation)
        {
            if (DataSource.ListLineStations.FirstOrDefault(lStat => (lStat.LineId == lineStation.LineId && lStat.StationCode == lineStation.StationCode && lStat.IsDeleted == false)) != null)//if this line station already exists in the list
                throw new DO.BadLineStationException(lineStation.LineId, lineStation.StationCode, "The new line station is already exist");
            #region
            ////update the line station index of all the next station
            //DO.LineStation next= DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.LineStationIndex== lineStation.LineStationIndex && lStat.IsDeleted == false));
            //DO.LineStation temp;
            //int i;
            //while (next != null)
            //{
            //    //temp = next;
            //    i = next.LineStationIndex + 1;
            //    temp = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.LineStationIndex == i && lStat.IsDeleted == false));
            //    ++next.LineStationIndex;
            //    next = temp;
            //}
            ////update prev and next
            //DO.LineStation prev = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.LineStationIndex == lineStation.LineStationIndex-1 && lStat.IsDeleted == false));
            //next = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.LineStationIndex == lineStation.LineStationIndex+1 && lStat.IsDeleted == false));
            //if(prev!=null)
            //{
            //    prev.NextStationCode = lineStation.StationCode;
            //    lineStation.PrevStationCode = prev.StationCode;
            //}
            //if(next!=null)
            //{
            //    lineStation.NextStationCode = next.StationCode;
            //    next.PrevStationCode = lineStation.StationCode;
            //}
            #endregion
            DataSource.ListLineStations.Add(lineStation.Clone());
        }
        public void UpdateLineStation(DO.LineStation lineStation)
        {
            DO.LineStation lStatFind = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.StationCode == lineStation.StationCode && lStat.IsDeleted == false));
            if (lStatFind == null)
                throw new DO.BadLineStationException(lineStation.LineId, lineStation.StationCode, "The station line does not exist");
            DO.LineStation newLineStation = lineStation.Clone();//copy of the bus that the function got
            DataSource.ListLineStations.Remove(lStatFind);
            DataSource.ListLineStations.Add(newLineStation);
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
            DO.LineStation lStatFind = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineId && lStat.StationCode == stationCode && lStat.IsDeleted == false));
            if (lStatFind == null)
                throw new DO.BadLineStationException(lineId, stationCode, "The station line does not exist");
            lStatFind.IsDeleted = true;//delete
            #region
            //DO.LineStation NextFind;
            //if (lStatFind.LineStationIndex>1)//if its not the first station
            //{
            //    DO.LineStation PrevFind= DataSource.ListLineStations.Find(prev => (prev.LineId == lineId && prev.LineStationIndex == lStatFind.LineStationIndex - 1 && prev.IsDeleted == false));
            //    NextFind = DataSource.ListLineStations.Find(next => (next.LineId == lineId  && next.LineStationIndex == lStatFind.LineStationIndex+1 && next.IsDeleted == false));
            //    if (NextFind != null)//if its not the last station
            //    {
            //        PrevFind.NextStationCode = NextFind.StationCode;
            //        NextFind.PrevStationCode = PrevFind.StationCode;
            //    }
            //    else
            //    {
            //        PrevFind.NextStationCode = 0;
            //    }
            //}
            //else//if its the first station
            //{
            //    NextFind = DataSource.ListLineStations.Find(next => (next.LineId == lineId && next.LineStationIndex == lStatFind.LineStationIndex + 1 && next.IsDeleted == false));
            //    if (NextFind != null)
            //    {
            //        NextFind.PrevStationCode = 0;
            //    }
            //}
            //int index;
            //while (NextFind != null)//update all the indexes of all the next stations
            //{
            //    index = NextFind.LineStationIndex;
            //    NextFind.LineStationIndex = NextFind.LineStationIndex - 1;
            //    NextFind = DataSource.ListLineStations.Find(next => (next.LineId == lineId && next.LineStationIndex == index + 1 && next.IsDeleted == false));
            //}
            #endregion
        }



        #endregion

        #region LineTrip
        public IEnumerable<DO.LineTrip> GetAllLineTrips()//returns a list of all the line trips
        {
            return from lTrip in DataSource.ListLineTrips
                   select lTrip.Clone();
        }
        public IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate)//returns a filtered list of the line trips
        {
            return from lTrip in DataSource.ListLineTrips
                   where predicate(lTrip)
                   select lTrip.Clone();
        }
        public DO.LineTrip GetLineTrip(int lineId, TimeSpan time)//returns line tri[ by its line id and time
        {
            DO.LineTrip lineTrip = DataSource.ListLineTrips.Find(l => l.LineId == lineId && l.StartAt == time && l.IsDeleted == false);

            if (lineTrip != null)
                return lineTrip.Clone();
            else
                throw new BadLineTripException(lineId, time, "The line trip does not exist");
        }
        public void AddLineTrip(DO.LineTrip lineTrip)
        {
            if (DataSource.ListLineTrips.FirstOrDefault(l => l.LineId == lineTrip.LineId && l.StartAt == lineTrip.StartAt && l.IsDeleted == false) != null)
                throw new BadLineTripException(lineTrip.LineId, lineTrip.StartAt, "The line trip is already exist");
            DataSource.ListLineTrips.Add(lineTrip.Clone());
        }
        public void UpdateLineTrip(DO.LineTrip lineTrip)
        {
            DO.LineTrip lTripFind = DataSource.ListLineTrips.Find(l => l.LineId == lineTrip.LineId && l.StartAt == lineTrip.StartAt && l.IsDeleted == false);
            if (lTripFind == null)
                throw new BadLineTripException(lineTrip.LineId, lineTrip.StartAt, "The line trip does not exist");
            DO.LineTrip newLTrip = lineTrip.Clone();//copy of the bus that the function got
            DataSource.ListLineTrips.Remove(lTripFind);
            DataSource.ListLineTrips.Add(newLTrip);
            //lTripFind = newLTrip;//update
        }
        public void UpdateLineTrip(int lineId, TimeSpan time, Action<DO.LineTrip> update)
        {
            DO.LineTrip lTripFind = DataSource.ListLineTrips.Find(l => l.LineId == lineId && l.StartAt == time && l.IsDeleted == false);
            if (lTripFind == null)
                throw new BadLineTripException(lineId, time, "The line trip does not exist");
            update(lTripFind);
        }
        public void DeleteLineTrip(int lineId, TimeSpan time)
        {

            DO.LineTrip lineTrip = DataSource.ListLineTrips.Find(l => l.LineId == lineId && l.StartAt == time && l.IsDeleted == false);
            if (lineTrip == null)
                throw new BadLineTripException(lineId, time, "The line trip does not exist");
            lineTrip.IsDeleted = true;
        }

        #endregion

        #region User
        public IEnumerable<DO.User> GetAllUsers()//returns a list of all the users

        {
            return from user in DataSource.ListUsers
                   select user.Clone();
        }
        public IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate)//returns a filtered list of the users
        {
            return from user in DataSource.ListUsers
                   where predicate(user)
                   select user.Clone();
        }
        public DO.User GetUser(string userName)//returns a user by its username
        {
            DO.User user = DataSource.ListUsers.FirstOrDefault(u => u.UserName == userName && u.IsDeleted == false);
            if (user == null)
                throw new DO.BadUserNameException(userName, "This user name does not exist");
            return user.Clone();
        }
        public void AddUser(DO.User user)
        {
            if (DataSource.ListUsers.FirstOrDefault(u => u.UserName == user.UserName && u.IsDeleted == false) != null)
                throw new BadUserNameException(user.UserName, $"the username: {user.UserName} is already exist");
            DataSource.ListUsers.Add(user.Clone());
        }
        public void UpdateUser(DO.User user)
        {
            DO.User userFind = DataSource.ListUsers.Find(u => u.UserName == user.UserName && u.IsDeleted == false);
            if (userFind == null)
                throw new BadUserNameException(user.UserName, $"the username: {user.UserName} does not exist");
            DO.User newUser = user.Clone();//copy of the bus that the function got
            userFind = newUser;//update
        }
        public void UpdateUser(string userName, Action<DO.User> update)
        {
            DO.User userFind = DataSource.ListUsers.Find(u => u.UserName == userName && u.IsDeleted == false);
            if (userFind == null)
                throw new BadUserNameException(userName, $"the username: {userName} does not exist");
            update(userFind);
        }
        public void DeleteUser(string userName)
        {
            DO.User user = DataSource.ListUsers.Find(u => u.UserName == userName && u.IsDeleted == false);

            if (user == null)
                throw new BadUserNameException(user.UserName, $"the username: {user.UserName} does not exist");
            user.IsDeleted = true;
        }

        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()//returns a list of all the stations
        {
            return from stat in DataSource.ListStations
                   where stat.IsDeleted == false
                   select stat.Clone();
        }
        public IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate)//returns a filtered list of the users
        {
            return from stat in DataSource.ListStations
                   where predicate(stat)
                   select stat.Clone();
        }
        public DO.Station GetStation(int code)//returns a station by its code
        {
            DO.Station station = DataSource.ListStations.Find(s => s.Code == code && s.IsDeleted == false);

            if (station != null)
                return station.Clone();
            else
                throw new BadStationCodeException(code, "The station does not exist");
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
        }

        #endregion

        #region Trip
        public IEnumerable<DO.Trip> GetAllTrips()//returns a list of all the trip
        {
            return from trip in DataSource.ListTrips
                   select trip.Clone();
        }
        public IEnumerable<DO.Trip> GetAllTripsBy(Predicate<DO.Trip> predicate)//returns a filtered list of the trip
        {
            return from trip in DataSource.ListTrips
                   where predicate(trip)
                   select trip.Clone();

        }
        public DO.Trip GetTrip(int tripId)//returns a trip by its trp id
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
