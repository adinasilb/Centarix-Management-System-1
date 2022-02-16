import React, { Component } from 'react';
import { MDBSelect }  from 'mdbreact';

export default class CustomField extends Component {

    constructor(props) {
        super(props);
        console.log("in custom field: " + props.text);
        this.state = { CustomFieldData: props.CustomFieldData };
    }

    Remove = (e) => {
        console.log("remove");
        e.preventDefault();
    }

    render() {
        this.defaultProps = {
            checkedRequired: false
        }
        var selectOptions = this.props.CustomFieldData.CustomDataTypes.map((data, i) => {
            return ({
                text: data.CustomDataTypeID,
                value: data.Name
            })
        });
        return (
            <div className="w-100">
                <div className="custom-fields row">
                    <div className="col-4">
                        <div className="form-group">
                            <label className="control-label" ></label>
                            <input className="form-control-plaintext border-bottom" />
                            <span className="centarix-error-style"></span>
                        </div>
                    </div>
                    <div className="col-4">
                        <div className="form-group">
                            <label className="control-label">Data Type</label>
                            <MDBSelect>
                                <option value="1">Hello</option>
                                {this.props.CustomFieldData.CustomDataTypes.map((dataType, i) => (
                                    <option value={dataType.CustomDataTypeID} key={i}>{dataType.Name}</option>
                                ))}
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
                            <span className="centarix-error-style"></span>
                        </div>
                    </div>
                    <div className="col-2 d-flex ml-5"> {/*style="height:auto;">*/}
                        <div className="form-check d-flex align-items-end pb-2">{/* style="height:auto;">*/}
                            <input type="checkbox" checked={this.checkedRequired} name="controlled"></input>
                            {/*<label className="form-check-label align-items-end"  >Required</label>*/}
                        </div>
                    </div>
                    <div className="col-1 d-flex"> {/*style="height:auto;">*/}
                        <a href="" onClick={this.Remove} className="remove-custom-field danger-filter align-items-end pb-2 d-flex" divclass="@Model.DivClass" cf="@Model.CustomFieldCounter" > {/*style="height:auto; font-size:1.75rem;">*/}
                            <i className="icon-delete-24px align-items-end"></i>
                        </a>
                    </div>
                </div>
            </div>
        )
    }
}