using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpressionApi.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class ExpressionTestController : ControllerBase {
        private readonly ILogger<ExpressionTestController> _logger;

        public ExpressionTestController (ILogger<ExpressionTestController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public void Get () {
            var date = DateTime.Today;
            var order = new Order { Customer = "Tom", Amount = 1000 };

            //それぞれ式ツリーで表現する
            ////リポジトリーのインターフェイス
            //①各条件の作成はそれぞれインフラによるので、それぞれのexpressionのクエリを作って、フィールドにセット、クエリーファクトリー
            //②検索処理ではフィールドにセットされた条件を使って検索する
            Expression<Func<Order, bool>> filter = x => (x.Customer == order.Customer && x.Amount > order.Amount) ||
                (x.TheDate == date && !x.Dicount);
            var visitor = new FilterConstructor ();
            //ここでstringではなくfilterコンテナーを返すようにすればいい
            //Expression.Call(pe, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));  みたいに内部でnestよべばいいはず
            var query = visitor.GetQuery (filter);
            Console.WriteLine (query);

        }
    }
}