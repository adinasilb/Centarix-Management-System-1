import React, { Component } from 'react';

export default class TopTabsCount extends Component {

    constructor(props) {
        super(props);
        this.state = { list: this.props.list };
    }

}