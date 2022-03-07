import React, { useState, useEffect } from 'react';
import { Route, Switch, MemoryRouter, Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import { createStore, } from 'redux';
//import * as ModalKeys from '../Constants/ModalKeys.jsx'
//import * as Routes from '../Constants/Routes.jsx'
//import ModalLoader from './modal-loader.jsx';
import reducer from '../ReduxRelatedUtils/reducers.jsx';
import { composeWithDevTools } from 'redux-devtools-extension';

export default function OrderTabRootComponent(props) {
        const store = createStore(reducer, { /*viewModel: props.viewModel,*/ modals: [] }, composeWithDevTools());
    function App() {

        return (
            <div>
                <div class="row">
                    <div class="col-12">
                        <span class="heading-1 modal-tab-name">Order</span>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row ">
                        <div class="col-7 align-vertical-middle-col">
                            <span class="small-text">If you want to process a single order immediately</span>
                        </div>
                        <div class="col-5">
                            <button type="submit" name="OrderMethod" value="OrderNow"
                                class="text w-100 @colorClass custom-button float-right submitOrder">
                Order Now /* figure out how to use get enum display name func here...
                            </button>
                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-7 align-vertical-middle-col">
                            <span class="small-text">If you want to order multiple items</span>
                        </div>
                        <div class="col-5">
                            <button type="submit" name="OrderMethod" value="AddToCart"
                                class="text w-100 @colorClass custom-button float-right submitOrder ">
                                Add To Cart
                            </button>
                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-7 align-vertical-middle-col">
                            <span class="small-text">Update the purchase of an item</span>
                        </div>
                        <div class="col-5">
                            <button type="submit" name="OrderMethod" value="AlreadyPurchased"
                                class="p-0 @colorClass text w-100 custom-button create-modal-submit submitOrder">
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

                    <Switch>
                        <Route exact path={Routes.ORDER_TAB} exact render={(props) => <ModalLoader {...props} modalKey={ModalKeys.ORDER_OPERATIONS} uid={props.location.key} />} />
                    </Switch>
            </div>
        )
    }
    return (
        <Provider store={store}></Provider>
    );
}