import { Popover, Typography } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import IndexFiterResults from './index-filter-results.jsx'
export default function IndexFilter(props) {

    return (
        <div className="container-fluid ">
            <div className="row">
        <PopupState  variant="popover" popupId="filterPopover">                   
            {(popupState) => (
                <div >
                    <input type="text" placeholder="Search" className="text  custom-button mx-3 search-by-name" />
                            <button type="button" aria-describedby="filterPopover"
                                className="text custom-button " value="Filter"   {...bindTrigger(popupState)}>
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

                        
                         
                                <IndexFiterResults />
      
                   
                    </Popover>
                </div>
            )}
        </PopupState>
            </div>
        </div>
    )
}