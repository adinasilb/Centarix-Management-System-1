
export default function CustomFieldButton(props) {

    //constructor(props) {
    //    super(props);
    //    this.state = { tabName: this.props.tabName };
    //    //this.OpenNewCustomField = this.OpenNewCustomField.bind(this);
    //}

    return (
        <input type="button" onClick={props.clickhandler} tabname={props.tabName} className={"section-bg-color custom-button custom-button-font mb-5"}
            value="+ Add Custom Field" />
    )
}