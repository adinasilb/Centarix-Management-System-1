import React from 'react';
export default class ActionItem extends React.Component {
    render() {
        return (
            <div>
                {this.props.author}
            </div>
        );
    }
}