import React, { lazy, Suspense} from 'react';
import { Provider } from 'react-redux';
import { createStore, } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import { MemoryRouter} from 'react-router-dom';
import reducer from '../ReduxRelatedUtils/reducers.jsx';

export default function RootComponent(props) {
    const store = createStore(reducer, { viewModel: props.viewModel, modals: [], tempRequestJson: {} }, composeWithDevTools());
    const Scripts = lazy(() => import('../scripts.jsx'));  
    const App = lazy(() => import('./app.jsx'));

    return (
       
        <Provider store={store}>
            
            <Suspense fallback={<div></div>}><Scripts key="scripts" /></Suspense>
            <MemoryRouter >
                <Suspense fallback={<div></div>}><App viewEnum={props.viewEnum} viewModel={props.viewModel} /></Suspense>
            </MemoryRouter>
        </Provider>

    );
}