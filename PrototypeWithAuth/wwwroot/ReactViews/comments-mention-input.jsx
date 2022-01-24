import React from 'react';
import { MentionsInput, Mention } from 'react-mentions'


export default class CommentInput extends React.Component {

    constructor(props) {
        // Required step: always call the parent class' constructor
        super(props);

        // Set the state directly. Use props if necessary.
        this.state = {
            value: ""
        };

        this.handleChange = this.handleChange.bind(this);
        this.renderUserSuggestion = this.renderUserSuggestion.bind(this);
        this.displayTransform = this.displayTransform.bind(this);
    }

    handleChange(event) {
        console.log("handle change")
        this.setState({ value: event.target.value });
    }

    renderUserSuggestion() {
        console.log("rendersuggestion")
    }

    displayTransform (id, display){
        return `@${display}`
    }

    render() {

        return (
            <div className="scrollable">
                <MentionsInput name={this.props.commentTextName} id={this.props.commentTextID} className="react-mentions border-bottom" value={this.state.value} onChange={this.handleChange}>
          
                <Mention
                    displayTransform={this.displayTransform}
                    trigger="@"
                    className="users"
                    data={this.props.users}
                        appendSpaceOnAdd={true}
                        style="color:blue"
                />
       
                <Mention 
                    displayTransform={this.displayTransform}
                    trigger="@"
                    className= "items"
                    data={this.props.items}
                    appendSpaceOnAdd={true}                    
                />
                </MentionsInput>
                </div>
        );
    }
}
