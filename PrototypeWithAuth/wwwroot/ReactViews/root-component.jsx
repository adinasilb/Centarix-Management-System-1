import React from 'react';
import {
    Link,
    BrowserRouter,
    Route,
    Switch,
    StaticRouter,
    Redirect,
    MemoryRouter,
    Router,
    useHistory
} from 'react-router-dom';
import DeleteModal from './delete-modal.jsx';
import _IndexTableData from './index-table-data.jsx';
import _IndexTableDataByVendor from './index-table-data-by-vendor.jsx';
import SettingsInventory from "./Requests/settings-inventory.jsx";
import FloatingActionBar from './floating-action-bar.jsx';


export default class RootComponent extends React.Component {
    constructor(props) {
        super(props);

        this.state = { viewModel: this.props.viewModel, viewEnum: this.props.viewEnum, bcColor: this.props.bcColor, ajaxLink: this.props.ajaxLink, btnText: this.props.btnText, sectionClass: this.props.sectionClass };
    }

    renderSwitch = () => {
        console.log(this.state.viewEnum);
        switch (this.state.viewEnum) {
            case "IndexTableDataByVendor":
                return (<_IndexTableDataByVendor viewModel={this.state.viewModel} showView={true} bcColor={this.state.bcColor} ajaxLink={this.state.ajaxLink} btnText={this.state.btnText} sectionClass={this.state.sectionClass} />);
                break;
            case "IndexTableData":
                return (<_IndexTableData viewModel={this.state.viewModel} showView={true} bcColor={this.state.bcColor} />);
                break;
            case "SettingsInventory":
                return (<SettingsInventory viewModel={this.state.viewModel} showView={true} />);
                break;
        }
    }


    render() {
        const app = (

            <div>
     
                <FloatingActionBar showFloatingActionBar={false} />
                {
                    this.renderSwitch()
                }

                <Switch>
                    <Route path="/DeleteModal" component={DeleteModal} />
                    <Route path="/_IndexTableData" component={_IndexTableData} />
                    <Route path="SettingsInventory" component={SettingsInventory} />
                </Switch>
            </div>
        );

        if (typeof window === 'undefined') {
            return (
                <StaticRouter
                    context={this.props.context}
                    location={this.props.location}
                >
                    {app}
                </StaticRouter>
            );
        }
        return <MemoryRouter >{app}</MemoryRouter>;
    }
}