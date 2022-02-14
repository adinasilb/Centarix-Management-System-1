import React, { Component } from 'react';

export default class CategoryListSettings extends Component {

    constructor(props) {
        super(props);
        this.state = { categories: this.props.categories };
    }

    render() {
        return (
            <div className="row lab-man-form">
                <div className={"col-12 category-cols ch-scrollable h-100" + (this.props.categories.ColumnNumber)}>
                    <input type="text" placeholder="search" className="search-by-category col-input" />
                    {this.props.categories.CategoryBases.map((CategoryBase, i) => (
                        <button key={"Category" + i} href="#" className={"category-field col-input " + " category-field" + this.props.categories.ColumnNumber + ((i == 0) ? " selected " : "")} data-catid={CategoryBase.ID}>
                            {CategoryBase.Description}
                        </button>
                    ))
                    }
                </div>
            </div>
        )
    }
}