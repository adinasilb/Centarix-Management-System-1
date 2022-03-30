import React, { lazy, Suspense} from 'react';
import { Provider } from 'react-redux';
import { createStore, } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import { MemoryRouter} from 'react-router-dom';
import reducer from '../ReduxRelatedUtils/reducers.jsx';
import { CurrencyEnum, PriceSortEnum } from '../Constants/AppUtility.jsx'

export default function RootComponent(props) {
    console.log("root component")
    const store = createStore(reducer, {
        viewModel: props.viewModel,
        navigationInfo: {
            pageType: props.viewModel?.PageType, sectionType: props.viewModel?.SectionType, sideBarType: props.viewModel?.SidebarType || {}  },       
        tabValue: props.viewModel?.TabValue,
        pageNumber: props.viewModel.PageNumber,
        reloadIndex: false,
        selectedCurrency: CurrencyEnum.NIS,
        priceSortEnums: PriceSortEnum.TotalVat,
        categorySelected: false,
        subcategorySelected: false,
        sourceSelected:false
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