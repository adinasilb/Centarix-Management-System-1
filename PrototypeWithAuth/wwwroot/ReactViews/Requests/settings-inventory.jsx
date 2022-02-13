import React, { Component } from 'react';
import CategoryListSettings from './category-list-settings.jsx';
import TopTabsCount from '../Shared/top-tabs-counts.jsx';
import SettingsForm from './settings-form.jsx';
export default class SettingsInventory extends Component {
    constructor(props) {
        super(props);

        this.state = { isLoaded: this.props.viewModel != undefined, viewModel: this.props.viewModel, showView: this.props.showView };
    }
    render() {
        if (this.state.isLoaded == true) {
            return (
                <div className="lab-man-form" >
                    <TopTabsCount list={this.props.viewModel.TopTabsList} />
                    <div className="row mb-5">
                        <div className="col-2 form-element-border-xsmall p-2 category-list-1 cat-col category-height ">
                            <CategoryListSettings categories={this.props.viewModel.Categories} />
                        </div>
                        <div className="col-2 form-element-border-xsmall p-2 category-list-1 cat-col category-height ">
                            <CategoryListSettings categories={this.props.viewModel.Subcategories} />
                        </div>
                        <div className="col-8 form-element-border-xsmall settings-form">
                            <SettingsForm SettingsForm={this.props.viewModel.SettingsForm} />
                        </div>
                    </div>
                </div>
            )
        }
        else {
            return (
                <div> Settings Inventory == States is not loaded</div>
            )
        }
    }
}