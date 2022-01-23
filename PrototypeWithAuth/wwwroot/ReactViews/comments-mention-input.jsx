import React from 'react';
import { MentionsInput, Mention } from 'react-mentions'

export default class CommentInput extends React.Component {

    constructor(props) {
        // Required step: always call the parent class' constructor
        super(props);

        // Set the state directly. Use props if necessary.
        this.state = {
           value:""
        }

        this.handleChange = this.handleChange.bind(this);
        this.renderUserSuggestion = this.renderUserSuggestion.bind(this);
    }

    handleChange(event) {
        this.setState({ value: event.target.value });
    }

    renderUserSuggestion() {
        console.log("rendersuggestion")
    }

    render() {

        return (
            <MentionsInput  class={this.props.classes} value={this.state.value} onChange={this.handleChange}>
                <Mention
                    trigger="@"
                    data={this.props.users}
                    
                />
            </MentionsInput>
        );
    }
}
