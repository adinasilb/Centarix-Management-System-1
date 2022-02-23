import React, { Component } from 'react';

export default class TopTabsCount extends Component {

    constructor(props) {
        super(props);
        this.state = { list: this.props.list };
        console.log("top-tabs: " + this.props.location)
    }

    render() {
        return (
            <div className="row d-flex">
                <div className="item-table h-auto mb-4">
                    <ul className="pl-0">
                        {this.props.list.map((tab, i) => (
                            <li key={"Tab" + i} className=" list-inline-item m-0 p-1 ">
                                <a className={((i == 0) ? "active" : "") + " lm-top pl-2 pt-4 text-center new-button pb-0 align-items-center"} value="2" href="#">
                                    <label className="new-button-text">{tab.Name}</label>
                                </a>
                            </li>
                        ))
                        }
                    </ul>
                </div>
            </div>
        )
    }
}