using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DLAPI
{
    //CRUD Logic:
    // Create - add new instance
    // Request - ask for an instance or for a collection
    // Update - update properties of an instance
    // Delete - delete an instance
    public interface IDL
    {
        #region Bus
        IEnumerable<Bus> GetAllBuses();
        IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate);
        Bus GetBus(int licenseNum);
        void AddBus(Bus bus);
        void UpdateBus(Bus bus);
        void UpdateBus(int licenseNum, Action<Bus> update); //method that knows to updt specific fields in Bus
        void DeleteBus(int licenseNum);
        #endregion

        #region AdjacentStations
        bool IsExistAdjacentStations(int stationCode1, int stationCode2);
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate);
        DO.AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2);
        void AddAdjacentStations(DO.AdjacentStations adjStations);
        void UpdateAdjacentStations(DO.AdjacentStations adjStations);
        void UpdateAdjacentStations(int stationCode1, int stationCode2, Action<DO.AdjacentStations> update);
        void DeleteAdjacentStations(int stationCode1, int stationCode2);

        #endregion

        #region Line
        IEnumerable<DO.Line> GetAllLines();
        IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate);
        DO.Line GetLine(int lineId);
        void AddLine(DO.Line line);
        void UpdateLine(DO.Line line);
        void UpdateLine(int lineId, Action<DO.Line> update);
        void DeleteLine(int lineId);
        #endregion

        #region LineStation
        IEnumerable<DO.LineStation> GetAllLineStations();
        IEnumerable<DO.LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate);
        DO.LineStation GetLineStation(int lineId, int stationCode);
        void AddLineStation(DO.LineStation lineStation);
        void UpdateLineStation(DO.LineStation lineStation);
        void UpdateLineStation(int lineId, int stationCode, Action<DO.LineStation> update);
        void DeleteLineStation(int lineId, int stationCode);

        #endregion

        #region LineTrip
        IEnumerable<DO.LineTrip> GetAllLineTrips();
        IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate);
        DO.LineTrip GetLineTrip(int lineTripId);
        void AddLineTrip(DO.LineTrip lineTrip);
        void UpdateLineTrip(DO.LineTrip lineTrip);
        void UpdateLineTrip(int lineTripId, Action<DO.LineTrip> update);
        void DeleteLineTrip(int lineTripId);

        #endregion

        #region User
        IEnumerable<DO.User> GetAllUsers();
        IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate);
        DO.User GetUser(string userName);
        void AddUser(DO.User user);
        void UpdateUser(DO.User user);
        void UpdateUser(string userName, Action<DO.User> update);
        void DeleteUser(string userName);
        #endregion

        #region Station
        IEnumerable<DO.Station> GetAllStations();
        IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate);
        DO.Station GetStation(int code);
        void AddStation(DO.Station station);
        void UpdateStation(DO.Station station);
        void UpdateStation(int code, Action<DO.Station> update);
        void DeleteStation(int code);
        #endregion

        #region Trip
        IEnumerable<DO.Trip> GetAllTrips();
        IEnumerable<DO.Trip> GetAllTripsBy(Predicate<DO.Trip> predicate);
        DO.Trip GetTrip(int tripId);
        void AddTrip(DO.Trip trip);
        void UpdateTrip(DO.Trip trip);
        void UpdateTrip(int tripId, Action<DO.Trip> update);
        void DeleteTrip(int tripId);
        #endregion
    }
}
