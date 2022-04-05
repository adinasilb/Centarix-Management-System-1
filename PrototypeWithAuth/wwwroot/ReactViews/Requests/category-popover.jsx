import { Popover, Typography, Tooltip, MenuItem } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import FormGroup from '@mui/material/FormGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { MenuItems } from '../Constants/AppUtility.jsx'
import {
    useState, useEffect, useRef
} from 'react'
import {connect,  useDispatch, batch } from 'react-redux';
function CategoryPopover(props) {
    const [state, setState] = useState({ categorySelected: props.viewModel.SelectedCategoryOption[0]??false, subcategorySelected: props.viewModel.SelectedCategoryOption[1]??false, sourceSelected: props.viewModel.SelectedCategoryOption[2] ??false })
    const dispatch = useDispatch();
    const didMount = useRef(false);
    const changeCategorySelected = (e) => {
        setState({ ...state, categorySelected: e.target.checked });
    }
    const changeSubcategorySelected = (e) => {
        setState({ ...state, subcategorySelected: e.target.checked });
    }
    const changeSourceSelected = (e) => {
        setState({ ...state, sourceSelected: e.target.checked });
    }
    useEffect(() => {
        if (didMount.current) {
            batch(() => {
                dispatch(Actions.setCategorySelected(state.categorySelected));
                dispatch(Actions.setSubCategorySelected(state.subcategorySelected));
                dispatch(Actions.setSourceSelected(state.sourceSelected));
            })

        }
        else {
            didMount.current = true;
        }
    }, [state.sourceSelected, state.categorySelected, state.sourceSelected])
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
                            {props.navigationInfo.sectionType == MenuItems.Accounting ?
                                <FormGroup>
                                    <FormControlLabel control={<Checkbox onChange={changeSourceSelected} checked={state.sourceSelected} />} label="Source" />
                                </FormGroup> : ""}
                            <FormGroup>
                                <FormControlLabel control={<Checkbox onChange={changeCategorySelected} checked={state.categorySelected} />} label="Category" />
                                    </FormGroup>
                            <FormGroup>
                                <FormControlLabel control={<Checkbox onChange={changeSubcategorySelected} checked={state.subcategorySelected} />} label="Subcategory" />
                                </FormGroup>
                          
                            </div>
                    </Popover>
                </div>
            )}
        </PopupState>
        
        )
}

const mapStateToProps = state => {
    return {
        navigationInfo: state.navigationInfo,
    };
};

export default connect(
    mapStateToProps
)(CategoryPopover)
