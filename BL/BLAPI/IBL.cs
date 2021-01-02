using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        IEnumerable<BO.Bus> GetAllSBuses();
        IEnumerable<BO.Bus> GetBusesBy(Predicate<BO.Bus> predicate);
        void UpdateBusDetails(BO.Bus bus);
        void DeleteBus(int licenseNum);
        #endregion

        #region Line
        BO.Line GetLine(int lineId);
        IEnumerable<BO.Line> GetAllLines();
        //IEnumerable<BO.ListedPerson> GetStudentIDNameList();
        IEnumerable<BO.Line> GelAllLinesBy(Predicate<BO.Line> predicate);
        void UpdateLineDetails(BO.Line line);
        void DeleteLine(int LineId);
        void AddBus(BO.Bus bus);
        #endregion




    }
}
