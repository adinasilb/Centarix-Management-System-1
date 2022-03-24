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
    const [vendors, setVendors] = useState({ filterText:"",  notSelectedShown: props.viewModel.Vendors, selected: props.viewModel.SelectedVendors, notSelectedHidden:[] });
    const [owners, setOwners] = useState({ filterText: "", notSelectedShown: props.viewModel.Owners, selected: props.viewModel.SelectedOwners, notSelectedHidden: [] });
    const [locations, setLocations] = useState({ filterText: "", notSelectedShown: props.viewModel.Locations, selected: props.viewModel.SelectedLocations, notSelectedHidden: [] });
    const [categories, setCategories] = useState({ filterText: "", notSelectedShown: props.viewModel.Categories, selected: props.viewModel.SelectedCategories, notSelectedHidden: []});
    const [subcategories, setSubcategories] = useState({ filterText: "", notSelectedShown: props.viewModel.Subcategories, selected: props.viewModel.SelectedSubcategories, notSelectedHidden: [] });

    useEffect(() => {
        setNumFilters(props.viewModel.NumFilters);
        setSearchByName(props.viewModel.SearchName);
        setCatalogNumber(props.viewModel.CatalogNumber);
        setIsArchived(props.viewModel.Archive);
        setVendors({ filterText: "", notSelectedShown: props.viewModel.Vendors, selected: props.viewModel.SelectedVendors, notSelectedHidden: [] });
        setOwners({ filterText: "", notSelectedShown: props.viewModel.Owners, selected: props.viewModel.SelectedOwners, notSelectedHidden: [] });
        setLocations({ filterText: "", notSelectedShown: props.viewModel.Locations, selected: props.viewModel.SelectedLocations, notSelectedHidden: [] });
        setCategories({ filterText: "", notSelectedShown: props.viewModel.Categories, selected: props.viewModel.SelectedCategories, notSelectedHidden: [] });
        setSubcategories({ filterText: "", notSelectedShown: props.viewModel.Subcategories, selected: props.viewModel.SelectedSubcategories, notSelectedHidden: [] });
    }, [props]);

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
        var newNotSelectedList = vendors.notSelectedShown.filter(v => v.VendorID != e.currentTarget.value);
        var vendorToMoveToSelected = vendors.notSelectedShown.filter(v => v.VendorID == e.currentTarget.value)[0];
        setVendors({ ...vendors, filterText:'', selected: [...vendors.selected, vendorToMoveToSelected], notSelectedShown: [...newNotSelectedList||[], ...vendors.notSelectedHidden||[]], notSelectedHidden: [] });
        setNumFilters(numFilters + 1);
    };
    const moveVendorToNotSelected = (e) => {
        var newSelectedList = vendors.selected.filter(v => v.VendorID != e.currentTarget.value);
        var vendorToMoveToNotSelected = vendors.selected.filter(v => v.VendorID == e.currentTarget.value)[0];
        setVendors({ ...vendors, filterText:'', selected: newSelectedList, notSelectedShown: [...[...vendors.notSelectedShown || [], ...vendors.notSelectedHidden || []], vendorToMoveToNotSelected], notSelectedHidden: [] });
        setNumFilters(numFilters - 1);
    };
    const filterVendors = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setVendors({
            ...vendors,
            filterText: value,
            notSelectedShown: [...vendors.notSelectedShown?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) >= 0)||[], ...vendors.notSelectedHidden?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) >= 0)||[]],
            notSelectedHidden: [...vendors.notSelectedShown?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) < 0)||[], ...vendors.notSelectedHidden?.filter(v => v.VendorEnName.toLowerCase().indexOf(value) < 0)],
        });
    };
    const filterOwners = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setOwners({
            ...owners,
            filterText: value,
            notSelectedShown: [...owners.notSelectedShown?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) >= 0) || [], ...owners.notSelectedHidden?.filter(v => (v.FirstName.toLowerCase() +" "+ v.LastName.toLowerCase()).indexOf(value) >= 0) || []],
            notSelectedHidden: [...owners.notSelectedShown?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) < 0) || [], ...owners.notSelectedHidden?.filter(v => (v.FirstName.toLowerCase() + " " + v.LastName.toLowerCase()).indexOf(value) < 0)],
        });
    };

    const filterLocations = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setLocations({
            ...locations,
            filterText: value,
            notSelectedShown: [...locations.notSelectedShown?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) >= 0) || [], ...locations.notSelectedHidden?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) >= 0) || []],
            notSelectedHidden: [...locations.notSelectedShown?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) < 0) || [], ...locations.notSelectedHidden?.filter(v => v.LocationTypeName.toLowerCase().indexOf(value) < 0)],
        });
    };

    const filterCategories = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setCategories({
            ...categories,
            filterText: value,
            notSelectedShown: [...categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || [], ...categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || []],
            notSelectedHidden: [...categories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) < 0) || [], ...categories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) < 0)],
        });
    };

    const filterSubcategories = (e) => {
        var value = e.currentTarget.value.toLowerCase();
        setSubcategories({
            ...subcategories,
            filterText: value,
            notSelectedShown: [...subcategories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || [], ...subcategories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) >= 0) || []],
            notSelectedHidden: [...subcategories.notSelectedShown?.filter(v => v.Description.toLowerCase().indexOf(value) < 0) || [], ...subcategories.notSelectedHidden?.filter(v => v.Description.toLowerCase().indexOf(value) < 0)],
        });
    };


    const moveOwnersToSelected = (e) => {
        var newNotSelectedList = owners.notSelectedShown.filter(v => v.Id != e.currentTarget.value);
        var ownerToMoveToSelected = owners.notSelectedShown.filter(v => v.Id == e.currentTarget.value)[0];
        setOwners({ ...owners, filterText: '', selected: [...owners.selected, ownerToMoveToSelected], notSelectedShown: newNotSelectedList });
        setNumFilters(numFilters + 1);
    };
    const moveOwnersToNotSelected = (e) => {
        var newSelectedList = owners.selected.filter(v => v.Id != e.currentTarget.value);
        var ownerToMoveToNotSelected = owners.selected.filter(v => v.Id == e.currentTarget.value)[0];
        setOwners({ ...owners, filterText: '', selected: newSelectedList, notSelectedShown: [...owners.notSelectedShown, ownerToMoveToNotSelected] });
        setNumFilters(numFilters - 1);
    };

    const moveLocationsToSelected = (e) => {
        var newNotSelectedList = locations.notSelectedShown.filter(v => v.LocationTypeID != e.currentTarget.value);
        var locationToMoveToSelected = locations.notSelectedShown.filter(v => v.LocationTypeID == e.currentTarget.value)[0];
        setLocations({ ...locations, filterText: '', selected: [...locations.selected, locationToMoveToSelected], notSelectedShown: newNotSelectedList });
        setNumFilters(numFilters + 1);
    };
    const moveLocationsToNotSelected = (e) => {
        var newSelectedList = locations.selected.filter(v => v.Id != e.currentTarget.value);
        var locationToMoveToNotSelected = locations.selected.filter(v => v.Id == e.currentTarget.value)[0];
        setLocations({ ...locations, filterText: '', selected: newSelectedList, notSelectedShown: [...locations.notSelectedShown, locationToMoveToNotSelected] });
        setNumFilters(numFilters - 1);
    };
    const moveCategoriesToSelected = (e) => {
        var newNotSelectedList = categories.notSelectedShown.filter(v => v.ID != e.currentTarget.value);
        var categoryToMoveToSelected = categories.notSelectedShown.filter(v => v.ID == e.currentTarget.value)[0];
        setCategories({ ...categories, filterText: '', selected: [...categories.selected, categoryToMoveToSelected], notSelectedShown: newNotSelectedList });
        setNumFilters(numFilters + 1);
    };
    const moveCategoriesToNotSelected = (e) => {
        var newSelectedList = categories.selected.filter(v => v.ID != e.currentTarget.value);
        var categoryToMoveToNotSelected = categories.selected.filter(v => v.ID == e.currentTarget.value)[0];
        setCategories({ ...categories, filterText: '',  selected: newSelectedList, notSelectedShown: [...categories.notSelectedShown, categoryToMoveToNotSelected] });
        setNumFilters(numFilters - 1);
    };

    const moveSubCategoriesToSelected = (e) => {
        var newNotSelectedList = subcategories.notSelectedShown.filter(v => v.ID != e.currentTarget.value);
        var subCategoryToMoveToSelected = subcategories.notSelectedShown.filter(v => v.ID == e.currentTarget.value)[0];
        setSubcategories({ ...subcategories, filterText: '',  selected: [...subcategories.selected, subCategoryToMoveToSelected], notSelectedShown: newNotSelectedList });
        setNumFilters(numFilters + 1);
    };
    const moveSubCategoriesToNotSelected = (e) => {
        var newSelectedList = subcategories.selected.filter(v => v.ID != e.currentTarget.value);
        var subCategoryToMoveToNotSelected = subcategories.selected.filter(v => v.ID == e.currentTarget.value)[0];
        setSubcategories({ ...subcategories, filterText: '',  selected: newSelectedList, notSelectedShown: [...subcategories.notSelectedShown, subCategoryToMoveToNotSelected] });
        setNumFilters(numFilters - 1);
    };

    const clearFilter = () => {
        var viewModel = { ...props.viewModel };
        var vendors =[ ...props.viewModel.Vendors||[], ...[...props.viewModel.SelectedVendors||[], ...vendors?.notSelectedShown||[]]];
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
        viewModel.NumFilters = 0;
        viewModel.Archive = false;
        viewModel.CatalogNumber = "";
        viewModel.SearchName = "";
        dispatch(Actions.setInventoryFilterViewModel(viewModel));
    }
    const applyFilter = (e) => {
        e.preventDefault();
        var viewModel = { ...props.viewModel };
        viewModel.Vendors = [...vendors.notSelected||[], ...vendors.notSelectedShown||[]];
        viewModel.SelectedVendors = vendors.selected;
        viewModel.Owners = owners.notSelectedShown;
        viewModel.SelectedOwners = owners.selected;
        viewModel.Locations = locations.notSelectedShown;
        viewModel.SelectedLocations = locations.selected;
        viewModel.Categories = categories.notSelectedShown;
        viewModel.SelectedCategories = categories.selected;
        viewModel.Subcategories = subcategories.notSelectedShown;
        viewModel.SelectedSubcategories = subcategories.selected;
        viewModel.NumFilters = numFilters;
        viewModel.Archive = isArchived;
        viewModel.CatalogNumber = catalogNumber;
        viewModel.SearchName = searchByName;
        dispatch(Actions.setInventoryFilterViewModel(viewModel)); 
    }
    return (<span>
        {console.log(vendors)}
        <div style={{ width: "1200px", height: "37rem", margin: "0" }} className={"container-fluid overflow-hidden p-4 " + props.navigationInfo.sectionType}>
                <div className="row">
                <div className=" col-1 offset-11">
                    <button type="button" className="close popover-close" data-dismiss="popover" style={{ fontSize: "2rem" }} onClick={()=>props.popupState.setOpen(false)} >&times;</button>
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
                        <input placeholder="Vendor" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterVendors} value={vendors.filterText ??''} />
                        <div className="inventory-filter-col not-selected ">
                            {vendors.notSelectedShown?.map(v =>
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
                    <input placeholder="Owner" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterOwners} value={owners.filterText ?? ''} />
                    <div className="inventory-filter-col not-selected">
                        {owners.notSelectedShown?.map(v =>
                            <button key={v.Id} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left"  value={v.Id} onClick={moveOwnersToSelected} ><span className="filter-button-description">{v.FirstName+" " + v.LastName}</span></button>
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
                        <input placeholder="Location" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterLocations} value={locations.filterText ?? ''} />
                        <div className="inventory-filter-col not-selected">
                            {locations.notSelectedShown?.map(v =>
                                <button key={v.LocationTypeID} id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left" value={v.LocationTypeID} onClick={moveLocationsToSelected}><span className="filter-button-description" >{v.LocationTypeName}</span></button>
                                )}
                            </div>
                        <div className="inventory-filter-col selected">
                            {locations.selected?.map(v =>
                                <button key={ v.LocationTypeID } id="" type="button" className="table-button btn-filter btn-filter-style my-1 w-100 text-left filter-btn-select" value={v.LocationTypeID} onClick={moveLocationsToNotSelected} ><span className="filter-button-description">{v.LocationTypeName}</span></button>
                               ) }
                            </div>
                    </div>
                    :""
                    }
                    <div className="border-right col filter-col category-col">
                    <input placeholder="Category" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterCategories} value={categories.filterText ?? ''}  />
                    <div className="inventory-filter-col not-selected">
                        {categories.notSelectedShown?.map(v =>
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
                    <input placeholder="Subcategory" className="mb-2 disabled-text w-100 form-control-plaintext border-bottom " onChange={filterSubcategories} value={subcategories.filterText ?? ''} />
                    <div className="inventory-filter-col not-selected">
                        {subcategories.notSelectedShown?.map(v =>
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