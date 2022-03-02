﻿import React, { useState, useEffect } from 'react';
import {
    Route,
    Switch,
    MemoryRouter,
    Router
} from 'react-router-dom';
import { Provider } from 'react-redux';
import { createStore, } from 'redux';
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import * as Routes from '../Constants/Routes.jsx'
import DocumentsCard from './documents-card.jsx';
import ModalLoader from './modal-loader.jsx';
import reducer from '../ReduxRelatedUtils/reducers.jsx';
import { composeWithDevTools } from 'redux-devtools-extension';
import { CheckEditable } from '../Utility/document-fuctions.jsx'



export default function DocumentsRootComponent(props) {
    const store = createStore(reducer, { viewModel: props.viewModel, modals: [] }, composeWithDevTools());


    const [docInfoRow1, setDocInfoRow1] = useState(props.documentsInfo);
    const [docInfoRow2, setDocInfoRow2] = useState([]);
    const [isEditable, setIsEditable] = useState();
    console.log(docInfoRow1)
    var docInforRow2 = [];
    if (props.documentsInfo.Length > 4) {
        setDocInfoRow1(props.documentsInfo.slice(0, 3));
        setDocInfoRow2(props.documentsInfo.slice(4));
    }
    useEffect(() => {
        setIsEditable(CheckEditable(props.modalType));
    },[])

   

    function App() {
        return (
            <div>
                <div className="row document-margin-bottom">
                    {docInfoRow1.map((docInfo, i) =>
                        <div key={i} className={(i == 3 ? "" : "document-margin") + " doc-card " + docInfo.FolderName} >
                            <DocumentsCard key={i} documentsInfo={docInfo} showSwitch={props.modalType == "Summary" ? false : true} isEditable={isEditable} />
                        </div>
                    )}
                </div>
                {docInforRow2.length > 0 ?
                    <div className="row document-margin-bottom">
                        {docInfoRow2.map((docInfo, i) =>
                            <div key={i} className={"document-margin doc-card " + docInfo.FolderName} >
                                <DocumentsCard key={"DocCard" + (i + 4)} documentsInfo={docInfo} modalType={props.modalType} />
                            </div>
                        )}
                    </div>
                    :
                    null
                }
                <Switch>
                    <Route exact path={Routes.DOCUMENTS} exact render={(props) => <ModalLoader {...props} modalKey={ModalKeys.DOCUMENTS} uid={props.location.key} />} />
                    <Route exact path={Routes.DELETE_DOCUMENTS} exact render={(props) => <ModalLoader {...props} modalKey={ModalKeys.DELETE_DOCUMENTS} uid={props.location.key} />} />
                </Switch>
            </div>
        )
    }
    return (
        <Provider store={store}><MemoryRouter><App /></MemoryRouter></Provider>
    );
}