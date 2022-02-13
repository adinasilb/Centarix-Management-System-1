import React, { Component } from 'react';

export default class SettingsForm extends Component {

    state = {
        categoryNameRows : 1
    }

    constructor(props) {
        super(props);
        this.state = { SettingsForm: this.props.SettingsForm };
    }

    render() {
        var autoHeight = {
            height: 'auto'
        }
        return (
            <form action="" method="get" class="lab-man-form" id="myForm">
                <div className="row top-bar border-bottom text-center text-justify under-row-margin d-flex" >
                    <div className="col-12 align-items-center justify-content-center mt-3 mb-3">
                        <span className="align-items-center">
                            <span className="lab-man-color">{this.props.SettingsForm.ItemCount}</span> Items &nbsp;&nbsp;&nbsp;&nbsp;
                              <span className="lab-man-color">{this.props.SettingsForm.RequestCount}</span> Requests
                          </span>
                    </div>
                </div>
                <div className="new-modal-header modal-line-1-header-with-back modal-sides ch-scrollable pt-0">
                    <div className="row modal-title-line justify-content-between under-row-margin container">
                        {
                            categoryNameRows = 1
                            //categoryNameRows = this.props.SettingsForm.Category.Description != null ?
                            //    Math.Ceiling((this.props.SettingsForm.Category.Description.Length) / 30) : 1
                        }
                        <div className="col-2">
                            <img src={this.props.SettingsForm.Category.ImageURL} className="sub-category-image top-modal-image" alt="Alternate Text" width="75" />
                        </div>
                        <div className="col-8">
                            <div className="modal-product-title" style=" margin-left: 2rem !important">
                                <textarea asp-for="Category.Description" className="form-control-plaintext border-bottom heading-1 category-name" placeholder="(category name)" rows={categoryNameRows} cols="50" maxlength="150" style="resize:none;"></textarea>
                            </div>
                            <span asp-validation-for="Category.Description" className="text-danger-centarix"></span>
                        </div>
                        <div className="col-2">
                            <input type="button" className="save-settings-form custom-button custom-button-font lab-man-background-color" value="Save" />
                        </div>

                    </div>
                </div>
            </form>
        )
    }
}