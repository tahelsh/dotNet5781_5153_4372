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

    }
}
