import React, { Component } from 'react';

export default class CustomFieldButton extends Component {

    constructor(props) {
        super(props);
        this.state = { tabName: this.props.tabName };
        this.OpenNewCustomField = this.OpenNewCustomField.bind(this);
    }

    OpenNewCustomField() {
        switch(this.props.tabName){
            case "details":
                var div = document.getElementsByClassName("custom-fields-details");
                div.append(<CustomFieldButton tabName={"details"} />);
                break;
        }
    }

    render() {
        return (
            <input type="button" onClick={this.OpenNewCustomField} className={"custom-button custom-cancel text border-dark " + this.tabName}
                value="+ Add Custom Field" />
        )
    }
}