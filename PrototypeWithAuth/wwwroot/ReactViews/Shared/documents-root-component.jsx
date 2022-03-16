import React, { useState, useEffect, lazy, Suspense } from 'react';
import {
    MemoryRouter
} from 'react-router-dom';
import { createStore, } from 'redux';
import { Provider } from 'react-redux';

import reducer from '../ReduxRelatedUtils/reducers.jsx';
import { composeWithDevTools } from 'redux-devtools-extension';



export default function DocumentsRootComponent(props) {
    const store = createStore(reducer, { viewModel: props.viewModel, modals: [], tempRequestJson: {} }, composeWithDevTools());
    const App = lazy(() => import('./document-app.jsx'));
      
   

    return (
        <Provider store={store}><MemoryRouter>   <Suspense fallback={<div></div>}><App modalType={props.modalType} documentsInfo={props.documentsInfo} /></Suspense></MemoryRouter></Provider>
    );
}