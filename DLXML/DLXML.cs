using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DLAPI;
using DO;
//using DO;

namespace DL
{
    sealed class DLXML : IDL
    {
        #region singelton
        static readonly DLXML instance = new DLXML();
        static DLXML() { }// static ctor to ensure instance init is done just before first usage
        DLXML() { } // default => private
        public static DLXML Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files

        string usersPath = @"UserXml.xml"; //XElement

        string busesPath = @"BusesXml.xml"; //XMLSerializer
        string adjacentStationsPath = @"AdjacentStationsXml.xml"; //XMLSerializer
        string linesPath = @"LinesXml.xml"; //XMLSerializer
        string lineStationsPath = @"LineStationsXml.xml"; //XMLSerializer
        string lineTripsPath = @"LineTripsXml.xml"; //XMLSerializer
        string stationsPath = @"StationsXml.xml"; //XMLSerializer
        string tripsPath = @"TripsXml.xml"; //XMLSerializer
        string runningNumberPath = @"TripsXml.xml"; //XMLSerializer
        #endregion

        #region User
        public IEnumerable<DO.User> GetAllUsers()
        {
            XElement usersRootElem = XMLTools.LoadListFromXMLElement(usersPath);

            return (from u in usersRootElem.Elements()
                    where bool.Parse(u.Element("IsDeleted").Value) == false
                    select new User()
                    {
                        UserName = u.Element("UserName").Value,
                        Name = u.Element("Name").Value,
                        Passcode = Int32.Parse(u.Element("Passcode").Value),
                        AdminAccess = bool.Parse(u.Element("AdminAccess").Value),
                        IsDeleted = bool.Parse(u.Element("IsDeleted").Value)
                    }
                   );
        }
        public IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate)
        {
            XElement usersRootElem = XMLTools.LoadListFromXMLElement(usersPath);

            return from u in usersRootElem.Elements()
                   let u1 = new User()
                   {
                       UserName = u.Element("UserName").Value,
                       Name = u.Element("Name").Value,
                       Passcode = Int32.Parse(u.Element("Passcode").Value),
                       AdminAccess = bool.Parse(u.Element("AdminAccess").Value),
                       IsDeleted = bool.Parse(u.Element("IsDeleted").Value)
                   }
                   where predicate(u1)
                   select u1;
        }
        public DO.User GetUser(string userName)
        {
            XElement usersRootElem = XMLTools.LoadListFromXMLElement(usersPath);
            User user = (from u in usersRootElem.Elements()
                         where u.Element("UserName").Value == userName && bool.Parse(u.Element("IsDeleted").Value) == false
                         select new User()
                         {
                             UserName = u.Element("UserName").Value,
                             Name = u.Element("Name").Value,
                             Passcode = Int32.Parse(u.Element("Passcode").Value),
                             AdminAccess = bool.Parse(u.Element("AdminAccess").Value),
                             IsDeleted = bool.Parse(u.Element("IsDeleted").Value)
                         }
                       ).FirstOrDefault();

            if (user == null)
                throw new DO.BadUserNameException(userName, "This user name does not exist");
            return user;
        }
        public void AddUser(DO.User user)
        {
            XElement usersRootElem = XMLTools.LoadListFromXMLElement(usersPath);

            XElement user1 = (from u in usersRootElem.Elements()
                              where u.Element("UserName").Value == user.UserName && bool.Parse(u.Element("IsDeleted").Value) == false
                              select u).FirstOrDefault();

            if (user1 != null)
                throw new BadUserNameException(user.UserName, "This user name is already exist");

            XElement userElem = new XElement("User",
                                   new XElement("UserName", user.UserName),
                                   new XElement("Name", user.Name),
                                   new XElement("Passcode", user.Passcode.ToString()),
                                   new XElement("AdminAccess", user.AdminAccess.ToString()),
                                   new XElement("IsDeleted", user.IsDeleted.ToString()));
            usersRootElem.Add(userElem);
            XMLTools.SaveListToXMLElement(usersRootElem, usersPath);
        }
        public void UpdateUser(DO.User user)
        {
            XElement usersRootElem = XMLTools.LoadListFromXMLElement(usersPath);

            XElement userElem = (from u in usersRootElem.Elements()
                                 where u.Element("UserName").Value == user.UserName && bool.Parse(u.Element("IsDeleted").Value) == false
                                 select u).FirstOrDefault();

            if (userElem != null)
            {
                userElem.Element("UserName").Value = user.UserName;
                userElem.Element("Name").Value = user.Name;
                userElem.Element("Passcode").Value = user.Passcode.ToString();
                userElem.Element("AdminAccess").Value = user.AdminAccess.ToString();
                userElem.Element("IsDeleted").Value = user.IsDeleted.ToString();
                XMLTools.SaveListToXMLElement(usersRootElem, usersPath);
            }
            else
                throw new BadUserNameException(user.UserName, "The user does not exist");
        }
        public void UpdateUser(string userName, Action<DO.User> update)
        {
            throw new NotImplementedException();
            //DO.User userFind = DataSource.ListUsers.Find(u => u.UserName == userName && u.IsDeleted == false);
            //if (userFind == null)
            //    throw new Exception();
            //update(userFind);
        }
        public void DeleteUser(string userName)
        {
            XElement usersRootElem = XMLTools.LoadListFromXMLElement(usersPath);
            XElement user = (from u in usersRootElem.Elements()
                             where u.Element("UserName").Value == userName && bool.Parse(u.Element("IsDeleted").Value) == false
                             select u).FirstOrDefault();

            if (user != null)
            {
                user.Element("IsDeleted").Value = true.ToString();
                XMLTools.SaveListToXMLElement(usersRootElem, usersPath);
            }
            else
                throw new BadUserNameException(userName, "The user does not exist");
        }

