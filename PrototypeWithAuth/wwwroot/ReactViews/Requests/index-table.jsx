import _IndexTableTabs from "./index-table-tabs.jsx"
import _IndexTableHeaders from './index-table-headers.jsx';
import _IndexTableData from "./index-table-data.jsx"
import IndexTableReduxStore from "./index-table-redux-store.jsx"

export default function _IndexTable(props) {
    return (<div>
        <_IndexTableTabs viewModel={props.viewModel} key={"indexTableTabs"}/>
        <_IndexTableHeaders key={"indexTableHeader"} />
        <_IndexTableData key={"indexTableData"} />
        <IndexTableReduxStore/>
    </div>
    )
}