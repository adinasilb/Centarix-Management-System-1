import _IndexTableTabs from "./index-table-tabs.jsx"
import _IndexTableHeadersByVendor from './index-table-headers-by-vendor.jsx';
import _IndexTableDataByVendor from "./index-table-data-by-vendor.jsx"
import IndexTableReduxStore from "./index-table-redux-store.jsx"

export default function _IndexTableByVendor(props) {
    return (<div>
        <_IndexTableTabs viewModel={props.viewModel} key={"indexTableTabs"}/>
        <_IndexTableHeadersByVendor key={"indexTableHeader"} />
        <_IndexTableDataByVendor key={"indexTableData"} />
        <IndexTableReduxStore/>
    </div>
    )
}