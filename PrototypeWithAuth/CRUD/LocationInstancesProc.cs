using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PrototypeWithAuth.CRUD
{
    public class LocationInstancesProc : ApplicationDbContextProc<LocationInstance>
    {
        public LocationInstancesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> UpdateAsync(AddLocationViewModel addLocationViewModel, SubLocationViewModel subLocationViewModel)
        {
            StringWithBool ReturnVal = new StringWithBool();
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    List<LocationInstance> waitingLocations = new List<LocationInstance>();
                    List<List<int>> placeholderInstanceIds = new List<List<int>>();
                    bool first = true;
                    var lastParent = new LocationInstance();
                    string currentAbbrev = "";
                    string typeName = "";
                    int typeId, parentId, typeNumber = 0;
                    int locationNumber = 1;
                    if (subLocationViewModel.LocationTypeParentID != 500)
                    {
                        //save parent
                        addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                        addLocationViewModel.LocationInstance.Width = 1;
                        addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;
                        addLocationViewModel.LocationInstance.LocationRoomInstanceID = subLocationViewModel.LocationInstances[0].LocationRoomInstanceID;

                        _context.Add(addLocationViewModel.LocationInstance);
                        await _context.SaveChangesAsync();
                    }
                    switch (subLocationViewModel.LocationTypeParentID)
                    {
                        case 100:

                            int previousH = addLocationViewModel.LocationInstance.Height;
                            int previousW = addLocationViewModel.LocationInstance.Width;
                            for (int a = 0; a < subLocationViewModel.LocationInstances.Count; a++)
                            {
                                waitingLocations.Clear();
                                placeholderInstanceIds.Add(new List<int>());
                                typeName = _locationTypesProc.ReadOne(new List<Expression<Func<LocationType, bool>>> { x => x.LocationTypeID == subLocationViewModel.LocationInstances[a].LocationTypeID })
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                                typeId = subLocationViewModel.LocationInstances[a].LocationTypeID;
                                parentId = 0;
                                int height = 0;
                                int width = 0;
                                if (a == 0) //this should just be for rack
                                {
                                    height = subLocationViewModel.LocationInstances[1].Height;
                                    width = 1;
                                }
                                else if (a == 1)
                                {
                                    height = subLocationViewModel.LocationInstances[2].Height;
                                    width = subLocationViewModel.LocationInstances[2].Height;
                                }
                                int amountOfParentLevels = 1;
                                if (!first)
                                {
                                    amountOfParentLevels = placeholderInstanceIds[a - 1].Count;
                                }
                                for (int w = 0; w < amountOfParentLevels; w++)//until finished with names from the list before
                                {
                                    if (first)
                                    {
                                        //if this is the first level - locations with no parents
                                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                                        first = false;
                                    }
                                    else
                                    {
                                        parentId = placeholderInstanceIds[a - 1][w]; //get the first id in the list in the depth before
                                    }
                                    lastParent = await ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == parentId });
                                    locationNumber = 1; //reset location number to order locations
                                    typeNumber = 1; //the number of this depth added to this name
                                                    //RESET THE HEIGHTS ANDS WIDTHS TO ACCOUNT FOR FIRSTS BEFORE RUNNIN OR ITLL CRASHs
                                    int sublocationHeight = subLocationViewModel.LocationInstances[a].Height;
                                    for (int x = 0; x < sublocationHeight; x++)
                                    {
                                        //add letter to place
                                        int unicode = x + 65;
                                        char character = (char)unicode;
                                        int sublocationWidth = subLocationViewModel.LocationInstances[a].Width;
                                        if (a == 2)
                                        {
                                            sublocationWidth = sublocationHeight;
                                            typeName = character.ToString();
                                            typeNumber = 1;
                                        }
                                        else if (sublocationWidth == 0)
                                        {
                                            sublocationWidth = 1;
                                        }
                                        for (int y = 0; y < sublocationWidth; y++)
                                        {
                                            //add number to place
                                            currentAbbrev = typeName + (typeNumber).ToString(); //add number to the type x + y is the current number but is zero based so add one
                                            typeNumber++; //increment this
                                            LocationInstance newSublocationInstance = new LocationInstance()
                                            {
                                                LocationInstanceAbbrev = currentAbbrev,
                                                LocationInstanceName = lastParent.LocationInstanceName + currentAbbrev,
                                                LocationInstanceParentID = parentId,
                                                Height = height,
                                                Width = width,
                                                LocationTypeID = typeId,
                                                LocationNumber = locationNumber
                                            };
                                            locationNumber++;
                                            _context.Add(newSublocationInstance);
                                            waitingLocations.Add(newSublocationInstance);
                                        }
                                    }
                                }
                                await _context.SaveChangesAsync();
                                foreach (var location in waitingLocations)
                                {
                                    placeholderInstanceIds[a].Add(location.LocationInstanceID);
                                }
                            }
                            break;
                        case 200:

                            previousH = addLocationViewModel.LocationInstance.Height;
                            previousW = addLocationViewModel.LocationInstance.Width;
                            for (int b = 0; b < subLocationViewModel.LocationInstances.Count; b++)
                            {
                                waitingLocations.Clear();
                                placeholderInstanceIds.Add(new List<int>());
                                typeName = _locationTypesProc.ReadOne(new List<Expression<Func<LocationType, bool>>> { x => x.LocationTypeID == subLocationViewModel.LocationInstances[b].LocationTypeID })
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                                typeId = subLocationViewModel.LocationInstances[b].LocationTypeID;
                                parentId = 0;
                                int height = 0;
                                int width = 0;
                                if (b == 0)
                                {
                                    height = subLocationViewModel.LocationInstances[1].Height;
                                    width = 1;
                                }
                                else if (b == 3)
                                {
                                    height = subLocationViewModel.LocationInstances[4].Height;
                                    width = subLocationViewModel.LocationInstances[4].Height;
                                }
                                else
                                {
                                    height = subLocationViewModel.LocationInstances[b - 1].Height;
                                    width = subLocationViewModel.LocationInstances[b - 1].Height;
                                }
                                int amountOfParentLevels = 1;
                                if (!first)
                                {
                                    amountOfParentLevels = placeholderInstanceIds[b - 1].Count;
                                }
                                for (int w = 0; w < amountOfParentLevels; w++)//until finished with names from the list before
                                {
                                    if (first)
                                    {
                                        //if this is the first level - locations with no parents
                                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                                        first = false;
                                    }
                                    else
                                    {
                                        parentId = placeholderInstanceIds[b - 1][w]; //get the first id in the list in the depth before
                                    }
                                    lastParent = await ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == parentId });
                                    if (lastParent != null && !lastParent.IsEmptyShelf)
                                    {
                                        locationNumber = 1;
                                        typeNumber = 1; //the number of this depth added to this name
                                                        //RESET THE HEIGHTS ANDS WIDTHS TO ACCOUNT FOR FIRSTS BEFORE RUNNIN OR ITLL CRASHs
                                        int sublocationHeight = subLocationViewModel.LocationInstances[b].Height;

                                        for (int x = 0; x < sublocationHeight; x++)
                                        {
                                            int unicode = x + 65;
                                            char character = (char)unicode;
                                            int sublocationWidth = subLocationViewModel.LocationInstances[b].Width;
                                            if (b == 4)
                                            {
                                                sublocationWidth = sublocationHeight;
                                                typeName = character.ToString();
                                                typeNumber = 1;
                                            }
                                            else if (sublocationWidth == 0)
                                            {
                                                sublocationWidth = 1;
                                            }
                                            for (int y = 0; y < sublocationWidth; y++)
                                            {
                                                currentAbbrev = typeName + (typeNumber).ToString();
                                                typeNumber++; //increment this

                                                LocationInstance newSublocationInstance = new LocationInstance()
                                                {
                                                    LocationInstanceAbbrev = currentAbbrev,
                                                    LocationInstanceName = lastParent.LocationInstanceName + currentAbbrev,
                                                    LocationInstanceParentID = parentId,
                                                    Height = height,
                                                    Width = width,
                                                    LocationNumber = locationNumber,
                                                    LocationTypeID = typeId
                                                };
                                                locationNumber++;
                                                if (b == 0) //Testing Shelves
                                                {
                                                    if (subLocationViewModel.EmptyShelves80?.ContainsKey(x) == true && subLocationViewModel.EmptyShelves80[x])
                                                    {
                                                        newSublocationInstance.IsEmptyShelf = true;
                                                    }
                                                }
                                                _context.Add(newSublocationInstance);
                                                waitingLocations.Add(newSublocationInstance);
                                            }
                                        }
                                    }
                                }
                                await _context.SaveChangesAsync();
                                foreach (var location in waitingLocations)
                                {
                                    placeholderInstanceIds[b].Add(location.LocationInstanceID);
                                }
                            }
                            break;
                        case 300:
                        case 400: //for now the same as 300
                            typeName = _locationTypesProc.ReadOne(new List<Expression<Func<LocationType, bool>>> { x => x.LocationTypeID == subLocationViewModel.LocationInstances[0].LocationTypeID })
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                            typeId = subLocationViewModel.LocationInstances[0].LocationTypeID;
                            typeNumber = 1;

                            parentId = 0;
                            parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                            lastParent = addLocationViewModel.LocationInstance;

                            int sublocationHeight1 = subLocationViewModel.LocationInstances[0].Height;

                            for (int x = 0; x < sublocationHeight1; x++)
                            {
                                waitingLocations.Clear();
                                int sublocationWidth1 = subLocationViewModel.LocationInstances[0].Width;
                                if (sublocationWidth1 == 0) { sublocationWidth1 = 1; }
                                for (int y = 0; y < sublocationWidth1; y++)
                                {
                                    currentAbbrev = typeName + (typeNumber).ToString();
                                    typeNumber++; //increment this
                                    LocationInstance newSublocationInstance = new LocationInstance()
                                    {
                                        LocationInstanceAbbrev = currentAbbrev,
                                        LocationInstanceName = lastParent.LocationInstanceName + currentAbbrev,
                                        LocationInstanceParentID = parentId,
                                        LocationTypeID = typeId,
                                        IsEmptyShelf = true
                                    };
                                    _context.Add(newSublocationInstance);
                                    waitingLocations.Add(newSublocationInstance);
                                }
                            }
                            await _context.SaveChangesAsync();
                            break;
                        case 500:
                            for (int i = 0; i < subLocationViewModel.LocationInstances.Count(); i++)
                            {
                                var childHeight = subLocationViewModel.LocationInstances[i].Height;

                                if (i == 0)
                                {
                                    var existingRoom = await ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationTypeID == 500 && li.LocationRoomInstanceID == subLocationViewModel.LocationInstances[i].LocationRoomInstanceID });
                                    if (existingRoom == null)
                                    {
                                        var room = await _locationRoomInstancesProc.ReadOneAsync(new List<Expression<Func<LocationRoomInstance, bool>>> { lp => lp.LocationRoomInstanceID == subLocationViewModel.LocationInstances[i].LocationRoomInstanceID });
                                        subLocationViewModel.LocationInstances[i].LocationInstanceName = room.LocationRoomInstanceAbbrev;
                                        subLocationViewModel.LocationInstances[i].LocationInstanceAbbrev = room.LocationRoomInstanceAbbrev;
                                        subLocationViewModel.LocationInstances[i].LocationTypeID = 500;
                                        subLocationViewModel.LocationInstances[i].Width = 1;
                                        subLocationViewModel.LocationInstances[i].Height = 1;

                                        _context.Add(subLocationViewModel.LocationInstances[i]);
                                        lastParent = subLocationViewModel.LocationInstances[i];
                                    }
                                    else
                                    {
                                        existingRoom.Height = existingRoom.Height++;
                                        _context.Update(existingRoom);
                                        lastParent = existingRoom;
                                    }
                                    await _context.SaveChangesAsync();

                                }
                                else if (i == 1)
                                {
                                    subLocationViewModel.LocationInstances[i].LocationInstanceParentID = lastParent.LocationInstanceID;

                                    subLocationViewModel.LocationInstances[i].Width = 1;
                                    var labPart = await _labPartsProc.ReadOneAsync(new List<Expression<Func<LabPart, bool>>> { lp => lp.LabPartID == subLocationViewModel.LocationInstances[i].LabPartID });

                                    var labPartByTypeCount = Read(new List<Expression<Func<LocationInstance, bool>>> { l => l.LabPartID == subLocationViewModel.LocationInstances[i].LabPartID && l.LocationInstanceParentID == subLocationViewModel.LocationInstances[i].LocationInstanceParentID }).Count();

                                    var labPartNameAbrev = labPart.LabPartNameAbbrev;
                                    labPartNameAbrev += (labPartByTypeCount + 1);

                                    subLocationViewModel.LocationInstances[i].LocationInstanceName = lastParent.LocationInstanceName + labPartNameAbrev;
                                    subLocationViewModel.LocationInstances[i].LocationInstanceAbbrev = labPartNameAbrev;
                                    subLocationViewModel.LocationInstances[i].LocationNumber = labPartByTypeCount + 1;


                                    if (!labPart.HasShelves)
                                    {
                                        subLocationViewModel.LocationInstances[i].IsEmptyShelf = !labPart.HasShelves;
                                        subLocationViewModel.LocationInstances[i].Height = 1;
                                    }
                                    else
                                    {
                                        subLocationViewModel.LocationInstances[i].IsEmptyShelf = subLocationViewModel.LocationInstances[i + 1].Height == 0;
                                        subLocationViewModel.LocationInstances[i].Height = subLocationViewModel.LocationInstances[i + 1].Height == 0 ? 1 : subLocationViewModel.LocationInstances[i + 1].Height;
                                    }
                                    _context.Add(subLocationViewModel.LocationInstances[i]);
                                    await _context.SaveChangesAsync();
                                    lastParent = subLocationViewModel.LocationInstances[i];
                                }
                                else if (i == 2)
                                {
                                    var childLocationType = await _locationTypesProc.ReadOneAsync(new List<Expression<Func<LocationType, bool>>> { lt => lt.LocationTypeID == subLocationViewModel.LocationInstances[i].LocationTypeID });

                                    for (int y = 0; y < childHeight; y++)
                                    {
                                        currentAbbrev = childLocationType.LocationTypeNameAbbre + (y + 1);
                                        _context.Add(new LocationInstance()
                                        {
                                            LocationInstanceParentID = lastParent.LocationInstanceID,
                                            LocationTypeID = subLocationViewModel.LocationInstances[i].LocationTypeID,
                                            Height = subLocationViewModel.LocationInstances[i].Height = 1,
                                            Width = subLocationViewModel.LocationInstances[i].Width = 1,
                                            LocationInstanceAbbrev = currentAbbrev,
                                            LocationInstanceName = lastParent.LocationInstanceName + currentAbbrev,
                                            IsEmptyShelf = true,
                                            LocationNumber = locationNumber
                                        });
                                        locationNumber++;
                                    }
                                    await _context.SaveChangesAsync();
                                }
                            }
                            break;
                        default:
                            //add reference to parent
                            //filling up the heights and widths with the ones put in for the location below them
                            addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                            addLocationViewModel.LocationInstance.Width = subLocationViewModel.LocationInstances[0].Width;
                            addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;
                            _context.Add(addLocationViewModel.LocationInstance);
                            await _context.SaveChangesAsync();

                            string nameAbbreviation = addLocationViewModel.LocationInstance.LocationInstanceName;

                            int prevHeight = addLocationViewModel.LocationInstance.Height;
                            int prevWidth = addLocationViewModel.LocationInstance.Width;
                            for (int z = 0; z < subLocationViewModel.LocationInstances.Count; z++)/*var locationInstance in subLocationViewModel.LocationInstances*/ //for each level in the sublevels
                            {
                                //initiate new lists of placeholders otherwise will get an error when you try to insert them
                                placeholderInstanceIds.Add(new List<int>());
                                //namesPlaceholder[z] = new List<string>();
                                //placeholderInstanceIds[z] = new List<int>();

                                typeName = _locationTypesProc.ReadOne(new List<Expression<Func<LocationType, bool>>> { x => x.LocationTypeID == subLocationViewModel.LocationInstances[z].LocationTypeID })
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                                typeId = subLocationViewModel.LocationInstances[z].LocationTypeID;
                                parentId = 0;
                                int height = 0;
                                int width = 0;
                                if (z < subLocationViewModel.LocationInstances.Count - 1)
                                {

                                    height = subLocationViewModel.LocationInstances[z + 1].Height;
                                    width = subLocationViewModel.LocationInstances[z + 1].Width;
                                }
                                string attachedName = "";
                                int amountOfParentLevels = 1;
                                if (!first)
                                {
                                    amountOfParentLevels = placeholderInstanceIds[z - 1].Count;
                                }
                                for (int w = 0; w < amountOfParentLevels; w++)//until finished with names from the list before
                                {
                                    if (first)
                                    {
                                        //if this is the first level - locations with no parents
                                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                                        attachedName = typeName;
                                        first = false;
                                    }
                                    else
                                    {
                                        parentId = placeholderInstanceIds[z - 1][w]; //get the first id in the list in the depth before
                                        attachedName = typeName; //NEEDS TO BE DONE BETTER
                                    }
                                    typeNumber = 1; //the number of this depth added to this name
                                                    //RESET THE HEIGHTS ANDS WIDTHS TO ACCOUNT FOR FIRSTS BEFORE RUNNIN OR ITLL CRASHs
                                    for (int x = 0; x < subLocationViewModel.LocationInstances[z].Height; x++)
                                    {
                                        //add letter to place
                                        int unicode = x + 65;
                                        char character = (char)unicode;
                                        for (int y = 0; y < subLocationViewModel.LocationInstances[z].Width; y++)
                                        {
                                            //add number to place
                                            string currentName = attachedName + (typeNumber).ToString(); //add number to the type x + y is the current number but is zero based so add one
                                            typeNumber++; //increment this
                                            LocationInstance newSublocationInstance = new LocationInstance()
                                            {
                                                LocationInstanceName = currentName,
                                                LocationInstanceParentID = parentId,
                                                Height = height,
                                                Width = width,
                                                LocationTypeID = typeId
                                            };
                                            _context.Add(newSublocationInstance);
                                            await _context.SaveChangesAsync();
                                            placeholderInstanceIds[z].Add(newSublocationInstance.LocationInstanceID);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                    await transaction.CommitAsync();
                    ReturnVal.SetStringAndBool(true, null);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ReturnVal.SetStringAndBool(false, "Error adding location. Please try again. " + AppUtility.GetExceptionMessage(ex));
                    
                }
            }
            return ReturnVal;
        }
    }
}
