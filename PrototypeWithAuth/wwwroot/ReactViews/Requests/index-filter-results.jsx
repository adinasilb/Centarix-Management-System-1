import FormGroup from '@mui/material/FormGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import { connect, useDispatch } from 'react-redux';
import { useState, useEffect
} from 'react'
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
function IndexFilterResults(props) {

    const dispatch = useDispatch();
    const [state, setState] = useState(initialState())

    const markAsArchived = (e) => {
        setState({ ...state, isArchived: e.currentTarget.checked });
    }
    const updateSearchName = (e) => {
        setState({ ...state, searchByName: e.currentTarget.value });
    }
    const updateCatalogNumber = (e) => {
        setState({ ...state, catalogNumber: e.currentTarget.value });
    }
    const moveVendorToSelected = (e) => {
        var newNotSelectedList = state.vendors.notSelectedShown.filter(v => v.VendorID != e.currentTarget.value);
        var vendorToMoveToSelected = state.vendors.notSelectedShown.filter(v => v.VendorID == e.currentTarget.value)[0];
        setState({ ...state, vendors: { ...state.vendors, filterText: '', selected: [...state.vendors.selected, vendorToMoveToSelected], notSelectedShown: [...newNotSelectedList || [], ...state.vendors.notSelectedHidden || []], notSelectedHidden: [] }, numFilters: state.numFilters+1 });
    };
    const moveVendorToNotSelected = (e) => {
        var newSelectedList = state.vendors.selected.filter(v => v.VendorID != e.currentTarget.value);
        var vendorToMoveToNotSelected = state.vendors.selected.filter(v => v.VendorID == e.currentTarget.value)[0];
        setState({ ...state, vendors: { ...state.vendors, filterText: '', selected: newSelectedList, notSelectedShown: [...[...state.vendors.notSelectedShown || [], ...state.vendors.notSelectedHidden || []], vendorToMoveToNotSelected], notSelectedHidden: [] }, numFilters: state.numFilters - 1 });
    };
    const moveOwnersToSelected = (e) => {
        var newNotSelectedList = state.owners.notSelectedShown.filter(v => v.Id != e.currentTarget.value);
        var ownerToMoveToSelected = state.owners.notSelectedShown.filter(v => v.Id == e.currentTarget.value)[0];
        setState({ ...state, owners: { ...state.owners, filterText: '', selected: [...state.owners.selected, ownerToMoveToSelected], notSelectedShown: newNotSelectedList }, numFilters: state.numFilters + 1 });
    };
    const moveOwnersToNotSelected = (e) => {
        var newSelectedList = state.owners.selected.filter(v => v.Id != e.currentTarget.value);
        var ownerToMoveToNotSelected = state.owners.selected.filter(v => v.Id == e.currentTarget.value)[0];
        setState({ ...state, owners: { ...state.owners, filterText: '', selected: newSelectedList, notSelectedShown: [...state.owners.notSelectedShown, ownerToMoveToNotSelected] }, numFilters: state.numFilters - 1 });
    };

    const moveLocationsToSelected = (e) => {
        var newNotSelectedList = state.locations.notSelectedShown.filter(v => v.LocationTypeID != e.currentTarget.value);
        var locationToMoveToSelected = state.locations.notSelectedShown.filter(v => v.LocationTypeID == e.currentTarget.value)[0];
        setState({ ...state, locations: { ...state.locations, filterText: '', selected: [...state.locations.selected, locationToMoveToSelected], notSelectedShown: newNotSelectedList||[] }, numFilters: state.numFilters + 1 });
    };
    const moveLocationsToNotSelected = (e) => {
        var newSelectedList = state.locations.selected.filter(v => v.LocationTypeID != e.currentTarget.value);
        var locationToMoveToNotSelected = state.locations.selected.filter(v => v.LocationTypeID == e.currentTarget.value)[0];
        setState({ ...state, locations: { ...state.locations, filterText: '', selected: newSelectedList, notSelectedShown: [...state.locations.notSelectedShown, locationToMoveToNotSelected] }, numFilters: state.numFilters - 1 });
    };
    const moveCategoriesToSelected = (e) => {
        var newNotSelectedList = state.categories.notSelectedShown.filter(v => v.ID != e.currentTarget.value);
        var categoryToMoveToSelected = state.categories.notSelectedShown.filter(v => v.ID == e.currentTarget.value)[0];
        setState({ ...state, categories: { ...state.categories, filterText: '', selected: [...state.categories.selected, categoryToMoveToSelected], notSelectedShown: newNotSelectedList }, numFilters: state.numFilters + 1 });
    };
    const moveCategoriesToNotSelected = (e) => {
        var newSelectedList = state.categories.selected.filter(v => v.ID != e.currentTarget.value);
        var categoryToMoveToNotSelected = state.categories.selected.filter(v => v.ID == e.currentTarget.value)[0];      
        setState({ ...state, categories: { ...state.categories, filterText: '', selected: newSelectedList, notSelectedShown: [...state.categories.notSelectedShown, categoryToMoveToNotSelected] }, numFilters: state.numFilters - 1 });
    };

    const moveSubCategoriesToSelected = (e) => {
        var newNotSelectedList = state.subcategories.notSelectedShown.filter(v => v.ID != e.currentTarget.value);
        var subCategoryToMoveToSelected = state.subcategories.notSelectedShown.filter(v => v.ID == e.currentTarget.value)[0];
        setState({ ...state, subcategories: { ...state.subcategories, filterText: '', selected: [...state.subcategories.selected, subCategoryToMoveToSelected], notSelectedShown: newNotSelectedList }, numFilters: state.numFilters + 1 });
    };
    const moveSubCategoriesToNotSelected = (e) => {
        var newSelectedList = state.subcategories.selected.filter(v => v.ID != e.currentTarget.value);
        var subCategoryToMoveToNotSelected = state.subcategories.selected.filter(v => v.ID == e.currentTarget.value)[0];
        setState({ ...state, subcategories: { ...state.subcategories, filterText: '', selected: newSelectedList, notSelectedShown: [...state.subcategories.notSelectedShown, subCategoryToMoveToNotSelected] }, numFilters: state.numFilters - 1 });
    };
    const filterVendors = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setState({
            ...state, vendors: {
                ...state.vendors,
                filterText: value,
                notSelectedShown: [...state.vendors.notSelectedShown?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) >= 0) || [], ...state.vendors.notSelectedHidden?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) >= 0) || []],
                notSelectedHidden: [...state.vendors.notSelectedShown?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) < 0) || [], ...state.vendors.notSelectedHidden?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) < 0)],
            } });
    };
    const filterOwners = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setState({
            ...state, owners: {
                ...state.owners,
                filterText: value,
                notSelectedShown: [...state.owners.notSelectedShown?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) >= 0) || [], ...state.owners.notSelectedHidden?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) >= 0) || []],
                notSelectedHidden: [...state.owners.notSelectedShown?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) < 0) || [], ...state.owners.notSelectedHidden?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) < 0)],
            }
        });
    };

    const filterLocations = (e) => {
        var value = e.currentTarget.value.toLowerCase();   
          setState({
              ...state, locations: {
                  ...state.locations,
                  filterText: value,
                  notSelectedShown: [...state.locations.notSelectedShown?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) >= 0) || [], ...state.locations.notSelectedHidden?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) >= 0) || []],
                  notSelectedHidden: [...state.locations.notSelectedShown?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) < 0) || [], ...state.locations.notSelectedHidden?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) < 0)],
              }
        });
    };

    const filterCategories = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setCategories({
            ...state.categories,
            filterText: value,
            notSelectedShown: [...state.categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || [], ...state.categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || []],
            notSelectedHidden: [...state.categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) < 0) || [], ...state.categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) < 0)],
        });
        setState({
            ...state, categories: {
                ...state.categories,
                filterText: value,
                notSelectedShown: [...state.categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || [], ...state.categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || []],
                notSelectedHidden: [...state.categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) < 0) || [], ...state.categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) < 0)],
            }
        });
    };

    const filterSubcategories = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setState({
            ...state, subcategories: {
                ...state.subcategories,
                filterText: value,
                notSelectedShown: [...state.subcategories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || [], ...state.subcategories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || []],
                notSelectedHidden: [...state.subcategories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) < 0) || [], ...state.subcategories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) < 0)],
            }
        });
    };


   
    const clearFilter = () => {
        setState({
            numFilters: props.viewModel.NumFilters,
            searchByName: props.viewModel.SearchName,
            catalogNumber: props.viewModel.CatalogNumber,
            isArchived: props.viewModel.Archive,
            vendors: { filterText: "", notSelectedShown: props.viewModel.Vendors, selected: [], notSelectedHidden: [] },
            owners: { filterText: "", notSelectedShown: props.viewModel.Owners, selected: [], notSelectedHidden: [] },
            locations: { filterText: "", notSelectedShown: props.viewModel.Locations, selected: [], notSelectedHidden: [] },
            categories: { filterText: "", notSelectedShown: props.viewModel.Categories, selected: [], notSelectedHidden: [] },
            subcategories: { filterText: "", notSelectedShown: props.viewModel.Subcategories, selected: [], notSelectedHidden: [] }
        });
    }
    const applyFilter = (e) => {
        dispatch(Actions.setSelectedFiltersViewModel({
            SelectedVendorsIDs:state.vendors.selected.map(v=> v.VendorID),
            SelectedOwnersIDs: state.owners.selected.map(v => v.Id),
            SelectedLocationsIDs: state.locations.selected.map(v => v.LocationTypeID),
            SelectedCategoriesIDs: state.categories.selected.map(v => v.ID),
            SelectedSubcategoriesIDs: state.subcategories.selected.map(v => v.ID),
            NumFilters: state.numFilters,
            Archive: state.isArchived,
            CatalogNumber: state.catalogNumber,
            SearchText: state.searchByName
        }));
    }
    return (<span>
        <div style={{ width: "1200px", height: "37rem", margin: "0" }} className={"container-fluid overflow-hidden p-4 " + props.navigationInfo.sectionType}>
                <div className="row">
                <div className=" col-1 offset-11">
                    <button type="button" className="close popover-close" data-dismiss="popover" style={{ fontSize: "2rem" }} onClick={()=>props.popupState.setOpen(false)} >&times;</button>
                    </div>
                </div>
     
                <div className="row py-4 align-items-center">
                    <div className="col-2">
                        <label className="text text-left font-weight-bold">Total Filters:</label>
                    <span className=" text numFilters font-weight-bold" value={state.numFilters}>{state.numFilters}</span>
                    </div>
                <div className="col-2">
                    <input type="text" placeholder="Search By Name" className="mb-2 w-100 form-control-plaintext border-bottom search-by-name-in-filter search-requests-in-filter" onChange={updateSearchName} value={state.searchByName??""} />
                </div>
                {!props.viewModel.IsProprietary ?
                    <div className="col-2">
                        <input type="text" placeholder="By Catalog Number" className="mb-2 w-100 form-control-plaintext border-bottom search-by-catalog-number" onChange={updateCatalogNumber} value={state.catalogNumber??""} />
                    </div>
                    : ""}
                <div className="col-3"></div>
                {props.viewModel.IsProprietary ?
                    <FormGroup>
                        <FormControlLabel control={<Checkbox checked={state.isArchived} onChange={markAsArchived} />} label="Archived" />
                    </FormGroup>                    
                    :""
                    }
                <div className="col ">
                    <button className="text text-right font-weight-bold clear-filters" style={{ border: "none", background: " none" }} onClick={clearFilter}>Clear All</button>
                    </div>
                <div className="col ">
                    <button type="button" className="text custom-cancel custom-button-small-font rounded-pill  custom-small-filter-button" onClick={applyFilter}>Apply</button>
                    </div>
                </div>
                <div className="row ">
                {!props.viewModel.IsProprietary ?
                    <div className="border-right col filter-col vendor-col">
                        <input placeholder="Vendor" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterVendors} value={state.vendors.filterText ??''} />
                        <div className="inventory-filter-col not-selected ">
                            {state.vendors.notSelectedShown?.map(v =>
                                <button key={v.VendorID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.VendorID} onClick={moveVendorToSelected} ><span className="filter-button-description">{v.VendorEnName}</span></button>
                            )}
                        </div>
                        <div className="inventory-filter-col selected">  
                            {state.vendors.selected?.map(v =>
                                <button key={v.VendorID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.VendorID} onClick={moveVendorToNotSelected} ><span className="filter-button-description">{v.VendorEnName}</span></button>
                            )}
                        </div>
                    </div>
                    :""
                    }
                <div className="border-right col filter-col owner-col">
                    <input placeholder="Owner" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterOwners} value={state.owners.filterText ?? ''} />
                    <div className="inventory-filter-col not-selected">
                        {state.owners.notSelectedShown?.map(v =>
                            <button key={v.Id} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left"  value={v.Id} onClick={moveOwnersToSelected} ><span className="filter-button-description">{v.FirstName+" " + v.LastName}</span></button>
                            )}
                        </div>
                    <div className="inventory-filter-col selected">
                        {state.owners.selected?.map(v =>
                            <button key={v.Id} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.Id} onClick={moveOwnersToNotSelected} ><span className="filter-button-description">{v.FirstName +v.LastName}</span></button>
                          ) }
                        </div>

                </div>
                {props.navigationInfo.sectionType != "Operations" ?
           
                        <div className="border-right col filter-col location-col">
                        <input placeholder="Location" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterLocations} value={state.locations.filterText ?? ''} />
                        <div className="inventory-filter-col not-selected">
                            {state.locations.notSelectedShown?.map(v =>
                                <button key={v.LocationTypeID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.LocationTypeID} onClick={moveLocationsToSelected}><span className="filter-button-description" >{v.LocationTypeName}</span></button>
                                )}
                            </div>
                        <div className="inventory-filter-col selected">
                            {state.locations.selected?.map(v =>
                                <button key={ v.LocationTypeID } id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.LocationTypeID} onClick={moveLocationsToNotSelected} ><span className="filter-button-description">{v.LocationTypeName}</span></button>
                               ) }
                            </div>
                    </div>
                    :""
                    }
                    <div className="border-right col filter-col category-col">
                    <input placeholder="Category" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterCategories} value={state.categories.filterText ?? ''}  />
                    <div className="inventory-filter-col not-selected">
                        {state.categories.notSelectedShown?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.ID} onClick={moveCategoriesToSelected} ><span className="filter-button-description">{v.Description}</span></button>
                           ) }
                        </div>
                    <div className="inventory-filter-col selected">
                        {state.categories.selected?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.ID} onClick={moveCategoriesToNotSelected} ><span className="filter-button-description">{v.Description}</span></button>
                        )}
                        </div>
                    </div>
                <div className="border-right col mh-100 filter-col subcategory-col">
                    <input placeholder="Subcategory" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterSubcategories} value={state.subcategories.filterText ?? ''} />
                    <div className="inventory-filter-col not-selected">
                        {state.subcategories.notSelectedShown?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left"  value={v.ID} onClick={moveSubCategoriesToSelected} ><span className="filter-button-description">{v.Description}</span></button>
                            )}
                        </div>
                    <div className="inventory-filter-col selected">
                        {state.subcategories.selected?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.ID} onClick={moveSubCategoriesToNotSelected} ><span className="filter-button-description">{v.Description}</span></button>
                            )}
                        </div>
                    </div>                
                </div>
        </div></span>
    )

    function initialState() {
        return {
            numFilters: props.viewModel.NumFilters,
            searchByName: props.viewModel.SearchName,
            catalogNumber: props.viewModel.CatalogNumber,
            isArchived: props.viewModel.Archive,
            vendors: { filterText: "", notSelectedShown: props.viewModel.Vendors, selected: props.viewModel.SelectedVendors||[], notSelectedHidden: [] },
            owners: { filterText: "", notSelectedShown: props.viewModel.Owners, selected: props.viewModel.SelectedOwners||[], notSelectedHidden: [] },
            locations: { filterText: "", notSelectedShown: props.viewModel.Locations, selected: props.viewModel.SelectedLocations||[], notSelectedHidden: [] },
            categories: { filterText: "", notSelectedShown: props.viewModel.Categories, selected: props.viewModel.SelectedCategories||[], notSelectedHidden: [] },
            subcategories: { filterText: "", notSelectedShown: props.viewModel.Subcategories, selected: props.viewModel.SelectedSubcategories||[], notSelectedHidden: [] }
        };
    }
}

const mapStateToProps = state => {
    console.log("mstp filter")
    return {
        navigationInfo: state.navigationInfo
    };
};

export default connect(
    mapStateToProps
)(IndexFilterResults)