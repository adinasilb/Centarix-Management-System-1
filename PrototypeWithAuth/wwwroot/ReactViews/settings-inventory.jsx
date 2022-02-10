import React, { Component } from 'react';
import CategoryListSettings from './category-list-settings.jsx';
import TopTabsCount from './top-tabs-counts.jsx';
export default class SettingsInventory extends Component {
    constructor(props) {
        super(props);

        this.state = { isLoaded: this.props.viewModel != undefined, viewModel: this.props.viewModel, showView: this.props.showView };
    }
    render() {
        if (this.state.isLoaded == true) {
            return (
                <div>
                    <TopTabsCount list={this.props.viewModel.TopTabsCount} />
                    <div> Settings Inventory == State is loaded</div>
                    <CategoryListSettings categories={this.props.viewModel.Categories} />
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