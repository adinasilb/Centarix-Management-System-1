
import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

import RootComponent from './Shared/root-component.jsx';
import DocumentsRootComponent from "./Shared/documents-root-component.jsx"

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

global.RootComponent = RootComponent
global.DocumentsRootComponent = DocumentsRootComponent;