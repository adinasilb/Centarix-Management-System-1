import { Popover, Typography } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import FormGroup from '@mui/material/FormGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Radio from '@mui/material/Radio';
import FormControl from '@mui/material/FormControl';
import RadioGroup from '@mui/material/RadioGroup';
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import {
    useState, useEffect, useRef
} from 'react'
import {  useDispatch, batch } from 'react-redux';
import { CurrencyEnum } from '../Constants/AppUtility.jsx'
export default function PricePopover(props) {
    const [state, setState] = useState({ selectedCurrency: props.viewModel.SelectedCurrency ?? CurrencyEnum.NIS, priceSortEnums: props.viewModel.PriceSortEnums.map(v => { return { priceSortEnum: v.PriceSortEnum, selected: v.Selected } }) })
    const dispatch = useDispatch();
    const changeSelectedCurrency = (e) => {
        setState({ ...state, selectedCurrency: e.target.defaultValue });
    }
    const changeSelectedPriceSortEnums = (e) => {
        const index = state.priceSortEnums.findIndex(p => {
            return p.priceSortEnum === e.target.defaultValue;
        });
        setState({
            ...state, priceSortEnums: [
                ...state.priceSortEnums.slice(0, index),
             {
                 ...state.priceSortEnums[index],
                 selected: e.target.checked,
            },
                ...state.priceSortEnums.slice(index+1)
    ] });
    }

    const didMount = useRef(false);

    useEffect(() => {
        if (didMount.current) {
            batch(() => {
                dispatch(Actions.setSelectedCurrency(state.selectedCurrency));
                dispatch(Actions.setPriceSortEnums(state.priceSortEnums.filter(p => { return p.selected })
                    .map(function (obj) { return obj.priceSortEnum; })));
            })
          
        }
        else {
            didMount.current = true;
        }
    }, [state.selectedCurrency, state.priceSortEnums])
    return (

        <PopupState variant="popover" popupId="pricePopover">
            {(popupState) => (
                <div className="table-icon-div">
                    <i className={"icon-centarix-icons-17"} style={{ fontSize: "1.6rem" }} aria-describedby="pricePopover"  {...bindTrigger(popupState)}></i>
                    <Popover
                        style={{
                            padding: '2rem',
                        }}
                        id="pricePopover"
                        {...bindPopover(popupState)}
                        anchorOrigin={{
                            vertical: 'bottom',
                            horizontal: 'center',
                        }}
                    >
                        <div className="m-3">
                                {state.priceSortEnums.map(v =>
                                    <div key={ v.priceSortEnum } >
                                        <FormGroup>
                                            <FormControlLabel className="m-0" control={<Checkbox onChange={changeSelectedPriceSortEnums} className="p-0" checked={v.selected} value={ v.priceSortEnum} />} label={v.priceSortEnum} />
                                        </FormGroup>
                                        <hr className="mx-0  my-2" />
                                    </div>
                                   
                                )}
                                <FormControl>
                                    <RadioGroup sx={{ p: 0 }}
                                        row
                                        aria-labelledby="currency"
                                        name="currency"
                                        value={state.selectedCurrency}
                                        onChange={changeSelectedCurrency}
                                    >            
                                        <FormControlLabel className="m-0" value="NIS" control={<Radio className="p-0" />} label="NIS" />
                                        <FormControlLabel className="m-0 pl-2" value="USD" control={<Radio className="p-0" />} label="USD" />
                                    </RadioGroup>
                                </FormControl>

                            </div>
         
                    </Popover>
                </div>
            )}
        </PopupState>

    )
}

