
export default function CustomFieldButton(props) {

    //constructor(props) {
    //    super(props);
    //    this.state = { tabName: this.props.tabName };
    //    //this.OpenNewCustomField = this.OpenNewCustomField.bind(this);
    //}

    return (
        <input type="button" onClick={props.clickhandler} className={"custom-button custom-cancel text border-dark "}
            value="+ Add Custom Field" />
    )
}