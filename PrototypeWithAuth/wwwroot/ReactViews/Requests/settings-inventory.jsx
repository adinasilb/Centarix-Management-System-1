import React, { Component, setState } from 'react';
import CategoryListSettings from './category-list-settings.jsx';
import TopTabsCount from '../Shared/top-tabs-counts.jsx';
import SettingsForm from './settings-form.jsx';
import { connect } from 'react-redux';
import axios from "axios";
class SettingsInventory extends Component {
    constructor(props) {
        alert("constructor settings inventroy");
        super(props);
        this.state = {
            isLoaded: this.props.viewModel != undefined, Categories: this.props.viewModel.Categories, Subcategories: this.props.viewModel.Subcategories,
            SettingsForm: this.props.viewModel.SettingsForm, showView: this.props.showView
        };
        this.changeCategory = this.changeCategory.bind(this);
    }

    changeCategory(ModelType, CatID) {
        alert("in change category");
        fetch("/Requests/GetSettingsFormJsonResult?ModelType=" + ModelType + "&CategoryID=" + CatID, {
            method: "GET"
        })
        .then((response) => { return response.json(); })
            .then(result => {
                alert(JSON.stringify(result));
                alert("in result");
                this.setState({ SettingsForm: result })
        });
        
    };
    componentWillUnmount() {
        console.log("component will unmount")
    }
    render() {
        if (this.state.isLoaded == true) {
            return (
                <div className="lab-man-form" >
                    <TopTabsCount list={this.props.viewModel.TopTabsList} />
                    <div className="row mb-5">
                        <div className="col-2 form-element-border-xsmall p-2 category-list-1 cat-col category-height ">
                            <CategoryListSettings categories={this.props.viewModel.Categories} columnNum="1" changeCategory={this.changeCategory} />
                        </div>
                        <div className="col-2 form-element-border-xsmall p-2 category-list-2 cat-col category-height ">
                            <CategoryListSettings categories={this.props.viewModel.Subcategories} columnNum="2" changeCategory={this.changeCategory} />
                        </div>
                        <div className="col-8 form-element-border-xsmall settings-form">
                            <SettingsForm SettingsForm={this.state.SettingsForm} />
                        </div>
                    </div>
                </div>
            )
        }
        else {
            return (
                <div></div>
            )
        }
    }
}

const mapStateToProps = state => {
    console.log("mpst setting inv")
    return {
        viewModel: state.viewModel
    };
};

export default connect(
    mapStateToProps
)(SettingsInventory)