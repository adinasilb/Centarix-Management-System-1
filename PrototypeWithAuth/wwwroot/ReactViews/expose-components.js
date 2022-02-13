import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

//import FloatingActionBar from './floating-action-bar.jsx'
import RootComponent from './root-component.jsx';
import CloseButton from './Utility/close-button.jsx'
import OpenModalButton from './Utility/open-modal-button.jsx';

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

//global.FloatingActionBar = FloatingActionBar;
global.RootComponent = RootComponent
global.CloseButton = CloseButton;
global.OpenModalButton = OpenModalButton;