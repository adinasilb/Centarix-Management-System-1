
export default function CustomFieldButton(props) {

    return (
        <input type="button" onClick={props.clickhandler} tabname={props.tabName} className={"section-bg-color custom-button custom-button-font mb-5"}
            value="+ Add Custom Field" />
    )
}