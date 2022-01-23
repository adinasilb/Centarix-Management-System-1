import React from 'react';
import { MentionsInput, Mention } from 'react-mentions'

const users = [
    {
        id: "1",
        display: "Jimmy"
    },
    {
        id: "2",
        display: "Ketut"
    },
    {
        id: "3",
        display: "Gede"
    }
];


export default class CommentInput extends React.Component {

    constructor(props) {
        // Required step: always call the parent class' constructor
        super(props);

        // Set the state directly. Use props if necessary.
        this.state = {
            value: ""
        };

        ///this.handleChange = this.handleChange.bind(this);
        this.renderUserSuggestion = this.renderUserSuggestion.bind(this);
    }

    handleChange = e => {
    e.preventDefault();
        console.log("handle change")
        this.setState({ value: event.target.value });
    }

    renderUserSuggestion() {
        console.log("rendersuggestion")
    }

    render() {

        return (
            <MentionsInput class={this.props.classes} value={this.state.value} onChange={this.handleChange}
                markup="@[__display__](__id__)" onChange={this.handleCommentChange}
            >
                <Mention
                    trigger="@"
                    data={users}
                    renderSuggestion={this.renderUserSuggestion}
                />
            </MentionsInput>
        );
    }
}