        #endregion

        #region Bus
        public IEnumerable<DO.Bus> GetAllBuses()
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            return from bus in ListBuses
                   where bus.IsDeleted == false
                   select bus;
        }
        public IEnumerable<DO.Bus> GetAllBusesBy(Predicate<DO.Bus> predicate)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            return from bus in ListBuses
                   where predicate(bus)
                   select bus;
        }
        public DO.Bus GetBus(int licenseNum)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            DO.Bus bus = ListBuses.Find(b => b.LicenseNum == licenseNum && b.IsDeleted == false);

            if (bus != null)
                return bus;
            else
                throw new BadLicenseNumException(licenseNum, "The bus does not exist");
        }
        public void AddBus(DO.Bus bus)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            if (ListBuses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum && b.IsDeleted == false) != null)
                throw new BadLicenseNumException(bus.LicenseNum, "The bus is already exists");
            ListBuses.Add(bus);
            XMLTools.SaveListToXMLSerializer(ListBuses, busesPath);
        }


        public void UpdateBus(DO.Bus bus)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            DO.Bus busFind = ListBuses.Find(b => b.LicenseNum == bus.LicenseNum && b.IsDeleted == false);
            if (busFind == null)
                throw new BadLicenseNumException(bus.LicenseNum, "The bus does not exist");
            DO.Bus newBus = bus;
            ListBuses.Remove(busFind);
            ListBuses.Add(newBus);
            XMLTools.SaveListToXMLSerializer(ListBuses, busesPath);
        }
        public void UpdateBus(int licenseNum, Action<DO.Bus> update)
        {
            //List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            //DO.Bus busFind = ListBuses.Find(b => b.LicenseNum == licenseNum && b.IsDeleted == false);
            //if (busFind == null)
            //    throw new BadLicenseNumException(licenseNum, "The bus does not exist");
            //update(busFind);
        }
        public void DeleteBus(int licenseNum)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            DO.Bus bus = ListBuses.Find(b => b.LicenseNum == licenseNum && b.IsDeleted == false);
            if (bus == null)
                throw new BadLicenseNumException(licenseNum, "The bus does not exist");
            //ListBuses.Remove(bus);
            bus.IsDeleted = true;//delete
            //ListBuses.Add(bus);
            XMLTools.SaveListToXMLSerializer(ListBuses, busesPath);
        }
        #endregion

        #region AdjacentStations
        public bool IsExistAdjacentStations(int stationCode1, int stationCode2)
        {
            XElement adjStationssRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            XElement adj = (from a in adjStationssRootElem.Elements()
                            where int.Parse(a.Element("StationCode1").Value) == stationCode1 && int.Parse(a.Element("StationCode2").Value) == stationCode2 && bool.Parse(a.Element("IsDeleted").Value) == false
                            select a).FirstOrDefault();
            if (adj == null)
                return false;
            return true;
        }
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStations()
        {
            XElement adjStationssRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            return (from a in adjStationssRootElem.Elements()
                    where bool.Parse(a.Element("IsDeleted").Value) == false
                    select new AdjacentStations()
                    {
                        StationCode1 = Int32.Parse(a.Element("StationCode1").Value),
                        StationCode2 = Int32.Parse(a.Element("StationCode2").Value),
                        Distance = double.Parse(a.Element("Distance").Value),
                        Time = TimeSpan.Parse(a.Element("Time").Value),
                        IsDeleted = Convert.ToBoolean(a.Element("IsDeleted").Value)
                    }
                   );
        }
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate)
        {
            XElement adjStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            return from a in adjStationsRootElem.Elements()
                   let a1 = new AdjacentStations()
                   {
                       StationCode1 = Int32.Parse(a.Element("StationCode1").Value),
                       StationCode2 = Int32.Parse(a.Element("StationCode2").Value),
                       Distance = double.Parse(a.Element("Distance").Value),
                       Time = TimeSpan.Parse(a.Element("Time").Value),
                       IsDeleted = Convert.ToBoolean(a.Element("IsDeleted").Value)
                   }
                   where predicate(a1)
                   select a1;
        }
        public DO.AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2)
        {
            XElement adjStationssRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            AdjacentStations adj = (from a in adjStationssRootElem.Elements()
                                    where int.Parse(a.Element("StationCode1").Value) == stationCode1 && int.Parse(a.Element("StationCode2").Value) == stationCode2 && bool.Parse(a.Element("IsDeleted").Value) == false
                                    select new AdjacentStations()
                                    {
                                        StationCode1 = Int32.Parse(a.Element("StationCode1").Value),
                                        StationCode2 = Int32.Parse(a.Element("StationCode2").Value),
                                        Distance = double.Parse(a.Element("Distance").Value),
                                        Time = TimeSpan.Parse(a.Element("Time").Value),
                                        IsDeleted = Convert.ToBoolean(a.Element("IsDeleted").Value)
                                    }
                        ).FirstOrDefault();

            if (adj == null)
                throw new DO.BadAdjacentStationsException(stationCode1, stationCode2, "The adjacent stations does not exist");

            return adj;
        }
        public void AddAdjacentStations(DO.AdjacentStations adjStations)
        {
            XElement adjStationssRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            XElement adj1 = (from a in adjStationssRootElem.Elements()
                             where int.Parse(a.Element("StationCode1").Value) == adjStations.StationCode1 && int.Parse(a.Element("StationCode2").Value) == adjStations.StationCode2 && bool.Parse(a.Element("IsDeleted").Value) == false
                             select a).FirstOrDefault();

            if (adj1 != null)
                throw new BadAdjacentStationsException(adjStations.StationCode1, adjStations.StationCode2, "The adjacent stations are already exist"); ;

            XElement adjElem = new XElement("AdjacentStations",
                                   new XElement("StationCode1", adjStations.StationCode1.ToString()),
                                   new XElement("StationCode2", adjStations.StationCode2.ToString()),
                                   new XElement("Distance", adjStations.Distance.ToString()),
                                   new XElement("Time", adjStations.Time.ToString()),
                                   new XElement("IsDeleted", adjStations.IsDeleted.ToString()));
            adjStationssRootElem.Add(adjElem);
            XMLTools.SaveListToXMLElement(adjStationssRootElem, adjacentStationsPath);
        }
        public void UpdateAdjacentStations(DO.AdjacentStations adjStations)
        {
            XElement adjStationssRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);
            XElement adj = (from a in adjStationssRootElem.Elements()
                            where int.Parse(a.Element("StationCode1").Value) == adjStations.StationCode1 && int.Parse(a.Element("StationCode2").Value) == adjStations.StationCode2 && bool.Parse(a.Element("IsDeleted").Value) == false
                            select a).FirstOrDefault();

            if (adj != null)
            {
                adj.Element("StationCode1").Value = adjStations.StationCode1.ToString();
                adj.Element("StationCode2").Value = adjStations.StationCode2.ToString();
                adj.Element("Distance").Value = adjStations.Distance.ToString();
                adj.Element("Time").Value = adjStations.Time.ToString();
                adj.Element("IsDeleted").Value = adjStations.IsDeleted.ToString();

                XMLTools.SaveListToXMLElement(adjStationssRootElem, adjacentStationsPath);
            }
            else
                throw new BadAdjacentStationsException(adjStations.StationCode1, adjStations.StationCode2, "The adjacent stations does not exist");
        }
        public void UpdateAdjacentStations(int stationCode1, int stationCode2, Action<DO.AdjacentStations> update)
        {
            throw new NotImplementedException();
            //DO.AdjacentStations adjFind = DataSource.ListAdjacentStations.Find(adj => (adj.StationCode1 == stationCode1 && adj.StationCode2 == stationCode2 && adj.IsDeleted == false));
            //if (adjFind == null)
            //    throw new BadAdjacentStationsException(stationCode1, stationCode2, "The adjacent stations does not exist");
            //update(adjFind);
        }
        public void DeleteAdjacentStations(int stationCode1, int stationCode2)
        {
            XElement adjStationssRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            XElement adj = (from a in adjStationssRootElem.Elements()
                             where int.Parse(a.Element("StationCode1").Value) == stationCode1 && int.Parse(a.Element("StationCode2").Value) == stationCode2 && bool.Parse(a.Element("IsDeleted").Value)==false
                             select a).FirstOrDefault();

            if (adj != null)
            {
                adj.Element("IsDeleted").Value = true.ToString();
                XMLTools.SaveListToXMLElement(adjStationssRootElem, adjacentStationsPath);
            }
            else
                throw new BadAdjacentStationsException(stationCode1, stationCode2, "The adjacent stations does not exist");
        }

        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            return from line in ListLines
                   where line.IsDeleted == false
                   select line;

        }
        public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            return from line in ListLines
                   where predicate(line)
                   select line;

        }
        public DO.Line GetLine(int lineId)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line line = ListLines.Find(l => l.LineId == lineId && l.IsDeleted == false);
            if (line != null)
                return line;
            else
                throw new BadLineIdException(lineId, "The Line ID does not exist");
        }
        public void AddLine(DO.Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            line.LineId = XMLTools.GetRunningNumber(runningNumberPath);
            if (ListLines.FirstOrDefault(l => l.LineId == line.LineId && l.IsDeleted == false) != null)
                throw new BadLineIdException(line.LineId, "The Line ID is already exist exist");
            ListLines.Add(line);
            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
        }
        public void UpdateLine(DO.Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line lineFind = ListLines.Find(l => l.LineId == line.LineId && l.IsDeleted == false);
            if (lineFind == null)
                throw new BadLineIdException(line.LineId, "The Line ID does not exist");
            ListLines.Remove(lineFind);
            ListLines.Add(line);
            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
        }
        public void UpdateLine(int lineId, Action<DO.Line> update)
        {
            throw new NotImplementedException();
            //DO.Line lineFind = DataSource.ListLines.Find(l => l.LineId == lineId && l.IsDeleted == false);
            //if (lineFind == null)
            //    throw new BadLineIdException(lineId, "The Line ID does not exist");
            //update(lineFind);
        }
        public void DeleteLine(int lineId)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line lineFind = ListLines.Find(l => l.LineId == lineId && l.IsDeleted == false);
            if (lineFind == null)
                throw new BadLineIdException(lineId, "The Line ID does not exist");
            lineFind.IsDeleted = true;
            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
        }

        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetAllLineStations()
        {
            throw new NotImplementedException();
            //return from lStat in DataSource.ListLineStations
            //       select lStat.Clone();
        }
        public IEnumerable<DO.LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate)
        {
            throw new NotImplementedException();
            //return from lStat in DataSource.ListLineStations
            //       where predicate(lStat)
            //       select lStat.Clone();
        }
        public DO.LineStation GetLineStation(int lineId, int stationCode)
        {
            throw new NotImplementedException();
            //DO.LineStation lineStation = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineId && lStat.StationCode == stationCode));

            //if (lineStation != null)
            //    return lineStation.Clone();
            //else
            //    throw new DO.BadLineStationException(lineId, stationCode, "The station line does not exist");
        }
        public void AddLineStation(DO.LineStation lineStation)
        {
            throw new NotImplementedException();
            //if (DataSource.ListLineStations.FirstOrDefault(lStat => (lStat.LineId == lineStation.LineId && lStat.StationCode == lineStation.StationCode && lStat.IsDeleted == false)) != null)//if this line station already exists in the list
            //    throw new DO.BadLineStationException(lineStation.LineId, lineStation.StationCode, "The new line station is already exist");

            //DataSource.ListLineStations.Add(lineStation.Clone());
        }
        public void UpdateLineStation(DO.LineStation lineStation)
        {
            throw new NotImplementedException();
            //DO.LineStation lStatFind = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineStation.LineId && lStat.StationCode == lineStation.StationCode && lStat.IsDeleted == false));
            //if (lStatFind == null)
            //    throw new DO.BadLineStationException(lineStation.LineId, lineStation.StationCode, "The station line does not exist");
            //DO.LineStation newLineStation = lineStation.Clone();//copy of the bus that the function got
            //DataSource.ListLineStations.Remove(lStatFind);
            //DataSource.ListLineStations.Add(newLineStation);
        }
        public void UpdateLineStation(int lineId, int stationCode, Action<DO.LineStation> update)
        {
            throw new NotImplementedException();
            //DO.LineStation lStatFind = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineId && lStat.StationCode == stationCode && lStat.IsDeleted == false));
            //if (lStatFind == null)
            //    throw new DO.BadLineStationException(lineId, stationCode, "The station line does not exist");
            //update(lStatFind);
        }
        public void DeleteLineStation(int lineId, int stationCode)
        {
            throw new NotImplementedException();
            //DO.LineStation lStatFind = DataSource.ListLineStations.Find(lStat => (lStat.LineId == lineId && lStat.StationCode == stationCode && lStat.IsDeleted == false));
            //if (lStatFind == null)
            //    throw new DO.BadLineStationException(lineId, stationCode, "The station line does not exist");
            //lStatFind.IsDeleted = true;//delete

        }



        #endregion

        #region LineTrip
        public IEnumerable<DO.LineTrip> GetAllLineTrips()
        {
            throw new NotImplementedException();
            //return from lTrip in DataSource.ListLineTrips
            //       select lTrip.Clone();
        }
        public IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate)
        {
            throw new NotImplementedException();
            //return from lTrip in DataSource.ListLineTrips
            //       where predicate(lTrip)
            //       select lTrip.Clone();
        }
        public DO.LineTrip GetLineTrip(int lineId, TimeSpan time)
        {
            throw new NotImplementedException();
            //DO.LineTrip lineTrip = DataSource.ListLineTrips.Find(l => l.LineId == lineId && l.StartAt == time && l.IsDeleted == false);

            //if (lineTrip != null)
            //    return lineTrip.Clone();
            //else
            //    throw new BadLineTripException(lineId, time, "The line trip does not exist");
        }
        public void AddLineTrip(DO.LineTrip lineTrip)
        {
            throw new NotImplementedException();
            //if (DataSource.ListLineTrips.FirstOrDefault(l => l.LineId == lineTrip.LineId && l.StartAt == lineTrip.StartAt && l.IsDeleted == false) != null)
            //    throw new BadLineTripException(lineTrip.LineId, lineTrip.StartAt, "The line trip is already exist");
            //DataSource.ListLineTrips.Add(lineTrip.Clone());
        }
        public void UpdateLineTrip(DO.LineTrip lineTrip)
        {
            throw new NotImplementedException();
            //DO.LineTrip lTripFind = DataSource.ListLineTrips.Find(l => l.LineId == lineTrip.LineId && l.StartAt == lineTrip.StartAt && l.IsDeleted == false);
            //if (lTripFind == null)
            //    throw new BadLineTripException(lineTrip.LineId, lineTrip.StartAt, "The line trip does not exist");
            //DO.LineTrip newLTrip = lineTrip.Clone();//copy of the bus that the function got
            //DataSource.ListLineTrips.Remove(lTripFind);
            //DataSource.ListLineTrips.Add(newLTrip);
            ////lTripFind = newLTrip;//update
        }
        public void UpdateLineTrip(int lineId, TimeSpan time, Action<DO.LineTrip> update)
        {
            throw new NotImplementedException();
            //DO.LineTrip lTripFind = DataSource.ListLineTrips.Find(l => l.LineId == lineId && l.StartAt == time && l.IsDeleted == false);
            //if (lTripFind == null)
            //    throw new BadLineTripException(lineId, time, "The line trip does not exist");
            //update(lTripFind);
        }
        public void DeleteLineTrip(int lineId, TimeSpan time)
        {
            throw new NotImplementedException();
            //DO.LineTrip lineTrip = DataSource.ListLineTrips.Find(l => l.LineId == lineId && l.StartAt == time && l.IsDeleted == false);
            //if (lineTrip == null)
            //    throw new BadLineTripException(lineId, time, "The line trip does not exist");
            //lineTrip.IsDeleted = true;
        }

        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            return from stat in ListStations
                   where stat.IsDeleted == false
                   select stat;
        }
        public IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            return from stat in ListStations
                   where predicate(stat)
                   select stat;
        }
        public DO.Station GetStation(int code)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            DO.Station station = ListStations.Find(s => s.Code == code && s.IsDeleted == false);
            if (station != null)
                return station;
            else
                throw new BadStationCodeException(code, "The station does not exist");
        }
        public void AddStation(DO.Station station)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            if (ListStations.FirstOrDefault(s => s.Code == station.Code && s.IsDeleted == false) != null)
                throw new DO.BadStationCodeException(station.Code, "A station with this code is already exist");
            ListStations.Add(station);
            XMLTools.SaveListToXMLSerializer(ListStations, stationsPath);
        }
        public void UpdateStation(DO.Station station)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            DO.Station statFind = ListStations.FirstOrDefault(s => s.Code == station.Code && s.IsDeleted == false);
            if (statFind == null)
                throw new BadStationCodeException(statFind.Code, "The station does not exist");
            ListStations.Remove(statFind);
            ListStations.Add(station);
            XMLTools.SaveListToXMLSerializer(ListStations, stationsPath);
        }
        public void UpdateStation(int code, Action<DO.Station> update)
        {
            throw new NotImplementedException();
            //DO.Station statFind = DataSource.ListStations.FirstOrDefault(s => s.Code == code && s.IsDeleted == false);
            //if (statFind == null)
            //    throw new BadStationCodeException(code, "The station does not exist");
            //update(statFind);
        }
        public void DeleteStation(int code)
        {
            //throw new NotImplementedException();
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            DO.Station statFind = ListStations.FirstOrDefault(s => s.Code == code && s.IsDeleted == false);
            if (statFind == null)
                throw new BadStationCodeException(code, "The station does not exist");
            statFind.IsDeleted = true;
            XMLTools.SaveListToXMLSerializer(ListStations, stationsPath);
        }

        #endregion

        #region Trip
        public IEnumerable<DO.Trip> GetAllTrips()
        {
            throw new NotImplementedException();
            //return from trip in DataSource.ListTrips
            //       select trip.Clone();
        }
        public IEnumerable<DO.Trip> GetAllTripsBy(Predicate<DO.Trip> predicate)
        {
            throw new NotImplementedException();
            //return from trip in DataSource.ListTrips
            //       where predicate(trip)
            //       select trip.Clone();

        }
        public DO.Trip GetTrip(int tripId)
        {
            throw new NotImplementedException();
            //DO.Trip trip = DataSource.ListTrips.Find(t => t.TripId == tripId && t.IsDeleted == false);
            //if (trip != null)
            //    return trip.Clone();
            //else
            //    throw new Exception();
        }
        public void AddTrip(DO.Trip trip)
        {
            throw new NotImplementedException();
            //if (DataSource.ListTrips.FirstOrDefault(t => t.TripId == trip.TripId && t.IsDeleted == false) != null)
            //    throw new Exception();
            //DataSource.ListTrips.Add(trip.Clone());
        }
        public void UpdateTrip(DO.Trip trip)
        {
            throw new NotImplementedException();
            //DO.Trip tripFind = DataSource.ListTrips.Find(t => t.TripId == trip.TripId && t.IsDeleted == false);
            //if (tripFind == null)
            //    throw new Exception();
            //DO.Trip newTrip = trip.Clone();//copy of the bus that the function got
            //tripFind = newTrip;//update
        }
        public void UpdateTrip(int tripId, Action<DO.Trip> update)
        {
            throw new NotImplementedException();
            //DO.Trip tripFind = DataSource.ListTrips.Find(t => t.TripId == tripId && t.IsDeleted == false);
            //if (tripFind == null)
            //    throw new Exception();
            //update(tripFind);
        }
        public void DeleteTrip(int tripId)
        {
            throw new NotImplementedException();
            //DO.Trip trip = DataSource.ListTrips.Find(t => t.TripId == tripId && t.IsDeleted == false);
            //if (trip == null)
            //    throw new Exception();
            //trip.IsDeleted = true;
        }

        #endregion
    }
}
