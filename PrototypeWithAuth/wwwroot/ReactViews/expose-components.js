
import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

//import FloatingActionBar from './floating-action-bar.jsx'

import RootComponent from './Shared/root-component.jsx';
import DocumentsRootComponent from "./Shared/documents-root-component.jsx"
import CloseButton from './Utility/close-button.jsx'
//import OrderTabRootComponent from './Shared/order-tab-root-component.jsx'

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;


//global.FloatingActionBar = FloatingActionBar;
global.RootComponent = RootComponent
global.CloseButton = CloseButton;
global.DocumentsRootComponent = DocumentsRootComponent;
//global.OrderTabRootComponent = OrderTabRootComponent;