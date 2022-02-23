

import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

import *as Font from '@fortawesome/fontawesome-free/css/all.min.css';
import *as Bootstrap from 'bootstrap-css-only/css/bootstrap.min.css';
import *as MdbReactCSS from 'mdbreact/dist/css/mdb.css';

//import FloatingActionBar from './floating-action-bar.jsx'
import RootComponent from './Shared/root-component.jsx';
import CloseButton from './Utility/close-button.jsx'

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

global.Font = Font;
global.Bootstrap = Bootstrap;
global.MdbReactCSS = MdbReactCSS;

//global.FloatingActionBar = FloatingActionBar;
global.RootComponent = RootComponent
global.CloseButton = CloseButton;