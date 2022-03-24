import React, { useState, useEffect } from 'react';
import { Route, Switch, MemoryRouter, Router, useHistory, Link } from 'react-router-dom';
import { Provider, connect } from 'react-redux';
import { createStore } from 'redux';
import * as Actions from '../ReduxRelatedUtils/actions.jsx'
import reducer from '../ReduxRelatedUtils/reducers.jsx';
import { composeWithDevTools } from 'redux-devtools-extension';
import * as Routes from '../Constants/Routes.jsx'

function OrderTab(props) {
    const history = useHistory();
    const [viewModel, setViewModel] = useState(props.viewModel)

    function submitOrder(orderMethod) {
        console.log("submit order from _ordertab");
        document.getElementById("loading").style.display = "block";
        console.log("orderMethod " + orderMethod)
        var formData = new FormData(document.getElementById("myForm"))
        fetch("/Requests/AddItemView?OrderMethod=" + orderMethod,
            {
                method: "POST",
                body: formData
            }
        )
            .then(response => response.json())
            .then(result => props.setTempRequestList(JSON.parse(result)))
        document.getElementById("loading").style.display = "none";
    }

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
                        <Link onClick={()=>submitOrder("OrderNow")} to={{
                                pathname: history.location.pathname + Routes.UPLOAD_QUOTE,
                            state: { ID: viewModel.GUID, moveToTerms: true}
                            }} >Order Now
                            </Link>
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
                                    pathname: Routes.ORDER_OPERATIONS_MODAL,
                                    state: { ID: 2 }
                                }} >Already Purchased
                                </Link>
                           
                        </div>
                    </div>
                </div>

            </div>
        )
}

const mapDispatchToProps = dispatch => (
    {
        setTempRequestList: (tempRequest) => dispatch(Actions.setTempRequestList(tempRequest))
    }
);

const mapStateToProps = state => {
    return {
        tempRequestList: state.tempRequestList.present,
        viewModel: state.viewModel
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(OrderTab);