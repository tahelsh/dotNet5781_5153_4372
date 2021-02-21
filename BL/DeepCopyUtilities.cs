using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class DeepCopyUtilities
    {
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
        }
        public static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
            return to;
        }

        public static BO.StationInLine CopyToStationInLine(this DO.Station st, DO.LineStation s)
            //the function return BO.StationInLine from data of  DO.Station and DO.LineStation
        {
            BO.StationInLine stationInLine = new BO.StationInLine();//this is what the function will return
            st.CopyPropertiesTo(stationInLine);//copy details from DO.Station to BO.StationInLine
            stationInLine.LineStationIndex = s.LineStationIndex;//copy index from DO.LineStation
            stationInLine.StationCode = s.StationCode;//copy station code from DO.LineStation
            return stationInLine;
        }
        public static BO.LineInStation CopyToLineInStation(this DO.Line l, DO.LineStation s)
        //the function return BO.LineInStation from data of DO.Line and DO.LineStation
        {
            BO.LineInStation lineInStation = new BO.LineInStation();//this is what the function will return
            l.CopyPropertiesTo(lineInStation);//copy details from DO.Line to BO.LineInStation
            lineInStation.LineStationIndex = s.LineStationIndex;//copy index from DO.LineStation
            lineInStation.Area = (BO.Area)Enum.Parse(typeof(BO.Area), l.Area.ToString());//copy area from DO.LineStation
            return lineInStation;
        }
    }
}

