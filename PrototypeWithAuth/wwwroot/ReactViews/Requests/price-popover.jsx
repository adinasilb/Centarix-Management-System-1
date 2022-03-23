import { Popover, Typography } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import FormGroup from '@mui/material/FormGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Radio from '@mui/material/Radio';
import FormControl from '@mui/material/FormControl';
import RadioGroup from '@mui/material/RadioGroup';
import { connect } from 'react-redux';
function PricePopover(props) {

    return (

        <PopupState variant="popover" popupId="pricePopover">
            {(popupState) => (
                <div className="table-icon-div">
                    <i className={"icon-centarix-icons-17"} style={{ fontSize: "1.6rem" }} aria-describedby="pricePopover"  {...bindTrigger(popupState)}></i>
                    <Popover
                        id="pricePopover"
                        {...bindPopover(popupState)}
                        anchorOrigin={{
                            vertical: 'bottom',
                            horizontal: 'center',
                        }}
                    >

                        <Typography sx={{ p: 1 }}>
                            <span>
                                {props.viewModel.PriceSortEnums.map(v =>
                                    <div key={ v.PriceSortEnum } >
                                        <FormGroup>
                                            <FormControlLabel className="m-0" control={<Checkbox className="p-0" checked={v.Selected} />} label={v.PriceSortEnum} />
                                        </FormGroup>          
                                        <hr class="mx-0  my-2" />
                                    </div>
                                   
                                )}
                                <FormControl>
                                    <RadioGroup sx={{ p: 0 }}
                                        row
                                        aria-labelledby="currency"
                                        name="currency"
                                        value={props.viewModel.SelectedCurrency}
                                    >            
                                        <FormControlLabel className="m-0" value="NIS" control={<Radio className="p-0" />} label="NIS" />
                                        <FormControlLabel className="m-0 pl-2" value="USD" control={<Radio className="p-0" />} label="USD" />
                                    </RadioGroup>
                                </FormControl>

                            </span>
                        </Typography>
                    </Popover>
                </div>
            )}
        </PopupState>

    )
}
const mapStateToProps = state => {
    console.log("mstp pricePopover")
    return {
        viewModel: state.pricePopoverViewModel
    };
};

export default connect(
    mapStateToProps
)(PricePopover)