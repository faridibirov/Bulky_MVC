using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
	void Update(OrderHeader obj);
	void UpdateStatus(int id, string orderStatusEN, string orderStatusRU, string? paymentStatusEN = null, string? paymentStatusRU = null);
	void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
}
