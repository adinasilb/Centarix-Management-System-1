import { Popover, Typography } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import IndexFilterResults from './index-filter-results.jsx'
import {
    useState, useEffect
} from 'react'
import { connect, useDispatch } from 'react-redux';

import * as Actions from '../ReduxRelatedUtils/actions.jsx';
export default function IndexFilter(props) {
    console.log("index filter")
    const [state, setState] = useState(initialState())
    const dispatch = useDispatch();

    const updateSearchName = (e) => {
        setState({ ...state, searchByName: e.target.value });
    }
    const globalUpdateSearchName = (e) => {
        console.log(e.target.value)
        if (e.target.value?.length >= 3 || e.keyCode == 13 || e.keyCode==8) {
            dispatch(Actions.setSearchText(e.target.value));
        }       
    }

    function initialState() {
        return {
            numFilters: props.viewModel.NumFilters,
            searchByName: props.viewModel.SearchName,
            catalogNumber: props.viewModel.CatalogNumber,
            isArchived: props.viewModel.Archive,
            vendors: { filterText: "", notSelectedShown: props.viewModel.Vendors, selected: props.viewModel.SelectedVendors || [], notSelectedHidden: [] },
            owners: { filterText: "", notSelectedShown: props.viewModel.Owners, selected: props.viewModel.SelectedOwners || [], notSelectedHidden: [] },
            locations: { filterText: "", notSelectedShown: props.viewModel.Locations, selected: props.viewModel.SelectedLocations || [], notSelectedHidden: [] },
            categories: { filterText: "", notSelectedShown: props.viewModel.Categories, selected: props.viewModel.SelectedCategories || [], notSelectedHidden: [] },
            subcategories: { filterText: "", notSelectedShown: props.viewModel.Subcategories, selected: props.viewModel.SelectedSubcategories || [], notSelectedHidden: [] }
        };
    }
    return (
        <div className="container-fluid ">
            <div className="row">
        <PopupState  variant="popover" popupId="filterPopover">                   
                    {(popupState) => {
                        var buttonClass = "custom-button-font section-bg-color";
                        if (popupState.isOpen == false) {
                            buttonClass = "section-outline-color";
                        }
                        return (

                            <div >

                                <input type="text" placeholder="Search" onChange={updateSearchName} onKeyUp={globalUpdateSearchName} value={state.searchByName ?? ""} className="text filter-and-search  custom-button  section-outline-color mx-3 search-by-name" />
                                {props.showFilter && <button type="button" aria-describedby="filterPopover"
                                    className={"text custom-button " + buttonClass} value="Filter"   {...bindTrigger(popupState)}>
                                    Filter
                                </button>
                                }
                                <Popover
                                    id="filterPopover"
                                    {...bindPopover(popupState)}
                                    anchorOrigin={{
                                        vertical: 'bottom',
                                        horizontal: 'center',
                                    }}
                                >
                                    <IndexFilterResults key="InventoryFilterResults" viewModel={props.viewModel} popupState={popupState} state={state} setState={setState} />
                                </Popover>
                            </div>
                        )
                    }}
        </PopupState>
            </div>
        </div>
    )
}
