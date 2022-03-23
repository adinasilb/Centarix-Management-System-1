import { Popover, Typography } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import IndexFiterResults from './index-filter-results.jsx'
export default function IndexFilter(props) {

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

                                <input type="text" placeholder="Search" className="text filter-and-search  custom-button  section-outline-color mx-3 search-by-name" />
                                <button type="button" aria-describedby="filterPopover"
                                    className={"text custom-button " + buttonClass} value="Filter"   {...bindTrigger(popupState)}>
                                    Filter
                                </button>
                                <Popover
                                    id="filterPopover"
                                    {...bindPopover(popupState)}
                                    anchorOrigin={{
                                        vertical: 'bottom',
                                        horizontal: 'center',
                                    }}
                                >



                                    <IndexFiterResults  popupState={popupState} />


                                </Popover>
                            </div>
                        )
                    }}
        </PopupState>
            </div>
        </div>
    )
}