using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class CompanyDayOffTypeData
    {

        public static List<CompanyDayOffType> Get()
        {
            List<CompanyDayOffType> list = new List<CompanyDayOffType>();
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 1,
                Name = "Purim 1"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 2,
                Name = "Purim 2"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 3,
                Name = "Erev Pesach"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 4,
                Name = "Pesach"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 5,
                Name = "Erev Shviei Pesach"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 6,
                Name = "Shviei Pesach"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 7,
                Name = "Yom Hazmaut"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 8,
                Name = "Erev Shavuous"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 9,
                Name = "Shavuous"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 10,
                Name = "Erev Rosh Hashana"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 11,
                Name = "Rosh Hashana 1"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 12,
                Name = "Rosh Hashana 2"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 13,
                Name = "Erev Yom Kippur"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 14,
                Name = "Yom Kippur"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 15,
                Name = "Erev Sukkot"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 16,
                Name = "Sukkot"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 17,
                Name = "Erev Simchat Torah"
            });
            list.Add(new CompanyDayOffType
            {
                CompanyDayOffTypeID = 18,
                Name = "Simchat Torah"
            });
            
            return list;
        }

    }
}
