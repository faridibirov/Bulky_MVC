using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility;

public static class SD
{
    public const string Role_Customer = "Customer";
    public const string Role_Company = "Company";
    public const string Role_Admin = "Admin";
    public const string Role_Employee = "Employee";

	public const string StatusPendingEN = "Pending";
	public const string StatusApprovedEN = "Approved";
	public const string StatusInProcessEN = "Processing";
	public const string StatusShippedEN = "Shipped";
	public const string StatusCancelledEN = "Cancelled";
	public const string StatusRefundedEN = "Refunded";

	public const string PaymentStatusPendingEN = "Pending";
	public const string PaymentStatusApprovedEN = "Approved";
	public const string PaymentStatusDelayedPaymentEN = "Approved For Delayed Payment";
	public const string PaymentStatusRejectedEN = "Rejected";

	public const string StatusPendingRU = "В Ожидании";
	public const string StatusApprovedRU = "Одобрен";
	public const string StatusInProcessRU = "В Обработке";
	public const string StatusShippedRU = "Отправлен";
	public const string StatusCancelledRU = "Отменен";
	public const string StatusRefundedRU = "Возвращен";

	public const string PaymentStatusPendingRU = "В Ождинании";
	public const string PaymentStatusApprovedRU = "Одобрен";
	public const string PaymentStatusDelayedPaymentRU = "Одобрено для отсрочки платежа";
	public const string PaymentStatusRejectedRU = "Отклонен";

	public const string SessionCart = "SessionShoppingCart";

}
