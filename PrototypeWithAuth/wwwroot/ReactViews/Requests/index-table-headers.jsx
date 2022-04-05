import CategoryPopover from "./category-popover.jsx"
import PricePopover from "./price-popover.jsx"
import EmptyPage from "../Shared/empty-page.jsx"
import { connect } from 'react-redux';
function _IndexTableHeaders(props) {

    return (

        <div>
            {props.viewModel.PagedList?.length > 0 ?
                <div>
                    <table className="table table-headerspaced table-noheaderlines table-hover mb-0">
                        <thead>
                            <tr className="text-center">
                                {props.viewModel.PagedList[0].Columns.map((col, i) =>
                                    <th key={i} width={col.Width + "%"} className={(col.Width == 0) ? " p-0" : ""}>
                                        <label>{col.Title}</label>
                                        {{
                                            "Price":
                                                <div className="d-inline-block">
                                                    <PricePopover viewModel={props.viewModel.PricePopoverViewModel} />
                                                </div>,
                                            "Category":
                                                <div className="d-inline-block">
                                                    <CategoryPopover viewModel={props.viewModel.CategoryPopoverViewModel} />
                                                </div>
                                        }[col.FilterEnum]}

                                    </th>
                                )}
                            </tr>
                        </thead>
                    </table>
                </div>
                :
                <div>
                    <EmptyPage />
                </div>
            }
                </div>


    )
}

const mapStateToProps = state => {
    return {
        viewModel: state.viewModel,
        navigationInfo: state.navigationInfo
    };
};

export default connect(
    mapStateToProps
)(_IndexTableHeaders)