import React, { Component } from 'react';
import { MDBSelect, MDBSelectOptions, MDBSelectOption, MDBSelectInput, MDBInput } from 'mdbreact';

export default class CustomField extends Component {
    constructor(props) {
        super(props);
        console.log("in custom field: " + props.text);
        this.state = { CustomFieldData: props.CustomFieldData, RemoveCustomField: props.RemoveCustomField };
    }


    changeSelectValue = (e) => {
        e.preventDefault();
    };

    render() {

        var RemoveCustomField = (customfield) => {
            customfield.preventDefault();
            console.log("in remove custom field");
        }

        this.defaultProps = {
            checkedRequired: false
        }
        return (
            <div className="w-100">
                <div className="custom-fields row">
                    <div className="col-4">
                        <div className="form-group">
                            <label className="control-label" >{"Field Name"}</label>
                            <MDBInput className="form-control-plaintext border-bottom" />
                            <span className="centarix-error-style"></span>
                        </div>
                    </div>
                    <div className="col-4">
                        <div className="form-group">
                            <label className="control-label">Data Type</label>
                            <MDBSelect className="form-control-plaintext border-bottom mdb-select" >
                                <MDBSelectInput selected="Choose your option" />
                                <MDBSelectOptions search>
                                    {this.props.CustomFieldData.CustomDataTypes.map((customField, i) => (
                                        <MDBSelectOption onClick={this.changeSelectValue} value={customField.CustomDataTypeID} key={i}>{customField.Name}</MDBSelectOption>
                                    ))}
                                    {/*<MDBSelectOption key={1}>{"hi"}</MDBSelectOption>*/}
                                    {/*<MDBSelectOption key={2}>{"hi"}</MDBSelectOption>*/}
                                    {/*<MDBSelectOption key={3}>{"hi"}</MDBSelectOption>*/}
                                    {/*<MDBSelectOption key={4}>{"hi"}</MDBSelectOption>*/}
                                    {/*<MDBSelectOption key={5}>{"hi"}</MDBSelectOption>*/}
                                </MDBSelectOptions>
                            </MDBSelect>
                            {/*@Html.DropDownListFor(*/}
                            {/*    m => m.CustomDataTypeID,*/}
                            {/*    new SelectList(*/}
                            {/*Model.CustomDataTypes,*/}
                            {/*dataValueField: "CustomDataTypeID",*/}
                            {/*dataTextField: "Name"),*/}
                            {/*    "Select A Data Type",*/}
                            {/*    new { @class = "mdb-select custom select-dropdown form-control-plaintext " + customMDBSelect, @searchable = "default value" }*/}
                            {/*)*/}
                            < span className="centarix-error-style" ></span>
                        </div>
                    </div>
                    <div className="col-2 d-flex ml-5"> {/*style="height:auto;">*/}
                        <div className="form-check d-flex align-items-end pb-2">{/* style="height:auto;">*/}
                            <MDBInput label="Required" type="checkbox" id="checkbox1" />
                            {/*<label className="form-check-label align-items-end"  >Required</label>*/}
                        </div>
                    </div>
                    <div className="col-1 d-flex"> {/*style="height:auto;">*/}
                        <a href="" onClick={this.RemoveClick} className="remove-custom-field danger-filter align-items-end pb-2 d-flex" divclass="@Model.DivClass" cf="@Model.CustomFieldCounter" > {/*style="height:auto; font-size:1.75rem;">*/}
                            <i className="icon-delete-24px align-items-end"></i>
                        </a>
                    </div>
                </div>
            </div>
        )
    }
}