import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

//import FloatingActionBar from './floating-action-bar.jsx'
import RootComponent from './root-component.jsx';
import CloseButton from './close-button.jsx'
import OpenModalButton from './open-modal-button.jsx';
import DocumentsRootComponent from './documents-root-component.jsx'

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

//global.FloatingActionBar = FloatingActionBar;
global.RootComponent = RootComponent
global.CloseButton = CloseButton;
global.OpenModalButton = OpenModalButton;
global.DocumentsRootComponent = DocumentsRootComponent;