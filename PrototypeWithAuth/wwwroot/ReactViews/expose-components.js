import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

//import FloatingActionBar from './floating-action-bar.jsx'
import RootComponent from './root-component.jsx';
import { openModal } from './modal-functions.jsx'
import CloseButton  from './close-button.jsx'

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

//global.FloatingActionBar = FloatingActionBar;
global.RootComponent = RootComponent
global.openModal = openModal;
global.CloseButton = CloseButton;


