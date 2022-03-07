import React, { useEffect, useRef, useState } from 'react';
import { useLocation} from 'react-router-dom'
import IndexTableColumn from './index-table-column.jsx'

export default function IndexTableRow(props) {
    return (
        <tr key={props.row.r.RequestID} className="text-center one-row">
            {props.row.Columns.map((col, i) => (
                <IndexTableColumn key={"col" + i} columnData={col}  />
            ))}
        </tr>);

}
