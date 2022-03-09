import React, { useState, useEffect } from 'react';
import { Route, Switch, MemoryRouter, Router, useHistory, Link } from 'react-router-dom';
import { Provider } from 'react-redux';
import { createStore, } from 'redux';
//import * as ModalKeys from '../Constants/ModalKeys.jsx'
//import * as Routes from '../Constants/Routes.jsx'
//import ModalLoader from './modal-loader.jsx';
import reducer from '../ReduxRelatedUtils/reducers.jsx';
import { composeWithDevTools } from 'redux-devtools-extension';
import * as Routes from '../Constants/Routes.jsx'

export default function OrderTab(props) {
    const history = useHistory();
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
                            <button type="submit" name="OrderMethod" value="OrderNow"
                                className="text w-100 float-right section-custom-input">
                                Order Now {/* figure out how to use get enum display name func here...*/}
                            </button>
                        </div>
                    </div>
                    <div className="row ">
                        <div className="col-7 align-vertical-middle-col">
                            <span className="small-text">If you want to order multiple items</span>
                        </div>
                        <div className="col-5">
                            <button type="submit" name="OrderMethod" value="AddToCart"
                                className="text w-100  float-right section-custom-input">
                                Add To Cart
                            </button>
                        </div>
                    </div>
                    <div className="row ">
                        <div className="col-7 align-vertical-middle-col">
                            <span className="small-text">Update the purchase of an item</span>
                        </div>
                        <div className="col-5">
                            
                                <Link to={{
                                    pathname: history.location.pathname + Routes.ORDER_OPERATIONS_MODAL,
                                    state: { ID: 2 }
                                }} >Already Purchased
                                </Link>
                           
                        </div>
                    </div>
                </div>

            </div>
        )
}