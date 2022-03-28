import React, { lazy, Suspense} from 'react';
import { Provider } from 'react-redux';
import { createStore, } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import { MemoryRouter} from 'react-router-dom';
import reducer from '../ReduxRelatedUtils/reducers.jsx';


export default function RootComponent(props) {
    const store = createStore(reducer, {
        viewModel: props.viewModel,
        modals: [],
        navigationInfo: {
            pageType: props.viewModel?.PageType, sectionType: props.viewModel?.SectionType, sideBarType: props.viewModel?.SideBarType || {}  },
        pricePopoverViewModel: {
            ...props.viewModel?.PricePopoverViewModel || {} },
        categoryPopoverViewModel: { ...props.viewModel?.CategoryPopoverViewModel || {}  },
        tabInfo: {
            tabValue: props.viewModel?.TabValue, tabs: [ ...props.viewModel?.Tabs ||[]]},
        selectedFilters: {},
        pageNumber: props.viewModel.PageNumber,
        reloadIndex: false
    }, composeWithDevTools());
    const Scripts = lazy(() => import('../scripts.jsx'));  
    const App = lazy(() => import('./app.jsx'));
  
    return (
        <Provider store={store}>
           
            <Suspense fallback={<div></div>}><Scripts key="scripts" /></Suspense>
            <MemoryRouter >
                <Suspense fallback={<div></div>}><App viewModel={props.viewModel} viewEnum={props.viewEnum} /></Suspense>
                </MemoryRouter>

        </Provider>

    );
}