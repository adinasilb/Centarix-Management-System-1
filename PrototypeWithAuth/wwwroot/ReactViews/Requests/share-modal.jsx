import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { ajaxPartialIndexTable, getRequestIndexString } from '../Utility/root-function.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import GlobalModal from '../Utility/global-modal.jsx';
import { Select, MenuItem, ListItemText, Checkbox, FormControl, InputLabel , OutlinedInput} from '@mui/material';
//import { MDBSelect } from 'mdbreact';

export default function ShareModal(props) {
    const dispatch = useDispatch();
    const location = useLocation();

    const [state, setState] = useState({ viewModel: null, requestID: location.state.ID, modelsEnum : location.state.modelsEnum, users:[]});
    console.log(location.state)
    useEffect(() => {
        var url = "/Requests/ShareModalJson?id=" + state.requestID + "& modelsEnum=" + state.modelsEnum+"&" + getRequestIndexString();
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setState({ ...state, viewModel: JSON.parse(result) });
            });

    }, [state.requestID]);
    const handleChange = (event) => {

        // On autofill we get a stringified value.
        const {
            target: { value },
        } = event;
        setState({
            ...state,
            users: value,
        });
    };
    var onSubmit = (e) => {
        e.preventDefault();
        e.stopPropagation();
        var url = "/" + document.getElementById('masterSectionType')?.value + "/ShareModal";
        document.getElementById("loading").style.display = "block";
        var formData = new FormData(e.target);

        for (var i = 0; i < state.users.length; i++) {
            formData.append('ApplicationUserIDs', state.users[i]);
        }

        formData
        fetch(url, {
            method: "POST",
            body: formData
        }).then(response => response.json())
            .then(result => {
                dispatch(Actions.removeModals(modals));
                document.getElementById("loading").style.display = "none";                

            }).catch(jqxhr => {
                dispatch(Actions.removeModals(modals));
                document.getElementById("loading").style.display = "none";
                document.querySelectorAll('.error-message').forEach(e => e.classList.add("d-none"));
                document.querySelector('.error-message').innerHTML = jqxhr;
                document.querySelector('.error-message').classList.remove("d-none");
            });

     
    }

    return (
        <GlobalModal backdrop={props.backdrop} size="" value={state.viewModel?.ID} modalKey={props.modalKey} key={state.viewModel?.ID} header={"Share " + state.viewModel?.ObjectDescription + " With"} >
            
            <form action="" method="post" className="sharemodal" encType="multipart/form-data" onSubmit={onSubmit} id={props.modalKey }>
                <input type="hidden" id="ID" name="ID" value={state.viewModel?.ID ?? ""} />
                <div className="contaner-fluid p-0">
                    <div className="row ">
                        <div className="col-10 offset-1">
                            {/*<MDBSelect  options={state.viewModel?.ApplicationUsers?.map((u) => (*/}
                            {/*    { text: u.Text, value: u.Value}*/}
                            {/*)) ?? []}  name="ApplicationUserIDs" id="ApplicationUserIDs" multiple/>*/}
                            <FormControl fullWidth>
                                <InputLabel id="users-label">Users</InputLabel>
                                <Select
                                    onChange={handleChange}
                                    labelId="users-label"                                 
                                    multiple
                                    value={state.users}
                                    renderValue={(selected) => {
                                        var text = [];
                                        for (var i = 0; i < selected.length; i++) {
                                            text.push(state.viewModel?.ApplicationUsers?.filter(u => u.Value == selected[i])[0]?.Text);
                                        }
                                        return text.join(",")
                                    }}
                                    input={<OutlinedInput label="Tag" />}>{
                                    state.viewModel?.ApplicationUsers?.map((u) => (
                                        <MenuItem key={u.Value} value={u.Value}>
                                            <Checkbox checked={state.users.indexOf(u.Value) > -1} />
                                            <ListItemText primary={u.Text} />
                                        </MenuItem>
                                    ))}
                            </Select>
                            </FormControl>
                          
                        </div>
                    </div>
                </div>
            </form>
        </GlobalModal>
    );

}
