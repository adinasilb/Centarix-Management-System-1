import _IndexTableTabs from "./index-table-tabs.jsx"
import _IndexTableHeaders from './index-table-headers.jsx';
import _IndexTableData from "./index-table-data.jsx"
import AjaxParitalIndexTable from "../Utility/root-function.jsx"

export default function _IndexTable(props) {
    return (<div>
        <_IndexTableTabs key={"indexTableTabs"}/>
        <_IndexTableHeaders key={"indexTableHeader"} />
        <_IndexTableData key={"indexTableData"} />
        <AjaxParitalIndexTable/>
    </div>
    )
}