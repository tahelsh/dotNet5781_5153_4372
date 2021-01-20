using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
//using BO;


namespace BLAPI
{
    public interface IBL
    {
        //Add Person to Course
        //get all courses for student
        //etc...
        #region Bus
        BO.Bus GetBus(int licenseNum);
        IEnumerable<BO.Bus> GetAllBuses();
        IEnumerable<BO.Bus> GetBusesBy(Predicate<BO.Bus> predicate);
        void UpdateBusDetails(BO.Bus bus);
        void DeleteBus(int licenseNum);
        void RefuelBus(BO.Bus busBO);
        void TreatBus(BO.Bus busBO);
        #endregion

        #region Line
        void AddNewLine(BO.Line lineBo);
        BO.Line GetLine(int lineId);
        IEnumerable<BO.Line> GetAllLines();
        //IEnumerable<BO.ListedPerson> GetStudentIDNameList();
        IEnumerable<BO.Line> GelAllLinesBy(Predicate<BO.Line> predicate);
        void UpdateLineDetails(BO.Line line);
        void DeleteLine(int LineId);
        void AddBus(BO.Bus bus);
        List<string> FindRoute(int stationCode1, int stationCode2);
        #endregion

        #region Station
        BO.Station StationDoBoAdapter(DO.Station stationDO);
        IEnumerable<BO.Station> GetAllStations();
        void AddStation(BO.Station stat);
        void DeleteStation(int code);
        void UpdateStation(BO.Station stationBO);
        IEnumerable<BO.LineTiming> GetLineTimingPerStation(BO.Station station, TimeSpan tsCurrentTime);

        #endregion

        #region LineStation
        void AddLineStation(BO.LineStation s);
        void DeleteLineStation(int lineId, int stationCode);
        
        #endregion

        #region AdjacentStations
        bool IsExistAdjacentStations(int stationCode1, int stationCode2);
        //void AddAdjacentStations(BO.AdjacentStation adjBO);
        #endregion

        #region StationInLine
        void UpdateTimeAndDistance(BO.StationInLine first, BO.StationInLine second);
        #endregion

        #region User
        void AddUser(BO.User userBO);
        BO.User SignIn(string username, int passcode);
        #endregion

        #region LineTrip
        void DeleteDepTime(int lineId, TimeSpan dep);
        void AddDepTime(int lineId, TimeSpan dep);

        #endregion

    }
}
