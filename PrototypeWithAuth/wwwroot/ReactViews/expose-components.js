import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';


// any css-in-js or other libraries you want to use server-side
import CommentInput from './comments-mention-input.jsx';

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;

global.CommentInput = CommentInput;

