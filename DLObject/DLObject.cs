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

    }
}
