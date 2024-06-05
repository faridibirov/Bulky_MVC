using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
	private readonly ApplicationDbContext _db;

	public OrderHeaderRepository(ApplicationDbContext db) :base(db)
	{
		_db = db;
	}

	
	public void Update(OrderHeader obj)
	{
		_db.OrderHeaders.Update(obj);
	}

	public void UpdateStatus(int id, string orderStatusEN, string orderStatusRU, string? paymentStatusEN = null, string? paymentStatusRU = null)
	{
		var orderFromDb=_db.OrderHeaders.FirstOrDefault(u=>u.Id == id);
		if (orderFromDb != null)
		{
			orderFromDb.OrderStatusEN= orderStatusEN;
			orderFromDb.OrderStatusRU= orderStatusRU;
			if(!string.IsNullOrEmpty(paymentStatusEN) || !string.IsNullOrEmpty(paymentStatusRU))
			{
				orderFromDb.PaymentStatusEN= paymentStatusEN;
				orderFromDb.PaymentStatusRU= paymentStatusRU;
			}
		}
	}

    public void UpdateBankPaymentID(int id, string orderId, string sessionId)
    {
        var orderHeaderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
        if (orderHeaderFromDb != null)
        {
            orderHeaderFromDb.SessionId = sessionId;
        }
    }

    public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
	{
		var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
		if (!string.IsNullOrEmpty(sessionId))
		{
			orderFromDb.SessionId= sessionId;
		}

		if(!string.IsNullOrEmpty(paymentIntentId))
		{
			orderFromDb.PaymentIntentId= paymentIntentId;
			orderFromDb.PaymentDate = DateTime.Now;
		}
	}
}
