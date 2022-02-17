

import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

import Font from '@fortawesome/fontawesome-free/css/all.min.css';
import Bootstrap from 'bootstrap-css-only/css/bootstrap.min.css';
import MdbReact from 'mdbreact/dist/css/mdb.css';

//import FloatingActionBar from './floating-action-bar.jsx'
import RootComponent from './root-component.jsx';
import CloseButton from './Utility/close-button.jsx'

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

global.Font = Font;
global.Bootstrap = Bootstrap;
global.MdbReact = MdbReact;

//global.FloatingActionBar = FloatingActionBar;
global.RootComponent = RootComponent
global.CloseButton = CloseButton;