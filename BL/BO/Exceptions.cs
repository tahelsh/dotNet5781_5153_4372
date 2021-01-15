using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class BadLicenseNumException : Exception
    {
        public int licenseNum;
        public BadLicenseNumException(string message, Exception innerException) :
         base(message, innerException) => licenseNum = ((DO.BadLicenseNumException)innerException).licenseNum;
        public override string ToString() => base.ToString() + $", bad license number: {licenseNum}";
    }
    public class BadInputException : Exception
    {
        public BadInputException(string message) : base(message) { }
        public override string ToString() => base.ToString();
    }
    public class BadLineIdException : Exception
    {
        public int ID;
        public BadLineIdException(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.BadTripIdException)innerException).ID;
        public override string ToString() => base.ToString() + $", bad Line ID number: {ID}";
    }
    public class BadStationCodeException : Exception
    {
        public int stationCode;
        public BadStationCodeException(string message, Exception innerException) :
            base(message, innerException) => stationCode = ((DO.BadStationCodeException)innerException).stationCode;
        public BadStationCodeException(int code, string message) :
           base(message) => stationCode = code;
        public override string ToString() => base.ToString() + $", bad station code number: {stationCode}";
    }
    public class BadUserNameException : Exception
    {
        public string userName;
        public BadUserNameException(string message, Exception innerException) :
           base(message, innerException) => userName = ((DO.BadUserNameException)innerException).userName;
        public BadUserNameException(string name, string message) :
            base(message) => userName = name;
        public override string ToString() => base.ToString() + $", bad station code number: {userName}";
    }
    public class BadLineStationException : Exception
    {
        public int lineId;
        public int stationCode;

        public BadLineStationException(string message, Exception innerException) :
          base(message, innerException)
        {
            lineId = ((DO.BadLineStationException)innerException).lineId;
            stationCode = ((DO.BadLineStationException)innerException).stationCode;
        }
        public BadLineStationException(int _lineId, int _stationCode, string message) :
            base(message)
        { lineId = _lineId; stationCode = _stationCode; }
        
        public override string ToString()
        {
            return Message;
        }
    }
    public class BadAdjacentStationsException : Exception
    {
        public int stationCode1;
        public int stationCode2;
        public BadAdjacentStationsException(string message, Exception innerException) :
          base(message, innerException)
        {
            stationCode1 = ((DO.BadAdjacentStationsException)innerException).stationCode1;
            stationCode2 = ((DO.BadAdjacentStationsException)innerException).stationCode2;
        }
        public override string ToString()
        {
            return Message;
        }
    }
    public class BadLineTripException : Exception
    {
        public int lineId;
        public TimeSpan depTime;//departure time
        public BadLineTripException(string message, Exception innerException) :
         base(message, innerException)
        {
            lineId = ((DO.BadLineTripException)innerException).lineId;
            depTime = ((DO.BadLineTripException)innerException).depTime;
        }
        public override string ToString()
        {
            return Message;
        }
    }
}
