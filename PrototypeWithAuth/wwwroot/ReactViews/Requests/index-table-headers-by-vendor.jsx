import CategoryPopover from "./category-popover.jsx"
import PricePopover from "./price-popover.jsx"
import EmptyPage from "../Shared/empty-page.jsx"
import { connect } from 'react-redux';
function _IndexTableHeadersByVendor(props) {

    return (

        <div>
            {props.viewModel.RequestsByVendor?.length > 0 ?
                <div>
                    <table className="table table-headerspaced table-noheaderlines table-hover mb-0">
                        <thead>
                            <tr className="text-center">
                                {props.viewModel.RequestsByVendor.map((rbv, i) => (
                                    <th key={i} width={rbv[0].Width + "%"} >
                                        <label>{rbv[0].Title}</label>
                                        {{
                                            "Price":
                                                <div className="d-inline-block">
                                                    <PricePopover />
                                                </div>,
                                            "Category":
                                                <div className="d-inline-block">
                                                    <CategoryPopover />
                                                </div>
                                        }[rbv[0].FilterEnum]}

                                    </th>
                                ))}
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
    console.log("mstp header")
    return {
        viewModel: state.viewModel,
        navigationInfo: state.navigationInfo
    };
};

export default connect(
    mapStateToProps
)(_IndexTableHeadersByVendor)