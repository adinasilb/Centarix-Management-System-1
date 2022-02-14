import React, { Component } from 'react';

export default class CustomField extends Component {

    constructor(props) {
        super(props);
        //this.state = { tabName: this.props.tabName };
    }

    render() {
        return (
            <div>CUSTOM FIELD ATTRIBUTE</div>
            //<input type="button" onClick={this.OpenNewCustomField} className={"custom-button custom-cancel text border-dark " + this.tabName}
            //    value="+ Add Custom Field" />
        )
    }
}