import FormGroup from '@mui/material/FormGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import { connect, useDispatch } from 'react-redux';
import { useState, useEffect
} from 'react'
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
function IndexFilterResults(props) {

    const dispatch = useDispatch();

    const [numFilters, setNumFilters] = useState(props.viewModel.NumFilters);
    const [searchByName, setSearchByName] = useState(props.viewModel.SearchName);
    const [catalogNumber, setCatalogNumber] = useState(props.viewModel.CatalogNumber);
    const [isArchived, setIsArchived] = useState(props.viewModel.Archive);
    const [vendors, setVendors] = useState({ notSelected: props.viewModel.Vendors, selected: props.viewModel.SelectedVendors });
    const [owners, setOwners] = useState({ notSelected: props.viewModel.Owners, selected: props.viewModel.SelectedOwners });
    const [locations, setLocations] = useState({ notSelected: props.viewModel.Locations, selected: props.viewModel.SelectedLocations });
    const [categories, setCategories] = useState({ notSelected: props.viewModel.Categories, selected: props.viewModel.SelectedCategories });
    const [subcategories, setSubcategories] = useState({ notSelected: props.viewModel.Subcategories, selected: props.viewModel.SelectedSubcategories });


    const markAsArchived = (e) => {
        setIsArchived(e.currentTarget.checked);
    }
    const updateSearchName = (e) => {
        setSearchByName(e.currentTarget.value);
    }
    const updateCatalogNumber = (e) => {
        setCatalogNumber(e.currentTarget.value);
    }
    const moveVendorToSelected = (e) => {
        var newNotSelectedList = vendors.notSelected.filter(v => v.VendorID != e.currentTarget.value);
        var vendorToMoveToSelected = vendors.notSelected.filter(v => v.VendorID == e.currentTarget.value)[0];
        setVendors({...vendors, selected: [...vendors.selected, vendorToMoveToSelected], notSelected: newNotSelectedList });
        setNumFilters(numFilters + 1);
    };
    const moveVendorToNotSelected = (e) => {
        console.log(e.currentTarget)
        var newSelectedList = vendors.selected.filter(v => v.VendorID != e.currentTarget.value);
        var vendorToMoveToNotSelected = vendors.selected.filter(v => v.VendorID == e.currentTarget.value)[0];
        setVendors({ ...vendors, selected: newSelectedList, notSelected: [...vendors.notSelected, vendorToMoveToNotSelected] });
        setNumFilters(numFilters - 1);
    };

    const moveOwnersToSelected = (e) => {
        var newNotSelectedList = owners.notSelected.filter(v => v.Id != e.currentTarget.value);
        var ownerToMoveToSelected = owners.notSelected.filter(v => v.Id == e.currentTarget.value)[0];
        setOwners({...owners, selected: [...owners.selected, ownerToMoveToSelected], notSelected: newNotSelectedList });
        setNumFilters(numFilters + 1);
    };
    const moveOwnersToNotSelected = (e) => {
        var newSelectedList = owners.selected.filter(v => v.Id != e.currentTarget.value);
        var ownerToMoveToNotSelected = owners.selected.filter(v => v.Id == e.currentTarget.value)[0];
        setOwners({...owners, selected: newSelectedList, notSelected: [...owners.notSelected, ownerToMoveToNotSelected] });
        setNumFilters(numFilters - 1);
    };

    const moveLocationsToSelected = (e) => {
        var newNotSelectedList = locations.notSelected.filter(v => v.LocationTypeID != e.currentTarget.value);
        var locationToMoveToSelected = locations.notSelected.filter(v => v.LocationTypeID == e.currentTarget.value)[0];
        setLocations({...locations, selected: [...locations.selected, locationToMoveToSelected], notSelected: newNotSelectedList });
        setNumFilters(numFilters + 1);
    };
    const moveLocationsToNotSelected = (e) => {
        var newSelectedList = locations.selected.filter(v => v.Id != e.currentTarget.value);
        var locationToMoveToNotSelected = locations.selected.filter(v => v.Id == e.currentTarget.value)[0];
        setLocations({ ...locations, selected: newSelectedList, notSelected: [...locations.notSelected, locationToMoveToNotSelected] });
        setNumFilters(numFilters - 1);
    };
    const moveCategoriesToSelected = (e) => {
        var newNotSelectedList = categories.notSelected.filter(v => v.ID != e.currentTarget.value);
        var categoryToMoveToSelected = categories.notSelected.filter(v => v.ID == e.currentTarget.value)[0];
        setCategories({ ...categories, selected: [...categories.selected, categoryToMoveToSelected], notSelected: newNotSelectedList });
        setNumFilters(numFilters + 1);
    };
    const moveCategoriesToNotSelected = (e) => {
        var newSelectedList = categories.selected.filter(v => v.ID != e.currentTarget.value);
        var categoryToMoveToNotSelected = categories.selected.filter(v => v.ID == e.currentTarget.value)[0];
        setCategories({ ...categories, selected: newSelectedList, notSelected: [...categories.notSelected, categoryToMoveToNotSelected] });
        setNumFilters(numFilters - 1);
    };

    const moveSubCategoriesToSelected = (e) => {
        var newNotSelectedList = subcategories.notSelected.filter(v => v.ID != e.currentTarget.value);
        var subCategoryToMoveToSelected = subcategories.notSelected.filter(v => v.ID == e.currentTarget.value)[0];
        setSubcategories({ ...subcategories, selected: [...subcategories.selected, subCategoryToMoveToSelected], notSelected: newNotSelectedList });
        setNumFilters(numFilters + 1);
    };
    const moveSubCategoriesToNotSelected = (e) => {
        var newSelectedList = subcategories.selected.filter(v => v.ID != e.currentTarget.value);
        var subCategoryToMoveToNotSelected = subcategories.selected.filter(v => v.ID == e.currentTarget.value)[0];
        setSubcategories({ ...subcategories, selected: newSelectedList, notSelected: [...subcategories.notSelected, subCategoryToMoveToNotSelected] });
        setNumFilters(numFilters - 1);
    };

    const clearFilter = () => {
        alert("in clearFilters")
        var viewModel = { ...props.viewModel };
        var vendors = [...props.viewModel.Vendors, ...props.viewModel.SelectedVendors];
        var owners = [...props.viewModel.Owners, ...props.viewModel.SelectedOwners];
        var locations = [...props.viewModel.Locations, ...props.viewModel.SelectedLocations];
        var categories = [...props.viewModel.Categories, ...props.viewModel.SelectedCategories];
        var subCategories = [...props.viewModel.Subcategories, ...props.viewModel.SelectedSubcategories];
        viewModel.Vendors = vendors;
        viewModel.SelectedVendors = [];
        viewModel.Owners = owners;
        viewModel.SelectedOwners = [];
        viewModel.Locations = locations;
        viewModel.SelectedLocations = [];
        viewModel.Categories = categories;
        viewModel.SelectedCategories = [];
        viewModel.Subcategories = subCategories;
        viewModel.SelectedSubcategories = [];
        viewModel.numFilters = 0;
        viewModel.Archive = false;
        viewModel.CatalogNumber = "";
        viewModel.SearchName = "";
        dispatch(Actions.setInventoryFilterViewModel(viewModel));
    }
    const applyFilter = (e) => {
        e.preventDefault();
        alert("in apply filters")

        var viewModel = { ...props.viewModel };
        viewModel.Vendors = vendors.notSelected;
        viewModel.SelectedVendors = vendors.selected;
        viewModel.Owners = owners.notSelected;
        viewModel.SelectedOwners = owners.selected;
        viewModel.Locations = locations.notSelected;
        viewModel.SelectedLocations = locations.selected;
        viewModel.Categories = categories.notSelected;
        viewModel.SelectedCategories = categories.selected;
        viewModel.Subcategories = subcategories.notSelected;
        viewModel.SelectedSubcategories = subcategories.selected;
        viewModel.numFilters = numFilters;
        viewModel.Archive = isArchived;
        viewModel.CatalogNumber = catalogNumber;
        viewModel.SearchName = searchByName;
        dispatch(Actions.setInventoryFilterViewModel(viewModel));
    }
    return (<span>
        {console.log(vendors)}
        <div style={{ width: "1200px", height: "35.75rem", margin: "0" }} className="container-fluid overflow-hidden">
                <div className="row">
                    <div className=" col-1 offset-11">
                        <button type="button" className="close popover-close" style={{fontSize:"2rem"}}>&times;</button>
                    </div>
                </div>
     
                <div className="row py-4 align-items-center">
                    <div className="col-2">
                        <label className="text text-left font-weight-bold">Total Filters:</label>
                    <span className=" text numFilters font-weight-bold" value={numFilters}>{numFilters}</span>
                    </div>
                <div className="col-2">
                    <input type="text" placeholder="Search By Name" className="mb-2 w-100 form-control-plaintext border-bottom search-by-name-in-filter search-requests-in-filter" onChange={updateSearchName} value={searchByName??""} />
                </div>
                {!props.viewModel.IsProprietary ?
                    <div className="col-2">
                        <input type="text" placeholder="By Catalog Number" className="mb-2 w-100 form-control-plaintext border-bottom search-by-catalog-number" onChange={updateCatalogNumber} value={catalogNumber??""} />
                    </div>
                    : ""}
                <div className="col-3"></div>
                {props.viewModel.IsProprietary ?
                    <FormGroup>
                        <FormControlLabel control={<Checkbox checked={isArchived} onChange={markAsArchived} />} label="Archived" />
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
                        <input placeholder="Vendor" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom vendor-search" />
                        <div className="inventory-filter-col not-selected ">
                            {vendors.notSelected?.map(v =>
                                <button key={v.VendorID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.VendorID} onClick={moveVendorToSelected} ><span className="filter-button-description">{v.VendorEnName}</span></button>
                            )}
                        </div>
                        <div className="inventory-filter-col selected">  
                            {vendors.selected?.map(v =>
                                <button key={v.VendorID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.VendorID} onClick={moveVendorToNotSelected} ><span className="filter-button-description">{v.VendorEnName}</span></button>
                            )}
                        </div>
                    </div>
                    :""
                    }
                    <div className="border-right col filter-col owner-col">
                        <input placeholder="Owner" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom owner-search" />
                    <div className="inventory-filter-col not-selected">
                        {owners.notSelected?.map(v =>
                            <button key={v.Id} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left"  value={v.Id} onClick={moveOwnersToSelected} ><span className="filter-button-description">{v.FirstName + v.LastName}</span></button>
                            )}
                        </div>
                    <div className="inventory-filter-col selected">
                        {owners.selected?.map(v =>
                            <button key={v.Id} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.Id} onClick={moveOwnersToNotSelected} ><span className="filter-button-description">{v.FirstName +v.LastName}</span></button>
                          ) }
                        </div>

                </div>
                {props.navigationInfo.sectionType != "Operations" ?
           
                        <div className="border-right col filter-col location-col">
                            <input placeholder="Location" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom location-search" />
                        <div className="inventory-filter-col not-selected">
                            {location.notSelected?.map(v =>
                                <button key={v.LocationTypeID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.LocationTypeID} onClick={moveLocationsToSelected}><span className="filter-button-description" >{v.LocationTypeName}</span></button>
                                )}
                            </div>
                        <div className="inventory-filter-col selected">
                            {location.selected?.map(v =>
                                <button key={ v.LocationTypeID } id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.LocationTypeID} onClick={moveLocationsToNotSelected} ><span className="filter-button-description">{v.LocationTypeName}</span></button>
                               ) }
                            </div>
                    </div>
                    :""
                    }
                    <div className="border-right col filter-col category-col">
                        <input placeholder="Category" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom category-search" />
                    <div className="inventory-filter-col not-selected">
                        {categories.notSelected?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.ID} onClick={moveCategoriesToSelected} ><span className="filter-button-description">{v.Description}</span></button>
                           ) }
                        </div>
                    <div className="inventory-filter-col selected">
                        {categories.selected?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.ID} onClick={moveCategoriesToNotSelected} ><span className="filter-button-description">{v.Description}</span></button>
                        )}
                        </div>
                    </div>
                    <div className="border-right col mh-100 filter-col subcategory-col">
                        <input placeholder="Subcategory" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom subCategory-search" />
                    <div className="inventory-filter-col not-selected">
                        {subcategories.notSelected?.map(v =>
                            <button key={v.ID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left"  value={v.ID} onClick={moveSubCategoriesToSelected} ><span className="filter-button-description">{v.Description}</span></button>
                            )}
                        </div>
                    <div className="inventory-filter-col selected">
                        {subcategories.selected?.map(v =>
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
        viewModel: state.inventoryFilterViewModel,
        navigationInfo: state.navigationInfo
    };
};

export default connect(
    mapStateToProps
)(IndexFilterResults)