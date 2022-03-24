import { Popover, Typography, Tooltip } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import FormGroup from '@mui/material/FormGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import { connect } from 'react-redux';
function CategoryPopover(props) {

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

                        <Typography sx={{p:1  }}> 
                            <span>
                                    <FormGroup>
                                        <FormControlLabel control={<Checkbox checked={props.viewModel.SelectedCategoryOption[0]} />} label="Category" />
                                    </FormGroup>
                                    <FormGroup>
                                        <FormControlLabel control={<Checkbox checked={props.viewModel.SelectedCategoryOption[1]} />} label="Subcategory" />
                                </FormGroup>
                                <FormGroup>
                                    <FormControlLabel control={<Checkbox checked={props.viewModel.SelectedCategoryOption[2]} />} label="Source" />
                                </FormGroup>
                            </span>
                        </Typography>
                    </Popover>
                </div>
            )}
        </PopupState>
        
        )
}
const mapStateToProps = state => {
    console.log("mstp category popover")
    return {
       viewModel: state.categoryPopoverViewModel
    };
};

export default connect(
    mapStateToProps
)(CategoryPopover)