import React, { Component } from 'react';
import { MDBSelect, MDBSelectOptions, MDBSelectOption, MDBSelectInput, MDBInput } from 'mdbreact';

export default class CustomField extends Component {
    constructor(props) {
        super(props);
        console.log("in custom field: " + props.text);
        this.state = { CustomFieldData: props.CustomFieldData, ValidationErrors: props.ValidationErrors };
    }


    changeSelectValue = (e) => {
        e.preventDefault();
    };

    render() {

        console.log("validation errors");
        console.log(this.state.ValidationErrors[1].dict["datatype"][0]);

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
                            </MDBSelectOptions>
                            </MDBSelect>
                            <span className="centarix-error-style" >{}</span>
                    </div>
                </div>
                <div className="col-2 d-flex ml-5"> {/*style="height:auto;">*/}
                    <div className="form-check d-flex align-items-end pb-2">{/* style="height:auto;">*/}
                        <MDBInput label="Required" type="checkbox" id={"checkbox" + this.props.number} />
                        {/*<label className="form-check-label align-items-end"  >Required</label>*/}
                    </div>
                </div>
                <div className="col-1 d-flex"> {/*style="height:auto;">*/}
                    <a href="" onClick={this.props.RemoveCustomField} className="remove-custom-field danger-filter align-items-end pb-2 d-flex" number={this.props.number} > {/*style="height:auto; font-size:1.75rem;">*/}
                        <i className="icon-delete-24px align-items-end"></i>
                    </a>
                </div>
            </div>
        </div>
        )
    }
}