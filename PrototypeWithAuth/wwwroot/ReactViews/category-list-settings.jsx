import React, { Component } from 'react';

export default class CategoryListSettings extends Component {

    constructor(props) {
        super(props);
        this.state = { categories : this.props.categories };
    }

    render() {
        return (
            <div className="row">
                <div className="col-6">
                    halfway columns!
                </div>
                <div className="col-6">
                    halfway columns!
                </div>
            </div>
        )
    }
}