$(function () {

	var ctxP = document.getElementById("labelChart").getContext('2d');

	var bodyStyles = window.getComputedStyle(document.body);
	var $orderInvColor = bodyStyles.getPropertyValue("--order-inv-color");
	var $protocolsColor = bodyStyles.getPropertyValue("--protocol-color");
	var $operationsColor = bodyStyles.getPropertyValue("--operations-color");
	var $biomarkersColor = bodyStyles.getPropertyValue("--biomarkers-color");
	var $timekeeperColor = bodyStyles.getPropertyValue("--timekeeper-color");
	var $labManageColor = bodyStyles.getPropertyValue("--lab-man-color");
	var $accountingColor = bodyStyles.getPropertyValue("--acc-color");
	var $expensesColor = bodyStyles.getPropertyValue("--expense-color");
	var $incomeColor = bodyStyles.getPropertyValue("--income-color");
	var $usersColor = bodyStyles.getPropertyValue("--user-color");
	var $orderInvColorHover = bodyStyles.getPropertyValue("--order-inv-25");
	var $protocolsColorHover = bodyStyles.getPropertyValue("--protocols-25");
	var $operationsColorHover = bodyStyles.getPropertyValue("--operations-25");
	var $biomarkersColorHover = bodyStyles.getPropertyValue("--biomarkers-25");
	var $timekeeperColorHover = bodyStyles.getPropertyValue("--timekeeper-25");
	var $labManageColorHover = bodyStyles.getPropertyValue("--lab-man-25");
	var $accountingColorHover = bodyStyles.getPropertyValue("--accounting-25");
	var $expensesColorHover = bodyStyles.getPropertyValue("--expenses-25");
	var $incomeColorHover = bodyStyles.getPropertyValue("--income-25");
	var $usersColorHover = bodyStyles.getPropertyValue("--users-25");



	var myPieChart = new Chart(ctxP, {
		plugins: [ChartDataLabels],
		type: 'pie',
		data: {
			labels: ["Expenses", "Orders & Inventory", "Protocols", "Accounting", "Users"],
			ChartDataLabels: ["1", "2", "3", "4", "5"],
			datasets: [{
				data: [210, 130, 120, 160, 120],
				backgroundColor: [$expensesColor, $orderInvColor, $protocolsColor, $accountingColor, $usersColor],
				hoverBackgroundColor: [$expensesColorHover, $orderInvColorHover, $protocolsColorHover, $accountingColorHover, $usersColorHover],
				borderWidth: 6
			}]
		},
		options: {
			responsive: true,
			//legend: {
			//	position: 'right',
			//	labels: {
			//		padding: 20,
			//		boxWidth: 10
			//	}
			//},
			plugins: {
				datalabels: {
					formatter: (value, ctx) => {
						let sum = 0;
						let dataArr = ctx.chart.data.datasets[0].data;
						dataArr.map(data => {
							sum += data;
						});
						let percentage = (value * 100 / sum).toFixed(2) + "%";
						return percentage;
					},
					color: 'white',
					labels: {
						title: {
							font: {
								size: '30'
							}
						}
					},
				}
			}
		}
	});

})