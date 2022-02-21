import React, { memo } from 'react';
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
import DocumentsCard from './documents-card.jsx';
import DocumentsModal from './documents-modal.jsx'

export default class DocumentsRootComponent extends React.Component {
    constructor(props) {
        super(props);
        
    }


    render() {
        var docInfoRow1 = this.props.documentsInfo;
        console.log(docInfoRow1)
        var docInforRow2 = [];
        if (this.props.documentsInfo.Length > 4) {
            docInfoRow1 = this.props.documentsInfo.slice(0, 3);;
            docInforRow2 = this.props.documentsInfo.slice(4)
        }
        const app = (
            <div>
                    <div className="row document-margin-bottom">
                    {docInfoRow1.map((docInfo, i) => 
                            <div key={i} className={(i == 3 ? "" : "document-margin") + " doc-card " + docInfo.FolderName} >
                                <DocumentsCard key={i} documentsInfo={docInfo} modalType={this.props.modalType} />
                            </div>
                        )}
                    </div>
                    {docInforRow2.length > 0 ?
                        <div className="row document-margin-bottom">
                            {docInfoRow2.map((docInfo, i) =>
                                <div key={i} className={"document-margin doc-card " + docInfo.FolderName} >
                                    <DocumentsCard key={"DocCard" + (i + 4)} documentsInfo={docInfo} modalType={this.props.modalType} />
                                </div>
                            )}
                        </div>
                        :
                        null
                    }
                <Switch>
                    <Route path="/DocumentsModal" component={DocumentsModal} />
                </Switch>
            </div>
        )
        return <MemoryRouter>{app}</MemoryRouter>
    }
}