using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class UnitTypeData
    {
        public static List<UnitType> Get()
        {
            List<UnitType> list = new List<UnitType>();
            list.Add(new UnitType
            {
                UnitTypeID = 1,
                UnitParentTypeID = 1,
                UnitTypeDescription = "Bottle"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 2,
                UnitParentTypeID = 1,
                UnitTypeDescription = "Box"
            });
            //new UnitType
            //{
            //    UnitTypeID = 19,
            //    UnitParentTypeID = 1,
            //    UnitTypeDescription = "Case"
            //},
            list.Add(new UnitType
            {
                UnitTypeID = 3,
                UnitParentTypeID = 1,
                UnitTypeDescription = "Pack"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 4,
                UnitParentTypeID = 1,
                UnitTypeDescription = "Bag"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 5,
                UnitParentTypeID = 1,
                UnitTypeDescription = "Unit"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 6,
                UnitParentTypeID = 1,
                UnitTypeDescription = "Vial"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 24,
                UnitParentTypeID = 1,
                UnitTypeDescription = "Tube"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 7,
                UnitParentTypeID = 2,
                UnitTypeDescription = "Kg"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 8,
                UnitParentTypeID = 2,
                UnitTypeDescription = "gr"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 9,
                UnitParentTypeID = 2,
                UnitTypeDescription = "mg"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 10,
                UnitParentTypeID = 2,
                UnitTypeDescription = "ug"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 11,
                UnitParentTypeID = 2,
                UnitTypeDescription = "Liter"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 12,
                UnitParentTypeID = 2,
                UnitTypeDescription = "ml"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 13,
                UnitParentTypeID = 2,
                UnitTypeDescription = "ul"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 14,
                UnitParentTypeID = 2,
                UnitTypeDescription = "gal"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 20,
                UnitParentTypeID = 2,
                UnitTypeDescription = "pmol"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 21,
                UnitParentTypeID = 2,
                UnitTypeDescription = "nmol"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 22,
                UnitParentTypeID = 2,
                UnitTypeDescription = "umol"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 23,
                UnitParentTypeID = 2,
                UnitTypeDescription = "mol"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 15,
                UnitParentTypeID = 3,
                UnitTypeDescription = "rxhs"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 16,
                UnitParentTypeID = 3,
                UnitTypeDescription = "test"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 17,
                UnitParentTypeID = 3,
                UnitTypeDescription = "preps"
            });
            list.Add(new UnitType
            {
                UnitTypeID = 18,
                UnitParentTypeID = 3,
                UnitTypeDescription = "assays"
            });
            list.Add(new UnitType
            {
                UnitTypeID = -1,
                UnitParentTypeID = 1,
                UnitTypeDescription = "Quartzy Unit"
            });
            return list;
        }
    }
}
