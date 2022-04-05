import React from 'react';
import { Page, View, Document} from '@react-pdf/renderer';
import OrderEmail from './order-email.jsx'


// Create Document Component
export default function OrderPdf(props) {
    <Document>
        <Page size="A4">
            <View >
                <OrderEmail viewModel={state.viewModel} tempRequestList={props.tempRequestList} navigationInfo={props.navigationInfo} />
            </View>
        </Page>
    </Document>
}