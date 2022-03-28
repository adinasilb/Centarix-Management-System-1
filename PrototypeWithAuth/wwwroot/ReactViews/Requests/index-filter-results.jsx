import FormGroup from '@mui/material/FormGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import { connect, useDispatch } from 'react-redux';

import * as Actions from '../ReduxRelatedUtils/actions.jsx';
function IndexFilterResults(props) {
    console.log("index filter results")
    const dispatch = useDispatch();


    const markAsArchived = (e) => {
        props.setState({ ...props.state, isArchived: e.currentTarget.checked });
    }
    const updateSearchName = (e) => {
        props.setState({ ...props.state, searchByName: e.currentTarget.value });
    }
    const updateCatalogNumber = (e) => {
        props.setState({ ...props.state, catalogNumber: e.currentTarget.value });
    }
    const moveVendorToSelected = (e) => {
        var newNotSelectedList = props.state.vendors.notSelectedShown.filter(v => v.VendorID != e.currentTarget.value);
        var vendorToMoveToSelected = props.state.vendors.notSelectedShown.filter(v => v.VendorID == e.currentTarget.value)[0];
        props.setState({ ...props.state, vendors: { ...props.state.vendors, filterText: '', selected: [...props.state.vendors.selected, vendorToMoveToSelected], notSelectedShown: [...newNotSelectedList || [], ...props.state.vendors.notSelectedHidden || []], notSelectedHidden: [] }, numFilters: props.state.numFilters+1 });
    };
    const moveVendorToNotSelected = (e) => {
        var newSelectedList = props.state.vendors.selected.filter(v => v.VendorID != e.currentTarget.value);
        var vendorToMoveToNotSelected = props.state.vendors.selected.filter(v => v.VendorID == e.currentTarget.value)[0];
        props.setState({ ...props.state, vendors: { ...props.state.vendors, filterText: '', selected: newSelectedList, notSelectedShown: [...[...props.state.vendors.notSelectedShown || [], ...props.state.vendors.notSelectedHidden || []], vendorToMoveToNotSelected], notSelectedHidden: [] }, numFilters: props.state.numFilters - 1 });
    };
    const moveOwnersToSelected = (e) => {
        var newNotSelectedList = props.state.owners.notSelectedShown.filter(v => v.Id != e.currentTarget.value);
        var ownerToMoveToSelected = props.state.owners.notSelectedShown.filter(v => v.Id == e.currentTarget.value)[0];
        props.setState({ ...props.state, owners: { ...props.state.owners, filterText: '', selected: [...props.state.owners.selected, ownerToMoveToSelected], notSelectedShown: newNotSelectedList }, numFilters: props.state.numFilters + 1 });
    };
    const moveOwnersToNotSelected = (e) => {
        var newSelectedList = props.state.owners.selected.filter(v => v.Id != e.currentTarget.value);
        var ownerToMoveToNotSelected = props.state.owners.selected.filter(v => v.Id == e.currentTarget.value)[0];
        props.setState({ ...props.state, owners: { ...props.state.owners, filterText: '', selected: newSelectedList, notSelectedShown: [...props.state.owners.notSelectedShown, ownerToMoveToNotSelected] }, numFilters: props.state.numFilters - 1 });
    };

    const moveLocationsToSelected = (e) => {
        var newNotSelectedList = props.state.locations.notSelectedShown.filter(v => v.LocationTypeID != e.currentTarget.value);
        var locationToMoveToSelected = props.state.locations.notSelectedShown.filter(v => v.LocationTypeID == e.currentTarget.value)[0];
        props.setState({ ...props.state, locations: { ...props.state.locations, filterText: '', selected: [...props.state.locations.selected, locationToMoveToSelected], notSelectedShown: newNotSelectedList||[] }, numFilters: props.state.numFilters + 1 });
    };
    const moveLocationsToNotSelected = (e) => {
        var newSelectedList = props.state.locations.selected.filter(v => v.LocationTypeID != e.currentTarget.value);
        var locationToMoveToNotSelected = props.state.locations.selected.filter(v => v.LocationTypeID == e.currentTarget.value)[0];
        props.setState({ ...props.state, locations: { ...props.state.locations, filterText: '', selected: newSelectedList, notSelectedShown: [...props.state.locations.notSelectedShown, locationToMoveToNotSelected] }, numFilters: props.state.numFilters - 1 });
    };
    const moveCategoriesToSelected = (e) => {
        var newNotSelectedList = props.state.categories.notSelectedShown.filter(v => v.ID != e.currentTarget.value);
        var categoryToMoveToSelected = props.state.categories.notSelectedShown.filter(v => v.ID == e.currentTarget.value)[0];
        props.setState({ ...props.state, categories: { ...props.state.categories, filterText: '', selected: [...props.state.categories.selected, categoryToMoveToSelected], notSelectedShown: newNotSelectedList }, numFilters: props.state.numFilters + 1 });
    };
    const moveCategoriesToNotSelected = (e) => {
        var newSelectedList = props.state.categories.selected.filter(v => v.ID != e.currentTarget.value);
        var categoryToMoveToNotSelected = props.state.categories.selected.filter(v => v.ID == e.currentTarget.value)[0];      
        props.setState({ ...props.state, categories: { ...props.state.categories, filterText: '', selected: newSelectedList, notSelectedShown: [...props.state.categories.notSelectedShown, categoryToMoveToNotSelected] }, numFilters: props.state.numFilters - 1 });
    };

    const moveSubCategoriesToSelected = (e) => {
        var newNotSelectedList = props.state.subcategories.notSelectedShown.filter(v => v.ID != e.currentTarget.value);
        var subCategoryToMoveToSelected = props.state.subcategories.notSelectedShown.filter(v => v.ID == e.currentTarget.value)[0];
        props.setState({ ...props.state, subcategories: { ...props.state.subcategories, filterText: '', selected: [...props.state.subcategories.selected, subCategoryToMoveToSelected], notSelectedShown: newNotSelectedList }, numFilters: props.state.numFilters + 1 });
    };
    const moveSubCategoriesToNotSelected = (e) => {
        var newSelectedList = props.state.subcategories.selected.filter(v => v.ID != e.currentTarget.value);
        var subCategoryToMoveToNotSelected = props.state.subcategories.selected.filter(v => v.ID == e.currentTarget.value)[0];
        props.setState({ ...props.state, subcategories: { ...props.state.subcategories, filterText: '', selected: newSelectedList, notSelectedShown: [...props.state.subcategories.notSelectedShown, subCategoryToMoveToNotSelected] }, numFilters: props.state.numFilters - 1 });
    };
    const filterVendors = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        props.setState({
            ...props.state, vendors: {
                ...props.state.vendors,
                filterText: value,
                notSelectedShown: [...props.state.vendors.notSelectedShown?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) >= 0) || [], ...props.state.vendors.notSelectedHidden?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) >= 0) || []],
                notSelectedHidden: [...props.state.vendors.notSelectedShown?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) < 0) || [], ...props.state.vendors.notSelectedHidden?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) < 0)],
            } });
    };
    const filterOwners = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        props.setState({
            ...props.state, owners: {
                ...props.state.owners,
                filterText: value,
                notSelectedShown: [...props.state.owners.notSelectedShown?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) >= 0) || [], ...props.state.owners.notSelectedHidden?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) >= 0) || []],
                notSelectedHidden: [...props.state.owners.notSelectedShown?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) < 0) || [], ...props.state.owners.notSelectedHidden?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) < 0)],
            }
        });
    };

    const filterLocations = (e) => {
        var value = e.currentTarget.value.toLowerCase();   
          props.setState({
              ...props.state, locations: {
                  ...props.state.locations,
                  filterText: value,
                  notSelectedShown: [...props.state.locations.notSelectedShown?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) >= 0) || [], ...props.state.locations.notSelectedHidden?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) >= 0) || []],
                  notSelectedHidden: [...props.state.locations.notSelectedShown?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) < 0) || [], ...props.state.locations.notSelectedHidden?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) < 0)],
              }
        });
    };

    const filterCategories = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setCategories({
            ...props.state.categories,
            filterText: value,
            notSelectedShown: [...props.state.categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || [], ...props.state.categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || []],
            notSelectedHidden: [...props.state.categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) < 0) || [], ...props.state.categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) < 0)],
        });
        props.setState({
            ...props.state, categories: {
                ...props.state.categories,
                filterText: value,
                notSelectedShown: [...props.state.categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || [], ...props.state.categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || []],
                notSelectedHidden: [...props.state.categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) < 0) || [], ...props.state.categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) < 0)],
            }
        });
    };

    const filterSubcategories = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        props.setState({
            ...props.state, subcategories: {
                ...props.state.subcategories,
                filterText: value,
                notSelectedShown: [...props.state.subcategories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || [], ...props.state.subcategories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || []],
                notSelectedHidden: [...props.state.subcategories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) < 0) || [], ...props.state.subcategories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) < 0)],
            }
        });
    };


   
    const clearFilter = () => {
        props.setState({
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
            SelectedVendorsIDs:props.state.vendors.selected.map(v=> v.VendorID),
            SelectedOwnersIDs: props.state.owners.selected.map(v => v.Id),
            SelectedLocationsIDs: props.state.locations.selected.map(v => v.LocationTypeID),
            SelectedCategoriesIDs: props.state.categories.selected.map(v => v.ID),
            SelectedSubcategoriesIDs: props.state.subcategories.selected.map(v => v.ID),
            NumFilters: props.state.numFilters,
            Archive: props.state.isArchived,
            CatalogNumber: props.state.catalogNumber,
            SearchText: props.state.searchByName
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
                    <span className=" text numFilters font-weight-bold" value={props.state.numFilters}>{props.state.numFilters}</span>
                    </div>
                <div className="col-2">
                    <input type="text" placeholder="Search By Name" className="mb-2 w-100 form-control-plaintext border-bottom search-by-name-in-filter search-requests-in-filter" onChange={updateSearchName} value={props.state.searchByName??""} />
                </div>
                {!props.viewModel.IsProprietary ?
                    <div className="col-2">
                        <input type="text" placeholder="By Catalog Number" className="mb-2 w-100 form-control-plaintext border-bottom search-by-catalog-number" onChange={updateCatalogNumber} value={props.state.catalogNumber??""} />
                    </div>
                    : ""}
                <div className="col-3"></div>
                {props.viewModel.IsProprietary ?
                    <FormGroup>
                        <FormControlLabel control={<Checkbox checked={props.state.isArchived} onChange={markAsArchived} />} label="Archived" />
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
                        <input placeholder="Vendor" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterVendors} value={props.state.vendors.filterText ??''} />
                        <div className="inventory-filter-col not-selected ">
                            {props.state.vendors.notSelectedShown?.map(v =>
                                <button key={v.VendorID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.VendorID} onClick={moveVendorToSelected} ><span className="filter-button-description">{v.VendorEnName}</span></button>
                            )}
                        </div>
                        <div className="inventory-filter-col selected">  
                            {props.state.vendors.selected?.map(v =>
                                <button key={v.VendorID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.VendorID} onClick={moveVendorToNotSelected} ><span className="filter-button-description">{v.VendorEnName}</span></button>
                            )}
                        </div>
                    </div>
                    :""
                    }
                <div className="border-right col filter-col owner-col">
                    <input placeholder="Owner" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterOwners} value={props.state.owners.filterText ?? ''} />
                    <div className="inventory-filter-col not-selected">
                        {props.state.owners.notSelectedShown?.map(v =>
                            <button key={v.Id} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left"  value={v.Id} onClick={moveOwnersToSelected} ><span className="filter-button-description">{v.FirstName+" " + v.LastName}</span></button>
                            )}
                        </div>
                    <div className="inventory-filter-col selected">
                        {props.state.owners.selected?.map(v =>
                            <button key={v.Id} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.Id} onClick={moveOwnersToNotSelected} ><span className="filter-button-description">{v.FirstName +v.LastName}</span></button>
                          ) }
                        </div>

                </div>
                {props.navigationInfo.sectionType != "Operations" ?
           
                        <div className="border-right col filter-col location-col">
                        <input placeholder="Location" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterLocations} value={props.state.locations.filterText ?? ''} />
                        <div className="inventory-filter-col not-selected">
                            {props.state.locations.notSelectedShown?.map(v =>
                                <button key={v.LocationTypeID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.LocationTypeID} onClick={moveLocationsToSelected}><span className="filter-button-description" >{v.LocationTypeName}</span></button>
                                )}
                            </div>
                        <div className="inventory-filter-col selected">
                            {props.state.locations.selected?.map(v =>
                                <button key={ v.LocationTypeID } id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.LocationTypeID} onClick={moveLocationsToNotSelected} ><span className="filter-button-description">{v.LocationTypeName}</span></button>
                               ) }
                            </div>
                    </div>
                    :""
                    }
                    <div className="border-right col filter-col category-col">
                    <input placeholder="Category" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterCategories} value={props.state.categories.filterText ?? ''}  />
                    <div className="inventory-filter-col not-selected">
                        {props.state.categories.notSelectedShown?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.ID} onClick={moveCategoriesToSelected} ><span className="filter-button-description">{v.Description}</span></button>
                           ) }
                        </div>
                    <div className="inventory-filter-col selected">
                        {props.state.categories.selected?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.ID} onClick={moveCategoriesToNotSelected} ><span className="filter-button-description">{v.Description}</span></button>
                        )}
                        </div>
                    </div>
                <div className="border-right col mh-100 filter-col subcategory-col">
                    <input placeholder="Subcategory" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterSubcategories} value={props.state.subcategories.filterText ?? ''} />
                    <div className="inventory-filter-col not-selected">
                        {props.state.subcategories.notSelectedShown?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left"  value={v.ID} onClick={moveSubCategoriesToSelected} ><span className="filter-button-description">{v.Description}</span></button>
                            )}
                        </div>
                    <div className="inventory-filter-col selected">
                        {props.state.subcategories.selected?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.ID} onClick={moveSubCategoriesToNotSelected} ><span className="filter-button-description">{v.Description}</span></button>
                            )}
                        </div>
                    </div>                
                </div>
        </div></span>
    )

   
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