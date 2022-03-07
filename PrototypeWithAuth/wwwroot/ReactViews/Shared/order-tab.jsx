import React, { useState, useEffect } from 'react';
import { Route, Switch, MemoryRouter, Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import { createStore, } from 'redux';
//import * as ModalKeys from '../Constants/ModalKeys.jsx'
//import * as Routes from '../Constants/Routes.jsx'
//import ModalLoader from './modal-loader.jsx';
import reducer from '../ReduxRelatedUtils/reducers.jsx';
import { composeWithDevTools } from 'redux-devtools-extension';

export default function OrderTab(props) {

        return (
            <div>
                <div className="row">
                    <div className="col-12">
                        <span className="heading-1 modal-tab-name">Order</span>
                    </div>
                </div>
                <div className="form-group">
                    <div className="row ">
                        <div className="col-7 align-vertical-middle-col">
                            <span className="small-text">If you want to process a single order immediately</span>
                        </div>
                        <div className="col-5">
                            <Link className="p-0 @colorClass text w-100 custom-button create-modal-submit submitOrder" to={{
                                pathname: "/OrderOperationsModalJson",
                                state: { ID: 2 }
                            }} >
                            </Link>
                            <button type="submit" name="OrderMethod" value="OrderNow"
                                className="text w-100 @colorClass custom-button float-right submitOrder">
                Order Now /* figure out how to use get enum display name func here...
                            </button>
                        </div>
                    </div>
                    <div className="row ">
                        <div className="col-7 align-vertical-middle-col">
                            <span className="small-text">If you want to order multiple items</span>
                        </div>
                        <div className="col-5">
                            <button type="submit" name="OrderMethod" value="AddToCart"
                                className="text w-100 @colorClass custom-button float-right submitOrder ">
                                Add To Cart
                            </button>
                        </div>
                    </div>
                    <div className="row ">
                        <div className="col-7 align-vertical-middle-col">
                            <span className="small-text">Update the purchase of an item</span>
                        </div>
                        <div className="col-5">
                            <button type="submit" name="OrderMethod" value="AlreadyPurchased"
                                className="p-0 @colorClass text w-100 custom-button create-modal-submit submitOrder">
                                Already Purchased
                            </button>
                            <Link className="p-0 @colorClass text w-100 custom-button create-modal-submit submitOrder" to={{
                                pathname: "/OrderOperationsModalJson",
                                state: { ID: 2 }
                            }} >
                            </Link>
                        </div>
                    </div>
                </div>

            </div>
        )
}