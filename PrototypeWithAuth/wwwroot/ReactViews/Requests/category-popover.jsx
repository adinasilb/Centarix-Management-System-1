import { Popover, Typography, Tooltip } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import FormGroup from '@mui/material/FormGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import { connect } from 'react-redux';
export default function CategoryPopover(props) {

    return (
        
        <PopupState  variant="popover" popupId="categoryPopover">
            {(popupState) => (
                <div className="table-icon-div">
                    <i className={"icon-centarix-icons-17"} style={{ fontSize: "1.6rem" }} aria-describedby="categoryPopover"  {...bindTrigger(popupState)}></i>
                    <Popover
                        id="categoryPopover"
                        {...bindPopover(popupState)}
                        anchorOrigin={{
                            vertical: 'bottom',
                            horizontal: 'center',
                        }}
                    >

                        <div className="m-3">
                                    <FormGroup>
                                        <FormControlLabel control={<Checkbox checked={props.viewModel.SelectedCategoryOption[0]} />} label="Category" />
                                    </FormGroup>
                                    <FormGroup>
                                        <FormControlLabel control={<Checkbox checked={props.viewModel.SelectedCategoryOption[1]} />} label="Subcategory" />
                                </FormGroup>
                                <FormGroup>
                                    <FormControlLabel control={<Checkbox checked={props.viewModel.SelectedCategoryOption[2]} />} label="Source" />
                                </FormGroup>
                            </div>
                    </Popover>
                </div>
            )}
        </PopupState>
        
        )
}