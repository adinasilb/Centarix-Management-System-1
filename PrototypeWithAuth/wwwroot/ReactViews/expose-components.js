import 'mdb-react-ui-kit/dist/css/mdb.min.css'
import '@fortawesome/fontawesome-free/css/all.min.css';
import 'bootstrap-css-only/css/bootstrap.min.css';
import 'mdbreact/dist/css/mdb.css';

import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

//import FloatingActionBar from './floating-action-bar.jsx'
import RootComponent from './Shared/root-component.jsx';
import CloseButton from './Utility/close-button.jsx'

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;


//global.FloatingActionBar = FloatingActionBar;
global.RootComponent = RootComponent
global.CloseButton = CloseButton;